import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { AuthGuard } from './_guards/auth.guard';

const routes: Routes = [
  { path: '', component: LoginComponent },
  { path: '', canActivate: [AuthGuard], children: [
    {path: 'home',  loadChildren: () => import('./home/home.module').then(m => m.HomeModule)},
    { path: 'orders', loadChildren: () => import('./order/order.module').then(m => m.OrderModule) },
    { path: 'product', loadChildren: () => import('./product/product.module').then(m => m.ProductModule) },
    { path: 'customers', loadChildren: () => import('./customer/customer.module').then(m => m.CustomerModule) },
    { path: 'statistics', loadChildren: () => import('./statistics/statistics.module').then(m => m.StatisticsModule) }
  ]}
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {scrollPositionRestoration: 'top'})],
  exports: [RouterModule]
})
export class AppRoutingModule { }
