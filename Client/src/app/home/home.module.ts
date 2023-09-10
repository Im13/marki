import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ArticleComponent } from './article/article.component';
import { FooterComponent } from './footer/footer.component';
import { HomeComponent } from './home.component';



@NgModule({
  declarations: [
    HomeComponent,
    ArticleComponent,
    FooterComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    HomeComponent
  ]
})
export class ShopModule { }
