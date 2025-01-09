import { TestBed } from '@angular/core/testing';

import { DynamictoolbarService } from './dynamictoolbar.service';

describe('DynamictoolbarService', () => {
  let service: DynamictoolbarService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DynamictoolbarService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
