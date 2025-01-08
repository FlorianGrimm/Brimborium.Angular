import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

import { DesignerNavigationComponent } from '../designer-navigation/designer-navigation.component';

@Component({
  selector: 'dsng-designer-home',
  imports: [RouterOutlet,
    DesignerNavigationComponent],
  templateUrl: './designer-home.component.html',
  styleUrl: './designer-home.component.css'
})
export class DesignerHomeComponent {

}
