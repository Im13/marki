import { Component, OnInit } from '@angular/core';
import { OrderService } from '../order.service';

@Component({
  selector: 'app-shopee-orders',
  templateUrl: './shopee-orders.component.html',
  styleUrls: ['./shopee-orders.component.css']
})
export class ShopeeOrdersComponent implements OnInit {
  fileName = '';

  ngOnInit(): void {

  }

  constructor(private orderService: OrderService) {}

  onFileSelected(event) {
    const file: File = event.target.files[0];

    if(file) {
      this.orderService.readExcelFile(file);
    }
  }

}
