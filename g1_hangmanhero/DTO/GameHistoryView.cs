using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace g1_hangmanhero.DTO
{
    public class GameHistoryView
    {
        public string Username { get; set; }
        public int Score { get; set; }
        public int Mistakes { get; set; }
        public int TimeTaken { get; set; }   
        public DateTime PlayedAt { get; set; } 
    }
}


