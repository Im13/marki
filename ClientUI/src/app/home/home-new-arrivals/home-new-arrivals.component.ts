import { Component, OnInit } from '@angular/core';
import { HomeService } from '../home.service';
import { Product } from 'src/app/_shared/_models/product';
import { ProductParams } from 'src/app/_shared/_models/productParams';
import { Router } from '@angular/router';
import { ProductUtilityService } from 'src/app/_shared/_services/product-utility.service';

// import Swiper core and required modules
import SwiperCore, {
  Navigation,
  Pagination,
  Scrollbar,
  A11y,
  SwiperOptions,
} from 'swiper';

// install Swiper modules
SwiperCore.use([Navigation, Pagination, Scrollbar, A11y]);

@Component({
  selector: 'app-home-new-arrivals',
  templateUrl: './home-new-arrivals.component.html',
  styleUrls: ['./home-new-arrivals.component.css']
})
export class HomeNewArrivalsComponent implements OnInit {
  products: readonly Product[] = [];
  productParams = new ProductParams();
  swiperConfig: SwiperOptions = {};

  constructor(
    private homeService: HomeService, 
    private router: Router,
    private productUtilityService: ProductUtilityService
  ){}

  ngOnInit() {
    this.swiperConfig = {
      spaceBetween: 10,
      navigation: {
        nextEl: '.swiper-button-next-custom',
        prevEl: '.swiper-button-prev-custom',
      },
      pagination: { 
        clickable: true,
        el: '.swiper-pagination-custom'
      },
      breakpoints: {
        0: {
          slidesPerView: 2,
          spaceBetween: 10,
        },
        768: {
          slidesPerView: 3,
          spaceBetween: 15,
        },
        1280: {
          slidesPerView: 4,
          spaceBetween: 20,
        },
      },
    };

    this.homeService.getNewArrivals(this.productParams).subscribe({
      next: response => {
        this.products = response.data.slice(1,9);
      },
      error: err => {
        console.log(err);
      }
    })
  }

  onViewMoreClick() {
    this.router.navigate(['/new-arrivals']);
  }

  redirectToProduct(slug: string) {
    this.router.navigate([`products/${slug}`])
  }

  getMainPhotoUrl(product: Product): string {
    return this.productUtilityService.getMainPhotoUrl(product);
  }
}
