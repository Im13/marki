import { Component, OnInit } from '@angular/core';
import { BasketService } from './basket.service';
import { BasketItem } from '../shared/models/basket';
import { NgIf, NgFor, AsyncPipe, DecimalPipe } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
    selector: 'app-basket',
    templateUrl: './basket.component.html',
    styleUrls: ['./basket.component.css'],
    standalone: true,
    imports: [NgIf, RouterLink, NgFor, AsyncPipe, DecimalPipe]
})
export class BasketComponent implements OnInit {

  constructor(public basketService: BasketService) { }

  ngOnInit(): void {
  }

  getCount(items: BasketItem[]) {
    return items.reduce((sum, item) => sum + item.quantity, 0);
  }

  incrementQuantity(item: BasketItem) {
    this.basketService.addItemToBasket(item);
  }

  removeItem(id: number, quantity: number) {
    this.basketService.removeItemFromBasket(id, quantity);
  }
}
