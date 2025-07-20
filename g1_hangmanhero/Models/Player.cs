using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace g1_hangmanhero.Models
{
    public class Player
    {
        public int PlayerId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; } // Store hashed passwords
        public DateTime? JoinDate { get; set; }
        public string DefaultDifficulty { get; set; } = "Easy"; // Default difficulty
        public int? Role { get; set; } = 0;
    }
}
