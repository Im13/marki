import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Product } from '../_shared/_models/product';
import { Photo } from '../_shared/_models/photo';
import { ProductSKU } from '../_shared/_models/productSKU';
import { BasketService } from '../basket/basket.service';
import { TrackingService } from '../_core/services/tracking.service';
import { Subject, takeUntil } from 'rxjs';
import { ProductDetailService } from './product-detail.service';

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.css']
})
export class ProductDetailComponent implements OnInit, OnDestroy {
  productSlug: string;
  product: Product;
  productPhotos: Photo[] = [];
  productDescription: string = '';
  selectedProductSKU: ProductSKU;
  selectedQuantity: number;
  private destroy$ = new Subject<void>();

  constructor(private route: ActivatedRoute, 
    private productDetailService: ProductDetailService, 
    private basketService: BasketService,
    private trackingService: TrackingService) {}

  ngOnInit(): void {
    // Lấy giá trị slug từ URL
    this.productSlug = this.route.snapshot.paramMap.get('slug');

    // Sử dụng slug để tìm sản phẩm
    this.productDetailService.getProductBySlug(this.productSlug).subscribe({
      next: response => {
        this.product = response;
        this.productPhotos = this.product.photos;
        this.productDescription = this.product.description;

        this.trackingService.trackProductView(this.product.id);
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

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  optionSelected(event: ProductSKU) {
    this.selectedProductSKU = event;
  }

  quantitySelected(event: number) {
    this.selectedQuantity = event;
  }

  addToCart() {
    this.basketService.addItemToBasket(this.selectedProductSKU, this.product, this.selectedQuantity);

    this.trackingService.trackAddToCart(this.product.id, this.selectedProductSKU.id)
    .pipe(
        takeUntil(this.destroy$)
    ).subscribe({
        next: () => console.log('Track ATC success'),
        error: (error) => console.error('Track ATC failed:', error)
    });
  }
}
