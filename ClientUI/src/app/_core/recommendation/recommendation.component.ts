import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { RecommendationDto } from 'src/app/_shared/_models/recommendationDTO';
import { RecommendationService } from '../services/recommendation.service';
import { TrackingService } from '../services/tracking.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-recommendation',
  templateUrl: './recommendation.component.html',
  styleUrls: ['./recommendation.component.css']
})
export class RecommendationComponent implements OnInit, OnDestroy {
  @Input() type: 'general' | 'trending' | 'similar' = 'general';
  @Input() productId?: number; // Required for type='similar'
  @Input() limit: number = 10;
  @Input() columns: number = 4;
  @Input() showColors: boolean = true;
  @Input() showSizes: boolean = true;
  @Input() showStock: boolean = true;
  @Input() showReason: boolean = true;
  @Input() title?: string;

  recommendations: RecommendationDto[] = [];
  loading: boolean = false;
  error: string | null = null;

  private destroy$ = new Subject<void>();

  constructor(
    private recommendationService: RecommendationService,
    private trackingService: TrackingService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadRecommendations();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  loadRecommendations(): void {
    this.loading = true;
    this.error = null;

    let request$;

    switch (this.type) {
      case 'trending':
        request$ = this.recommendationService.getTrendingProducts(this.limit);
        break;
      case 'similar':
        if (!this.productId) {
          this.error = 'Product ID is required for similar recommendations';
          this.loading = false;
          return;
        }
        request$ = this.recommendationService.getSimilarProducts(this.productId, this.limit);
        break;
      default:
        request$ = this.recommendationService.getRecommendations(this.limit);
    }

    request$
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (data) => {
          this.recommendations = data;
          this.loading = false;
        },
        error: (error) => {
          this.error = 'Không thể tải gợi ý sản phẩm';
          this.loading = false;
          console.error('Failed to load recommendations:', error);
        }
      });
  }

  onProductClick(product: RecommendationDto): void {
    // Track click
    this.trackingService.trackRecommendationClick(product.productId, product.reasonCode)
      .subscribe();

    // Navigate to product detail
    this.router.navigate(['/products', product.productSlug]);
  }

  getGridClass(): string {
    return `grid-cols-${this.columns}`;
  }

  retry(): void {
    this.loadRecommendations();
  }

  /**
   * Get color hex code from color name
   */
  getColorCode(colorName: string): string {
    const colorMap: { [key: string]: string } = {
      'đỏ': '#FF0000',
      'red': '#FF0000',
      'xanh': '#0000FF',
      'blue': '#0000FF',
      'đen': '#000000',
      'black': '#000000',
      'trắng': '#FFFFFF',
      'white': '#FFFFFF',
      'vàng': '#FFFF00',
      'yellow': '#FFFF00',
      'hồng': '#FFC0CB',
      'pink': '#FFC0CB',
      'xám': '#808080',
      'gray': '#808080',
      'nâu': '#8B4513',
      'brown': '#8B4513',
      'cam': '#FFA500',
      'orange': '#FFA500'
    };

    return colorMap[colorName.toLowerCase()] || '#CCCCCC';
  }
}
