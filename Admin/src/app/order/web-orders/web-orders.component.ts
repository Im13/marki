import { Component, ViewChild } from '@angular/core';
import { AllSiteOrdersComponent } from './all-site-orders/all-site-orders.component';

@Component({
  selector: 'app-web-orders',
  templateUrl: './web-orders.component.html',
  styleUrls: ['./web-orders.component.css']
})
export class WebOrdersComponent {
  @ViewChild(AllSiteOrdersComponent) allSiteOrderComp:AllSiteOrdersComponent;

  addOrder(){}

  reloadOrder() {
    this.allSiteOrderComp.getOrders();
  }
}
