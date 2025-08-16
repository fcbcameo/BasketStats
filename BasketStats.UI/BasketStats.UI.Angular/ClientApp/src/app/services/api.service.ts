import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { CompetitionDto, MatchDto, PlayerDto, PlayerSeasonStatsDto, TeamSeasonStatsDto } from '../models/dtos';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ApiService {
  private baseUrl = environment.apiBaseUrl;
  constructor(private http: HttpClient) {}

  getPlayers(): Observable<PlayerDto[]> {
    return this.http.get<PlayerDto[]>(this.baseUrl + 'api/players');
  }

  getPlayerStats(playerId: string, competitionId?: string): Observable<PlayerSeasonStatsDto> {
    let params = new HttpParams();
    if (competitionId) params = params.set('competitionId', competitionId);
    return this.http.get<PlayerSeasonStatsDto>(`${this.baseUrl}api/players/${playerId}/stats`, { params });
  }

  getAllPlayersStats(competitionId?: string): Observable<PlayerSeasonStatsDto[]> {
    let url = this.baseUrl + 'api/players/stats';
    if (competitionId) url += `?competitionId=${competitionId}`;
    return this.http.get<PlayerSeasonStatsDto[]>(url);
  }

  getTeamStats(competitionId?: string): Observable<TeamSeasonStatsDto> {
    let url = this.baseUrl + 'api/team/stats';
    if (competitionId) url += `?competitionId=${competitionId}`;
    return this.http.get<TeamSeasonStatsDto>(url);
  }

  getCompetitions(): Observable<CompetitionDto[]> {
    return this.http.get<CompetitionDto[]>(this.baseUrl + 'api/competitions');
  }

  createCompetition(payload: { name: string }): Observable<any> {
    return this.http.post(this.baseUrl + 'api/competitions', payload);
  }

  uploadMatchStats(competitionId: string, file: File) {
    const form = new FormData();
    form.append('file', file, file.name);
    return this.http.post(`${this.baseUrl}api/competitions/${competitionId}/matches`, form);
  }

  getMatches(): Observable<MatchDto[]> {
    return this.http.get<MatchDto[]>(this.baseUrl + 'api/matches');
  }

  deleteMatch(matchId: string) {
    return this.http.delete(`${this.baseUrl}api/matches/${matchId}`);
  }
}
