import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductListingComponent } from './product-listing.component';
import { CollectionMapConst } from '../_shared/_consts/collectionsMapConst';

const routes: Routes = [
  { path: '', redirectTo: '/', pathMatch: 'full' },
  { path: 'new-arrivals', 
    component: ProductListingComponent, 
    data: { 
      breadcrumb: 'New Arrivals', 
      pageTitle: 'New Arrivals',
      queryType: 'new-arrivals',
      collectionId: CollectionMapConst['ao']}
    },
  { path: 'all', 
    component: ProductListingComponent, 
    data: { 
      breadcrumb: 'All', 
      pageTitle: 'All',
      queryType: 'all',
      collectionId: CollectionMapConst['all']}
    },
  { path: 'tops', component: ProductListingComponent, data: { 
    breadcrumb: 'Tops', 
    pageTitle: 'Tops',
    queryType: 'tops',
    collectionId: CollectionMapConst['tops']}
  },
  { path: 'bottoms', component: ProductListingComponent, data: { 
    breadcrumb: 'Bottoms', 
    pageTitle: 'Bottoms',
    queryType: 'bottoms',
    collectionId: CollectionMapConst['bottoms']}
  },
  { path: 'dresses', component: ProductListingComponent, data: { 
    breadcrumb: 'Dresses', 
    pageTitle: 'Dresses',
    queryType: 'dresses',
    collectionId: CollectionMapConst['dresses']}
  },
  { path: '**', redirectTo: '/', pathMatch: 'full' }
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ]
})
export class ProductListingRoutingModule { }
