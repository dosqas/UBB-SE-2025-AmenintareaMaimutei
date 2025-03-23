using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models
{
    public class Admin
    {
        public int AdminID { get; set; }
        public int UserID { get; set; }

        public Admin(int adminId, int userId)
        {
            AdminID = adminId;
            UserID = userId;
        }
    }
}
