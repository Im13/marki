import { AfterViewInit, Component, ElementRef, Input, OnChanges, OnDestroy, OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { Product } from 'src/app/_shared/_models/product';
import { ProductParams } from 'src/app/_shared/_models/productParams';
import { ProductListingService } from '../product-listing.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-product-items',
  templateUrl: './product-items.component.html',
  styleUrls: ['./product-items.component.css']
})
export class ProductItemsComponent implements OnInit, OnChanges, AfterViewInit, OnDestroy {
  @Input() collectionId: number | null = null;
  @Input() queryType: 'all' | 'new-arrivals' | 'category' = 'category';
  @ViewChild('scrollSentinel', { read: ElementRef }) scrollSentinel !: ElementRef;

  products: Product[] = [];
  productParams = new ProductParams();
  loading = false;
  error: string | null = null;

  // Biến cho infinite scroll
  hasMoreProducts = true;
  isLoadingMore = false;
  totalCount = 0;

  // Intersection Observer
  private observer: IntersectionObserver | null = null;

  constructor(private productListingService: ProductListingService, private router: Router) {
    // Đặt pageSize = 8 để chỉ lấy 8 sản phẩm mỗi lần
    this.productParams.pageSize = 8;
    this.productParams.pageIndex = 1;
  }

  ngOnDestroy(): void {
    if (this.observer) {
      this.observer.disconnect();
      this.observer = null;
    }
  }

  ngOnInit(): void {
    this.loadProducts();
  }

  ngAfterViewInit(): void {
    // Setup Intersection Observer sau khi view đã được render
    this.setupIntersectionObserver();
  }

  setupIntersectionObserver(): void {
    // Disconnect observer cũ nếu có
    if (this.observer) {
      this.observer.disconnect();
    }

    // Cấu hình observer
    const options: IntersectionObserverInit = {
      root: null, // null = viewport
      rootMargin: '200px', // Trigger trước khi đến sentinel 200px
      threshold: 0.1 // Trigger khi 10% sentinel visible
    };

    // Tạo observer mới
    this.observer = new IntersectionObserver((entries) => {
      entries.forEach(entry => {
        // Khi sentinel xuất hiện trong viewport
        if (entry.isIntersecting && !this.isLoadingMore && this.hasMoreProducts) {
          this.loadMoreProducts();
        }
      });
    }, options);

    // Bắt đầu observe sentinel element
    if (this.scrollSentinel && this.scrollSentinel.nativeElement) {
      this.observer.observe(this.scrollSentinel.nativeElement);
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    // Reload products khi collectionId hoặc queryType thay đổi
    if (changes['collectionId'] || changes['queryType']) {
      //Reset lại state khi filter thay đổi
      this.resetProductsState();
      this.loadProducts();

      // Setup lại observer sau khi load xong
      setTimeout(() => {
        this.setupIntersectionObserver();
      }, 100);
    }
  }

  resetProductsState() {
    this.products = [];
    this.productParams.pageIndex = 1;
    this.hasMoreProducts = true;
    this.totalCount = 0;
    this.isLoadingMore = false;
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
        this.productParams.typeId = 0;
        break;
    }

    this.productListingService.getProducts(this.productParams).subscribe({
      next: response => {
        this.products = response.data;
        this.totalCount = response.count;
        this.loading = false;

        // Kiểm tra xem còn sản phẩm để load không
        this.checkIfHasMore();
      },
      error: err => {
        console.error('Error loading products:', err);
        this.error = 'Không thể tải sản phẩm. Vui lòng thử lại sau.';
        this.loading = false;
      }
    });
  }

  loadMoreProducts(): void {
    // Kiểm tra điều kiện trước khi load
    if (this.isLoadingMore || !this.hasMoreProducts) {
      return;
    }

    this.isLoadingMore = true;
    this.productParams.pageIndex++;

    this.productListingService.getProducts(this.productParams).subscribe({
      next: response => {
        // Thêm sản phẩm mới vào danh sách hiện tại
        this.products = [...this.products, ...response.data];
        this.totalCount = response.count;
        this.isLoadingMore = false;

        // Kiểm tra xem còn sản phẩm để load không
        this.checkIfHasMore();
      },
      error: err => {
        console.error('Error loading more products:', err);
        this.isLoadingMore = false;
        // Giảm pageIndex về nếu load fail
        this.productParams.pageIndex--;
      }
    });
  }

  checkIfHasMore(): void {
    const loadedProducts = this.products.length;
    this.hasMoreProducts = loadedProducts < this.totalCount;
  }


  redirectToProduct(slug: string) {
    this.router.navigate([`products/${slug}`])
  }
}
