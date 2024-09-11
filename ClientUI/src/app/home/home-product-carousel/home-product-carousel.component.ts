import { Component, Input, OnInit } from '@angular/core';

// import Swiper core and required modules
import SwiperCore, { Navigation, Pagination, Scrollbar, A11y, SwiperOptions } from 'swiper';

// install Swiper modules
SwiperCore.use([Navigation, Pagination, Scrollbar, A11y]);

@Component({
  selector: 'app-home-product-carousel',
  templateUrl: './home-product-carousel.component.html',
  styleUrls: ['./home-product-carousel.component.css']
})
export class HomeProductCarouselComponent implements OnInit {
  @Input('sectionName') sectionName: string = '';
  swiperConfig: SwiperOptions = {};

  ngOnInit(): void {
    this.swiperConfig = {
      spaceBetween: 10,
      navigation: false,
      pagination: { clickable: true},
      breakpoints: {
        0: {
          slidesPerView: 2,
          spaceBetween: 10
        },
        768: {
          slidesPerView: 3,
          spaceBetween: 15
        }
      }
    }
  }
}
