import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes } from '@angular/router';
import { StatisticsComponent } from './statistics.component';
import { ShopeeRevenueComponent } from './shopee-revenue/shopee-revenue.component';

const routes: Routes = [
  { path: '', component: StatisticsComponent },
  { path: 'shopee-revenue', component: ShopeeRevenueComponent }
]

@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ]
})
export class StatisticsRoutingModule { }
