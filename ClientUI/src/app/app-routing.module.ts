import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  { path: '', loadChildren: () => import('./home/home.module').then(m => m.HomeModule), data: { breadcrumb: 'Home'} },
  { path: 'new-arrivals', loadChildren: () => import('./new-arrivals/new-arrivals.module').then(m => m.NewArrivalsModule), data: { breadcrumb: 'New arrivals'}},
  { path: 'collections', loadChildren: () => import('./collections/collections.module').then(m => m.CollectionsModule), data: { breadcrumb: 'Collection'}}
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {scrollPositionRestoration: 'enabled'})],
  exports: [RouterModule]
})
export class AppRoutingModule { }
