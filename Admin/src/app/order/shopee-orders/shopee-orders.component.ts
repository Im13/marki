import { Component, OnInit } from '@angular/core';
import { OrderService } from '../order.service';
import { ShopeeOrder } from 'src/app/shared/models/shopeeOrder';

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
    var orders: ShopeeOrder[];
    const file: File = event.target.files[0];

    if(file) {
      orders = this.orderService.readExcelFile(file);

      if(orders) {
        // this.orderService.uploadShopeeOrdersFile(orders).subscribe({
        //   next: () => {
        //     console.log('done');
        //   }
        // });
      }
    }
  }

}
