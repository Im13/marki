import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CoreModule } from '../core/core.module';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { SharedModule } from '../shared/shared.module';
import { RouterModule } from '@angular/router';
import { BodyContainerComponent } from './body-container/body-container.component';
import { OverviewChartComponent } from './body-container/overview-chart/overview-chart.component';
import { OrderResourcesComponent } from './body-container/order-resources/order-resources.component';
import { SideNotificationComponent } from './body-container/side-notification/side-notification.component';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { OverviewHeadComponent } from './overview-head/overview-head.component';
import { DashboardComponent } from './dashboard.component';
import { DashboardRoutingModule } from './dashboard-routing.module';

@NgModule({
  declarations: [
    DashboardComponent,
    OverviewHeadComponent,
    BodyContainerComponent,
    OverviewChartComponent,
    OrderResourcesComponent,
    SideNotificationComponent
  ],
  imports: [
    CommonModule,
    CoreModule,
    RouterModule,
    TabsModule.forRoot(),
    SharedModule,
    BsDatepickerModule.forRoot(),
    DashboardRoutingModule
  ]
})
export class DashboardModule { }
