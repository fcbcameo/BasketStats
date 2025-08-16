import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../services/api.service';
import { TeamSeasonStatsDto, CompetitionDto } from '../../models/dtos';

@Component({
  standalone: true,
  selector: 'app-team-stats',
  imports: [CommonModule],
  template: `
  <h1>Team Season Stats</h1>
  <div *ngIf="!competitions" class="loading">Loading competitions...</div>
  <div *ngIf="competitions">
    <div class="stats-card">
      <h3>Filter by Competition</h3>
      <label class="form-label">Select Competition:</label>
      <select class="form-select" (change)="onCompetitionChanged($event)">
        <option value="">All Competitions (Aggregated)</option>
        <option *ngFor="let c of competitions" [value]="c.id" [selected]="selectedCompetitionId===c.id">{{c.name}}</option>
      </select>
      <p class="text-info" *ngIf="selectedCompetitionId">Showing stats for: <strong>{{ getSelectedCompetitionName() }}</strong></p>
      <p class="text-info" *ngIf="!selectedCompetitionId">Showing aggregated stats for <strong>all competitions</strong></p>
    </div>

    <div *ngIf="!stats" class="loading">Loading team stats...</div>
    <div *ngIf="stats">
      <div class="stats-card">
        <h3>Team Overview</h3>
        <div class="stat-item"><span class="stat-label">Games Played</span><span class="stat-value">{{stats.gamesPlayed}}</span></div>
        <div class="stat-item"><span class="stat-label">Total Points</span><span class="stat-value">{{stats.totalPoints}}</span></div>
        <div class="stat-item"><span class="stat-label">Points Per Game</span><span class="stat-value">{{stats.pointsPerGame | number:'1.1-1'}}</span></div>
        <div class="stat-item"><span class="stat-label">Total Assists</span><span class="stat-value">{{stats.totalAssists}}</span></div>
        <div class="stat-item"><span class="stat-label">Assists Per Game</span><span class="stat-value">{{stats.assistsPerGame | number:'1.1-1'}}</span></div>
        <div class="stat-item"><span class="stat-label">Total Rebounds</span><span class="stat-value">{{stats.totalRebounds}}</span></div>
        <div class="stat-item"><span class="stat-label">Rebounds Per Game</span><span class="stat-value">{{stats.reboundsPerGame | number:'1.1-1'}}</span></div>
      </div>

      <div class="stats-card">
        <h3>Shooting Performance</h3>
        <div class="stat-item"><span class="stat-label">Field Goal %</span><span class="stat-value">{{stats.fieldGoalPercentage | number:'1.1-1'}}%</span></div>
        <div class="stat-item"><span class="stat-label">Field Goals</span><span class="stat-value">{{stats.totalFieldGoalsMade}} / {{stats.totalFieldGoalsAttempted}}</span></div>
        <div class="stat-item"><span class="stat-label">3-Point %</span><span class="stat-value">{{stats.threePointPercentage | number:'1.1-1'}}%</span></div>
        <div class="stat-item"><span class="stat-label">3-Pointers</span><span class="stat-value">{{stats.totalThreePointersMade}} / {{stats.totalThreePointersAttempted}}</span></div>
        <div class="stat-item"><span class="stat-label">2-Point %</span><span class="stat-value">{{stats.twoPointPercentage | number:'1.1-1'}}%</span></div>
        <div class="stat-item"><span class="stat-label">2-Pointers</span><span class="stat-value">{{stats.totalTwoPointersMade}} / {{stats.totalTwoPointersAttempted}}</span></div>
        <div class="stat-item"><span class="stat-label">Free Throw %</span><span class="stat-value">{{stats.freeThrowPercentage | number:'1.1-1'}}%</span></div>
        <div class="stat-item"><span class="stat-label">Free Throws</span><span class="stat-value">{{stats.totalFreeThrowsMade}} / {{stats.totalFreeThrowsAttempted}}</span></div>
      </div>

      <div class="stats-card">
        <h3>Rebounding & Defense</h3>
        <div class="stat-item"><span class="stat-label">Offensive Rebounds</span><span class="stat-value">{{stats.totalOffensiveRebounds}} ({{stats.offensiveReboundsPerGame | number:'1.1-1'}}/game)</span></div>
        <div class="stat-item"><span class="stat-label">Defensive Rebounds</span><span class="stat-value">{{stats.totalDefensiveRebounds}} ({{stats.defensiveReboundsPerGame | number:'1.1-1'}}/game)</span></div>
        <div class="stat-item"><span class="stat-label">Steals</span><span class="stat-value">{{stats.totalSteals}} ({{stats.stealsPerGame | number:'1.1-1'}}/game)</span></div>
        <div class="stat-item"><span class="stat-label">Blocks</span><span class="stat-value">{{stats.totalBlocks}} ({{stats.blocksPerGame | number:'1.1-1'}}/game)</span></div>
      </div>

      <div class="stats-card">
        <h3>Other Statistics</h3>
        <div class="stat-item"><span class="stat-label">Turnovers</span><span class="stat-value">{{stats.totalTurnovers}} ({{stats.turnoversPerGame | number:'1.1-1'}}/game)</span></div>
        <div class="stat-item"><span class="stat-label">Personal Fouls</span><span class="stat-value">{{stats.totalPersonalFouls}}</span></div>
        <div class="stat-item"><span class="stat-label">Total Minutes</span><span class="stat-value">{{stats.totalMinutes}}</span></div>
      </div>
    </div>
  </div>
  `
})
export class TeamStatsComponent implements OnInit {
  stats: TeamSeasonStatsDto | null = null;
  competitions: CompetitionDto[] | null = null;
  selectedCompetitionId: string | null = null;
  constructor(private api: ApiService) {}
  ngOnInit() {
    this.api.getCompetitions().subscribe((c: CompetitionDto[]) => this.competitions = c);
    this.load();
  }
  onCompetitionChanged(e: Event) {
    const v = (e.target as HTMLSelectElement).value;
    this.selectedCompetitionId = v || null;
    this.load();
  }
  load() {
    this.stats = null;
    this.api.getTeamStats(this.selectedCompetitionId ?? undefined).subscribe((s: TeamSeasonStatsDto) => this.stats = s);
  }
  getSelectedCompetitionName() {
    const c = this.competitions?.find((x: CompetitionDto) => x.id === this.selectedCompetitionId);
    return c?.name ?? 'All Competitions';
  }
}
