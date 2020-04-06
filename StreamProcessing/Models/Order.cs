using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Models
{
    [Serializable]
    public class Order
    {
        [BsonElement]

        public int OrderID { get; set; }

        [BsonElement]

        public DateTime OrderDate { get; set; }

        [BsonElement]

        public string ShipName { get; set; }

        [BsonElement]

        public string ShipAddress { get; set; }

        [BsonElement]

        public string ShipCity { get; set; }

        [BsonElement]
        public string ShipCountry { get; set; }
    }
}
