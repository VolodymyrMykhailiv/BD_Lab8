using System;
using System.Collections.Generic;

namespace Lab8.Models;

public partial class Match
{
    public int MatchId { get; set; }

    public DateTime Date { get; set; }

    public string Status { get; set; } = null!;

    public int HostTeamGoals { get; set; }

    public int AwayTeamGoals { get; set; }

    public int? StadiumId { get; set; }

    public int HomeTeamId { get; set; }

    public int AwayTeamId { get; set; }

    public int? TournamentId { get; set; }

    public virtual Team AwayTeam { get; set; } = null!;

    public virtual Team HomeTeam { get; set; } = null!;

    public virtual Stadium? Stadium { get; set; }
}
