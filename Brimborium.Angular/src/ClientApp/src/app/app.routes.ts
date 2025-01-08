import { Routes } from '@angular/router';
import { RootHomeComponent } from './root/root-home/root-home.component';
//import { DesignerHomeComponent } from 'designer';
//import { DesignerHomeComponent } from './designer/designer-home/designer-home.component';
import { designerRoutes } from 'designer';

const appRoutes: Routes = [
  {path:'', component:RootHomeComponent},
];

export const routes: Routes = [
  ...appRoutes,
  ...designerRoutes
];
