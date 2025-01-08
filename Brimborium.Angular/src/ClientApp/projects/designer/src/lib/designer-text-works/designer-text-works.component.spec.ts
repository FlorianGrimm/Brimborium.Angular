import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DesignerTextWorksComponent } from './designer-text-works.component';

describe('DesignerTextWorksComponent', () => {
  let component: DesignerTextWorksComponent;
  let fixture: ComponentFixture<DesignerTextWorksComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DesignerTextWorksComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DesignerTextWorksComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
