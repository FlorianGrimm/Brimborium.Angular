import { loggingConfiguration } from "./logging.configuration";
import { loggingService } from "./logging.service";

var invoketion = 1;

export function logging(logging?: true | false | "noValue") {
  if (logging === false) {
    return function (target: any, propertyKey: string, descriptor: PropertyDescriptor) {
      return;
    }
  }
  if (logging === "noValue") {
    return function (target: any, propertyKey: string, descriptor: PropertyDescriptor) {
      if (!loggingConfiguration.isInjectLoggingEnabled) {
        return;
      }
      if (descriptor) {
        if ((descriptor.configurable === true)
          && (descriptor.enumerable === false)
          && (descriptor.writable === true)) {
          //

          const method = descriptor.value as ((this: any, ...args: any) => any);
          const wrapedMethod = function wrapMethod(...args: any): any {
            var localInvoketion = invoketion++;
            const pi = `${propertyKey} ${localInvoketion}`;
            { let [f, m, e] = loggingService.write(this, pi, "call", undefined); f(m, e); }
            try {
              const result = method.apply(this, args);
              { let [f, m, e] = loggingService.write(this, pi, "return", result); f(m, e); }
              return result;
            } catch (error) {
              { let [f, m, e] = loggingService.write(this, pi, "fails", error as any); f(m, e); }
              throw error;
            }
          } as ((this: any, ...args: any) => any);
          descriptor.value = wrapedMethod;
        }
      }
    };
  }
  {
    return function (target: any, propertyKey: string, descriptor: PropertyDescriptor) {
      if (!loggingConfiguration.isInjectLoggingEnabled) {
        return;
      }
      if (descriptor) {
        if ((descriptor.configurable === true)
          && (descriptor.enumerable === false)
          && (descriptor.writable === true)) {
          //

          const method = descriptor.value as ((this: any, ...args: any) => any);
          const wrapedMethod = function wrapMethod(...args: any): any {
            var localInvoketion = invoketion++;
            const pi = `${propertyKey} ${localInvoketion}`;
            { let [f, m, e] = loggingService.write(this, pi, "call", args); f(m, e); }
            try {
              const result = method.apply(this, args);
              { let [f, m, e] = loggingService.write(this, pi, "return", result); f(m, e); }
              return result;
            } catch (error) {
              { let [f, m, e] = loggingService.write(this, pi, "fails", error as any); f(m, e); }
              throw error;
            }
          } as ((this: any, ...args: any) => any);
          descriptor.value = wrapedMethod;
        }
      }
    };
  }
}
