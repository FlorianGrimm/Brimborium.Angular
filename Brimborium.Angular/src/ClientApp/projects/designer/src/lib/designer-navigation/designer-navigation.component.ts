import { Component } from '@angular/core';

import { DesignerTreeComponent } from '../designer-tree/designer-tree.component';
import { Router, RouterModule } from '@angular/router';

import type { DesignerNavigationTreeNode } from '../types';

@Component({
  selector: 'dsng-designer-navigation',
  imports: [RouterModule,DesignerTreeComponent],
  templateUrl: './designer-navigation.component.html',
  styleUrl: './designer-navigation.component.css'
})
export class DesignerNavigationComponent {
  items: DesignerNavigationTreeNode[] = [
    { title: "TextWorks",
      commands: ['/designer/textworks'],
      children:[
        { title: "Thoughts", commands: ['/designer/list/Thoughts'] },
        { title: "Definition", commands: ['/designer/list/Definition'] },
        { title: "Documentation", commands: ['/designer/list/Documentation'] },
      ]
    },
    { title: "Definitions", commands: ['/designer/definitions'] ,
      children:[
        { title: "Entity", commands: ['/designer/list/Entity'] },
        { title: "Dataflow", commands: ['/designer/list/Dataflow'] },
        { title: "Action", commands: ['/designer/list/Action'] },
        { title: "Page", commands: ['/designer/list/Page'] },
        { title: "Navigation", commands: ['/designer/list/Navigation'] },
      ]
    },
    { title: "Settings", commands: ['/designer/settings'] ,
      children:[
        { title: "MetaDesigner", commands: ['/designer/list/MetaDesigner'] },
      ]
    },
  ];

  constructor(
    private router:Router
  ) {
  }

  handleClickNode = (node: DesignerNavigationTreeNode)=>{
    console.log(node);
    if (node.commands){
      this.router.navigate(node.commands, node.extras);
    }
    return false;
  }
}
