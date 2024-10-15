import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CollectionsComponent } from './collections.component';
import { CollectionMapConst } from '../_shared/_consts/collectionsMapConst';

const routes: Routes = [
  { path: '', redirectTo: '/', pathMatch: 'full' },
  { path: 'ao', component: CollectionsComponent, data: { breadcrumb: 'Áo', collectionId: CollectionMapConst['ao']}},
  { path: 'quan', component: CollectionsComponent, data: { breadcrumb: 'Quần', collectionId: CollectionMapConst['quan']}},
  { path: 'chan-vay', component: CollectionsComponent, data: { breadcrumb: 'Chân váy', collectionId: CollectionMapConst['chan-vay']}},
  { path: 'ao-khoac', component: CollectionsComponent, data: { breadcrumb: 'Áo khoác', collectionId: CollectionMapConst['ao-khoac']}},
  { path: '**', redirectTo: '/', pathMatch: 'full' }
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ]
})
export class CollectionsRoutingModule { }
