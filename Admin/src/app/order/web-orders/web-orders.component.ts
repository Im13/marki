import { Component, OnInit, ViewChild } from '@angular/core';
import { AllSiteOrdersComponent } from './all-site-orders/all-site-orders.component';
import { Router } from '@angular/router';
import { OrderService } from '../order.service';

@Component({
  selector: 'app-web-orders',
  templateUrl: './web-orders.component.html',
  styleUrls: ['./web-orders.component.css']
})
export class WebOrdersComponent implements OnInit {
  @ViewChild(AllSiteOrdersComponent) allSiteOrderComp: AllSiteOrdersComponent;
  orderStatusCounts: { [key: string]: number } = {};

  constructor(private router: Router, private orderService: OrderService) { }

  ngOnInit(): void {
    this.orderService.getStatusCounts().subscribe(counts => {
      console.log('Order status counts:', counts);
      this.orderStatusCounts = counts;
    });
  }

  addOrder() {
    this.router.navigate(['/orders/add'])
  }

  reloadOrder() {
    this.allSiteOrderComp.getOrders();
  }
}
