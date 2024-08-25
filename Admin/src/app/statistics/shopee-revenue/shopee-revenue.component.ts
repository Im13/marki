import { Component } from '@angular/core';
import { ShopeeOrderParams } from 'src/app/shared/models/shopeeOrderParams';
import { ShopeeOrderProducts } from 'src/app/shared/models/shopeeOrderProducts';
import { StatisticsService } from '../statistics.service';

@Component({
  selector: 'app-shopee-revenue',
  templateUrl: './shopee-revenue.component.html',
  styleUrls: ['./shopee-revenue.component.css']
})
export class ShopeeRevenueComponent {
  today: Date;
  shopeeOrderProducts: ShopeeOrderProducts[] = [];
  orderParams = new ShopeeOrderParams();
  totalRevenue = 0;

  constructor(private statisticService: StatisticsService) {
    this.today = new Date();
    this.orderParams.date = this.today.toString();
  }

  getOrders() {
    this.totalRevenue = 0;

    this.statisticService.getShopeeOrdersStatistics(this.orderParams).subscribe({
      next: response => {
        this.shopeeOrderProducts = response;
        this.shopeeOrderProducts.forEach(prod => {
          this.totalRevenue += prod.revenue;
        });
        console.log(this.shopeeOrderProducts);
        // this.orderParams.pageIndex = response.pageIndex;
        // this.orderParams.pageSize = response.pageSize;
        // this.totalCount = response.count;
      },
      error: err => {
        console.log(err);
      }
    });
  }

  onDateChange(value: Date) {
    this.totalRevenue = 0;
    this.orderParams.date = value.toString();
    this.getOrders();
  }
}
