﻿using Microsoft.EntityFrameworkCore;
using Microsoft.eShopOnContainers.Services.Ordering.Domain.AggregatesModel.BuyerAggregate;
using Microsoft.eShopOnContainers.Services.Ordering.Domain.Seedwork;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.eShopOnContainers.Services.Ordering.Infrastructure.Repositories
{
    public class BuyerRepository
        : IBuyerRepository
    {
        private readonly OrderingContext _context;

        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _context;
            }
        }

        public BuyerRepository(OrderingContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Buyer Add(Buyer buyer)
        {
            if (buyer.IsTransient())
            {
                var buyer1 = _context.Buyers
                    .Add(buyer)
                    .Entity;
                

                return buyer1;

            }
            else
            {
                return buyer;
            }
        }

        public Buyer Update(Buyer buyer)
        {
            var buyer1 = _context.Buyers
                        .Update(buyer)
                        .Entity;
            

            return buyer1;
        }

        public async Task<Buyer> FindAsync(string identity)
        {
            return await _context.Buyers
                        .Include(b => b.PaymentMethods)
                        .Where(b => b.IdentityGuid == identity)
                        .SingleOrDefaultAsync(); 
            
        }

        public async Task<Buyer> FindByIdAsync(string id)
        {
            return await _context.Buyers
                        .Include(b => b.PaymentMethods)
                        .Where(b => b.Id == int.Parse(id))
                        .SingleOrDefaultAsync(); 
            
        }
    }
}
