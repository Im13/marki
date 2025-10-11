import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, retry, throwError } from 'rxjs';
import { RecommendationDto } from 'src/app/_shared/_models/recommendationDTO';
import { environment } from 'src/environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class RecommendationService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  /**
   * Lấy recommendations cho user
   */
  getRecommendations(limit: number = 10): Observable<RecommendationDto[]> {
    const params = new HttpParams().set('limit', limit.toString());
    
    return this.http.get<RecommendationDto[]>(this.apiUrl, { 
      params,
      withCredentials: true // Important: Include SessionId cookie
    }).pipe(
      retry(2), // Retry 2 times nếu fail
      catchError(this.handleError)
    );
  }

  /**
   * Lấy trending products
   */
  getTrendingProducts(limit: number = 10): Observable<RecommendationDto[]> {
    const params = new HttpParams().set('limit', limit.toString());
    
    return this.http.get<RecommendationDto[]>(`${this.apiUrl}trending`, {
      params,
      withCredentials: true
    }).pipe(
      retry(2),
      catchError(this.handleError)
    );
  }

  /**
   * Lấy similar products
   */
  getSimilarProducts(productId: number, limit: number = 8): Observable<RecommendationDto[]> {
    const params = new HttpParams().set('limit', limit.toString());
    
    return this.http.get<RecommendationDto[]>(`${this.apiUrl}recommendations/similar/${productId}`, {
      params,
      withCredentials: true
    }).pipe(
      retry(2),
      catchError(this.handleError)
    );
  }

  private handleError(error: any) {
    console.error('Recommendation service error:', error);
    return throwError(() => new Error('Failed to load recommendations'));
  }
}
