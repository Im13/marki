import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StatisticsComponent } from './statistics.component';
import { ShopeeRevenueComponent } from './shopee-revenue/shopee-revenue.component';
import { StatisticsRoutingModule } from './statistics-routing.module';
import { RouterModule } from '@angular/router';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { CoreModule } from '../_core/core.module';
import { SharedModule } from '../_shared/shared.module';
import { MetaAdsDashboardComponent } from './meta-ads-dashboard/meta-ads-dashboard.component';


@NgModule({
  declarations: [
    StatisticsComponent,
    ShopeeRevenueComponent,
    MetaAdsDashboardComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    StatisticsRoutingModule,
    BsDatepickerModule.forRoot(),
    CoreModule,
    SharedModule
  ]
})
export class StatisticsModule { }
