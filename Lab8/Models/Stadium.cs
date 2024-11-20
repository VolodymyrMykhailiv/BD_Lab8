using System;
using System.Collections.Generic;

namespace Lab8.Models;

public partial class Stadium
{
    public int StadiumId { get; set; }

    public string Name { get; set; } = null!;

    public string City { get; set; } = null!;

    public string? Country { get; set; }

    public int? Capacity { get; set; }

    public virtual ICollection<Match> Matches { get; set; } = new List<Match>();

    public virtual ICollection<Team> Teams { get; set; } = new List<Team>();
}
