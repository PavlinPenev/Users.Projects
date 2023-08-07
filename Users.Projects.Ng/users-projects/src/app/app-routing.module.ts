import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './submodules/dashboard/components/dashboard/dashboard.component';

const routes: Routes = [
  {path: '', component: DashboardComponent,
  loadChildren: () =>
  import('./submodules/dashboard/dashboard.module').then((m) => m.DashboardModule)},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
