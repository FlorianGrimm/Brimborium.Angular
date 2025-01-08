import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DesignerStartComponent } from './designer-start.component';

describe('DesignerStartComponent', () => {
  let component: DesignerStartComponent;
  let fixture: ComponentFixture<DesignerStartComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DesignerStartComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DesignerStartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
