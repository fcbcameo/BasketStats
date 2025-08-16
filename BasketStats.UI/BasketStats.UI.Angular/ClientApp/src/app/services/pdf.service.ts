import { Injectable } from '@angular/core';
import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';
import { PlayerSeasonStatsDto } from '../models/dtos';

@Injectable({ providedIn: 'root' })
export class PdfService {
  async generatePlayerStatsReport(players: PlayerSeasonStatsDto[], competitionName: string) {
    const doc = new jsPDF({ orientation: 'landscape', unit: 'pt', format: 'a4' });

    const width = doc.internal.pageSize.getWidth();
    const height = doc.internal.pageSize.getHeight();

    // Header banner
    doc.setFillColor(29, 66, 138); // nba blue
    doc.rect(0, 0, width, 60, 'F');
    doc.setFillColor(201, 8, 42); // nba red
    doc.rect(0, 55, width, 5, 'F');

    // Title
    doc.setTextColor(255, 255, 255);
    doc.setFontSize(18);
    doc.text('BasketStats - Player Statistics Report', 80, 38);
    doc.setFontSize(12);
    doc.text(`Competition: ${competitionName}`, 80, 58);

    // Prepare table data
    const head = [[
      'Player','GP','PPG','RPG','APG','FG%','3P%','FT%','SPG','BPG','TOV','MPG','EFF','Total PTS','Total REB','Total AST','Total STL','Total BLK'
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

    // Add logo bottom-right (load image and then save)
    try {
      const imgData = await this.loadImageAsDataUrl('/images/basketballstats.png');
      const imgW = 80, imgH = 80;
      doc.addImage(imgData, 'PNG', width - imgW - 24, height - imgH - 24, imgW, imgH);
    } catch {
      // ignore if image load fails
    }

    doc.save(`PlayerStats_${competitionName.replace(/[^a-z0-9]/gi, '_')}.pdf`);
  }

  private loadImageAsDataUrl(src: string): Promise<string> {
    return new Promise((resolve, reject) => {
      const img = new Image();
      img.crossOrigin = 'anonymous';
      img.onload = () => {
        const canvas = document.createElement('canvas');
        canvas.width = img.width; canvas.height = img.height;
        const ctx = canvas.getContext('2d');
        if (!ctx) { reject('no ctx'); return; }
        ctx.drawImage(img, 0, 0);
        resolve(canvas.toDataURL('image/png'));
      };
      img.onerror = reject;
      img.src = src;
    });
  }
}
