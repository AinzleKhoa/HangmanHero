using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace g1_hangmanhero.Models
{
    public class Word
    {
        public int WordId { get; set; }
        public string Text { get; set; }
        public string Category { get; set; }
        public string Difficulty { get; set; }
        public virtual ICollection<GameHistory> GameHistories { get; set; }
    }
}
