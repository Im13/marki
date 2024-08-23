import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home.component';
import { OverviewHeadComponent } from './overview-head/overview-head.component';
import { CoreModule } from '../core/core.module';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { SharedModule } from '../shared/shared.module';
import { RouterModule } from '@angular/router';
import { HomeRoutingModule } from './home-routing.module';

@NgModule({
  declarations: [
    HomeComponent,
    OverviewHeadComponent
  ],
  imports: [
    CommonModule,
    HomeRoutingModule,
    CoreModule,
    RouterModule,
    TabsModule.forRoot(),
    SharedModule
  ]
})
export class HomeModule { }
