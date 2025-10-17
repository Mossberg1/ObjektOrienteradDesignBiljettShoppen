using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    internal class Loge
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LogeNumber { get; set; }
        public int NumberOfPeople { get; set; }
        public decimal Price { get; set; }
    }

}
