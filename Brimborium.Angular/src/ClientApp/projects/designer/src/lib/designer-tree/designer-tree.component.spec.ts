import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DesignerTreeComponent } from './designer-tree.component';

describe('DesignerTreeComponent', () => {
  let component: DesignerTreeComponent;
  let fixture: ComponentFixture<DesignerTreeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DesignerTreeComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DesignerTreeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
