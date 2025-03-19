using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models
{
    public class Admin
    {
        public Guid AdminID { get; set; }
        public Guid UserID { get; set; }

        public Admin(Guid adminId, Guid userId)
        {
            AdminID = adminId;
            UserID = userId;
        }
    }
}
