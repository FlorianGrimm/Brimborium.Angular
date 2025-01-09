import { Component, inject, Input, OnDestroy, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatMenuModule } from '@angular/material/menu';
import { RouterModule } from '@angular/router';
import {
  DynamicToolbarComponent, DynamicToolbarService,
  MenuGroup, MenuItems,
  ToolbarGroup, ToolbarItems
} from 'dynamictoolbar';
import { BehaviorSubject, filter, Subscription, tap } from 'rxjs';

@Component({
  selector: 'app-root-toolbar',
  imports: [MatToolbarModule, MatButtonModule, MatIconModule, MatMenuModule, RouterModule, DynamicToolbarComponent],
  templateUrl: './root-toolbar.component.html',
  styleUrl: './root-toolbar.component.scss'
})
export class RootToolbarComponent implements OnInit, OnDestroy {
  subscription: Subscription = new Subscription();
  dynamicToolbarService = inject(DynamicToolbarService);

  private _tplMenuMainOne$ = new BehaviorSubject<TemplateRef<unknown> | undefined>(undefined);
  public get tplMenuMainOne(): TemplateRef<unknown> | undefined {
    return this._tplMenuMainOne$.getValue();
  }
  @ViewChild('tplMenuMainOne')
  public set tplMenuMainOne(value: TemplateRef<unknown> | undefined) {
    this._tplMenuMainOne$.next(value);
  }

  mainMenuGroupOne: MenuGroup = { id: "one", order: 1 };
  mainMenuGroupOne$ = new BehaviorSubject<MenuItems>([]);

  toolbarGroupOne: ToolbarGroup = { id: "one", order: 1 };
  toolbarGroupOne$ = new BehaviorSubject<ToolbarItems>([]);

  toolbarGroupTwo: ToolbarGroup = { id: "two", order: 2 };
  toolbarGroupTwo$ = new BehaviorSubject<ToolbarItems>([]);

  private _tplTbOne$ = new BehaviorSubject<TemplateRef<unknown> | undefined>(undefined);
  public get tplTbOne(): TemplateRef<unknown> | undefined {
    return this._tplTbOne$.getValue();
  }
  @ViewChild('tplTbOne')
  public set tplTbOne(value: TemplateRef<unknown> | undefined) {
    this._tplTbOne$.next(value);
  }

  private _tplTbTwo$ = new BehaviorSubject<TemplateRef<unknown> | undefined>(undefined);
  public get tplTbTwo(): TemplateRef<unknown> | undefined {
    return this._tplTbTwo$.getValue();
  }

  @ViewChild('tplTbTwo')
  public set tplTbTwo(value: TemplateRef<unknown> | undefined) {
    this._tplTbTwo$.next(value);
  }


  constructor() {
  }

  ngOnInit(): void {
    this.subscription.add(this.dynamicToolbarService.registerMainMenuGroup(this.mainMenuGroupOne, this.mainMenuGroupOne$));
    this.subscription.add(this.dynamicToolbarService.registerToolbarGroup(this.toolbarGroupOne, this.toolbarGroupOne$));
    this.subscription.add(this.dynamicToolbarService.registerToolbarGroup(this.toolbarGroupTwo, this.toolbarGroupTwo$));

    /*
        const menuMainMenuItemsOne: MenuItems = [{
          id: "menuMainMenuSwitch",
          title: "menuMainMenuSwitch",
          template: this.tplMenuMainOne
        }];
        this.mainMenuGroupOne$.next(menuMainMenuItemsOne);
    */

    this._tplMenuMainOne$.pipe(
      tap({
        subscribe: () => { console.log('subscribe'); },
        next: (value) => { console.log('next', value); },
        complete: () => { console.log('complete'); },
        error: (err) => { console.log('error', err); },
        unsubscribe: () => { console.log('unsubscribe'); }
      }),
      filter(item => item !== undefined)
    ).subscribe({
      next: (value) => {
        this.mainMenuGroupOne$.next([
          {
            id: "menuMainMenuSwitch",
            title: "menuMainMenuSwitch",
            template:value
          }
        ]);
      },
    });

    this._tplTbOne$.pipe(
      // tap({
      //   subscribe: () => { console.log('subscribe'); },
      //   next: (value) => { console.log('next', value); },
      //   complete: () => { console.log('complete'); },
      //   error: (err) => { console.log('error', err); },
      //   unsubscribe: () => { console.log('unsubscribe'); }
      // }),
      filter(item => item !== undefined)
    ).subscribe({
      next: (value) => {
        this.toolbarGroupOne$.next([
          {
            id: "1",
            title: "1",
            template:value
          }
        ]);
      },
    });

    this._tplTbTwo$.pipe(
      // tap({
      //   subscribe: () => { console.log('subscribe'); },
      //   next: (value) => { console.log('next', value); },
      //   complete: () => { console.log('complete'); },
      //   error: (err) => { console.log('error', err); },
      //   unsubscribe: () => { console.log('unsubscribe'); }
      // }),
      filter(item => item !== undefined)
    ).subscribe({
      next: (value) => {
        this.toolbarGroupTwo$.next([
          {
            id: "2",
            title: "2",
            template: value
          }
        ]);
      },
    });

  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

}
