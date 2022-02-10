using RavenSmoke.Data;
using RavenSmoke.Models.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavenSmoke.Services
{
    public class CustomerService
    {
        private readonly Guid _userId;

        public CustomerService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreateCustomer(CustomerCreate model)
        {
            var entity = new Customer()
            {
                OwnerId = _userId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                CreatedUtc = DateTimeOffset.Now,
            };

            using (var ctx = new ApplicationDbContext())
            {
                ctx.Customers.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public IEnumerable<CustomerListItem> GetCustomers()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query = ctx
                    .Customers
                    .Select(
                    e =>
                    new CustomerListItem
                    {
                        CustomerId = e.CustomerId,
                        FirstName = e.FirstName,
                        LastName = e.LastName,

                    }
                    );
                return query.ToList();
            }
        }

        public CustomerDetails GetCustomerById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx
                    .Customers
                    .Single(e => e.CustomerId == id);
                return new CustomerDetails
                {
                    CustomerId = entity.CustomerId,
                    FirstName = entity.FirstName,
                    LastName = entity.LastName,
                    Address = entity.Address,
                    CreatedUtc = entity.CreatedUtc,
                };
            }
        }

            public bool UpdateCustomer(CustomerEdit model)
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var entity = ctx
                        .Customers
                        .Single(e => e.CustomerId == model.CustomerId);

                    entity.CustomerId = model.CustomerId;
                    entity.FirstName = model.FirstName;
                    entity.LastName = model.LastName;
                    entity.Address = model.Address;
                    entity.ModifiedUtc = DateTimeOffset.UtcNow;

                    return ctx.SaveChanges() == 1;
                }
            }
        }
    }
