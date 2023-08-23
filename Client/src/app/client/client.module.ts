import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home/home.component';
import { ClientRoutingModule } from './client-routing.module';
import { HeaderComponent } from './home/header/header.component';
import { ArticleComponent } from './home/article/article.component';
import { FooterComponent } from './home/footer/footer.component';
import { CarouselModule } from 'ngx-bootstrap/carousel';

@NgModule({
  declarations: [
    HomeComponent,
    HeaderComponent,
    ArticleComponent,
    FooterComponent
  ],
  imports: [
    CommonModule,
    ClientRoutingModule,
    CarouselModule.forRoot()
  ]
})
export class ClientModule { }
