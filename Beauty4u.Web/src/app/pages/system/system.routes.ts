import { Routes } from '@angular/router';
import { StoreDetailsComponent } from './store-details/store-details.component';
import { roleGuard } from 'src/app/services/authentication/role.guard';

export const SystemRoutes: Routes = [{
    path: '',
    children: [
        {
            path: 'store-details',
            component: StoreDetailsComponent,
            canActivate: [roleGuard(['Admin', 'Data-Analyst', 'IT Team'])]
        }
    ]
}]