import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StatisticsComponent } from './statistics.component';
import { ShopeeRevenueComponent } from './shopee-revenue/shopee-revenue.component';
import { MetaAdsDashboardComponent } from './meta-ads-dashboard/meta-ads-dashboard.component';

const routes: Routes = [
  { path: '', component: StatisticsComponent },
  { path: 'shopee-revenue', component: ShopeeRevenueComponent, data: { title: 'Doanh thu Shopee' } },
  { path: 'meta-ads', component: MetaAdsDashboardComponent, data: { title: 'Quảng cáo Facebook' } }
]

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ]
})
export class StatisticsRoutingModule { }
