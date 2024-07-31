import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { OrderService } from '../order.service';
import { OrderParams } from 'src/app/shared/models/orderParams';
import { Order } from 'src/app/shared/models/order';

@Component({
  selector: 'app-order-list',
  templateUrl: './order-list.component.html',
  styleUrls: ['./order-list.component.css']
})
export class OrderListComponent implements OnInit {
  orderParams = new OrderParams();
  orders: Order[] = [];

  constructor(private router: Router, private orderService: OrderService) {}

  ngOnInit(): void {
    this.getOrders();
  }

  addOrder() {
    this.router.navigate(['/add'])
  }

  getOrders() {
    this.orderService.getProducts(this.orderParams).subscribe({
      next: response => {
        this.orders = response.data;
        this.orderParams.pageIndex = response.pageIndex;
        this.orderParams.pageSize = response.pageSize;
      },
      error: err => {
        console.log(err);
      }
    });
  }
}
