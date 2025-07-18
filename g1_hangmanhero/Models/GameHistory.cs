using System;
using System.Collections.Generic;

namespace g1_hangmanhero.Models;

public partial class GameHistory
{
    public int GameId { get; set; }

    public int PlayerId { get; set; }

    public int WordId { get; set; }

    public int Score { get; set; }

    public int Mistakes { get; set; }

    public int TimeTaken { get; set; }

    public DateTime? PlayedAt { get; set; }

    public virtual Player Player { get; set; } = null!;

    public virtual Word Word { get; set; } = null!;
}
