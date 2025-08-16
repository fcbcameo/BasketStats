import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../services/api.service';
import { PlayerSeasonStatsDto, CompetitionDto } from '../../models/dtos';
import { PdfService } from '../../services/pdf.service';

@Component({
  standalone: true,
  selector: 'app-player-stats-overview',
  imports: [CommonModule],
  template: `
  <div class="stats-card">
    <h1>Player Statistics Overview</h1>
    <div class="competition-selector">
      <h4>Competition</h4>
      <select class="form-select" (change)="onCompetitionChanged($event)">
        <option value="">All Competitions (Aggregated)</option>
        <option *ngFor="let c of competitions" [value]="c.id" [selected]="selectedCompetitionId===c.id">{{c.name}}</option>
      </select>
    </div>

    <div *ngIf="!players" class="loading">Loading player statistics...</div>
    <div *ngIf="players">
      <div class="stats-card">
        <div style="display:flex; justify-content:flex-end; margin-bottom:8px">
          <button class="btn btn-primary" (click)="exportPdf()">Export to PDF</button>
        </div>
        <div class="table-responsive">
          <table class="table">
            <thead>
              <tr>
                <th>Player</th><th>GP</th><th>PPG</th><th>RPG</th><th>APG</th><th>FG%</th><th>3P%</th><th>FT%</th>
                <th>SPG</th><th>BPG</th><th>TOV</th><th>MPG</th><th>EFF</th><th>Total PTS</th><th>Total REB</th><th>Total AST</th><th>Total STL</th><th>Total BLK</th>
              </tr>
            </thead>
            <tbody>
              <tr *ngFor="let p of players">
                <td><strong style="color: var(--nba-accent-orange)">{{p.playerName}}</strong></td>
                <td>{{p.gamesPlayed}}</td>
                <td>{{p.pointsPerGame | number:'1.1-1'}}</td>
                <td>{{p.reboundsPerGame | number:'1.1-1'}}</td>
                <td>{{p.assistsPerGame | number:'1.1-1'}}</td>
                <td>{{p.fieldGoalPercentage | number:'1.1-1'}}</td>
                <td>{{p.threePointPercentage | number:'1.1-1'}}</td>
                <td>{{p.freeThrowPercentage | number:'1.1-1'}}</td>
                <td>{{p.stealsPerGame | number:'1.1-1'}}</td>
                <td>{{p.blocksPerGame | number:'1.1-1'}}</td>
                <td>{{p.turnoversPerGame | number:'1.1-1'}}</td>
                <td>{{p.minutesPerGame | number:'1.1-1'}}</td>
                <td>{{p.efficiency | number:'1.1-1'}}</td>
                <td>{{p.totalPoints}}</td>
                <td>{{p.totalRebounds}}</td>
                <td>{{p.totalAssists}}</td>
                <td>{{p.totalSteals}}</td>
                <td>{{p.totalBlocks}}</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
  `
})
export class PlayerStatsOverviewComponent implements OnInit {
  players: PlayerSeasonStatsDto[] | null = null;
  competitions: CompetitionDto[] = [];
  selectedCompetitionId: string | null = null;
  constructor(private api: ApiService, private pdf: PdfService) {}

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
    this.players = null;
    this.api.getAllPlayersStats(this.selectedCompetitionId ?? undefined)
      .subscribe((ps: PlayerSeasonStatsDto[]) => this.players = ps);
  }

  exportPdf() {
    if (!this.players || this.players.length === 0) return;
    const compName = this.selectedCompetitionId ? (this.competitions.find((c: CompetitionDto) => c.id === this.selectedCompetitionId)?.name ?? '') : 'All Competitions';
    this.pdf.generatePlayerStatsReport(this.players, compName);
  }
}
