import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
  <div class="page">
    <aside class="sidebar">
      <div class="top-row navbar navbar-dark">
        <div class="container-fluid">
          <a class="navbar-brand" routerLink="/">
            <img src="/images/basketballstats.png" alt="BasketStats Logo" />
            BasketStats
          </a>
          <button class="navbar-toggler d-md-none" (click)="toggleSidebar()">
            <i class="bi bi-list"></i>
          </button>
        </div>
      </div>
      <nav class="nav-scrollable">
        <a routerLink="/" class="nav-link"><i class="bi bi-house-door-fill"></i><span>Home</span></a>
        <a routerLink="/players" class="nav-link"><i class="bi bi-person-lines-fill"></i><span>Players</span></a>
        <a routerLink="/player-stats-overview" class="nav-link"><i class="bi bi-table"></i><span>Player Stats Overview</span></a>
        <a routerLink="/team-stats" class="nav-link"><i class="bi bi-people-fill"></i><span>Team Stats</span></a>
        <a routerLink="/competitions" class="nav-link"><i class="bi bi-trophy-fill"></i><span>Competitions</span></a>
        <a routerLink="/matches" class="nav-link"><i class="bi bi-calendar-event-fill"></i><span>Matches</span></a>
      </nav>
    </aside>

    <main>
      <article class="content">
        <router-outlet></router-outlet>
      </article>
    </main>
  </div>
  `,
  styles: []
})
export class AppComponent {
  toggleSidebar() {
    document.querySelector('.sidebar')?.classList.toggle('open');
  }
}
