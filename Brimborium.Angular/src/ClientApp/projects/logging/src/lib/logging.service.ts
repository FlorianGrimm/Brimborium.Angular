import { Injectable } from '@angular/core';
import { BehaviorSubject, Subject } from 'rxjs';

import type { LogLevel, LogLevelNumber, LogLevelText, LoggingMessage } from './types';

type LogResult = [((message: string, arg: unknown) => void), string, unknown];


/**
 * Service for logging messages with different log levels.
 *
 * @remarks
 * This service allows logging messages to both the console and a stream.
 * It supports different log levels such as debug, info, warn, and error.
 * The call is a so much unexpected, because I want to maintain the feature of F12-Console to click on the link,
 * ehich is the one of the best features IMHO.
 *
 * @example
 * ```typescript
 * const loggingService = new LoggingService();
 * { const [f, m, e] = loggingService.log(this, "constructor", "start"); f(m,e); }
 * { const [f, m, e] = loggingService.log(this, "doSomething", "step", {value:42}); f(m,e); }});
 * ```
 *
 * @public
 */
@Injectable({
  providedIn: 'root'
})
export class LoggingService {
  /**
   * The current limit log level. Messages with a log level higher than this will not be logged.
   */
  private _limitLogLevelConsole: LogLevelNumber = 0;

  /**
   * Gets the current limit log level.
   */
  public get limitLogLevelConsole(): LogLevel { return this._limitLogLevelConsole; }

  /**
   * Sets the limit log level.
   */
  public set limitLogLevelConsole(value: LogLevel) { this._limitLogLevelConsole = convertToLogLevelNumber(value); }

  /**
   * The current limit log level. Messages with a log level higher than this will not be logged.
   */
  private _limitLogLevelStream: LogLevelNumber = 0;

  /**
   * Gets the current limit log level.
   */
  public get limitLogLevelStream(): LogLevel { return this._limitLogLevelStream; }

  /**
   * Sets the limit log level.
   */
  public set limitLogLevelStream(value: LogLevel) { this._limitLogLevelStream = convertToLogLevelNumber(value); }

  /**
   * the Subject that publish the logMessages.
   */
  public readonly logMessages$ = new Subject<LoggingMessage>();

  constructor() { }

  /**
   * Writes a log message with the specified log level.
   * @param logLevel The level of the log message.
   * @param that The context (usually `this`) from where the log is called.
   * @param method The method name or reference from where the log is called.
   * @param message The log message.
   * @param args Additional arguments or a function returning additional arguments.
   * @returns A log result array.
   * @example
   * { const [f, m, e] = loggingService.log(this, "constructor", "start"); f(m,e); }
   * { const [f, m, e] = loggingService.log(this, "doSomething", "step", {value:42}); f(m,e); }
   */
  write(logLevel: LogLevel, that: any | undefined, method?: string | any | undefined, message?: string | undefined, args?: (() => any) | any | undefined): LogResult {
    const currentLogLevel = convertToLogLevelNumber(logLevel);
    const logToConsole = (
      (0 <= this._limitLogLevelConsole)
      && (0 <= currentLogLevel)
      && (this._limitLogLevelConsole >= currentLogLevel));

    const logToStream = (
      this.logMessages$.observed
      && (0 <= this._limitLogLevelStream)
      && (0 <= currentLogLevel)
      && (this._limitLogLevelStream >= currentLogLevel));

    if (!logToConsole && !logToStream) { return nologResult; }

    const outThat = that && that.constructor ? that.constructor.name : "";
    const outMethod = method ? (typeof (method) === "function" ? method.name : method.toString()) : "";
    const outLocation = (outThat !== "" && outMethod !== "") ? `${outThat}:${outMethod}` : (outThat || outMethod);
    const outExtra = convertArgs(args);

    if (logToStream) {
      const logMessage: LoggingMessage = {
        dt: new Date(),
        logLevel: convertToLogLevelText(currentLogLevel),
        location: outLocation,
        msg: message ?? "",
        extra: outExtra
      };
      this.logMessages$.next(logMessage)
    }
    if (logToConsole && (currentLogLevel !== -1)) {
      const logResult: LogResult = [getConsoleLogLevelNumber(currentLogLevel), `${outLocation}:${message}`, outExtra];
      return logResult;
    } else {
      return nologResult;
    }
  }

