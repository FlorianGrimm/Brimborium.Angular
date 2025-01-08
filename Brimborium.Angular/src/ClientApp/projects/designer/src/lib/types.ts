import { NavigationExtras } from '@angular/router';

export type DesignerNavigationTreeNode = {
  title:string;
  commands?: any[],
  extras?: NavigationExtras;
  children?: DesignerNavigationTreeNode[];
};
