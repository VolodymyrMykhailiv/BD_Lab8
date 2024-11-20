using System;
using System.Collections.Generic;

namespace Lab8.Models;

public partial class Team
{
    public int TeamId { get; set; }

    public string Name { get; set; } = null!;

    public string Country { get; set; } = null!;

    public int? FoundationYear { get; set; }

    public int? StadiumId { get; set; }

    public virtual Coach? Coach { get; set; }

    public virtual ICollection<Match> MatchAwayTeams { get; set; } = new List<Match>();

    public virtual ICollection<Match> MatchHomeTeams { get; set; } = new List<Match>();

    public virtual ICollection<Player> Players { get; set; } = new List<Player>();

    public virtual Stadium? Stadium { get; set; }

    public virtual ICollection<TournamentsTeam> TournamentsTeams { get; set; } = new List<TournamentsTeam>();
}
