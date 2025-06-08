using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Persistence.EF.DbContexts;

namespace Persistence.EF
{
    public class SeederData
    {
        private readonly UserDbContext _dbContext;

        public SeederData(UserDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                if (!_dbContext.Role.Any())
                {
                    _dbContext.AddRange(GetRules());
                    _dbContext.SaveChanges();
                }
            }
        }

        private IEnumerable<Role> GetRules()
        {
            return new List<Role>
            {
                new Role{
                    Name = "admin"
                },
                new Role{
                    Name = "user"
                }
            };
        }
    }
}