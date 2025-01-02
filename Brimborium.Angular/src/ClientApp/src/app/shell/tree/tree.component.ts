import { ChangeDetectionStrategy, Component } from '@angular/core';
import { MatTreeModule } from '@angular/material/tree';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { ArrayDataSource, DataSource } from '@angular/cdk/collections';

import { files } from './example-data';

// https://next.material.angular.io/components/tree/overview

/** File node data with possible child nodes. */
export interface FileNode {
  name: string;
  type: string;
  children?: FileNode[];
}

@Component({
  selector: 'app-tree',
  templateUrl: './tree.component.html',
  styleUrl: './tree.component.scss',
  standalone: true,
  imports: [MatTreeModule, MatButtonModule, MatIconModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TreeComponent {

  dataSource: DataSource<FileNode>;

  constructor() {
    this.dataSource = new ArrayDataSource(files);
  }

  childrenAccessor(node: FileNode) {
    return node.children ?? [];
  }

  hasChild(index: number, node: FileNode): boolean {
    return !!node.children && node.children.length > 0;
  }
}
