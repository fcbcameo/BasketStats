import { Injectable } from '@angular/core';
import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';
import { PlayerSeasonStatsDto } from '../models/dtos';

@Injectable({ providedIn: 'root' })
export class PdfService {
  private safeName(name: string) {
    return (name || 'All Competitions').replace(/[^a-z0-9]+/gi, '_');
  }
  private timeStamp(d = new Date()) {
    const pad = (n: number) => n.toString().padStart(2, '0');
    return `${d.getFullYear()}${pad(d.getMonth() + 1)}${pad(d.getDate())}_${pad(d.getHours())}${pad(d.getMinutes())}${pad(d.getSeconds())}`;
  }

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

    const comp = this.safeName(competitionName);
    const ts = this.timeStamp();
    doc.save(`PlayerStats_${comp}_${ts}.pdf`);
  }

  async generatePlayerStatsReportAdvanced(players: PlayerSeasonStatsDto[], competitionName: string) {
    const doc = new jsPDF({ orientation: 'landscape', unit: 'pt', format: 'a4' });

    const width = doc.internal.pageSize.getWidth();
    const height = doc.internal.pageSize.getHeight();

    // Header banner
    doc.setFillColor(29, 66, 138);
    doc.rect(0, 0, width, 60, 'F');
    doc.setFillColor(201, 8, 42);
    doc.rect(0, 55, width, 5, 'F');

    // Title
    doc.setTextColor(255, 255, 255);
    doc.setFontSize(18);
    doc.text('BasketStats - Advanced Player Statistics Report', 80, 38);
    doc.setFontSize(12);
    doc.text(`Competition: ${competitionName}`, 80, 58);

    // thresholds for top-3 highlighting (PPG, RPG, DRPG, ORPG, APG, SPG, BPG, TOV)
    const topThreshold = (values: number[]) => {
      const uniq = Array.from(new Set(values.map(v => Number(v.toFixed(1))))).sort((a,b)=>b-a);
      return uniq.length >= 3 ? uniq[2] : (uniq[uniq.length-1] ?? 0);
    };
    const ppgT = topThreshold(players.map(p => p.pointsPerGame));
    const rpgT = topThreshold(players.map(p => p.reboundsPerGame));
    const drpgT = topThreshold(players.map(p => p.defensiveReboundsPerGame));
    const orpgT = topThreshold(players.map(p => p.offensiveReboundsPerGame));
    const apgT = topThreshold(players.map(p => p.assistsPerGame));
    const spgT = topThreshold(players.map(p => p.stealsPerGame));
    const bpgT = topThreshold(players.map(p => p.blocksPerGame));
    const tovT = topThreshold(players.map(p => p.turnoversPerGame));

    // Column order with abbreviations
    const head = [[
      'Player','GP','MPG','PPG','RPG','DRPG','ORPG','APG','SPG','BPG','TOV',
      'FG%','FGM/G','FGA/G',
      '2P%','2PM/G','2PA/G',
      '3P%','3PM/G','3PA/G',
      'FT%','FTM/G','FTA/G',
      'EFF'
    ]];

    const idx = {
      MPG: 2, PPG: 3, RPG: 4, DRPG: 5, ORPG: 6, APG: 7, SPG: 8, BPG: 9, TOV: 10
    } as const;

    const body = players.map(p => {
      const g = Math.max(1, p.gamesPlayed);
      const fgmG = p.totalFieldGoalsMade / g;
      const fgaG = p.totalFieldGoalsAttempted / g;
      const twomG = p.totalTwoPointersMade / g;
      const twoaG = p.totalTwoPointersAttempted / g;
      const threemG = p.totalThreePointersMade / g;
      const threeaG = p.totalThreePointersAttempted / g;
      const ftmG = p.totalFreeThrowsMade / g;
      const ftaG = p.totalFreeThrowsAttempted / g;
      return [
        p.playerName,
        p.gamesPlayed,
        p.minutesPerGame.toFixed(1),
        p.pointsPerGame.toFixed(1),
        p.reboundsPerGame.toFixed(1),
        p.defensiveReboundsPerGame.toFixed(1),
        p.offensiveReboundsPerGame.toFixed(1),
        p.assistsPerGame.toFixed(1),
        p.stealsPerGame.toFixed(1),
        p.blocksPerGame.toFixed(1),
        p.turnoversPerGame.toFixed(1),
        p.fieldGoalPercentage.toFixed(1),
        fgmG.toFixed(1), fgaG.toFixed(1),
        p.twoPointPercentage.toFixed(1),
        twomG.toFixed(1), twoaG.toFixed(1),
        p.threePointPercentage.toFixed(1),
        threemG.toFixed(1), threeaG.toFixed(1),
        p.freeThrowPercentage.toFixed(1),
        ftmG.toFixed(1), ftaG.toFixed(1),
        p.efficiency.toFixed(1)
      ];
    });

    autoTable(doc, {
      head,
      body,
      startY: 80,
      styles: { fontSize: 9, cellPadding: 4 },
      headStyles: { fillColor: [29, 66, 138], textColor: [255, 255, 255] },
      alternateRowStyles: { fillColor: [245, 245, 245] },
      didParseCell: data => {
        if (data.section === 'body') {
          const rowIdx = data.row.index;
          const player = players[rowIdx];
          if (data.column.index === idx.PPG && player.pointsPerGame >= ppgT) data.cell.styles.fontStyle = 'bold';
          if (data.column.index === idx.RPG && player.reboundsPerGame >= rpgT) data.cell.styles.fontStyle = 'bold';
          if (data.column.index === idx.DRPG && player.defensiveReboundsPerGame >= drpgT) data.cell.styles.fontStyle = 'bold';
          if (data.column.index === idx.ORPG && player.offensiveReboundsPerGame >= orpgT) data.cell.styles.fontStyle = 'bold';
          if (data.column.index === idx.APG && player.assistsPerGame >= apgT) data.cell.styles.fontStyle = 'bold';
          if (data.column.index === idx.SPG && player.stealsPerGame >= spgT) data.cell.styles.fontStyle = 'bold';
          if (data.column.index === idx.BPG && player.blocksPerGame >= bpgT) data.cell.styles.fontStyle = 'bold';
          if (data.column.index === idx.TOV && player.turnoversPerGame >= tovT) data.cell.styles.fontStyle = 'bold';
        }
      }
    });

    try {
      const imgData = await this.loadImageAsDataUrl('/images/basketballstats.png');
      const imgW = 80, imgH = 80;
      doc.addImage(imgData, 'PNG', width - imgW - 24, height - imgH - 24, imgW, imgH);
    } catch {}

    const comp = this.safeName(competitionName);
    const ts = this.timeStamp();
    doc.save(`PlayerStats_Advanced_${comp}_${ts}.pdf`);
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
