import { TestBed } from '@angular/core/testing';

import { RxUtilsService } from './rxutils.service';

describe('RxutilsService', () => {
  let service: RxUtilsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RxUtilsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
