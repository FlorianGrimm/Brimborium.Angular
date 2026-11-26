import { TestBed } from '@angular/core/testing';
import { RootUrl } from './root-url';

describe('RootUrl', () => {
  let service: RootUrl;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RootUrl);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
