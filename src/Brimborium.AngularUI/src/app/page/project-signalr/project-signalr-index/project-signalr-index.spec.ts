import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ProjectSignalrIndex } from './project-signalr-index';

describe('ProjectSignalrIndex', () => {
  let component: ProjectSignalrIndex;
  let fixture: ComponentFixture<ProjectSignalrIndex>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProjectSignalrIndex],
    }).compileComponents();

    fixture = TestBed.createComponent(ProjectSignalrIndex);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
