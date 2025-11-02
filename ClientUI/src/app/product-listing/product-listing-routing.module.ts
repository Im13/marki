import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductListingComponent } from './product-listing.component';
import { CollectionMapConst } from '../_shared/_consts/collectionsMapConst';

const routes: Routes = [
  // All Products
  { 
    path: 'all', 
    component: ProductListingComponent, 
    data: { 
      breadcrumb: 'All Products', 
      pageTitle: 'All Products',
      queryType: 'all',
      collectionId: CollectionMapConst['all']
    }
  },
  
  // New Arrivals
  { 
    path: 'new-arrivals', 
    component: ProductListingComponent, 
    data: { 
      breadcrumb: 'New Arrivals', 
      pageTitle: 'New Arrivals',
      queryType: 'new-arrivals',
      collectionId: null  // New arrivals không cần collectionId
    }
  },
  
  // Tops
  { 
    path: 'tops', 
    component: ProductListingComponent, 
    data: { 
      breadcrumb: 'Tops', 
      pageTitle: 'Tops',
      queryType: 'category',
      collectionId: CollectionMapConst['tops']
    }
  },
  
  // Bottoms
  { 
    path: 'bottoms', 
    component: ProductListingComponent, 
    data: { 
      breadcrumb: 'Bottoms', 
      pageTitle: 'Bottoms',
      queryType: 'category',
      collectionId: CollectionMapConst['bottoms']
    }
  },
  
  // Dresses
  { 
    path: 'dresses', 
    component: ProductListingComponent, 
    data: { 
      breadcrumb: 'Dresses', 
      pageTitle: 'Dresses',
      queryType: 'category',
      collectionId: CollectionMapConst['dresses']
    }
  },
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class ProductListingRoutingModule { }
