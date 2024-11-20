using System;
using System.Collections.Generic;

namespace Lab8.Models;

public partial class Tournament
{
    public int TournamentId { get; set; }

    public string Name { get; set; } = null!;

    public string Season { get; set; } = null!;

    public virtual ICollection<TournamentsTeam> TournamentsTeams { get; set; } = new List<TournamentsTeam>();
}
