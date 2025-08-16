import { bootstrapApplication } from '@angular/platform-browser';
import { provideHttpClient } from '@angular/common/http';
import { Routes, provideRouter, withInMemoryScrolling } from '@angular/router';
import { provideAnimations } from '@angular/platform-browser/animations';
import { AppComponent } from './app/app.component';
import { HomeComponent } from './app/pages/home/home.component';
import { PlayersComponent } from './app/pages/players/players.component';
import { PlayerStatsDetailComponent } from './app/pages/player-stats-detail/player-stats-detail.component';
import { PlayerStatsOverviewComponent } from './app/pages/player-stats-overview/player-stats-overview.component';
import { TeamStatsComponent } from './app/pages/team-stats/team-stats.component';
import { CompetitionsComponent } from './app/pages/competitions/competitions.component';
import { UploadStatsComponent } from './app/pages/upload-stats/upload-stats.component';
import { MatchesComponent } from './app/pages/matches/matches.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'players', component: PlayersComponent },
  { path: 'players/:playerId/stats', component: PlayerStatsDetailComponent },
  { path: 'player-stats-overview', component: PlayerStatsOverviewComponent },
  { path: 'team-stats', component: TeamStatsComponent },
  { path: 'competitions', component: CompetitionsComponent },
  { path: 'competitions/:competitionId/upload', component: UploadStatsComponent },
  { path: 'matches', component: MatchesComponent },
];

bootstrapApplication(AppComponent, {
  providers: [
    provideRouter(routes, withInMemoryScrolling({ anchorScrolling: 'enabled' })),
    provideHttpClient(),
    provideAnimations(),
  ]
});
