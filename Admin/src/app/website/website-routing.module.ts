import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { WebsiteComponent } from './website.component';
import { BannerComponent } from './banner/banner.component';

const routes: Routes = [
  { path: '', component: WebsiteComponent, data: { title: 'Cài đặt website' } },
  { path: 'banner', component: BannerComponent, data: { title: 'Banner' } }
]

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ]
})
export class WebsiteRoutingModule { }
