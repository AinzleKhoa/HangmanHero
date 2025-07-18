using System;
using System.Collections.Generic;

namespace g1_hangmanhero.Models;

public partial class Word
{
    public int WordId { get; set; }

    public string Text { get; set; } = null!;

    public string Category { get; set; } = null!;

    public string Difficulty { get; set; } = null!;

    public virtual ICollection<GameHistory> GameHistories { get; set; } = new List<GameHistory>();
}
