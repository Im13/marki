import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StatisticsComponent } from './statistics.component';
import { ShopeeRevenueComponent } from './shopee-revenue/shopee-revenue.component';
import { StatisticsRoutingModule } from './statistics-routing.module';
import { RouterModule } from '@angular/router';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';


@NgModule({
  declarations: [
    StatisticsComponent,
    ShopeeRevenueComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    StatisticsRoutingModule,
    BsDatepickerModule.forRoot()
  ]
})
export class StatisticsModule { }
