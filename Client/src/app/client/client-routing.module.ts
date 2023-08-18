import { RouterModule, Routes } from "@angular/router";
import { HomeComponent } from "./home/home.component";
import { NgModule } from "@angular/core";

const routes: Routes = [
    {
        path: '',
        component: HomeComponent
    }
];

export const clientRouting = RouterModule.forChild(routes);

@NgModule({
    imports: [clientRouting],
    exports: [RouterModule]
})

export class ClientRoutingModule {}