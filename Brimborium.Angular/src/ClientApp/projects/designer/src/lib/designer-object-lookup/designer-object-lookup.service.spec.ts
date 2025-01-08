import { TestBed } from '@angular/core/testing';

import { DesignerObjectLookupService } from './designer-object-lookup.service';

describe('DesignerObjectLookupService', () => {
  let service: DesignerObjectLookupService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DesignerObjectLookupService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
