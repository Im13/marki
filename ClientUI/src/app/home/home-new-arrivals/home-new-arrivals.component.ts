import { Component, OnInit } from '@angular/core';
import { HomeService } from '../home.service';
import { Product } from 'src/app/_shared/_models/product';
import { ProductParams } from 'src/app/_shared/_models/productParams';
import { Router } from '@angular/router';
import { ProductUtilityService } from 'src/app/_shared/_services/product-utility.service';

@Component({
  selector: 'app-home-new-arrivals',
  templateUrl: './home-new-arrivals.component.html',
  styleUrls: ['./home-new-arrivals.component.css']
})
export class HomeNewArrivalsComponent implements OnInit {
  products: readonly Product[] = [];
  productParams = new ProductParams();

  constructor(
    private homeService: HomeService, 
    private router: Router,
    private productUtilityService: ProductUtilityService
  ){}

  ngOnInit(): void {
    this.homeService.getNewArrivals(this.productParams).subscribe({
      next: response => {
        //Get first 4 products to display
        this.products = response.data.slice(1,5);
      },
      error: err => {
        console.log(err);
      }
    })
  }

  onViewMoreClick() {
    this.router.navigate(['/new-arrivals']);
  }

  redirectToProduct(slug: string) {
    this.router.navigate([`products/${slug}`])
  }

  getMainPhotoUrl(product: Product): string {
    return this.productUtilityService.getMainPhotoUrl(product);
  }
}
