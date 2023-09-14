import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ArticleComponent } from './article/article.component';
import { FooterComponent } from '../core/footer/footer.component';
import { HomeComponent } from './home.component';
import { SwiperModule } from 'swiper/angular';



@NgModule({
  declarations: [
    HomeComponent,
    ArticleComponent,
    FooterComponent
  ],
  imports: [
    CommonModule,
    SwiperModule
  ],
  exports: [
    HomeComponent,
    FooterComponent
  ]
})
export class ShopModule { }
