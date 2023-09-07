using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace ADO_EF_29._08._2023_1_.Data.Entity
{
    public class Department
    {
        public Guid Id { get; set; }
        public String Name { get; set; } = null!;

        public DateTime? DeleteDt { get; set; }

    }
}
