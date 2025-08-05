import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { HealthCheckItem, HealthTableRow } from 'src/interfaces/health-check-result';
import { HealthCheckService } from 'src/app/services/health-check.service';
import { LoadingService } from 'src/app/services/loading.service';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-health-check',
  imports: [CommonModule, MatTableModule, MatIconModule, MatCardModule, MatDividerModule, MatButtonModule],
  templateUrl: './health-check.component.html',
  styleUrl: './health-check.component.scss'
})

export class HealthCheckComponent implements OnInit {

  healthData: HealthCheckItem[] = [];
  tableData: HealthTableRow[] = [];
  displayedColumns: string[] = ['storeCode', 'storeName', 'dbStatus', 'apiStatus'];

  constructor(private healthService: HealthCheckService, private loadingService: LoadingService) {

  }

  ngOnInit(): void {
    
  }

  checkServerHealth() {
    this.loadingService.show("Checking servers..", false);
    this.healthService.getServersHealth().subscribe({
      next: data => {
        this.healthData = data;

        const map = new Map<string, HealthTableRow>();

        for (const item of this.healthData) {
          const match = item.name.match(/^(\d{3}) - ([^()]+) \(([^)]+)\) - (\w+)$/);
          if (!match) continue;

          const [_, code, name, shortCode, type] = match;
          if (!map.has(code)) {
            map.set(code, { storeCode: code, storeName: name });
          }

          const row = map.get(code)!;
          if (type.toLowerCase() === 'db') row.dbStatus = item.value;
          if (type.toLowerCase() === 'api') row.apiStatus = item.value;
        }

        this.tableData = Array.from(map.values()).sort((a, b) => a.storeCode.localeCompare(b.storeCode));
        this.loadingService.hide();
      },
      error: err => {
        console.error('Health API error', err);
        this.loadingService.hide();
      }
    });


  }
}