using System;
using System.Collections.Generic;

namespace g1_hangmanhero.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string? CustomerName { get; set; }

    public string? Phone { get; set; }

    public string? Note { get; set; }

    public virtual ICollection<Use> Uses { get; set; } = new List<Use>();
}
