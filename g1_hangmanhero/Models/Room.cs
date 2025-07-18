using System;
using System.Collections.Generic;

namespace g1_hangmanhero.Models;

public partial class Room
{
    public int RoomId { get; set; }

    public bool? Status { get; set; }

    public int? CategoryId { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<Use> Uses { get; set; } = new List<Use>();
}
