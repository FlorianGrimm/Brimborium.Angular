import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import cfg from './cfg.json';
import { MatMenuModule } from '@angular/material/menu';
import { MatIconModule } from '@angular/material/icon';
import { MatSidenavModule } from '@angular/material/sidenav';
import { AsyncPipe } from '@angular/common';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatListModule } from '@angular/material/list';
import { Observable } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';
import { TreeComponent } from "./shell/tree/tree.component";
import { NavigationComponent } from "./shell/navigation/navigation.component";
import { RootToolbarComponent } from './root/root-toolbar/root-toolbar.component';


@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    MatMenuModule,
    MatIconModule,
    MatSidenavModule,
    MatToolbarModule,
    MatButtonModule,
    MatListModule,
    RootToolbarComponent
],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit {
  title = 'ClientApp';
  ngOnInit(): void {
    console.log(cfg);
    this.title = cfg.Greetings;
  }

}
