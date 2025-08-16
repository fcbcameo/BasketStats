export interface PlayerDto {
  id: string;
  name: string;
}

export interface PlayerSeasonStatsDto {
  playerId: string;
  playerName: string;
  gamesPlayed: number;
  pointsPerGame: number;
  reboundsPerGame: number;
  assistsPerGame: number;
  fieldGoalPercentage: number;
  threePointPercentage: number;
  freeThrowPercentage: number;
  stealsPerGame: number;
  blocksPerGame: number;
  turnoversPerGame: number;
  minutesPerGame: number;
  efficiency: number;
  totalPoints: number;
  totalRebounds: number;
  totalAssists: number;
  totalSteals: number;
  totalBlocks: number;
}

export interface TeamSeasonStatsDto {
  gamesPlayed: number;
  totalPoints: number;
  pointsPerGame: number;
  totalAssists: number;
  assistsPerGame: number;
  totalRebounds: number;
  reboundsPerGame: number;
  fieldGoalPercentage: number;
  totalFieldGoalsMade: number;
  totalFieldGoalsAttempted: number;
  threePointPercentage: number;
  totalThreePointersMade: number;
  totalThreePointersAttempted: number;
  twoPointPercentage: number;
  totalTwoPointersMade: number;
  totalTwoPointersAttempted: number;
  freeThrowPercentage: number;
  totalFreeThrowsMade: number;
  totalFreeThrowsAttempted: number;
  totalOffensiveRebounds: number;
  offensiveReboundsPerGame: number;
  totalDefensiveRebounds: number;
  defensiveReboundsPerGame: number;
  totalSteals: number;
  stealsPerGame: number;
  totalBlocks: number;
  blocksPerGame: number;
  totalTurnovers: number;
  turnoversPerGame: number;
  totalPersonalFouls: number;
  totalMinutes: number;
}

export interface CompetitionDto {
  id: string;
  name: string;
}

export interface MatchDto {
  id: string;
  matchDate: string;
  competitionId: string;
  playerStatCount: number;
}
