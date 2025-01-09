import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RxutilsComponent } from './rxutils.component';

describe('RxutilsComponent', () => {
  let component: RxutilsComponent;
  let fixture: ComponentFixture<RxutilsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RxutilsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RxutilsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
