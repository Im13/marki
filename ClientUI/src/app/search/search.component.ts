import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subject, Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { ProductListingService } from '../product-listing/product-listing.service';
import { Product } from '../_shared/_models/product';
import { ProductParams } from '../_shared/_models/productParams';
import { ProductUtilityService } from '../_shared/_services/product-utility.service';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit, OnDestroy {
  searchTerm$ = new Subject<string>();
  searchSubscription?: Subscription;
  
  products: Product[] = [];
  loading = false;
  error: string | null = null;
  searchTerm = '';
  hasSearched = false;

  constructor(
    private productListingService: ProductListingService,
    private productUtilityService: ProductUtilityService
  ) {}

  ngOnInit(): void {
    // Setup debounce cho search input
    this.searchSubscription = this.searchTerm$
      .pipe(
        debounceTime(500), // Đợi 500ms sau khi người dùng ngừng gõ
        distinctUntilChanged() // Chỉ search khi giá trị thay đổi
      )
      .subscribe(term => {
        this.performSearch(term);
      });
  }

  ngOnDestroy(): void {
    if (this.searchSubscription) {
      this.searchSubscription.unsubscribe();
    }
  }

  onSearchInput(event: Event): void {
    const value = (event.target as HTMLInputElement).value;
    this.searchTerm = value;
    this.searchTerm$.next(value);
  }

  performSearch(term: string): void {
    // Reset nếu search term rỗng
    if (!term || term.trim().length === 0) {
      this.products = [];
      this.hasSearched = false;
      this.error = null;
      return;
    }

    this.loading = true;
    this.error = null;
    this.hasSearched = true;

    const productParams = new ProductParams();
    productParams.search = term.trim();
    productParams.pageSize = 20; // Hiển thị 20 sản phẩm đầu tiên
    productParams.pageIndex = 1;
    productParams.typeId = 0; // Tìm trong tất cả categories

    this.productListingService.getProducts(productParams).subscribe({
      next: response => {
        this.products = response.data;
        // Debug: Kiểm tra photos
        console.log('=== SEARCH RESULTS ===');
        this.products.forEach((product, index) => {
          console.log(`Product ${index}: ${product.name}`, {
            hasPhotos: !!product.photos,
            photosLength: product.photos?.length || 0,
            photos: product.photos,
            photoUrl: this.getMainPhotoUrl(product)
          });
        });
        this.loading = false;
      },
      error: err => {
        console.error('Error searching products:', err);
        this.error = 'Có lỗi xảy ra khi tìm kiếm. Vui lòng thử lại.';
        this.loading = false;
        this.products = [];
      }
    });
  }

  clearSearch(): void {
    this.searchTerm = '';
    this.products = [];
    this.hasSearched = false;
    this.error = null;
  }

  /**
   * Lấy URL của photo chính (isMain = true) của product
   * Fallback về photo đầu tiên nếu không có photo chính
   */
  getMainPhotoUrl(product: Product): string {
    if (!product) {
      return '';
    }
    
    if (!product.photos || product.photos.length === 0) {
      return '';
    }
    
    const url = this.productUtilityService.getMainPhotoUrl(product);
    if (!url) {
      console.warn(`getMainPhotoUrl: No URL found for product "${product.name}"`, product.photos);
    }
    return url;
  }

  /**
   * Xử lý khi ảnh load thành công
   */
  onImageLoad(event: Event): void {
    const img = event.target as HTMLImageElement;
    if (img) {
      img.style.display = 'block';
    }
  }

  /**
   * Xử lý lỗi khi load ảnh - ngăn vòng lặp vô hạn
   */
  onImageError(event: Event): void {
    const img = event.target as HTMLImageElement;
    if (!img) return;
    
    // Ngăn vòng lặp: chỉ set placeholder một lần
    // Nếu đã là placeholder hoặc đã có data attribute, không làm gì
    if (img.dataset['errorHandled'] === 'true' || img.src.includes('data:image')) {
      // Ẩn ảnh nếu placeholder cũng không load được
      img.style.display = 'none';
      return;
    }
    
    // Đánh dấu đã xử lý error
    img.dataset['errorHandled'] = 'true';
    
    // Thử dùng data URL placeholder (1x1 transparent pixel) thay vì file
    img.src = 'data:image/svg+xml,%3Csvg xmlns=\'http://www.w3.org/2000/svg\' width=\'1\' height=\'1\'%3E%3C/svg%3E';
  }
}
