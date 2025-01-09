/**
 * Represents the different levels of logging.
 *
 * - `-1` or `'disabled'`: Logging is disabled.
 * - `0` or `'error'`: Error level logging.
 * - `1` or `'warn'`: Warning level logging.
 * - `2` or `'info'`: Information level logging.
 * - `3` or `'debug'`: Debug level logging.
 */
export type LogLevel = (
  -1 | 'disabled' |
  0 | 'error' |
  1 | 'warn' |
  2 | 'info' |
  3 | 'debug');

export type LogLevelText = (
    'disabled' |
    'error' |
    'warn' |
    'info' |
    'debug');

/**
 * Represents the numeric values of logging levels.
 *
 * - `-1`: Logging is disabled.
 * - `0`: Error level logging.
 * - `1`: Warning level logging.
 * - `2`: Information level logging.
 * - `3`: Debug level logging.
 */
export type LogLevelNumber = (
  -1 |
  0 |
  1 |
  2 |
  3);

/**
 * Represents a logging message.
 *
 * @property {Date} dt - The date and time when the log message was created.
 * @property {string} location - The location where the log message was generated.
 * @property {string} msg - The log message content.
 * @property {string} extra - Any additional information related to the log message.
 */
export type LoggingMessage = {
  dt: Date;
  logLevel: LogLevelText;
  location: string;
  msg: string;
  extra: string
};
