import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../services/api.service';
import { MatchDto } from '../../models/dtos';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

@Component({
  standalone: true,
  selector: 'app-matches',
  imports: [CommonModule, MatTableModule, MatButtonModule, MatPaginatorModule, MatSortModule, MatSnackBarModule],
  template: `
  <h1>All Matches</h1>
  <div *ngIf="!dataSource" class="loading">Loading matches...</div>
  <table mat-table [dataSource]="dataSource" matSort class="mat-elevation-z4 full-width" *ngIf="dataSource">
    <ng-container matColumnDef="matchDate">
      <th mat-header-cell *matHeaderCellDef mat-sort-header>Match Date</th>
      <td mat-cell *matCellDef="let m">{{ m.matchDate | date:'shortDate' }}</td>
    </ng-container>
    <ng-container matColumnDef="playerStatCount">
      <th mat-header-cell *matHeaderCellDef mat-sort-header>Player Stats Uploaded</th>
      <td mat-cell *matCellDef="let m">{{ m.playerStatCount }}</td>
    </ng-container>
    <ng-container matColumnDef="actions">
      <th mat-header-cell *matHeaderCellDef>Actions</th>
      <td mat-cell *matCellDef="let m"><button mat-raised-button color="warn" (click)="delete(m.id)">Delete</button></td>
    </ng-container>
    <tr mat-header-row *matHeaderRowDef="displayedColumns; sticky: true"></tr>
    <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>
  </table>
  <mat-paginator *ngIf="dataSource" [pageSize]="20" [pageSizeOptions]="[10,20,50,100]"></mat-paginator>
  `,
  styles:[`.full-width{width:100%}`]
})
export class MatchesComponent implements OnInit {
  dataSource: MatTableDataSource<MatchDto> | null = null;
  displayedColumns = ['matchDate','playerStatCount','actions'];
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  constructor(private api: ApiService, private snackBar: MatSnackBar) {}
  ngOnInit() { this.load(); }
  load() { this.api.getMatches().subscribe(x => { this.dataSource = new MatTableDataSource(x); queueMicrotask(()=>{ if(this.dataSource){ this.dataSource.paginator = this.paginator; this.dataSource.sort = this.sort; } }); }); }
  delete(id: string) {
    if (!confirm('Are you sure you want to delete this match? This will remove all associated player stats.')) return;
    this.api.deleteMatch(id).subscribe({
      next: () => { this.snackBar.open('Match deleted', 'OK', { duration: 3000 }); this.load(); },
      error: () => { this.snackBar.open('Failed to delete match', 'Dismiss', { duration: 4000 }); }
    });
  }
}
