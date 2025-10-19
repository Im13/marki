import { Injectable } from '@angular/core';
import { Product } from '../_models/product';

@Injectable({
  providedIn: 'root'
})
export class ProductUtilityService {

  constructor() { }

  /**
   * Lấy URL của photo chính (isMain = true) của một product
   * Nếu không có photo chính, sẽ fallback về photo đầu tiên
   * @param product - Product cần lấy photo
   * @returns URL của photo hoặc chuỗi rỗng nếu không có photo nào
   */
  getMainPhotoUrl(product: Product): string {
    if (!product.photos || product.photos.length === 0) {
      return '';
    }

    const mainPhoto = product.photos.find(p => p.isMain);
    return mainPhoto ? mainPhoto.url : product.photos[0]?.url || '';
  }

  /**
   * Kiểm tra xem product có photo chính không
   * @param product - Product cần kiểm tra
   * @returns true nếu có photo chính, false nếu không
   */
  hasMainPhoto(product: Product): boolean {
    if (!product.photos || product.photos.length === 0) {
      return false;
    }
    return product.photos.some(p => p.isMain);
  }
}
