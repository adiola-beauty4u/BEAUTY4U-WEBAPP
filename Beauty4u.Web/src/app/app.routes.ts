import { Routes } from '@angular/router';
import { BlankComponent } from './layouts/blank/blank.component';
import { FullComponent } from './layouts/full/full.component';
import { roleGuard } from './services/authentication/role.guard';
import { ResetpasswordComponent } from './pages/authentication/resetpassword/resetpassword.component';
import { Beauty4uComponent } from './layouts/beauty4u/beauty4u.component';
import { ScheduledjobsComponent } from './pages/scheduledjobs/scheduledjobs.component';

export const routes: Routes = [
  {
    path: '',
    component: Beauty4uComponent,
    children: [
      {
        path: '',
        redirectTo: '/dashboard',
        pathMatch: 'full',
      },
      {
        path: 'dashboard',
        loadChildren: () =>
          import('./pages/pages.routes').then((m) => m.PagesRoutes),
      },
      {
        path: 'ui-components',
        loadChildren: () =>
          import('./pages/ui-components/ui-components.routes').then(
            (m) => m.UiComponentsRoutes
          ),
      },
      {
        path: 'extra',
        loadChildren: () =>
          import('./pages/extra/extra.routes').then((m) => m.ExtraRoutes),
      },
      {
        path: 'products',
        loadChildren: () =>
          import('./pages/products/products.routes').then((m) => m.ProductRoutes),
        //canActivate: [roleGuard(['Admin', 'User'])]
      },
      {
        path: 'authentication/reset-password',
        component: ResetpasswordComponent,
        canActivate: [roleGuard([])]
      }, {
        path: 'promotions',
        loadChildren: () => import('./pages/promotions/promotions.routes').then((m) => m.PromotionsRoutes),
      },
      {
        path: 'testpages',
        loadChildren: () =>
          import('./pages/testpages/testpages.routes').then(
            (m) => m.TestPagesRoutes
          ),
      },
      {
        path: 'scheduledJobs',
        component: ScheduledjobsComponent,
      }
    ],
  },
  {
    path: '',
    component: BlankComponent,
    children: [
      {
        path: 'authentication',
        loadChildren: () =>
          import('./pages/authentication/authentication.routes').then(
            (m) => m.AuthenticationRoutes
          ),
      },
    ],
  },
  {
    path: '**',
    redirectTo: 'authentication/error',
  },
];
