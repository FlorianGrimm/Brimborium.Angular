import { Injectable } from '@angular/core';
import { BehaviorSubject, delay, Observable, Subscription } from 'rxjs';

import type {
   MenuGroup, MenuGroupItems, MenuItems,
   ToolbarGroup, ToolbarGroupItems, ToolbarItems
 } from './type';

@Injectable({
  providedIn: 'root'
})
export class DynamicToolbarService {

  constructor() { }

  // MainMenu

  public readonly mapMainMenuGroups = new Map<string, Readonly<MenuGroupItems>>;
  public readonly lstMainMenuGroups = new BehaviorSubject<Readonly<MenuGroupItems[]>>([]);

  registerMainMenuGroup(menuGroup: MenuGroup, value$: Observable<MenuItems>): Subscription {
    const result = new Subscription();
    this.addMainMenuGroup(menuGroup);
    var valueSubscription = value$.pipe(
      delay(1)
    ).subscribe({
      next: (value) => {
        this.setMainMenuGroup(menuGroup, value);
      },
      complete: () => {
        this.removeMainMenuGroup(menuGroup);
      },
      error: (error) => { }
    });
    result.add(valueSubscription);
    return result;
  }
  private addMainMenuGroup(menuGroup: MenuGroup) {
    const prev = this.lstMainMenuGroups.getValue();
    const menuGroupItems: MenuGroupItems = { index: prev.length, menuGroup: Object.freeze(menuGroup), items: [] };
    this.mapMainMenuGroups.set(menuGroup.id, menuGroupItems);
    const added = [...this.lstMainMenuGroups.getValue(), menuGroupItems];
    added.sort((a, b) => {
      let c = a.menuGroup.order - b.menuGroup.order;
      if (c != 0) { return c; }
      c = a.index - b.index;
      return c;
    });
    const current = added
      .map((item, index) => ({ index: index, menuGroup: item.menuGroup, items: item.items }))
      .map(item => Object.freeze(item));
    const next = Object.freeze(current);
    next.forEach((item) => this.mapMainMenuGroups.set(item.menuGroup.id, item));
    this.lstMainMenuGroups.next(next);
  }
  private setMainMenuGroup(menuGroup: MenuGroup, value: MenuItems | undefined | null) {
    const prev = this.mapMainMenuGroups.get(menuGroup.id);
    if (prev) {
      const current: MenuGroupItems = ({
        index: prev.index,
        menuGroup: prev.menuGroup,
        items: Object.freeze<MenuItems>(value ?? [])
      });
      const next = Object.freeze(current);
      this.mapMainMenuGroups.set(next.menuGroup.id, next);
      const lstCurrent = this.lstMainMenuGroups.getValue().map((item, index) => (index == next.index) ? next : item)
      const lstNext = Object.freeze(lstCurrent);
      this.lstMainMenuGroups.next(lstNext);
    }
  }
  private removeMainMenuGroup(menuGroup: MenuGroup) {
    const prev = this.mapMainMenuGroups.get(menuGroup.id);
    if (prev){
      const lstCurrent = this.lstMainMenuGroups.getValue()
        .filter((item, index) => (index != prev.index))
        .map((item, index) => ({ index: index, menuGroup: item.menuGroup, items: item.items }))
        .map(item => Object.freeze(item));
        ;
      const lstNext = Object.freeze(lstCurrent);

      this.mapMainMenuGroups.delete(menuGroup.id);
      this.lstMainMenuGroups.next(lstNext);
    }
  }

  // Toolbar

  public readonly mapToolbarGroups = new Map<string, Readonly<ToolbarGroupItems>>;
  public readonly lstToolbarGroups = new BehaviorSubject<Readonly<ToolbarGroupItems[]>>([]);

