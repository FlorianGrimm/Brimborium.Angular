import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DesignerHomeComponent } from './designer-home.component';

describe('DesignerHomeComponent', () => {
  let component: DesignerHomeComponent;
  let fixture: ComponentFixture<DesignerHomeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DesignerHomeComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DesignerHomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
