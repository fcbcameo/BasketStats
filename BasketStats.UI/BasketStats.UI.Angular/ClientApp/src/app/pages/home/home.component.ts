import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';

@Component({
  standalone: true,
  selector: 'app-home',
  imports: [CommonModule],
  template: `
  <div class="stats-card">
    <h1>Welcome to BasketStats</h1>
    <p class="text-info">Analyze players, teams, and matches with an NBA.com-inspired UI.</p>
  </div>
  `
})
export class HomeComponent {}
