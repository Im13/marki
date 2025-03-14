import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WebsiteComponent } from './website.component';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  declarations: [
    WebsiteComponent
  ],
  imports: [
    CommonModule,
    SharedModule
  ]
})
export class WebsiteModule { }
