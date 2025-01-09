import { MonoTypeOperatorFunction, tap } from "rxjs";
import { LoggingService, loggingService } from "./logging.service";

export function logNext<T>(
  that: any | undefined,
  method: any | undefined,
  msg: string,
  extraArgs?: (value: T) => (any | undefined)
): MonoTypeOperatorFunction<T> {
  return tap((value) => {
    let [f, m, e] = loggingService.write(that, method, msg, extraArgs?.(value));
    f(m, e);
  });
}

export type TapObserverValue<T> = {
  next: T;
  error: T;
  complete: T;
  subscribe: T;
  unsubscribe: T;
  finalize: T;
};

export function logObservable<T>(
  loggingService: LoggingService,
  that: any | undefined,
  method: any | undefined,
  obsMsg: Partial<TapObserverValue<string>> | string,
  extraArgs?: (value: T) => (any | undefined)
): MonoTypeOperatorFunction<T> {
  return tap({
    next: (value) => {
      const msg = typeof obsMsg === 'string' ? `next ${obsMsg}` : obsMsg.next;
      if (msg === undefined) return;
      let [f, m, e] = loggingService.write(that, method, msg, extraArgs?.(value));
      f(m, e);
    },
    error: (err) => {
      const msg = typeof obsMsg === 'string' ? `error ${obsMsg}` : obsMsg.next;
      if (msg === undefined) return;
      let [f, m, e] = loggingService.write(that, method, msg, err);
      f(m, e);
    },
    complete: () => {
      const msg = typeof obsMsg === 'string' ? `complete ${obsMsg}` : obsMsg.next;
      if (msg === undefined) return;
      let [f, m, e] = loggingService.write(that, method, msg);
      f(m, e);
    },
    subscribe: () => {
      const msg = typeof obsMsg === 'string' ? `subscribe ${obsMsg}` : obsMsg.next;
      if (msg === undefined) return;
      let [f, m, e] = loggingService.write(that, method, msg);
      f(m, e);
    },
    unsubscribe: () => {
      const msg = typeof obsMsg === 'string' ? `unsubscribe ${obsMsg}` : obsMsg.next;
      if (msg === undefined) return;
      let [f, m, e] = loggingService.write(that, method, msg);
      f(m, e);
    },
    finalize: () => {
      const msg = typeof obsMsg === 'string' ? `finalize ${obsMsg}` : obsMsg.next;
      if (msg === undefined) return;
      let [f, m, e] = loggingService.write(that, method, msg);
      f(m, e);
    }
  });
}

/*

  logObservable(this.loggingService, this, 'loadAuxData', {}),

*/
