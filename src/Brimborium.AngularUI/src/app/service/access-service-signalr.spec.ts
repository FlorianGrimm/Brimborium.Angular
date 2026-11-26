import { TestBed } from '@angular/core/testing';
import { AccessServiceSignalr, AccessServiceSignalrFactory } from './access-service-signalr';

export class AccessServiceSignalrTest extends AccessServiceSignalr {
  constructor() { super(); }
}

describe('AccessServiceSignalr', () => {
  let serviceTest: AccessServiceSignalrTest;
  let service: AccessServiceSignalr;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    serviceTest = new AccessServiceSignalrTest();
    TestBed.inject(AccessServiceSignalrFactory).instance = serviceTest;
    service = TestBed.inject(AccessServiceSignalr);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
