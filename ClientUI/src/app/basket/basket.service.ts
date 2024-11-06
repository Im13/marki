import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { environment } from 'src/environments/environment.development';
import { Basket, BasketItem, BasketTotals } from '../_shared/_models/basket';
import { HttpClient } from '@angular/common/http';
import { Product } from '../_shared/_models/product';
import { ProductSKU } from '../_shared/_models/productSKU';

@Injectable({
  providedIn: 'root'
})
export class BasketService {
  baseUrl = environment.apiUrl;
  private basketSource = new BehaviorSubject<Basket | null>(null);
  basketSource$ = this.basketSource.asObservable(); 
  private basketTotalSource = new BehaviorSubject<BasketTotals | null>(null);
  basketTotalSource$ = this.basketTotalSource.asObservable();

  constructor(private http: HttpClient) { }

  getBasket(id: string) {
    return this.http.get<Basket>(this.baseUrl + 'basket?id=' + id).subscribe({
      next: basket => {
        console.log('Basket: ', basket)
        this.basketSource.next(basket);
        this.calculateTotals();
      }
    });
  }

  setBasket(basket: Basket) {
    console.log('Before push: ',basket);
    return this.http.post<Basket>(this.baseUrl + 'basket', basket).subscribe({
      next: basket => {
        this.basketSource.next(basket);
        this.calculateTotals();
      }
    });
  }

  getCurrentBasketValue() {
    return this.basketSource.value;
  }

  addItemToBasket(item: ProductSKU | BasketItem, product: Product, quantity: number) {
    if(this.isProductSKU(item)) item = this.mapProductItemToBasketItem(item, product.name, product.photos[0].url);

    console.log('Items: ', item)
    
    const basket = this.getCurrentBasketValue() ?? this.createBasket();
    basket.items = this.addOrUpdateItem(basket.items, item, quantity);
    console.log('Basket 2:', basket)
    this.setBasket(basket);
  }

  removeItemFromBasket(id: number, quantity = 1) {
    const basket = this.getCurrentBasketValue();
    if(!basket) return;

    const item = basket.items.find(x => x.id === id);
    if(item) {
      item.quantity -= quantity;
      if(item.quantity === 0) {
        basket.items = basket.items.filter(x => x.id !== id);
      }

      if(basket.items.length > 0) this.setBasket(basket);
      else this.deleteBasket(basket);
    }
  }

  deleteBasket(basket: Basket) {
    return this.http.delete(this.baseUrl + 'basket?id=' + basket.id).subscribe({
      next: () => {
        this.deleteLocalBasket();
      }
    });
  }

  deleteLocalBasket() {
    this.basketSource.next(null);
    this.basketTotalSource.next(null);
    localStorage.removeItem('basket_id');
  }

  private addOrUpdateItem(items: BasketItem[], itemToAdd: BasketItem, quantity: number): BasketItem[] {
    const item = items.find(x => x.id === itemToAdd.id);
    if(item) item.quantity += quantity;
    else {
      itemToAdd.quantity = quantity;
      items.push(itemToAdd);
    }

    return items;
  }

  private createBasket(): Basket {
    const basket = new Basket();
    localStorage.setItem('basket_id', basket.id);
    return basket;
  }

  private mapProductItemToBasketItem(item: ProductSKU, productName: string, imageUrl: string) : BasketItem {
    return {
      id: item.id,
      productName: productName,
      price: item.price,
      quantity: 0,
      pictureUrl: imageUrl,
      sku: item.sku,
      productSKUValues: item.productSKUValues
    }
  }

  private calculateTotals() {
    const basket = this.getCurrentBasketValue();

    if(!basket) return;

    const subtotal = basket.items.reduce((a, b) => (b.price * b.quantity) + a, 0);

    const shipping = subtotal >= 700000 ? 0 : 30000;
    const total = subtotal + shipping;
    this.basketTotalSource.next({shipping, subtotal, total});
  }

  private isProductSKU(item: ProductSKU | BasketItem): item is ProductSKU {
    return (item as ProductSKU) !== undefined;
  }
}
