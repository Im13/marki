import { Component, Input, OnInit } from '@angular/core';
import { Product } from 'src/app/_shared/_models/product';
import { ProductParams } from 'src/app/_shared/_models/productParams';
import { CollectionsService } from '../product-listing.service';

@Component({
  selector: 'app-collection-products',
  templateUrl: './collection-products.component.html',
  styleUrls: ['./collection-products.component.css']
})
export class CollectionProductsComponent implements OnInit {
  @Input('collectionId') collectionId: number;
  products: Product[] = [];
  productParams = new ProductParams();

  constructor(private collectionService: CollectionsService) {}

  ngOnInit(): void {
    this.productParams.typeId = this.collectionId;

    this.collectionService.getProducts(this.productParams).subscribe({
      next: response => {
        this.products = response.data;
      },
      error: err => {
        console.log(err);
      }
    })
  }
}
