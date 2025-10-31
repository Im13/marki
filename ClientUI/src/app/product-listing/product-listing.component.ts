import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-product-listing',
  templateUrl: './product-listing.component.html',
  styleUrls: ['./product-listing.component.css']
})
export class ProductListingComponent implements OnInit {
  collectionId: number;
  products: any[];

  constructor(
    private route: ActivatedRoute
    // private productService: ProductService
  ) { }

  ngOnInit(): void {
    // Truy cập collectionId từ route data
    this.collectionId = +this.route.snapshot.data['collectionId'];

    console.log(this.collectionId);
  }
}
