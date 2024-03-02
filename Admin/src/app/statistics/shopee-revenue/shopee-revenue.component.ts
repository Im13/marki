import { Component } from '@angular/core';
import { ShopeeOrder } from 'src/app/shared/models/shopeeOrder';

@Component({
  selector: 'app-shopee-revenue',
  templateUrl: './shopee-revenue.component.html',
  styleUrls: ['./shopee-revenue.component.css']
})
export class ShopeeRevenueComponent {
  today: Date;
  shopeeOrders: ShopeeOrder[] = [];

  constructor() {
    this.today = new Date();
  }
}
