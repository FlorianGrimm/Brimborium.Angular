import { Component, inject, OnDestroy, OnInit, ViewContainerRef  } from '@angular/core';
import { AsyncPipe, JsonPipe } from '@angular/common';
import { NgTemplateOutlet, CommonModule } from '@angular/common';
import { TemplateRef } from '@angular/core';

import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatMenuModule } from '@angular/material/menu';
import { RouterModule } from '@angular/router';
import { BehaviorSubject, Subscription, tap } from 'rxjs';
import { MenuGroupItems,ToolbarGroupItems } from '../type';

import { DynamicToolbarService } from '../dynamictoolbar.service';

@Component({
  selector: 'dyntb-dynamictoolbar',
  imports: [AsyncPipe, MatToolbarModule, MatButtonModule, MatIconModule, MatMenuModule, RouterModule, CommonModule],
  templateUrl: './dynamictoolbar.component.html',
  styleUrl: './dynamictoolbar.component.css'
})
export class DynamicToolbarComponent implements OnInit, OnDestroy {
  private subscription = new Subscription();
  private dynamicToolbarService = inject(DynamicToolbarService);

  public readonly mainMenuItems$ = new BehaviorSubject<Readonly<MenuGroupItems[]>>([]);
  public readonly toolbarItems$ = new BehaviorSubject<Readonly<ToolbarGroupItems[]>>([]);
  public readonly pageMenuItems$ = new BehaviorSubject<Readonly<MenuGroupItems[]>>([]);

  ngOnInit(): void {
    this.subscription.add(
      this.dynamicToolbarService.lstMainMenuGroups.pipe(
        tap({})
      ).subscribe({
        next: (value) => {
          this.mainMenuItems$.next(value);
        },
      })
    );
    this.subscription.add(
      this.dynamicToolbarService.lstToolbarGroups.pipe(
        tap({})
      ).subscribe({
        next: (value) => {
          this.toolbarItems$.next(value);
        },
      })
    );
    this.subscription.add(
      this.dynamicToolbarService.lstPageMenuGroups.pipe(
        tap({})
      ).subscribe({
        next: (value) => {
          this.pageMenuItems$.next(value);
        },
      })
    );
  }
  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

}
