import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DesignerPageNotFoundComponent } from './designer-page-not-found.component';

describe('DesignerPageNotFoundComponent', () => {
  let component: DesignerPageNotFoundComponent;
  let fixture: ComponentFixture<DesignerPageNotFoundComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DesignerPageNotFoundComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DesignerPageNotFoundComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
