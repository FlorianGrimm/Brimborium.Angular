import { Routes } from '@angular/router';
import { HomeIndex } from './page/home/home-index/home-index';
import { ProjectRestIndex } from './page/project-rest/project-rest-index/project-rest-index';
import { ProjectSignalrIndex } from './page/project-signalr/project-signalr-index/project-signalr-index';

export const routes: Routes = [
  { path: "", pathMatch: 'full', redirectTo: "ui" },
  { path: "ui", title:"Home", component: HomeIndex },
  { path: "ui/project-rest", title:"Project Rest", component: ProjectRestIndex },
  { path: "ui/project-signalr", title:"Project SignalR", component: ProjectSignalrIndex },
];
