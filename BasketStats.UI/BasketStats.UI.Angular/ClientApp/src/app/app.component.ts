import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterModule, MatSidenavModule, MatToolbarModule, MatListModule, MatIconModule, MatButtonModule],
  template: `
  <mat-sidenav-container class="shell">
    <mat-sidenav mode="side" opened class="sidenav">
      <mat-toolbar color="primary" class="brand">
        <img src="/images/basketballstats.png" alt="BasketStats Logo" />
        <span>BasketStats</span>
      </mat-toolbar>
      <mat-nav-list>
        <a mat-list-item routerLink="/"><mat-icon>home</mat-icon><span>Home</span></a>
        <a mat-list-item routerLink="/players"><mat-icon>person</mat-icon><span>Players</span></a>
        <a mat-list-item routerLink="/player-stats-overview"><mat-icon>table_chart</mat-icon><span>Player Stats Overview</span></a>
        <a mat-list-item routerLink="/team-stats"><mat-icon>groups</mat-icon><span>Team Stats</span></a>
        <a mat-list-item routerLink="/competitions"><mat-icon>emoji_events</mat-icon><span>Competitions</span></a>
        <a mat-list-item routerLink="/matches"><mat-icon>event</mat-icon><span>Matches</span></a>
      </mat-nav-list>
    </mat-sidenav>

    <mat-sidenav-content>
      <mat-toolbar class="topbar" color="primary">
        <button mat-icon-button (click)="toggle()" class="hide-desktop"><mat-icon>menu</mat-icon></button>
        <span class="title">NBA Inspired Dashboard</span>
      </mat-toolbar>
      <div class="content"><router-outlet></router-outlet></div>
    </mat-sidenav-content>
  </mat-sidenav-container>
  `,
  styles: [
    `.shell { height: 100vh; }
     .sidenav { width: 300px; background: linear-gradient(180deg, #121317, #0f0f10); color: #fff; }
     .brand img { height: 40px; margin-right: 12px; filter: drop-shadow(0 2px 6px rgba(0,0,0,.35)); }
     .brand span { font-weight: 800; letter-spacing: .5px; text-transform: uppercase; }
     .topbar { box-shadow: 0 2px 12px rgba(0,0,0,.35); }
     .content { padding: 24px; }
     .title { font-weight: 700; letter-spacing: .5px; }
     a.mat-mdc-list-item { color: #cfd6e4; }
     a.mat-mdc-list-item:hover { background: rgba(255,255,255,.06); }
     .hide-desktop { display: none; }
     @media (max-width: 768px) { .hide-desktop { display:inline-flex } }
    `
  ]
})
export class AppComponent {
  toggle() { /* Could be wired to a responsive sidenav if desired */ }
}
