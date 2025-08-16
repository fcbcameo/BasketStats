import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../services/api.service';
import { CompetitionDto, PlayerSeasonStatsDto } from '../../models/dtos';
import { MatSelectModule } from '@angular/material/select';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { ActivatedRoute } from '@angular/router';

@Component({
  standalone: true,
  selector: 'app-player-stats-detail',
  imports: [CommonModule, MatSelectModule, MatCardModule, MatFormFieldModule],
  template: `
  <div *ngIf="!competitions" class="loading">Loading competitions...</div>
  <div *ngIf="competitions">
    <mat-card class="stats-card">
      <h3>Filter by Competition</h3>
      <mat-form-field appearance="fill" style="width:300px">
        <mat-label>Competition</mat-label>
        <mat-select (selectionChange)="onCompetitionChanged($event.value)">
          <mat-option [value]="null">All Competitions (Aggregated)</mat-option>
          <mat-option *ngFor="let c of competitions" [value]="c.id" [selected]="selectedCompetitionId===c.id">{{c.name}}</mat-option>
        </mat-select>
      </mat-form-field>
      <p class="text-info" *ngIf="selectedCompetitionId">Showing stats for: <strong>{{ getSelectedCompetitionName() }}</strong></p>
      <p class="text-info" *ngIf="!selectedCompetitionId">Showing aggregated stats for <strong>all competitions</strong></p>
    </mat-card>

    <div *ngIf="!stats" class="loading">Loading player stats...</div>
    <div *ngIf="stats">
      <h1>{{stats.playerName}}</h1>
      <mat-card class="stats-card">
        <h3>Overview</h3>
        <div class="stat-item"><span class="stat-label">Games Played</span><span class="stat-value">{{stats.gamesPlayed}}</span></div>
        <div class="stat-item"><span class="stat-label">Total Points</span><span class="stat-value">{{stats.totalPoints}}</span></div>
        <div class="stat-item"><span class="stat-label">Points Per Game</span><span class="stat-value">{{stats.pointsPerGame | number:'1.1-1'}}</span></div>
        <div class="stat-item"><span class="stat-label">Total Rebounds</span><span class="stat-value">{{stats.totalRebounds}}</span></div>
        <div class="stat-item"><span class="stat-label">Rebounds Per Game</span><span class="stat-value">{{stats.reboundsPerGame | number:'1.1-1'}}</span></div>
      </mat-card>
    </div>
  </div>
  `
})
export class PlayerStatsDetailComponent implements OnInit {
  playerId!: string;
  stats: PlayerSeasonStatsDto | null = null;
  competitions: CompetitionDto[] | null = null;
  selectedCompetitionId: string | null = null;

  constructor(private api: ApiService, private route: ActivatedRoute) {}

  ngOnInit() {
    this.playerId = this.route.snapshot.paramMap.get('playerId') as string;
    this.api.getCompetitions().subscribe(c => this.competitions = c);
    this.loadStats();
  }

  onCompetitionChanged(value: string | null) { this.selectedCompetitionId = value || null; this.loadStats(); }

  loadStats() { this.stats = null; this.api.getPlayerStats(this.playerId, this.selectedCompetitionId ?? undefined).subscribe(s => this.stats = s); }

  getSelectedCompetitionName() { const c = this.competitions?.find(x => x.id === this.selectedCompetitionId); return c?.name ?? 'All Competitions'; }
}
