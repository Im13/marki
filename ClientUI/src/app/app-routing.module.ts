import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  { path: '', loadChildren: () => import('./home/home.module').then(m => m.HomeModule), data: { breadcrumb: 'Home' } },
  // Tất cả products routes đều qua product-listing module
  {
    path: '',
    loadChildren: () => import('./product-listing/product-listing-routing.module').then(m => m.ProductListingRoutingModule)
  },
  { path: 'products', loadChildren: () => import('./product-detail/product-detail.module').then(m => m.ProductDetailModule), data: { breadcrumb: 'Products' } },
  { path: 'basket', loadChildren: () => import('./basket/basket.module').then(m => m.BasketModule), data: { breadcrumb: 'Basket' } },
  { path: 'checkout', loadChildren: () => import('./checkout/checkout.module').then(m => m.CheckoutModule), data: { breadcrumb: 'Checkout' } }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { scrollPositionRestoration: 'enabled' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
