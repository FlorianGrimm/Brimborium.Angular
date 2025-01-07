import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DesignerNavigationComponent } from './designer-navigation.component';

describe('DesignerNavigationComponent', () => {
  let component: DesignerNavigationComponent;
  let fixture: ComponentFixture<DesignerNavigationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DesignerNavigationComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DesignerNavigationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
