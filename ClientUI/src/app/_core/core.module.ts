import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzMenuModule } from 'ng-zorro-antd/menu';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzDropDownModule } from 'ng-zorro-antd/dropdown';
import { NzCollapseModule } from 'ng-zorro-antd/collapse';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzCarouselModule } from 'ng-zorro-antd/carousel';
import { NzTypographyModule } from 'ng-zorro-antd/typography';
import { NzBackTopModule } from 'ng-zorro-antd/back-top';
import { SwiperModule } from 'swiper/angular';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzBadgeModule } from 'ng-zorro-antd/badge';
import { DescriptionComponent } from './description/description.component';
import { ReactiveFormsModule } from '@angular/forms';
import { NzSelectModule }  from 'ng-zorro-antd/select';
import { BreadcrumbModule } from 'xng-breadcrumb';
import { CommonBreadcrumbComponent } from './common-breadcrumb/common-breadcrumb.component';
import { HeaderComponent } from './header/header.component';
import { CartDropdownComponent } from './cart-dropdown/cart-dropdown.component';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { RouterModule } from '@angular/router';
import { RecommendationComponent } from './recommendation/recommendation.component';

@NgModule({
  declarations: [
    DescriptionComponent,
    HeaderComponent,
    CartDropdownComponent,
    CommonBreadcrumbComponent,
    RecommendationComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    NzBreadCrumbModule,
    NzMenuModule,
    NzGridModule,
    NzDropDownModule,
    NzCollapseModule,
    CollapseModule.forRoot(),
    NzInputModule,
    NzCarouselModule,
    NzTypographyModule,
    NzBackTopModule,
    SwiperModule,
    NzButtonModule,
    NzDividerModule,
    NzBadgeModule,
    ReactiveFormsModule,
    NzSelectModule,
    BreadcrumbModule,
    NzIconModule
  ],
  exports: [
    RouterModule,
    NzLayoutModule,
    NzBreadCrumbModule,
    NzMenuModule,
    NzGridModule,
    NzDropDownModule,
    NzCollapseModule,
    CollapseModule,
    NzInputModule,
    NzCarouselModule,
    NzTypographyModule,
    NzBackTopModule,
    SwiperModule,
    NzButtonModule,
    NzDividerModule,
    NzBadgeModule,
    DescriptionComponent,
    ReactiveFormsModule,
    NzSelectModule,
    CommonBreadcrumbComponent,
    HeaderComponent,
    CartDropdownComponent,
    NzIconModule,
    RecommendationComponent
  ]
})
export class CoreModule { }
