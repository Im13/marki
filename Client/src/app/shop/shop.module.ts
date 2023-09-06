import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ShopComponent } from './shop.component';
import { ArticleComponent } from './article/article.component';
import { FooterComponent } from './footer/footer.component';



@NgModule({
  declarations: [
    ShopComponent,
    ArticleComponent,
    FooterComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    ShopComponent
  ]
})
export class ShopModule { }
