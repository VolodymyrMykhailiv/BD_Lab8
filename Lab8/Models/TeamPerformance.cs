using System;
using System.Collections.Generic;

namespace Lab8.Models;

public partial class TeamPerformance
{
    public string TeamName { get; set; } = null!;

    public long TotalMatches { get; set; }

    public decimal? Wins { get; set; }

    public decimal? Losses { get; set; }

    public decimal? GoalsScored { get; set; }

    public decimal? GoalsConceded { get; set; }
}
