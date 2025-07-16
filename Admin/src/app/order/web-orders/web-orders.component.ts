import { Component, OnInit, ViewChild } from '@angular/core';
import { AllSiteOrdersComponent } from './all-site-orders/all-site-orders.component';
import { Router } from '@angular/router';
import { OrderService } from '../order.service';
import { saveAs } from 'file-saver';

@Component({
  selector: 'app-web-orders',
  templateUrl: './web-orders.component.html',
  styleUrls: ['./web-orders.component.css']
})
export class WebOrdersComponent implements OnInit {
  @ViewChild(AllSiteOrdersComponent) allSiteOrderComp: AllSiteOrdersComponent;
  orderStatusCounts: { [key: string]: number } = {};
  totalOrders: number = 0;
  checkedIds = new Set<number>();

  constructor(private router: Router, private orderService: OrderService) { }

  ngOnInit(): void {
    this.countOrders();
  }
  
  checkItem(checkedIds: Set<number>) {
    this.checkedIds = checkedIds;
  }

  countOrders() {
    this.orderService.getStatusCounts().subscribe(counts => {
      this.orderStatusCounts = counts;

      this.totalOrders = Object.values(counts).reduce((sum, count) => sum + count, 0);
    });
  }

  addOrder() {
    this.router.navigate(['/orders/add'])
  }

  reloadOrder() {
    this.allSiteOrderComp.getOrders();
  }

  printOrders() {
    const selectedOrders = this.allSiteOrderComp.selectedOrders;
    
    if (!selectedOrders || selectedOrders.length === 0) {
      // Có thể show thông báo cho user
      return;
    }
    this.orderService.exportOrdersToExcel(selectedOrders).subscribe(blob => {
      saveAs(blob, 'orders.xlsx');
    });
  }
}
