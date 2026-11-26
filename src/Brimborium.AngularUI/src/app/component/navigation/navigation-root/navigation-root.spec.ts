import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NavigationRoot } from './navigation-root';

describe('NavigationRoot', () => {
  let component: NavigationRoot;
  let fixture: ComponentFixture<NavigationRoot>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NavigationRoot],
    }).compileComponents();

    fixture = TestBed.createComponent(NavigationRoot);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
