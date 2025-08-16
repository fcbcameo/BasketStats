import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../services/api.service';
import { MatchDto } from '../../models/dtos';

@Component({
  standalone: true,
  selector: 'app-matches',
  imports: [CommonModule],
  template: `
  <h1>All Matches</h1>
  <div *ngIf="!matches" class="loading">Loading matches...</div>
  <table class="table" *ngIf="matches">
    <thead><tr><th>Match Date</th><th>Player Stats Uploaded</th><th>Actions</th></tr></thead>
    <tbody>
      <tr *ngFor="let m of matches">
        <td>{{ m.matchDate | date:'shortDate' }}</td>
        <td>{{ m.playerStatCount }}</td>
        <td><button class="btn btn-danger" (click)="delete(m.id)">Delete</button></td>
      </tr>
    </tbody>
  </table>
  `
})
export class MatchesComponent implements OnInit {
  matches: MatchDto[] | null = null;
  constructor(private api: ApiService) {}
  ngOnInit() { this.load(); }
  load() { this.api.getMatches().subscribe((x: MatchDto[]) => this.matches = x); }
  delete(id: string) {
    if (!confirm('Are you sure you want to delete this match?')) return;
    this.api.deleteMatch(id).subscribe(() => this.load());
  }
}
