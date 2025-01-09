import { TestBed } from '@angular/core/testing';

import { LoggingService } from './logging.service';
import { LoggingMessage } from './types';

describe('LoggingService', () => {
  let loggingService: LoggingService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    loggingService = TestBed.inject(LoggingService);
  });

  it('limitLogLevel', () => {
    loggingService.limitLogLevelConsole = 1;
    loggingService.limitLogLevelStream = 1;
    let lastValue: LoggingMessage | undefined = undefined;
    const subscribtion = loggingService.logMessages$.subscribe({
      next: (value) => { lastValue = value; }
    });
    expect(loggingService).toBeTruthy();
    {
      const [f, m, e] = loggingService.error(undefined, "error", "msg"); /* f(m,e); */
      expect(m).toBe("error:msg");
      expect(lastValue).not.toBe(undefined);
      expect(lastValue!.location).toBe("error");
      lastValue = undefined;
    }
    {
      const [f, m, e] = loggingService.warn(undefined, "warn", "msg"); /* f(m,e); */
      expect(m).toBe("warn:msg");
      expect(lastValue).not.toBe(undefined);
      expect(lastValue!.location).toBe("warn");
      lastValue = undefined;
    }
    {
      const [f, m, e] = loggingService.info(undefined, "info", "msg"); /* f(m,e); */
      expect(m).toBe("");
      expect(lastValue).toBe(undefined);
      lastValue = undefined;
    }
    {
      const [f, m, e] = loggingService.debug(undefined, "debug", "msg"); /* f(m,e); */
      expect(m).toBe("");
      expect(lastValue).toBe(undefined);
      lastValue = undefined;
    }
    subscribtion.unsubscribe();
  });
});
