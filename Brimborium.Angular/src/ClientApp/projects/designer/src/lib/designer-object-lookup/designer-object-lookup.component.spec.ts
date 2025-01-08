import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DesignerObjectLookupComponent } from './designer-object-lookup.component';

describe('DesignerObjectLookupComponent', () => {
  let component: DesignerObjectLookupComponent;
  let fixture: ComponentFixture<DesignerObjectLookupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DesignerObjectLookupComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DesignerObjectLookupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
