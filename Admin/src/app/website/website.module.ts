import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WebsiteComponent } from './website.component';
import { SharedModule } from '../_shared/shared.module';
import { WebsiteRoutingModule } from './website-routing.module';
import { BannerComponent } from './banner/banner.component';
import { CoreModule } from '../_core/core.module';
import { AllBannerComponent } from './banner/all-banner/all-banner.component';
import { AddBannerModalComponent } from './banner/add-banner-modal/add-banner-modal.component';

@NgModule({
  declarations: [
    WebsiteComponent,
    BannerComponent,
    AllBannerComponent,
    AddBannerModalComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    WebsiteRoutingModule,
    CoreModule
  ]
})
export class WebsiteModule { }
