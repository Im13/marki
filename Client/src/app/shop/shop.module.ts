import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ShopComponent } from './shop.component';
import { HomeComponent } from './home/home.component';
import { ArticleComponent } from './home/article/article.component';
import { FooterComponent } from './home/footer/footer.component';



@NgModule({
  declarations: [
    ShopComponent,
    ArticleComponent,
    FooterComponent,
    HomeComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    ShopComponent,
    HomeComponent
  ]
})
export class ShopModule { }
