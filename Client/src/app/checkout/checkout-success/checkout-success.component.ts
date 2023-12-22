import { Component, OnInit } from '@angular/core';
import { CheckoutService } from '../checkout.service';
import { Order } from 'src/app/shared/models/order';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-checkout-success',
  templateUrl: './checkout-success.component.html',
  styleUrls: ['./checkout-success.component.css']
})
export class CheckoutSuccessComponent implements OnInit {
  order: Order | undefined;

  constructor(private checkoutService: CheckoutService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    const routeParams = this.route.snapshot.paramMap;
    const orderId = routeParams.get('id');

    this.checkoutService.getOrderByIdOnly(+orderId).subscribe({
      next: order => {
        console.log(order);
        this.order = order;
      }
    });
  }

}