  /**
   * log a debug message
   * @param that the calling this
   * @param method the calling method
   * @param message the message to log
   * @param args the values to log
   * @returns an array
   * @example
   * { const [f, m, e] = loggingService.log(this, "constructor", "start"); f(m,e); }
   * { const [f, m, e] = loggingService.log(this, "doSomething", "step", {value:42}); f(m,e); }
   */
  debug(that: any | undefined, method?: string | undefined, message?: string | any | undefined, args?: (() => any) | any | undefined): LogResult {
    return this.write("debug", that, method, message, args);
  }

  /**
   * log a info message
   * @param that the calling this
   * @param method the calling method
   * @param message the message to log
   * @param args the values to log
   * @returns an array
   * @example
   * { const [f, m, e] = loggingService.log(this, "constructor", "start"); f(m,e); }
   * { const [f, m, e] = loggingService.log(this, "doSomething", "step", {value:42}); f(m,e); }
   */
  log(that: any | undefined, method?: string | undefined, message?: string | any | undefined, args?: (() => any) | any | undefined): LogResult {
    return this.write("info", that, method, message, args);
  }

  /**
   * log a info message
   * @param that the calling this
   * @param method the calling method
   * @param message the message to log
   * @param args the values to log
   * @returns an array
   * @example
   * { const [f, m, e] = loggingService.info(this, "constructor", "start"); f(m,e); }
   * { const [f, m, e] = loggingService.info(this, "doSomething", "step", {value:42}); f(m,e); }
   */
  info(that: any | undefined, method?: string | undefined, message?: string | any | undefined, args?: (() => any) | any | undefined): LogResult {
    return this.write("info", that, method, message, args);
  }

  /**
   * log a info message
   * @param that the calling this
   * @param method the calling method
   * @param message the message to log
   * @param args the values to log
   * @returns an array
   * @example
   * { const [f, m, e] = loggingService.warn(this, "constructor", "start"); f(m,e); }
   * { const [f, m, e] = loggingService.warn(this, "doSomething", "step", {value:42}); f(m,e); }
   */
  warn(that: any | undefined, method?: string | undefined, message?: string | any | undefined, args?: (() => any) | any | undefined): LogResult {
    return this.write("warn", that, method, message, args);
  }

  /**
   * log a error message
   * @param that the calling this
   * @param method the calling method
   * @param message the message to log
   * @param args the values to log
   * @returns an array
   * @example
   * { const [f, m, e] = loggingService.error(this, "constructor", "start"); f(m,e); }
   * { const [f, m, e] = loggingService.error(this, "doSomething", "step", {value:42}); f(m,e); }
   */
  error(that: any | undefined, method?: string | undefined, message?: string | any | undefined, args?: (() => any) | any | undefined): LogResult {
    return this.write("error", that, method, message, args);
  }
}

const nologResult: LogResult = [nolog, "", {} as any];

function nolog(msg: string, extraArgs: any): void { }

export function convertToLogLevelNumber(logLevel: LogLevel): LogLevelNumber {
  switch (logLevel) {
    case 0: return 0;
    case 1: return 1;
    case 2: return 2;
    case 3: return 3;
    case 'error': return 0;
    case 'warn': return 1;
    case 'info': return 2;
    case 'debug': return 3;
    default: return 1;
  }
}

function convertToLogLevelText(logLevel: LogLevelNumber): LogLevelText {
  switch (logLevel) {
    case -1: return 'disabled';
    case 0: return 'error';
    case 1: return 'warn';
    case 2: return 'info';
    case 3: return 'debug';
    default: return "info";
  }
}


function getConsoleLogLevelNumber(logLevel: (0 | 1 | 2 | 3)) {
  switch (logLevel) {
    case 0: return console.error;
    case 1: return console.warn;
    case 2: return console.info;
    case 3: return console.debug;
    default: return nolog;
  }
}


function convertArgs(args: any) {
  let argsText: string;
  if (args === undefined) { return "undefined"; }
  if (args === null) { return "null"; }
  if (typeof (args) === "string") { return args; }
  if (typeof (args) === "boolean") { return args.toString(); }
  if (typeof (args) === "number") { return args.toString(); }
  if (typeof (args) === "bigint") { return args.toString(); }
  if (typeof (args) === "symbol") { return args.toString(); }
  if (typeof (args) === "function") {
    try {
      return convertArgs(args());
    } catch {
      return "";
    }
  }
  if (typeof (args) === "object") {
    if (Array.isArray(args)) {
      if (20 < args.length) {
        return JSON.stringify(args.slice(20));
      }
    }
    return JSON.stringify(args);
  }
  return "";

}


export const loggingService = new LoggingService();
