import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../services/api.service';
import { CompetitionDto } from '../../models/dtos';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';

@Component({
  standalone: true,
  selector: 'app-competitions',
  imports: [CommonModule, FormsModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatCardModule],
  template: `
  <h1>Competitions</h1>
  <mat-card class="stats-card">
    <h3>Create a New Competition</h3>
    <div style="display:flex; gap:8px; align-items:center">
      <mat-form-field appearance="fill" style="flex:1">
        <mat-label>Competition Name</mat-label>
        <input matInput [(ngModel)]="name" />
      </mat-form-field>
      <button mat-raised-button color="primary" (click)="create()">Create</button>
    </div>
    <p class="text-info" *ngIf="error">{{error}}</p>
  </mat-card>

  <mat-card class="stats-card">
    <h3>All Competitions</h3>
    <ul>
      <li *ngFor="let c of competitions">{{c.name}}</li>
    </ul>
  </mat-card>
  `
})
export class CompetitionsComponent implements OnInit {
  competitions: CompetitionDto[] = [];
  name = '';
  error: string | null = null;
  constructor(private api: ApiService) {}
  ngOnInit() { this.load(); }
  load() { this.api.getCompetitions().subscribe(x => this.competitions = x); }
  create() {
    this.error = null;
    if (!this.name.trim()) { this.error = 'Name is required'; return; }
    this.api.createCompetition({ name: this.name.trim() }).subscribe({
      next: () => { this.name=''; this.load(); },
      error: () => { this.error = 'Failed to create'; }
    });
  }
}
