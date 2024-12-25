import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StatisticsComponent } from './statistics.component';
import { ShopeeRevenueComponent } from './shopee-revenue/shopee-revenue.component';

const routes: Routes = [
  { path: '', component: StatisticsComponent },
  { path: 'shopee-revenue', component: ShopeeRevenueComponent, data: { title: 'Doanh thu Shopee' } }
]

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ]
})
export class StatisticsRoutingModule { }
