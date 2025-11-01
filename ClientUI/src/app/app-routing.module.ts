import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  { 
    path: '', 
    loadChildren: () => import('./home/home.module').then(m => m.HomeModule), 
    data: { breadcrumb: 'Home' },
    pathMatch: 'full'
  },
  
  // Product Detail
  { 
    path: 'products', 
    loadChildren: () => import('./product-detail/product-detail.module').then(m => m.ProductDetailModule), 
    data: { breadcrumb: 'Products' } 
  },
  
  // Basket
  { 
    path: 'basket', 
    loadChildren: () => import('./basket/basket.module').then(m => m.BasketModule), 
    data: { breadcrumb: 'Basket' } 
  },
  
  // Checkout
  { 
    path: 'checkout', 
    loadChildren: () => import('./checkout/checkout.module').then(m => m.CheckoutModule), 
    data: { breadcrumb: 'Checkout' } 
  },
  { 
    path: '', 
    loadChildren: () => import('./product-listing/product-listing.module').then(m => m.ProductListingModule)
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { scrollPositionRestoration: 'enabled' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
