import { Routes } from '@angular/router';
import { DesignerHomeComponent } from './designer/designer-home/designer-home.component';
import { RootHomeComponent } from './root/root-home/root-home.component';

export const routes: Routes = [
  {path:'', component:RootHomeComponent},
  {path:'designer', component:DesignerHomeComponent},
];
