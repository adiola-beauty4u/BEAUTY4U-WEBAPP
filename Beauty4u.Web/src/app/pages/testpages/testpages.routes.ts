import { Routes } from '@angular/router';
import { roleGuard } from 'src/app/services/authentication/role.guard';
import { TestpageComponent } from './testpage/testpage.component';
import { Testpage2Component } from './testpage2/testpage2.component';

export const TestPagesRoutes: Routes = [
  {
    path: '',
    children: [
      {
        path: 'testpage',
        component: TestpageComponent,
      },
    ],
  },
  {
    path: '',
    children: [
      {
        path: 'testpage2',
        component: Testpage2Component,
      },
    ],
  },
];
