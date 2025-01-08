import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input, Output, OnDestroy, output } from '@angular/core';
import { MatTreeModule } from '@angular/material/tree';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { ArrayDataSource } from '@angular/cdk/collections';

import { DesignerNavigationTreeNode } from '../types';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'dsng-designer-tree',
  templateUrl: './designer-tree.component.html',
  styleUrl: './designer-tree.component.css',
  imports: [MatTreeModule, MatButtonModule, MatIconModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DesignerTreeComponent implements OnDestroy {
  private _items: DesignerNavigationTreeNode[] = [];
  items$ = new BehaviorSubject<DesignerNavigationTreeNode[]>([]);

  dataSource: ArrayDataSource<DesignerNavigationTreeNode> = new ArrayDataSource<DesignerNavigationTreeNode>(this.items$);

  clickNode = output<DesignerNavigationTreeNode>();

  constructor(
    private ref: ChangeDetectorRef
  ) {
  }

  ngOnDestroy(): void {
  }

  public get items(): DesignerNavigationTreeNode[] {
    return this.items$.getValue();
  }

  @Input('items')
  public set items(value: (DesignerNavigationTreeNode[]) | null | undefined) {
    this.items$.next(value ?? []);
  }


  childrenAccessor = (node: DesignerNavigationTreeNode) => node.children ?? [];

  hasChild = (_: number, node: DesignerNavigationTreeNode) => !!node.children && node.children.length > 0;

  trackByFn = (_:number, node: DesignerNavigationTreeNode) => node.title;

  handleClick=(node: DesignerNavigationTreeNode)=>{
    this.clickNode.emit(node);
  }
}
