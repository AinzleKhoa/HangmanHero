using System;
using System.Collections.Generic;

namespace g1_hangmanhero.Models;

public partial class Player
{
    public int PlayerId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public DateTime? JoinDate { get; set; }

    public string DefaultDifficulty { get; set; } = null!;

    public virtual ICollection<GameHistory> GameHistories { get; set; } = new List<GameHistory>();
}
