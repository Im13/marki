import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home.component';
import { OverviewHeadComponent } from './overview-head/overview-head.component';
import { CoreModule } from '../core/core.module';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { SharedModule } from '../shared/shared.module';
import { RouterModule } from '@angular/router';
import { HomeRoutingModule } from './home-routing.module';
import { BodyContainerComponent } from './body-container/body-container.component';
import { OverviewChartComponent } from './body-container/overview-chart/overview-chart.component';
import { OrderResourcesComponent } from './body-container/order-resources/order-resources.component';
import { SideNotificationComponent } from './body-container/side-notification/side-notification.component';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';

@NgModule({
  declarations: [
    HomeComponent,
    OverviewHeadComponent,
    BodyContainerComponent,
    OverviewChartComponent,
    OrderResourcesComponent,
    SideNotificationComponent
  ],
  imports: [
    CommonModule,
    HomeRoutingModule,
    CoreModule,
    RouterModule,
    TabsModule.forRoot(),
    SharedModule,
    BsDatepickerModule.forRoot()
  ]
})
export class HomeModule { }
