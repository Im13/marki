import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StatisticsComponent } from './statistics.component';
import { ShopeeRevenueComponent } from './shopee-revenue/shopee-revenue.component';
import { StatisticsRoutingModule } from './statistics-routing.module';
import { RouterModule } from '@angular/router';



@NgModule({
  declarations: [
    StatisticsComponent,
    ShopeeRevenueComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    StatisticsRoutingModule
  ]
})
export class StatisticsModule { }
