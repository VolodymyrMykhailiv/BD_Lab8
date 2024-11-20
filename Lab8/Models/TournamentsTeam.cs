using System;
using System.Collections.Generic;

namespace Lab8.Models;

public partial class TournamentsTeam
{
    public int TournamentId { get; set; }

    public int TeamId { get; set; }

    public int? TeamPoints { get; set; }

    public virtual Team Team { get; set; } = null!;

    public virtual Tournament Tournament { get; set; } = null!;
}