  registerToolbarGroup(toolbarGroup: ToolbarGroup, value$: Observable<ToolbarItems>): Subscription {
    const result = new Subscription();
    this.addToolbarGroup(toolbarGroup);
    var valueSubscription = value$.pipe(
      delay(1)
    ).subscribe({
      next: (value) => {
        this.setToolbarGroup(toolbarGroup, value);
      },
      complete: () => {
        this.removeToolbarGroup(toolbarGroup);
      },
      error: (error) => { }
    });
    result.add(valueSubscription);
    return result;
  }
  private addToolbarGroup(toolbarGroup: ToolbarGroup) {
    const prev = this.lstToolbarGroups.getValue();
    const toolbarGroupItems: ToolbarGroupItems = { index: prev.length, toolbarGroup: Object.freeze(toolbarGroup), items: [] };
    this.mapToolbarGroups.set(toolbarGroup.id, toolbarGroupItems);
    const added = [...this.lstToolbarGroups.getValue(), toolbarGroupItems];
    added.sort((a, b) => {
      let c = a.toolbarGroup.order - b.toolbarGroup.order;
      if (c != 0) { return c; }
      c = a.index - b.index;
      return c;
    });
    const current = added
      .map((item, index) => ({ index: index, toolbarGroup: item.toolbarGroup, items: item.items }))
      .map(item => Object.freeze(item));
    const next = Object.freeze(current);
    next.forEach((item) => this.mapToolbarGroups.set(item.toolbarGroup.id, item));
    this.lstToolbarGroups.next(next);
  }
  private setToolbarGroup(toolbarGroup: ToolbarGroup, value: ToolbarItems | undefined | null) {
    const prev = this.mapToolbarGroups.get(toolbarGroup.id);
    if (prev) {
      const current: ToolbarGroupItems = ({
        index: prev.index,
        toolbarGroup: prev.toolbarGroup,
        items: Object.freeze<ToolbarItems>(value ?? [])
      });
      const next = Object.freeze(current);
      this.mapToolbarGroups.set(next.toolbarGroup.id, next);
      const lstCurrent = this.lstToolbarGroups.getValue().map((item, index) => (index == next.index) ? next : item)
      const lstNext = Object.freeze(lstCurrent);
      this.lstToolbarGroups.next(lstNext);
    }
  }
  private removeToolbarGroup(toolbarGroup: ToolbarGroup) {
    const prev = this.mapToolbarGroups.get(toolbarGroup.id);
    if (prev){
      const lstCurrent = this.lstToolbarGroups.getValue()
        .filter((item, index) => (index != prev.index))
        .map((item, index) => ({ index: index, toolbarGroup: item.toolbarGroup, items: item.items }))
        .map(item => Object.freeze(item));
        ;
      const lstNext = Object.freeze(lstCurrent);

      this.mapToolbarGroups.delete(toolbarGroup.id);
      this.lstToolbarGroups.next(lstNext);
    }
  }

  // pagemenu

  public readonly mapPageMenuGroups = new Map<string, Readonly<MenuGroupItems>>;
  public readonly lstPageMenuGroups = new BehaviorSubject<Readonly<MenuGroupItems[]>>([]);

  registerPageMenuGroup(menuGroup: MenuGroup, value$: Observable<MenuItems>): Subscription {
    const result = new Subscription();
    this.addPageMenuGroup(menuGroup);
    var valueSubscription = value$.pipe(
      delay(1)
    ).subscribe({
      next: (value) => {
        this.setPageMenuGroup(menuGroup, value);
      },
      complete: () => {
        this.removePageMenuGroup(menuGroup);
      },
      error: (error) => { }
    });
    result.add(valueSubscription);
    return result;
  }
  private addPageMenuGroup(menuGroup: MenuGroup) {
    const prev = this.lstPageMenuGroups.getValue();
    const menuGroupItems: MenuGroupItems = { index: prev.length, menuGroup: Object.freeze(menuGroup), items: [] };
    this.mapPageMenuGroups.set(menuGroup.id, menuGroupItems);
    const added = [...this.lstPageMenuGroups.getValue(), menuGroupItems];
    added.sort((a, b) => {
      let c = a.menuGroup.order - b.menuGroup.order;
      if (c != 0) { return c; }
      c = a.index - b.index;
      return c;
    });
    const current = added
      .map((item, index) => ({ index: index, menuGroup: item.menuGroup, items: item.items }))
      .map(item => Object.freeze(item));
    const next = Object.freeze(current);
    next.forEach((item) => this.mapPageMenuGroups.set(item.menuGroup.id, item));
    this.lstPageMenuGroups.next(next);
  }
  private setPageMenuGroup(menuGroup: MenuGroup, value: MenuItems | undefined | null) {
    const prev = this.mapPageMenuGroups.get(menuGroup.id);
    if (prev) {
      const current: MenuGroupItems = ({
        index: prev.index,
        menuGroup: prev.menuGroup,
        items: Object.freeze<MenuItems>(value ?? [])
      });
      const next = Object.freeze(current);
      this.mapPageMenuGroups.set(next.menuGroup.id, next);
      const lstCurrent = this.lstPageMenuGroups.getValue().map((item, index) => (index == next.index) ? next : item)
      const lstNext = Object.freeze(lstCurrent);
      this.lstPageMenuGroups.next(lstNext);
    }
  }
  private removePageMenuGroup(menuGroup: MenuGroup) {
    const prev = this.mapPageMenuGroups.get(menuGroup.id);
    if (prev){
      const lstCurrent = this.lstPageMenuGroups.getValue()
        .filter((item, index) => (index != prev.index))
        .map((item, index) => ({ index: index, menuGroup: item.menuGroup, items: item.items }))
        .map(item => Object.freeze(item));
        ;
      const lstNext = Object.freeze(lstCurrent);

      this.mapPageMenuGroups.delete(menuGroup.id);
      this.lstPageMenuGroups.next(lstNext);
    }
  }
}
