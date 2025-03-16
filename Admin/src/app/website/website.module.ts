import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WebsiteComponent } from './website.component';
import { SharedModule } from '../shared/shared.module';
import { WebsiteRoutingModule } from './website-routing.module';
import { BannerComponent } from './banner/banner.component';
import { CoreModule } from '../core/core.module';
import { AllBannerComponent } from './banner/all-banner/all-banner.component';

@NgModule({
  declarations: [
    WebsiteComponent,
    BannerComponent,
    AllBannerComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    WebsiteRoutingModule,
    CoreModule
  ]
})
export class WebsiteModule { }
