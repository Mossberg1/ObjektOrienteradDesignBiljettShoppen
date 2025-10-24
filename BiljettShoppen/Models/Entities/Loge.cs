using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Entities.Base;

namespace Models.Entities
{
    public class Loge : BookableSpace
    {
        public string Name { get; set; }
        public string LogeNumber { get; set; }
        public int NumberOfPeople { get; set; }

        public override string GetDescription()
        {
            return Name;
        }
    }

}
