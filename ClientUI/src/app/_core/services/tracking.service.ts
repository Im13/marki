import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TrackingService {
  private apiUrl = `${environment.apiUrl}recommendations/track`;
  private viewStartTime: number | null = null;
  private currentProductId: number | null = null;

  constructor(private http: HttpClient) {
    this.setupUnloadListener();
  }

  /**
   * Track product view
   */
  trackProductView(productId: number): void {
    this.currentProductId = productId;
    this.viewStartTime = Date.now();
  }

  /**
   * Track add to cart
   */
  trackAddToCart(productId: number, skuId?: number): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/cart`, {
      productId,
      skuId
    }, {
      withCredentials: true
    });
  }

  /**
   * Track purchase
   */
  trackPurchase(productIds: number[]): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/purchase`, productIds, {
      withCredentials: true
    });
  }

  /**
   * Track recommendation click
   */
  trackRecommendationClick(productId: number, reasonCode: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/click`, {
      productId,
      reasonCode
    }, {
      withCredentials: true
    });
  }

  /**
   * Send view tracking when user leaves
   */
  private setupUnloadListener(): void {
    window.addEventListener('beforeunload', () => {
      if (this.viewStartTime && this.currentProductId) {
        const durationSeconds = Math.floor((Date.now() - this.viewStartTime) / 1000);

        // Use sendBeacon for guaranteed delivery
        const data = JSON.stringify({
          productId: this.currentProductId,
          durationSeconds
        });

        navigator.sendBeacon(
          `${environment.apiUrl}/recommendations/track/view`,
          new Blob([data], { type: 'application/json' })
        );
      }
    });
  }
}
