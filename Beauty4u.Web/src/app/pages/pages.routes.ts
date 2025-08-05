import { Routes } from '@angular/router';
import { StarterComponent } from './starter/starter.component';
import { DashboardsComponent } from './dashboards/dashboards.component';

export const PagesRoutes: Routes = [
  {
    path: '',
    component: DashboardsComponent,
    data: {
      title: 'Starter',
      urls: [
        { title: 'Dashboard', url: '/dashboard' },
        { title: 'Starter' },
      ],
    },
  },
];
