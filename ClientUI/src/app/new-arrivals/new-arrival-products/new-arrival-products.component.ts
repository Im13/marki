import { Component } from '@angular/core';
import { NewArrivalsService } from '../new-arrivals.service';
import { Product } from 'src/app/_shared/_models/product';
import { ProductParams } from 'src/app/_shared/_models/productParams';

@Component({
  selector: 'app-new-arrival-products',
  templateUrl: './new-arrival-products.component.html',
  styleUrls: ['./new-arrival-products.component.css']
})
export class NewArrivalProductsComponent {
  products: readonly Product[] = [];
  productParams = new ProductParams();

  constructor(private newArrivalsService: NewArrivalsService){}

  ngOnInit(): void {
    this.newArrivalsService.getNewArrivals(this.productParams).subscribe({
      next: response => {
        //Get first 4 products to display
        this.products = response.data;
      },
      error: err => {
        console.log(err);
      }
    })
  }
}
