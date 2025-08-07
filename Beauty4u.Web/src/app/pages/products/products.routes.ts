import { Routes } from '@angular/router';
import { RegistrationComponent } from './bulk/registration/registration.component';
import { UpdateComponent } from './bulk/update/update.component';
import { roleGuard } from 'src/app/services/authentication/role.guard';
import { TransferComponent } from './transfer/transfer.component';
import { ProductsComponent } from './products.component';

export const ProductRoutes: Routes = [
  {
    path: '',
    children: [
      {
        path: 'bulk-registration',
        component: RegistrationComponent,
        canActivate: [roleGuard(['Admin', 'Data-Analyst', 'IT Team', 'President', 'Vice President'])]
      },
      {
        path: 'bulk-update',
        component: UpdateComponent,
        canActivate: [roleGuard(['Admin', 'Data-Analyst', 'IT Team', 'President', 'Vice President'])]
      },
      {
        path: 'transfer',
        component: TransferComponent,
        canActivate: [roleGuard(['Admin', 'Data-Analyst', 'IT Team', 'President', 'Vice President'])]
      },
    ],
  },
];
