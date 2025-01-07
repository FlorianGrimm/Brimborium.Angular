import { Component } from '@angular/core';
import { DesignerNavigationComponent } from '@app/designer/designer-navigation/designer-navigation.component'

@Component({
  selector: 'app-designer-home',
  imports: [DesignerNavigationComponent],
  templateUrl: './designer-home.component.html',
  styleUrl: './designer-home.component.scss'
})
export class DesignerHomeComponent {
}
