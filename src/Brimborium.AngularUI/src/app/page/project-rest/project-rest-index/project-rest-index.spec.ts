import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ProjectRestIndex } from './project-rest-index';

describe('ProjectRestIndex', () => {
  let component: ProjectRestIndex;
  let fixture: ComponentFixture<ProjectRestIndex>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProjectRestIndex],
    }).compileComponents();

    fixture = TestBed.createComponent(ProjectRestIndex);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
