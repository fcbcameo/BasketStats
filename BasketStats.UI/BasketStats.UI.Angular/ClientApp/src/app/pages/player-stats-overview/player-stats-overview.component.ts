import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../services/api.service';
import { CompetitionDto, PlayerSeasonStatsDto } from '../../models/dtos';
import { PdfService } from '../../services/pdf.service';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';

@Component({
  standalone: true,
  selector: 'app-player-stats-overview',
  imports: [CommonModule, MatTableModule, MatButtonModule, MatSelectModule, MatCardModule, MatFormFieldModule, MatPaginatorModule, MatSortModule],
  template: `
  <mat-card class="stats-card">
    <h1>Player Statistics Overview</h1>
    <div class="competition-selector" style="margin-bottom: 16px;">
      <h4>Competition</h4>
      <mat-form-field appearance="fill" style="width:300px">
        <mat-label>Competition</mat-label>
        <mat-select (selectionChange)="onCompetitionChanged($event.value)">
          <mat-option [value]="null">All Competitions (Aggregated)</mat-option>
          <mat-option *ngFor="let c of competitions" [value]="c.id">{{c.name}}</mat-option>
        </mat-select>
      </mat-form-field>
    </div>

    <div *ngIf="!dataSource" class="loading">Loading player statistics...</div>
    <div *ngIf="dataSource">
      <div style="display:flex; gap:8px; justify-content:flex-end; margin-bottom:8px">
        <button mat-raised-button color="primary" (click)="exportPdf()">Export to PDF</button>
        <button mat-raised-button color="accent" (click)="exportPdfAdvanced()">Advanced Stats export to PDF</button>
      </div>
      <table mat-table [dataSource]="dataSource" matSort class="mat-elevation-z4 full-width">
        <ng-container matColumnDef="playerName"><th mat-header-cell *matHeaderCellDef mat-sort-header>Player</th><td mat-cell *matCellDef="let p"><strong style="color: var(--nba-accent-orange)">{{p.playerName}}</strong></td></ng-container>
        <ng-container matColumnDef="gamesPlayed"><th mat-header-cell *matHeaderCellDef mat-sort-header>GP</th><td mat-cell *matCellDef="let p">{{p.gamesPlayed}}</td></ng-container>
        <ng-container matColumnDef="pointsPerGame"><th mat-header-cell *matHeaderCellDef mat-sort-header>PPG</th><td mat-cell *matCellDef="let p">{{p.pointsPerGame | number:'1.1-1'}}</td></ng-container>
        <ng-container matColumnDef="reboundsPerGame"><th mat-header-cell *matHeaderCellDef mat-sort-header>RPG</th><td mat-cell *matCellDef="let p">{{p.reboundsPerGame | number:'1.1-1'}}</td></ng-container>
        <ng-container matColumnDef="assistsPerGame"><th mat-header-cell *matHeaderCellDef mat-sort-header>APG</th><td mat-cell *matCellDef="let p">{{p.assistsPerGame | number:'1.1-1'}}</td></ng-container>
        <ng-container matColumnDef="fieldGoalPercentage"><th mat-header-cell *matHeaderCellDef mat-sort-header>FG%</th><td mat-cell *matCellDef="let p">{{p.fieldGoalPercentage | number:'1.1-1'}}</td></ng-container>
        <ng-container matColumnDef="threePointPercentage"><th mat-header-cell *matHeaderCellDef mat-sort-header>3P%</th><td mat-cell *matCellDef="let p">{{p.threePointPercentage | number:'1.1-1'}}</td></ng-container>
        <ng-container matColumnDef="freeThrowPercentage"><th mat-header-cell *matHeaderCellDef mat-sort-header>FT%</th><td mat-cell *matCellDef="let p">{{p.freeThrowPercentage | number:'1.1-1'}}</td></ng-container>
        <ng-container matColumnDef="stealsPerGame"><th mat-header-cell *matHeaderCellDef mat-sort-header>SPG</th><td mat-cell *matCellDef="let p">{{p.stealsPerGame | number:'1.1-1'}}</td></ng-container>
        <ng-container matColumnDef="blocksPerGame"><th mat-header-cell *matHeaderCellDef mat-sort-header>BPG</th><td mat-cell *matCellDef="let p">{{p.blocksPerGame | number:'1.1-1'}}</td></ng-container>
        <ng-container matColumnDef="turnoversPerGame"><th mat-header-cell *matHeaderCellDef mat-sort-header>TOV</th><td mat-cell *matCellDef="let p">{{p.turnoversPerGame | number:'1.1-1'}}</td></ng-container>
        <ng-container matColumnDef="minutesPerGame"><th mat-header-cell *matHeaderCellDef mat-sort-header>MPG</th><td mat-cell *matCellDef="let p">{{p.minutesPerGame | number:'1.1-1'}}</td></ng-container>
        <ng-container matColumnDef="efficiency"><th mat-header-cell *matHeaderCellDef mat-sort-header>EFF</th><td mat-cell *matCellDef="let p">{{p.efficiency | number:'1.1-1'}}</td></ng-container>
        <ng-container matColumnDef="totalPoints"><th mat-header-cell *matHeaderCellDef mat-sort-header>Total PTS</th><td mat-cell *matCellDef="let p">{{p.totalPoints}}</td></ng-container>
        <ng-container matColumnDef="totalRebounds"><th mat-header-cell *matHeaderCellDef mat-sort-header>Total REB</th><td mat-cell *matCellDef="let p">{{p.totalRebounds}}</td></ng-container>
        <ng-container matColumnDef="totalAssists"><th mat-header-cell *matHeaderCellDef mat-sort-header>Total AST</th><td mat-cell *matCellDef="let p">{{p.totalAssists}}</td></ng-container>
        <ng-container matColumnDef="totalSteals"><th mat-header-cell *matHeaderCellDef mat-sort-header>Total STL</th><td mat-cell *matCellDef="let p">{{p.totalSteals}}</td></ng-container>
        <ng-container matColumnDef="totalBlocks"><th mat-header-cell *matHeaderCellDef mat-sort-header>Total BLK</th><td mat-cell *matCellDef="let p">{{p.totalBlocks}}</td></ng-container>
        <tr mat-header-row *matHeaderRowDef="displayedColumns; sticky: true"></tr>
        <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>
      </table>
      <mat-paginator [pageSize]="20" [pageSizeOptions]="[10,20,50,100]"></mat-paginator>
    </div>
  </mat-card>
  `
})
export class PlayerStatsOverviewComponent implements OnInit {
  dataSource: MatTableDataSource<PlayerSeasonStatsDto> | null = null;
  competitions: CompetitionDto[] = [];
  selectedCompetitionId: string | null = null;
  displayedColumns = ['playerName','gamesPlayed','pointsPerGame','reboundsPerGame','assistsPerGame','fieldGoalPercentage','threePointPercentage','freeThrowPercentage','stealsPerGame','blocksPerGame','turnoversPerGame','minutesPerGame','efficiency','totalPoints','totalRebounds','totalAssists','totalSteals','totalBlocks'];
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  constructor(private api: ApiService, private pdf: PdfService) {}
  ngOnInit() { this.api.getCompetitions().subscribe(c => this.competitions = c); this.load(); }
  onCompetitionChanged(v: string|null) { this.selectedCompetitionId = v || null; this.load(); }
  load() { this.dataSource = null; this.api.getAllPlayersStats(this.selectedCompetitionId ?? undefined).subscribe(ps => { this.dataSource = new MatTableDataSource(ps); queueMicrotask(()=>{ if(this.dataSource){ this.dataSource.paginator = this.paginator; this.dataSource.sort = this.sort; } }); }); }
  exportPdf() { const players = this.dataSource?.data ?? []; if (!players.length) return; const compName = this.selectedCompetitionId ? (this.competitions.find(c => c.id === this.selectedCompetitionId)?.name ?? '') : 'All Competitions'; this.pdf.generatePlayerStatsReport(players, compName); }
  exportPdfAdvanced() { const players = this.dataSource?.data ?? []; if (!players.length) return; const compName = this.selectedCompetitionId ? (this.competitions.find(c => c.id === this.selectedCompetitionId)?.name ?? '') : 'All Competitions'; this.pdf.generatePlayerStatsReportAdvanced(players, compName); }
}
