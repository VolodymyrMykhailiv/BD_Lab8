using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace Lab8.Models;

public partial class FootballDbContext : DbContext
{
    public FootballDbContext()
    {
    }

    public FootballDbContext(DbContextOptions<FootballDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Coach> Coaches { get; set; }

    public virtual DbSet<Match> Matches { get; set; }

    public virtual DbSet<Player> Players { get; set; }

    public virtual DbSet<Stadium> Stadia { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<TeamPerformance> TeamPerformances { get; set; }

    public virtual DbSet<Tournament> Tournaments { get; set; }

    public virtual DbSet<TournamentsTeam> TournamentsTeams { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;database=football_events;user id=root;password=18072005", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.39-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Coach>(entity =>
        {
            entity.HasKey(e => e.CoachId).HasName("PRIMARY");

            entity.ToTable("coach");

            entity.HasIndex(e => e.TeamId, "team_id_UNIQUE").IsUnique();

            entity.Property(e => e.CoachId).HasColumnName("coach_id");
            entity.Property(e => e.Birthday).HasColumnName("birthday");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Nationality)
                .HasMaxLength(52)
                .HasColumnName("nationality");
            entity.Property(e => e.Surname)
                .HasMaxLength(52)
                .HasColumnName("surname");
            entity.Property(e => e.TeamId).HasColumnName("team_id");

            entity.HasOne(d => d.Team).WithOne(p => p.Coach)
                .HasForeignKey<Coach>(d => d.TeamId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("coach_ibfk_1");
        });

        modelBuilder.Entity<Match>(entity =>
        {
            entity.HasKey(e => e.MatchId).HasName("PRIMARY");

            entity.ToTable("match");

            entity.HasIndex(e => e.AwayTeamId, "away_team_id");

            entity.HasIndex(e => e.TournamentId, "fk_tournament");

            entity.HasIndex(e => e.HomeTeamId, "home_team_id");

            entity.HasIndex(e => e.StadiumId, "stadium_id");

            entity.Property(e => e.MatchId).HasColumnName("match_id");
            entity.Property(e => e.AwayTeamGoals).HasColumnName("away_team_goals");
            entity.Property(e => e.AwayTeamId).HasColumnName("away_team_id");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.HomeTeamId).HasColumnName("home_team_id");
            entity.Property(e => e.HostTeamGoals).HasColumnName("host_team_goals");
            entity.Property(e => e.StadiumId).HasColumnName("stadium_id");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'unknown'")
                .HasColumnType("enum('scheduled','in_progress','finished','cancelled','unknown')")
                .HasColumnName("status");
            entity.Property(e => e.TournamentId).HasColumnName("tournament_id");

            entity.HasOne(d => d.AwayTeam).WithMany(p => p.MatchAwayTeams)
                .HasForeignKey(d => d.AwayTeamId)
                .HasConstraintName("match_ibfk_3");

            entity.HasOne(d => d.HomeTeam).WithMany(p => p.MatchHomeTeams)
                .HasForeignKey(d => d.HomeTeamId)
                .HasConstraintName("match_ibfk_2");

            entity.HasOne(d => d.Stadium).WithMany(p => p.Matches)
                .HasForeignKey(d => d.StadiumId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("match_ibfk_1");
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.PlayerId).HasName("PRIMARY");

            entity.ToTable("player");

            entity.HasIndex(e => e.Birthday, "idx_player_birtday");

            entity.HasIndex(e => new { e.Name, e.Surname }, "idx_player_full_name");

            entity.HasIndex(e => e.Position, "idx_player_position");

            entity.HasIndex(e => e.TeamId, "team_id");

            entity.Property(e => e.PlayerId).HasColumnName("player_id");
            entity.Property(e => e.Birthday).HasColumnName("birthday");
            entity.Property(e => e.Name)
                .HasMaxLength(52)
                .HasColumnName("name");
            entity.Property(e => e.Nationality)
                .HasMaxLength(52)
                .HasColumnName("nationality");
            entity.Property(e => e.Position)
                .HasMaxLength(32)
                .HasColumnName("position");
            entity.Property(e => e.Salary)
                .HasPrecision(10, 2)
                .HasColumnName("salary");
            entity.Property(e => e.Surname)
                .HasMaxLength(52)
                .HasColumnName("surname");
            entity.Property(e => e.TeamId).HasColumnName("team_id");

            entity.HasOne(d => d.Team).WithMany(p => p.Players)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("player_ibfk_1");
        });

        modelBuilder.Entity<Stadium>(entity =>
        {
            entity.HasKey(e => e.StadiumId).HasName("PRIMARY");

            entity.ToTable("stadium");

            entity.HasIndex(e => e.Name, "idx_stadium_name");

            entity.Property(e => e.StadiumId).HasColumnName("stadium_id");
            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .HasColumnName("city");
            entity.Property(e => e.Country)
                .HasMaxLength(50)
                .HasColumnName("country");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.TeamId).HasName("PRIMARY");

            entity.ToTable("team");

            entity.HasIndex(e => e.Name, "idx_team_name");

            entity.HasIndex(e => e.StadiumId, "stadium_id");

            entity.Property(e => e.TeamId).HasColumnName("team_id");
            entity.Property(e => e.Country)
                .HasMaxLength(52)
                .HasColumnName("country");
            entity.Property(e => e.FoundationYear).HasColumnName("foundation_year");
            entity.Property(e => e.Name)
                .HasMaxLength(52)
                .HasColumnName("name");
            entity.Property(e => e.StadiumId).HasColumnName("stadium_id");

            entity.HasOne(d => d.Stadium).WithMany(p => p.Teams)
                .HasForeignKey(d => d.StadiumId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("team_ibfk_1");
        });

        modelBuilder.Entity<TeamPerformance>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("team_performance");

            entity.Property(e => e.GoalsConceded)
                .HasPrecision(32)
                .HasColumnName("goals_conceded");
            entity.Property(e => e.GoalsScored)
                .HasPrecision(32)
                .HasColumnName("goals_scored");
            entity.Property(e => e.Losses)
                .HasPrecision(23)
                .HasColumnName("losses");
            entity.Property(e => e.TeamName)
                .HasMaxLength(52)
                .HasColumnName("team_name");
            entity.Property(e => e.TotalMatches).HasColumnName("total_matches");
            entity.Property(e => e.Wins)
                .HasPrecision(23)
                .HasColumnName("wins");
        });

        modelBuilder.Entity<Tournament>(entity =>
        {
            entity.HasKey(e => e.TournamentId).HasName("PRIMARY");

            entity.ToTable("tournament");

            entity.HasIndex(e => new { e.Name, e.Season }, "unique_name_season").IsUnique();

            entity.Property(e => e.TournamentId).HasColumnName("tournament_id");
            entity.Property(e => e.Name)
                .HasMaxLength(52)
                .HasColumnName("name");
            entity.Property(e => e.Season)
                .HasMaxLength(9)
                .HasColumnName("season");
        });

        modelBuilder.Entity<TournamentsTeam>(entity =>
        {
            entity.HasKey(e => new { e.TournamentId, e.TeamId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("tournaments_teams");

            entity.HasIndex(e => e.TeamId, "team_id");

            entity.Property(e => e.TournamentId).HasColumnName("tournament_id");
            entity.Property(e => e.TeamId).HasColumnName("team_id");
            entity.Property(e => e.TeamPoints).HasColumnName("team_points");

            entity.HasOne(d => d.Team).WithMany(p => p.TournamentsTeams)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("tournaments_teams_ibfk_2");

            entity.HasOne(d => d.Tournament).WithMany(p => p.TournamentsTeams)
                .HasForeignKey(d => d.TournamentId)
                .HasConstraintName("tournaments_teams_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
