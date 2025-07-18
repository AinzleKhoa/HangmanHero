using System;
using System.Collections.Generic;

namespace g1_hangmanhero.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public int? NumberOfSeat { get; set; }

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
