import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';

interface RouteData {
  pageTitle: string;
  queryType: 'all' | 'new-arrivals' | 'category';
  collectionId?: number | null;
}

@Component({
  selector: 'app-product-listing',
  templateUrl: './product-listing.component.html',
  styleUrls: ['./product-listing.component.css']
})
export class ProductListingComponent implements OnInit, OnDestroy {
  collectionId: number | null = null;
  pageTitle: string = '';
  queryType: RouteData['queryType'] = 'category';
  
  private destroy$ = new Subject<void>();

  constructor(private route: ActivatedRoute) { }

  ngOnInit(): void {
    // Subscribe to route data changes để handle việc chuyển trang
    this.route.data
      .pipe(takeUntil(this.destroy$))
      .subscribe((data: RouteData) => {
        this.queryType = data.queryType || 'category';
        this.pageTitle = data.pageTitle || '';
        this.collectionId = data.collectionId ?? null;
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
