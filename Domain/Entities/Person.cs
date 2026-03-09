using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Person : BaseEntity
    {
        public required string Name { get; set; }
        public required string Surname { get; set; }
        
        [NotMapped]
        public string Details => $"{Name} {Surname}";
    }
}
