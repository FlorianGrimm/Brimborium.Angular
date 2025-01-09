import { Component, inject, Input, OnDestroy, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatMenuModule } from '@angular/material/menu';
import { RouterModule } from '@angular/router';
import { DynamicToolbarComponent, DynamicToolbarService, ToolbarGroup, ToolbarItems } from 'dynamictoolbar';
import { BehaviorSubject, filter, Subscription, tap } from 'rxjs';

@Component({
  selector: 'app-root-toolbar',
  imports: [MatToolbarModule, MatButtonModule, MatIconModule, MatMenuModule, RouterModule, DynamicToolbarComponent],
  templateUrl: './root-toolbar.component.html',
  styleUrl: './root-toolbar.component.scss'
})
export class RootToolbarComponent implements OnInit, OnDestroy {
  subscription:Subscription=new Subscription();
  dynamicToolbarService = inject(DynamicToolbarService);

  toolbarGroupOne: ToolbarGroup={id:"one", order:1};
  toolbarGroupOne$ = new BehaviorSubject<ToolbarItems>([]);

  toolbarGroupTwo: ToolbarGroup={id:"two", order:2};
  toolbarGroupTwo$ = new BehaviorSubject<ToolbarItems>([]);

  private _tbOne$ = new BehaviorSubject<TemplateRef<unknown> | undefined>(undefined);
  public get tbOne(): TemplateRef<unknown> | undefined {
    return this._tbOne$.getValue();
  }
  //@ViewChild(TemplateRef) myFragment: TemplateRef<unknown> | undefined;
  //@Input('tbOne')
  @ViewChild('tbOne')
  public set tbOne(value: TemplateRef<unknown> | undefined) {
    this._tbOne$.next(value);
  }

  private _tbTwo$ = new BehaviorSubject<TemplateRef<unknown> | undefined>(undefined);
  public get tbTwo(): TemplateRef<unknown> | undefined {
    return this._tbTwo$.getValue();
  }

  //@Input('tbTwo')
  @ViewChild('tbTwo')
  public set tbTwo(value: TemplateRef<unknown> | undefined) {
    this._tbTwo$.next(value);
  }

  constructor() {
  }

  ngOnInit(): void {
    this.subscription.add(this.dynamicToolbarService.registerToolbarGroup(this.toolbarGroupOne, this.toolbarGroupOne$));
    this.subscription.add(this.dynamicToolbarService.registerToolbarGroup(this.toolbarGroupTwo, this.toolbarGroupTwo$));

    this._tbOne$.pipe(
      tap({
        subscribe:()=>{console.log('subscribe');},
        next:(value)=>{console.log('next',value);},
        complete:()=>{console.log('complete');},
        error:(err)=>{console.log('error', err);},
        unsubscribe:()=>{console.log('unsubscribe');}
      }),
      filter(item=>item!==undefined)
    ).subscribe({
      next:(value)=> {
        this.toolbarGroupOne$.next([
          {
            id:"1",
            title:"1",
            template: this.tbOne
          }
        ]);
      },
    });

    this._tbTwo$.pipe(
      tap({
        subscribe:()=>{console.log('subscribe');},
        next:(value)=>{console.log('next',value);},
        complete:()=>{console.log('complete');},
        error:(err)=>{console.log('error', err);},
        unsubscribe:()=>{console.log('unsubscribe');}
      }),
      filter(item=>item!==undefined)
    ).subscribe({
      next:(value)=> {
        this.toolbarGroupTwo$.next([
          {
            id:"2",
            title:"2",
            template: this.tbTwo
          }
        ]);
      },
    });

  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

}
