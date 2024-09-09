import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home.component';
import { CoreModule } from '../_core/core.module';
import { HomeRoutingModule } from './home-routing.module';
import { HomeCarouselComponent } from './home-carousel/home-carousel.component';
import { HomeNewArrivalsComponent } from './home-new-arrivals/home-new-arrivals.component';
import { HomeCollectionBannersComponent } from './home-collection-banners/home-collection-banners.component';



@NgModule({
  declarations: [
    HomeComponent,
    HomeCarouselComponent,
    HomeNewArrivalsComponent,
    HomeCollectionBannersComponent
  ],
  imports: [
    CommonModule,
    HomeRoutingModule,
    CoreModule
  ]
})
export class HomeModule { }
