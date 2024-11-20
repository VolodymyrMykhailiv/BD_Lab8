using System;
using System.Collections.Generic;

namespace Lab8.Models;

public partial class Coach
{
    public int CoachId { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public DateOnly? Birthday { get; set; }

    public string? Nationality { get; set; }

    public int? TeamId { get; set; }

    public virtual Team? Team { get; set; }
}
