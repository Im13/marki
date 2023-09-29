import { Component, Input, OnInit } from '@angular/core';
import { BasketService } from 'src/app/basket/basket.service';
import { Product } from 'src/app/shared/models/product';
import SwiperCore, { Navigation, Pagination, Scrollbar, A11y, SwiperOptions } from 'swiper';

//install Swiper modules
SwiperCore.use([Navigation, Pagination, Scrollbar, A11y]);

@Component({
  selector: 'app-article',
  templateUrl: './article.component.html',
  styleUrls: ['./article.component.css']
})
export class ArticleComponent implements OnInit {
  @Input('all-products') products: Product[];
  swiperConfig: SwiperOptions = {
    spaceBetween: 10,
    navigation: true,
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

  constructor(private basketService: BasketService) { }

  ngOnInit(): void {
  }

  addItemToBasket(product: Product) {
    this.basketService.addItemToBasket(product);
  }
}
