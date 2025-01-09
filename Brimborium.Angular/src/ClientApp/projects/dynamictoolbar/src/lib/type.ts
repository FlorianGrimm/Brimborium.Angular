import { TemplateRef } from '@angular/core';

// menu
export type MenuGroup = {
  id: string;
  order: number;
}

export type MenuItems = MenuItem[];

export type MenuItem<T = any> = {
  id: string;
  title: string;

  template?: TemplateRef<T>;
  context?: T;
}

export type MenuGroupItems = {
  index: number;
  menuGroup: MenuGroup;
  items: Readonly<MenuItems>;
};


// toolbar
export type ToolbarGroup = {
  id: string;
  order: number;
}

export type ToolbarItems = ToolbarItem[];

export type ToolbarItem<T = any> = {
  id: string;
  title: string;

  template?: TemplateRef<T>;
  context?: T;
}

export type ToolbarGroupItems = {
  index: number;
  toolbarGroup: ToolbarGroup;
  items: Readonly<ToolbarItems>;
};
