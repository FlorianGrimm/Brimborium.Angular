import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RootToolbarComponent } from './root-toolbar.component';

describe('RootToolbarComponent', () => {
  let component: RootToolbarComponent;
  let fixture: ComponentFixture<RootToolbarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RootToolbarComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RootToolbarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
