import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ApiService } from '../../services/api.service';
import { PlayerDto } from '../../models/dtos';

@Component({
  standalone: true,
  selector: 'app-players',
  imports: [CommonModule, RouterModule],
  template: `
  <h1>All Players</h1>
  <div class="stats-card" *ngIf="!players"><div class="loading">Loading players...</div></div>
  <table class="table" *ngIf="players">
    <tbody>
      <tr *ngFor="let p of players">
        <td>{{p.name}}</td>
        <td><a class="btn btn-primary" [routerLink]="['/players', p.id, 'stats']">View Stats</a></td>
      </tr>
    </tbody>
  </table>
  `
})
export class PlayersComponent implements OnInit {
  players: PlayerDto[] | null = null;
  constructor(private api: ApiService) {}
  ngOnInit() {
    this.api.getPlayers().subscribe((p: PlayerDto[]) => this.players = p);
  }
}
