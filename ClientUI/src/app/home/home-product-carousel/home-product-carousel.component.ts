import { Component, Input, OnInit } from '@angular/core';
import { Product } from 'src/app/_shared/_models/product';

// import Swiper core and required modules
import SwiperCore, {
  Navigation,
  Pagination,
  Scrollbar,
  A11y,
  SwiperOptions,
} from 'swiper';
import { HomeService } from '../home.service';
import { ProductParams } from 'src/app/_shared/_models/productParams';

// install Swiper modules
SwiperCore.use([Navigation, Pagination, Scrollbar, A11y]);

@Component({
  selector: 'app-home-product-carousel',
  templateUrl: './home-product-carousel.component.html',
  styleUrls: ['./home-product-carousel.component.css'],
})
export class HomeProductCarouselComponent implements OnInit {
  @Input('sectionName') sectionName: string = '';
  @Input('typeId') typeId: number;
  swiperConfig: SwiperOptions = {};
  products: Product[] = [];

  constructor(private homeService: HomeService) {}

  ngOnInit(): void {
    this.swiperConfig = {
      spaceBetween: 10,
      navigation: false,
      pagination: { clickable: true },
      breakpoints: {
        0: {
          slidesPerView: 2,
          spaceBetween: 10,
        },
        768: {
          slidesPerView: 3,
          spaceBetween: 15,
        },
      },
    };

    if (this.typeId != null) {
      var productParams = new ProductParams(this.typeId);

      this.homeService.getByType(productParams).subscribe({
        next: res => {
          this.products = res.data.slice(0,6);
        },
        error: err => {
          console.log(err);
        }
      })
    }
  }
}
