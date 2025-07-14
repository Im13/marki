import { Component, ViewChild } from '@angular/core';
import { AllSiteOrdersComponent } from './all-site-orders/all-site-orders.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-web-orders',
  templateUrl: './web-orders.component.html',
  styleUrls: ['./web-orders.component.css']
})
export class WebOrdersComponent {
  @ViewChild(AllSiteOrdersComponent) allSiteOrderComp:AllSiteOrdersComponent;

  constructor(private router: Router) {}

  addOrder(){
    this.router.navigate(['/orders/add'])
  }

  reloadOrder() {
    this.allSiteOrderComp.getOrders();
  }
}
