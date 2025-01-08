import { TestBed } from '@angular/core/testing';

import { DesignerNavigationService } from './designer-navigation.service';

describe('DesignerNavigationService', () => {
  let service: DesignerNavigationService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DesignerNavigationService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
