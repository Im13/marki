import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AllProductsService } from './all-products.service';
import { Product } from '../_shared/_models/product';
import { Photo } from '../_shared/_models/photo';
import { ProductSKU } from '../_shared/_models/productSKU';
import { BasketService } from '../basket/basket.service';

@Component({
  selector: 'app-all-products',
  templateUrl: './all-products.component.html',
  styleUrls: ['./all-products.component.css']
})
export class AllProductsComponent implements OnInit {
  productSlug: string;
  product: Product;
  productPhotos: Photo[] = [];
  productDescription: string = '';
  selectedProductSKU: ProductSKU;
  selectedQuantity: number;

  constructor(private route: ActivatedRoute, private allProductService: AllProductsService, private basketService: BasketService) {}

  ngOnInit(): void {
    // Lấy giá trị slug từ URL
    this.productSlug = this.route.snapshot.paramMap.get('slug');

    // Sử dụng slug để tìm sản phẩm
    this.allProductService.getProductBySlug(this.productSlug).subscribe({
      next: response => {
        console.log(response)
        this.product = response;
        this.productPhotos = this.product.photos;
        this.productDescription = this.product.description;
      },
      error: err => {
        if(err.status == 404) {
          console.log("Cannot find product");
        } else if (err.status == 400) {
          console.log(err);
        }
      }
    });
  }

  optionSelected(event: ProductSKU) {
    this.selectedProductSKU = event;
  }

  quantitySelected(event: number) {
    this.selectedQuantity = event;
  }

  addToCart() {
    this.basketService.addItemToBasket(this.selectedProductSKU, this.product, this.selectedQuantity);
  }
}
