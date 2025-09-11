import { Routes } from '@angular/router';
import { roleGuard } from 'src/app/services/authentication/role.guard';
import { ProductsComponent } from './products/products.component';
import { PromotionComponent } from './promotion/promotion.component';
import { PromotionsSearchComponent } from './promotions-search/promotions-search.component';

export const PromotionsRoutes: Routes = [
    {
        path:'',
        children: [
            {
                path: 'search-products',
                component: ProductsComponent,
                canActivate: [roleGuard(['Admin', 'Data-Analyst', 'IT Team', 'President', 'Vice President'])]
            },
            {
                path: 'search-promotions',
                component: PromotionsSearchComponent,
                canActivate: [roleGuard(['Admin', 'Data-Analyst', 'IT Team', 'President', 'Vice President'])]
            },
            {
                path: 'promotion',
                component: PromotionComponent,
                canActivate: [roleGuard(['Admin', 'Data-Analyst', 'IT Team', 'President', 'Vice President'])]
            }
        ]
    }
];