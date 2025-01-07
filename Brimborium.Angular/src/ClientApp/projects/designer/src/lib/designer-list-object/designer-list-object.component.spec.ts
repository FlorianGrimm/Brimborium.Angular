import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DesignerListObjectComponent } from './designer-list-object.component';

describe('DesignerListObjectComponent', () => {
  let component: DesignerListObjectComponent;
  let fixture: ComponentFixture<DesignerListObjectComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DesignerListObjectComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DesignerListObjectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
