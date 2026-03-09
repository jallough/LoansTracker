using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Domain.Entities
{
    public class Loan : BaseEntity
    {
        public Person? Person { get; set; }
        public long PersonId { get; set; }
        public double Amount { get; set; }
        public DateOnly Date { get; set; }
        public string? Description { get; set; }
    }
}
