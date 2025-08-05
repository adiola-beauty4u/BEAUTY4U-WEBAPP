import { Component } from '@angular/core';
import { HealthCheckComponent } from 'src/app/components/health-check/health-check.component';

@Component({
  selector: 'app-dashboards',
  imports: [HealthCheckComponent],
  templateUrl: './dashboards.component.html',
  styleUrl: './dashboards.component.scss'
})
export class DashboardsComponent {

}
