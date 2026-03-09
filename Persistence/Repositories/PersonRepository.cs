using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Persistence.DataContext;
using Microsoft.Extensions.Logging;
namespace Persistence.SQL
{
    public class PersonRepository : SQLRepository<Person>, IPersonRepository
    {
        public PersonRepository(LoansDbContext dbContext,ILogger<PersonRepository> logger) : base(dbContext,logger)
        {
        }
    }
}
