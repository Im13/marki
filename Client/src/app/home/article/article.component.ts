import { Component, Input, OnInit } from '@angular/core';
import { BasketService } from 'src/app/basket/basket.service';
import { Product } from 'src/app/shared/models/product';
import SwiperCore, { Navigation, Pagination, Scrollbar, A11y } from 'swiper';

//install Swiper modules
SwiperCore.use([Navigation, Pagination, Scrollbar, A11y]);

@Component({
  selector: 'app-article',
  templateUrl: './article.component.html',
  styleUrls: ['./article.component.css']
})
export class ArticleComponent implements OnInit {
  @Input('all-products') products: Product[];

  constructor(private basketService: BasketService) { }

  ngOnInit(): void {
  }

  addItemToBasket(product: Product) {
    this.basketService.addItemToBasket(product);
  }
}
