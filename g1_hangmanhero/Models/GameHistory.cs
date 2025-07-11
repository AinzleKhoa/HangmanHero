using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace g1_hangmanhero.Models
{
    public class GameHistory
    {
        public int GameId { get; set; }
        public int PlayerId { get; set; } // Foreign key
        public string WordUsed { get; set; }
        public string Category { get; set; }
        public string Difficulty { get; set; }
        public int Score { get; set; }
        public int Mistakes { get; set; }
        public int TimeTaken { get; set; }
        public DateTime PlayedAt { get; set; }

        // Navigation property for the related player
        public Player Player { get; set; }
    }
}
