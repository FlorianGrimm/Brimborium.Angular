import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DynamicToolbarComponent } from './dynamictoolbar.component';

describe('DynamictoolbarComponent', () => {
  let component: DynamicToolbarComponent;
  let fixture: ComponentFixture<DynamicToolbarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DynamicToolbarComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DynamicToolbarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
