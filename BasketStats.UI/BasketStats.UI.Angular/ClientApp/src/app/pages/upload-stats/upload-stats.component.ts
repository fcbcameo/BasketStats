import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../services/api.service';
import { CompetitionDto } from '../../models/dtos';
import { FormsModule } from '@angular/forms';

@Component({
  standalone: true,
  selector: 'app-upload-stats',
  imports: [CommonModule, FormsModule],
  template: `
  <h1>Upload Stats</h1>
  <div class="stats-card">
    <label class="form-label">Competition</label>
    <select class="form-select" [(ngModel)]="competitionId">
      <option *ngFor="let c of competitions" [value]="c.id">{{c.name}}</option>
    </select>
  </div>

  <div class="stats-card">
    <label class="form-label">Select Stats CSV File:</label>
    <input type="file" (change)="onFileChange($event)" class="form-control" />
    <button class="btn btn-primary" [disabled]="!file || !competitionId || uploading" (click)="upload()">
      {{ uploading ? 'Uploading...' : 'Upload File' }}
    </button>
    <p class="text-info" *ngIf="message">{{message}}</p>
  </div>
  `
})
export class UploadStatsComponent implements OnInit {
  competitions: CompetitionDto[] = [];
  competitionId: string | null = null;
  file: File | null = null;
  uploading = false;
  message: string | null = null;

  constructor(private api: ApiService) {}

  ngOnInit() { this.api.getCompetitions().subscribe(c => this.competitions = c); }

  onFileChange(e: Event) {
    const input = e.target as HTMLInputElement;
    this.file = input.files && input.files.length ? input.files[0] : null;
    this.message = null;
  }

  upload() {
    if (!this.file || !this.competitionId) return;
    this.uploading = true;
    this.api.uploadMatchStats(this.competitionId, this.file).subscribe({
      next: () => { this.message = `File '${this.file!.name}' uploaded successfully!`; this.uploading = false; },
      error: (err) => { this.message = 'Upload failed'; this.uploading = false; }
    });
  }
}
