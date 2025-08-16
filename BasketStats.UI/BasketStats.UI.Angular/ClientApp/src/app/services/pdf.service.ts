import { Injectable } from '@angular/core';
import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';
import { PlayerSeasonStatsDto } from '../models/dtos';

@Injectable({ providedIn: 'root' })
export class PdfService {
  generatePlayerStatsReport(players: PlayerSeasonStatsDto[], competitionName: string) {
    const doc = new jsPDF({ orientation: 'landscape', unit: 'pt', format: 'a4' });

    // Header with gradient-like banner using rectangles
    const width = doc.internal.pageSize.getWidth();
    doc.setFillColor(29, 66, 138);
    doc.rect(0, 0, width, 60, 'F');
    doc.setFillColor(201, 8, 42);
    doc.rect(0, 55, width, 5, 'F');

    // Logo if available (served at /images)
    const logo = new Image();
    logo.src = '/images/basketballstats.png';
    // Note: add image after it's loaded; jsPDF doesn't await image load in this simple service

    doc.setTextColor(255, 255, 255);
    doc.setFontSize(18);
    doc.text('BasketStats - Player Statistics Report', 80, 38);
    doc.setFontSize(12);
    doc.text(`Competition: ${competitionName}`, 80, 58);

    // Table
    const head = [[
      'Player', 'GP', 'PPG', 'RPG', 'APG', 'FG%', '3P%', 'FT%', 'SPG', 'BPG', 'TOV', 'MPG', 'EFF', 'Total PTS', 'Total REB', 'Total AST', 'Total STL', 'Total BLK'
    ]];
    const body = players.map(p => [
      p.playerName,
      p.gamesPlayed,
      p.pointsPerGame.toFixed(1),
      p.reboundsPerGame.toFixed(1),
      p.assistsPerGame.toFixed(1),
      p.fieldGoalPercentage.toFixed(1),
      p.threePointPercentage.toFixed(1),
      p.freeThrowPercentage.toFixed(1),
      p.stealsPerGame.toFixed(1),
      p.blocksPerGame.toFixed(1),
      p.turnoversPerGame.toFixed(1),
      p.minutesPerGame.toFixed(1),
      p.efficiency.toFixed(1),
      p.totalPoints,
      p.totalRebounds,
      p.totalAssists,
      p.totalSteals,
      p.totalBlocks
    ]);

    autoTable(doc, {
      head,
      body,
      startY: 80,
      styles: { fontSize: 9, cellPadding: 4 },
      headStyles: { fillColor: [29, 66, 138], textColor: [255, 255, 255] },
      alternateRowStyles: { fillColor: [245, 245, 245] }
    });

    doc.save(`PlayerStats_${competitionName.replace(/[^a-z0-9]/gi, '_')}.pdf`);
  }
}
