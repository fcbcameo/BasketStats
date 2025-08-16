import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { ApiService } from '../../services/api.service';
import { PlayerDto } from '../../models/dtos';

@Component({
  standalone: true,
  selector: 'app-players',
  imports: [CommonModule, RouterModule, MatTableModule, MatButtonModule, MatPaginatorModule, MatSortModule],
  template: `
  <h1>All Players</h1>
  <div class="stats-card" *ngIf="!dataSource"><div class="loading">Loading players...</div></div>
  <table mat-table [dataSource]="dataSource" matSort class="mat-elevation-z4 full-width" *ngIf="dataSource">
    <ng-container matColumnDef="name">
      <th mat-header-cell *matHeaderCellDef mat-sort-header> Player </th>
      <td mat-cell *matCellDef="let p"> {{p.name}} </td>
    </ng-container>
    <ng-container matColumnDef="actions">
      <th mat-header-cell *matHeaderCellDef></th>
      <td mat-cell *matCellDef="let p">
        <a mat-raised-button color="primary" [routerLink]="['/players', p.id, 'stats']">View Stats</a>
      </td>
    </ng-container>
    <tr mat-header-row *matHeaderRowDef="displayedColumns; sticky: true"></tr>
    <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>
  </table>
  <mat-paginator *ngIf="dataSource" [pageSize]="20" [pageSizeOptions]="[10,20,50,100]"></mat-paginator>
  `,
  styles:[`.full-width{width:100%}`]
})
export class PlayersComponent implements OnInit {
  dataSource: MatTableDataSource<PlayerDto> | null = null;
  displayedColumns = ['name','actions'];
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  constructor(private api: ApiService) {}
  ngOnInit() { this.api.getPlayers().subscribe(p => { this.dataSource = new MatTableDataSource(p); queueMicrotask(() => { if(this.dataSource){ this.dataSource.paginator = this.paginator; this.dataSource.sort = this.sort; } }); }); }
}
