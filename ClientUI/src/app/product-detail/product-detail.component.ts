import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Product } from '../_shared/_models/product';
import { Photo } from '../_shared/_models/photo';
import { ProductSKU } from '../_shared/_models/productSKU';
import { BasketService } from '../basket/basket.service';
import { TrackingService } from '../_core/services/tracking.service';
import { BehaviorSubject, Subject, takeUntil } from 'rxjs';
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
  
  selectedProductSKU: ProductSKU;
  selectedQuantity: number;

  productId$ = new BehaviorSubject<number>(null);
  private destroy$ = new Subject<void>();

  constructor(private route: ActivatedRoute, 
    private productDetailService: ProductDetailService, 
    private basketService: BasketService,
    private trackingService: TrackingService) {}

  ngOnInit(): void {
    // Lấy giá trị slug từ URL
    // Subscribe to route params changes
    this.route.paramMap
      .pipe(takeUntil(this.destroy$))
      .subscribe(params => {
        this.productSlug = params.get('slug');
        this.loadProduct();
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private loadProduct() {
    this.productDetailService.getProductBySlug(this.productSlug)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: response => {
          this.product = response;
          this.productPhotos = this.product.photos;
          this.productId$.next(this.product.id);

          this.trackingService.trackProductView(this.product.id);
        },
        error: err => {
          if(err.status == 404) {
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
    // Kiểm tra xem đã có selectedProductSKU và selectedQuantity chưa
    if (!this.selectedProductSKU) {
      console.error('Chưa chọn sản phẩm SKU');
      return;
    }

    if (!this.selectedQuantity || this.selectedQuantity <= 0) {
      console.error('Số lượng không hợp lệ');
      return;
    }

    this.basketService.addItemToBasket(this.selectedProductSKU, this.product, this.selectedQuantity);

    this.trackingService.trackAddToCart(this.product.id, this.selectedProductSKU.id)
    .pipe(
        takeUntil(this.destroy$)
    ).subscribe({
        next: () => {},
        error: (error) => console.error('Track ATC failed:', error)
    });
  }
}
