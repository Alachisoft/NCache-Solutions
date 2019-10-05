namespace Microsoft.eShopOnContainers.Services.Ordering.API.Application.Queries
{
    using Dapper;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using System;
    using System.Collections.Generic;
    using Alachisoft.NCache.Client;
    using Alachisoft.NCache.Runtime.Caching;
    using Microsoft.Extensions.Configuration;

    public class OrderQueries
        : IOrderQueries
    {
        private string _connectionString = string.Empty;
        private ICache _cache { get; }
        private IConfiguration _configuration { get; }

        public OrderQueries(IConfiguration configuration, ICache cache)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _connectionString = !string.IsNullOrWhiteSpace(configuration["ConnectionString"]) ? configuration["ConnectionString"] : throw new ArgumentNullException("Connection string not given");
            _cache = cache;
        }


        public async Task<Order> GetOrderAsync(int id)
        {

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var result = await connection.QueryAsync<dynamic>(
                   @"select o.[Id] as ordernumber,o.OrderDate as date, o.Description as description,
                        o.Address_City as city, o.Address_Country as country, o.Address_State as state, o.Address_Street as street, o.Address_ZipCode as zipcode,
                        os.Name as status, 
                        oi.ProductName as productname, oi.Units as units, oi.UnitPrice as unitprice, oi.PictureUrl as pictureurl
                        FROM ordering.Orders o
                        LEFT JOIN ordering.Orderitems oi ON o.Id = oi.orderid 
                        LEFT JOIN ordering.orderstatus os on o.OrderStatusId = os.Id
                        WHERE o.Id=@id"
                        , new { id }
                    );

                if (result.AsList().Count == 0)
                    throw new KeyNotFoundException();

                return MapOrderItems(result);
            }
        }

        public async Task<IEnumerable<OrderSummary>> GetOrdersFromUserAsync(Guid userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                return await connection.QueryAsync<OrderSummary>(@"SELECT o.[Id] as ordernumber,o.[OrderDate] as [date],os.[Name] as [status], SUM(oi.units*oi.unitprice) as total
                     FROM [ordering].[Orders] o
                     LEFT JOIN[ordering].[orderitems] oi ON  o.Id = oi.orderid 
                     LEFT JOIN[ordering].[orderstatus] os on o.OrderStatusId = os.Id                     
                     LEFT JOIN[ordering].[buyers] ob on o.BuyerId = ob.Id
                     WHERE ob.IdentityGuid = @userId
                     GROUP BY o.[Id], o.[OrderDate], os.[Name] 
                     ORDER BY o.[Id]", new { userId });
            }
        }

        public async Task<IEnumerable<CardType>> GetCardTypesAsync()
        {
            if (_cache == null)
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    return await connection.QueryAsync<CardType>("SELECT * FROM ordering.cardtypes");
                }
            }
            else
            {
                var key = $"Get All CardTypes";

                var cardTypes = await Task.Run(() => _cache.Get<List<CardType>>(key));

                if (cardTypes == null)
                {
                    using (var connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();

                        var temp = await connection.QueryAsync<CardType>("SELECT * FROM ordering.cardtypes");

                        if (temp != null && temp.AsList().Count > 0)
                        {
                            cardTypes = new List<CardType>();
                            cardTypes.AddRange(temp);

                            var cacheItem = new CacheItem(cardTypes)
                            {
                                Expiration = new Expiration(ExpirationType.Absolute, TimeSpan.FromHours(12))
                            };

                            await _cache.InsertAsync(key, cacheItem);
                        }

                        return temp;
                    }
                }

                return cardTypes;
            }
        }

        private Order MapOrderItems(dynamic result)
        {
            var order = new Order
            {
                ordernumber = result[0].ordernumber,
                date = result[0].date,
                status = result[0].status,
                description = result[0].description,
                street = result[0].street,
                city = result[0].city,
                zipcode = result[0].zipcode,
                country = result[0].country,
                orderitems = new List<Orderitem>(),
                total = 0
            };

            foreach (dynamic item in result)
            {
                var orderitem = new Orderitem
                {
                    productname = item.productname,
                    units = item.units,
                    unitprice = (double)item.unitprice,
                    pictureurl = item.pictureurl
                };

                order.total += item.units * item.unitprice;
                order.orderitems.Add(orderitem);
            }

            return order;
        }
    }
}
