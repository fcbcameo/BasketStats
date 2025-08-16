import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { ApiService } from '../../services/api.service';
import { CompetitionDto, PlayerSeasonStatsDto } from '../../models/dtos';

@Component({
  standalone: true,
  selector: 'app-player-stats-detail',
  imports: [CommonModule],
  template: `
  <div *ngIf="!competitions" class="loading">Loading competitions...</div>
  <div *ngIf="competitions">
    <div class="stats-card">
      <h3>Filter by Competition</h3>
      <div>
        <label class="form-label">Select Competition:</label>
        <select class="form-select" (change)="onCompetitionChanged($event)">
          <option value="">All Competitions (Aggregated)</option>
          <option *ngFor="let c of competitions" [value]="c.id" [selected]="selectedCompetitionId===c.id">{{c.name}}</option>
        </select>
        <p class="text-info" *ngIf="selectedCompetitionId">Showing stats for: <strong>{{ getSelectedCompetitionName() }}</strong></p>
        <p class="text-info" *ngIf="!selectedCompetitionId">Showing aggregated stats for <strong>all competitions</strong></p>
      </div>
    </div>

    <div *ngIf="!stats" class="loading">Loading player stats...</div>
    <div *ngIf="stats">
      <h1>{{stats.playerName}}</h1>
      <div class="stats-card">
        <h3>Overview</h3>
        <div class="stat-item"><span class="stat-label">Games Played</span><span class="stat-value">{{stats.gamesPlayed}}</span></div>
        <div class="stat-item"><span class="stat-label">Total Points</span><span class="stat-value">{{stats.totalPoints}}</span></div>
        <div class="stat-item"><span class="stat-label">Points Per Game</span><span class="stat-value">{{stats.pointsPerGame | number:'1.1-1'}}</span></div>
        <div class="stat-item"><span class="stat-label">Total Rebounds</span><span class="stat-value">{{stats.totalRebounds}}</span></div>
        <div class="stat-item"><span class="stat-label">Rebounds Per Game</span><span class="stat-value">{{stats.reboundsPerGame | number:'1.1-1'}}</span></div>
      </div>
    </div>
  </div>
  `
})
export class PlayerStatsDetailComponent implements OnInit {
  playerId!: string;
  stats: PlayerSeasonStatsDto | null = null;
  competitions: CompetitionDto[] | null = null;
  selectedCompetitionId: string | null = null;

  constructor(private route: ActivatedRoute, private api: ApiService) {}

  ngOnInit() {
    this.playerId = this.route.snapshot.paramMap.get('playerId')!;
    this.api.getCompetitions().subscribe((c: CompetitionDto[]) => this.competitions = c);
    this.loadStats();
  }

  onCompetitionChanged(e: Event) {
    const value = (e.target as HTMLSelectElement).value;
    this.selectedCompetitionId = value || null;
    this.loadStats();
  }

  loadStats() {
    this.stats = null;
    this.api.getPlayerStats(this.playerId, this.selectedCompetitionId ?? undefined)
      .subscribe((s: PlayerSeasonStatsDto) => this.stats = s);
  }

  getSelectedCompetitionName() {
    const c = this.competitions?.find((x: CompetitionDto) => x.id === this.selectedCompetitionId);
    return c?.name ?? 'Unknown Competition';
  }
}
