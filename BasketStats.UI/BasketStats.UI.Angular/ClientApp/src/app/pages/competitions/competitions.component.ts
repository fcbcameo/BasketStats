import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../services/api.service';
import { CompetitionDto } from '../../models/dtos';
import { FormsModule } from '@angular/forms';

@Component({
  standalone: true,
  selector: 'app-competitions',
  imports: [CommonModule, FormsModule],
  template: `
  <h1>Competitions</h1>
  <div class="stats-card">
    <h3>Create a New Competition</h3>
    <div style="display:flex; gap:8px; align-items:center">
      <input class="form-control" [(ngModel)]="name" placeholder="Competition Name" />
      <button class="btn btn-primary" (click)="create()">Create</button>
    </div>
    <p class="text-info" *ngIf="error">{{error}}</p>
  </div>

  <div class="stats-card">
    <h3>All Competitions</h3>
    <ul>
      <li *ngFor="let c of competitions">{{c.name}}</li>
    </ul>
  </div>
  `
})
export class CompetitionsComponent implements OnInit {
  competitions: CompetitionDto[] = [];
  name = '';
  error: string | null = null;
  constructor(private api: ApiService) {}
  ngOnInit() {
    this.load();
  }
  load() {
    this.api.getCompetitions().subscribe((x: CompetitionDto[]) => this.competitions = x);
  }
  create() {
    this.error = null;
    if (!this.name.trim()) { this.error = 'Name is required'; return; }
    this.api.createCompetition({ name: this.name.trim() }).subscribe({
      next: () => { this.name=''; this.load(); },
      error: (e: any) => { this.error = 'Failed to create'; }
    });
  }
}
