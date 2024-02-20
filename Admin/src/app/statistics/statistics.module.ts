import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StatisticsComponent } from './statistics.component';
import { ShopeeRevenueComponent } from './shopee-revenue/shopee-revenue.component';



@NgModule({
  declarations: [
    StatisticsComponent,
    ShopeeRevenueComponent
  ],
  imports: [
    CommonModule
  ]
})
export class StatisticsModule { }
