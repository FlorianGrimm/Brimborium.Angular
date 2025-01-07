import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DesignerObjectComponent } from './designer-object.component';

describe('DesignerObjectComponent', () => {
  let component: DesignerObjectComponent;
  let fixture: ComponentFixture<DesignerObjectComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DesignerObjectComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DesignerObjectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
