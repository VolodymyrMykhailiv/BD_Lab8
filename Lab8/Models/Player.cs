using System;
using System.Collections.Generic;

namespace Lab8.Models;

public partial class Player
{
    public int PlayerId { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public DateOnly? Birthday { get; set; }

    public string? Nationality { get; set; }

    public string Position { get; set; } = null!;

    public int? TeamId { get; set; }

    public decimal? Salary { get; set; }

    public virtual Team? Team { get; set; }
}
