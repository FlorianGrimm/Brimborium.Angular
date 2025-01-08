import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DesignerDefinitionsComponent } from './designer-definitions.component';

describe('DesignerDefinitionsComponent', () => {
  let component: DesignerDefinitionsComponent;
  let fixture: ComponentFixture<DesignerDefinitionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DesignerDefinitionsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DesignerDefinitionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
