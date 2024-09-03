import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { AuthGuard } from './_guards/auth.guard';
import { HomeComponent } from './home/home.component';
import { AdminGuard } from './_guards/admin.guard';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: '', component: HomeComponent, canActivate: [AuthGuard], children: [
    { path: '',  redirectTo: 'dashboard', pathMatch: 'full' },
    { path: 'dashboard', loadChildren: () => import('./dashboard/dashboard.module').then(m => m.DashboardModule) },
    { path: 'orders', loadChildren: () => import('./order/order.module').then(m => m.OrderModule) },
    { path: 'product', loadChildren: () => import('./product/product.module').then(m => m.ProductModule), canActivate: [AdminGuard] },
    { path: 'customers', loadChildren: () => import('./customer/customer.module').then(m => m.CustomerModule) },
    { path: 'statistics', loadChildren: () => import('./statistics/statistics.module').then(m => m.StatisticsModule) }
  ]}
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {scrollPositionRestoration: 'top'})],
  exports: [RouterModule]
})
export class AppRoutingModule { }
