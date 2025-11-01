import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { Product } from 'src/app/_shared/_models/product';
import { ProductParams } from 'src/app/_shared/_models/productParams';
import { ProductListingService } from '../product-listing.service';

@Component({
  selector: 'app-product-items',
  templateUrl: './product-items.component.html',
  styleUrls: ['./product-items.component.css']
})
export class ProductItemsComponent implements OnInit, OnChanges {
  @Input() collectionId: number | null = null;
  @Input() queryType: 'all' | 'new-arrivals' | 'category' = 'category';
  
  products: Product[] = [];
  productParams = new ProductParams();
  loading = false;
  error: string | null = null;

  constructor(private productListingService: ProductListingService) {}

  ngOnInit(): void {
    this.loadProducts();
  }

  ngOnChanges(changes: SimpleChanges): void {
    // Reload products khi collectionId hoặc queryType thay đổi
    if (changes['collectionId'] || changes['queryType']) {
      this.loadProducts();
    }
  }

  loadProducts(): void {
    this.loading = true;
    this.error = null;
    
    // Xử lý query khác nhau tùy theo queryType
    switch (this.queryType) {
      case 'all':
        // Lấy tất cả sản phẩm (typeId = 0 hoặc không truyền)
        this.productParams.typeId = 0;
        break;
      
      case 'category':
        // Lấy sản phẩm theo category
        if (this.collectionId !== null && this.collectionId !== 0) {
          this.productParams.typeId = this.collectionId;
        } else {
          this.productParams.typeId = 0;
        }
        break;
      
      case 'new-arrivals':
        // Lấy sản phẩm mới (có thể thêm sort by date nếu API support)
        this.productParams.typeId = 0;
        // TODO: Thêm sort by createdDate nếu API có support
        break;
    }

    console.log('Loading products with params:', this.productParams);

    this.productListingService.getProducts(this.productParams).subscribe({
      next: response => {
        this.products = response.data;
        this.loading = false;
        console.log('Loaded products:', this.products.length);
      },
      error: err => {
        console.error('Error loading products:', err);
        this.error = 'Không thể tải sản phẩm. Vui lòng thử lại sau.';
        this.loading = false;
      }
    });
  }
}
