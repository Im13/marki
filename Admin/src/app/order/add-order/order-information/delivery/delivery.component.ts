import { Component } from '@angular/core';

@Component({
  selector: 'app-delivery',
  templateUrl: './delivery.component.html',
  styleUrls: ['./delivery.component.css']
})
export class DeliveryComponent {
  deliveryCompany: string;
  shipmentCode: string;
  shipmentCost: string;
}
