import { TestBed } from '@angular/core/testing';
import { AccessServiceRestImpl, AccessServiceRestFactory, AccessServiceRest } from './access-service-rest';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { authInterceptor } from './auth-Interceptor';
import { provideHttpClientTesting } from '@angular/common/http/testing';

export class AccessServiceRestTest extends AccessServiceRest {
  override test(): string {
    return "test";
  }
}

/*

TestBed.configureTestingModule({
  providers: [provideHttpClient(withInterceptors([authInterceptor])), provideHttpClientTesting()],
});

*/
describe('AccessServiceRest', () => {
  let service: AccessServiceRest;
  let accessServiceRestTest: AccessServiceRestTest;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideHttpClient(withInterceptors([authInterceptor])),
        provideHttpClientTesting(),
      ],
    });
    accessServiceRestTest = new AccessServiceRestTest();
    TestBed.inject(AccessServiceRestFactory).instance = accessServiceRestTest;
    service = TestBed.inject(AccessServiceRest);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('test', () => {
    expect(service.test()).toBe("test");
  });

});
