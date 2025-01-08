import type { ActivatedRouteSnapshot, Routes } from '@angular/router';
import { DesignerHomeComponent } from './designer-home/designer-home.component';
import { DesignerStartComponent } from './designer-start/designer-start.component';
import { DesignerTextWorksComponent } from './designer-text-works/designer-text-works.component';
import { DesignerDefinitionsComponent } from './designer-definitions/designer-definitions.component';
import { DesignerSettingsComponent } from './designer-settings/designer-settings.component';
import { DesignerListObjectComponent } from './designer-list-object/designer-list-object.component';
import { DesignerObjectLookupComponent } from './designer-object-lookup/designer-object-lookup.component';
import { DesignerObjectComponent } from './designer-object/designer-object.component';
import { DesignerPageNotFoundComponent } from './designer-page-not-found/designer-page-not-found.component';

export const designerRoutes: Routes = [
  { path:'designer', component:DesignerHomeComponent,
    children:[
      {path: '', component:DesignerStartComponent},
      {path: 'textworks', component:DesignerTextWorksComponent},
      {path: 'definitions', component:DesignerDefinitionsComponent},
      {path: 'settings', component:DesignerSettingsComponent},
      {path: 'list', component:DesignerStartComponent},
      {path: 'list/:metaName', component:DesignerListObjectComponent},
      {path: 'object', component:DesignerStartComponent},
      {path: 'object/:id', component:DesignerObjectLookupComponent},
      {path: 'object/:metaName}/:id', component:DesignerObjectComponent},
      {path: '**', component:DesignerPageNotFoundComponent},
    ]
  },
];
