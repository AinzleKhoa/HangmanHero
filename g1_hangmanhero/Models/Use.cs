using System;
using System.Collections.Generic;

namespace g1_hangmanhero.Models;

public partial class Use
{
    public int CustomerId { get; set; }

    public int RoomId { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Room Room { get; set; } = null!;
}
