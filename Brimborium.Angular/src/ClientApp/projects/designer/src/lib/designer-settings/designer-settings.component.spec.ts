import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DesignerSettingsComponent } from './designer-settings.component';

describe('DesignerSettingsComponent', () => {
  let component: DesignerSettingsComponent;
  let fixture: ComponentFixture<DesignerSettingsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DesignerSettingsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DesignerSettingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
