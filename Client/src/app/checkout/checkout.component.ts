import { Component, OnInit } from '@angular/core';
import { BasketService } from '../basket/basket.service';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.css']
})
export class CheckoutComponent implements OnInit {
  isDisplayingOrder: boolean = true;

  constructor(public basketService: BasketService) { }

  ngOnInit(): void {
  }

  toggleDisplayOrder() {
    this.isDisplayingOrder = !this.isDisplayingOrder;
  }

}
