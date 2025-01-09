type LoggingConfiguration = {
  isInjectLoggingEnabled: boolean;
  isInjectLoggingTimedEnabled: boolean;
  isInjectLoggingTimeLimit: number;
};

export const loggingConfiguration: LoggingConfiguration = {
  isInjectLoggingEnabled: true,
  isInjectLoggingTimedEnabled: false,
  isInjectLoggingTimeLimit: 0,
};

const localStorageKeyLoggingConfiguration = "loggingConfiguration";

export function configureLogging(enable: boolean = false) {
  // initial
  loggingConfiguration.isInjectLoggingEnabled = enable;
  loggingConfiguration.isInjectLoggingTimedEnabled = false;
  loggingConfiguration.isInjectLoggingTimeLimit = 0;

  // configure
  if (enable) {
    // done
  } else {
    const loggingConfigurationJSON = window.localStorage.getItem(localStorageKeyLoggingConfiguration);
    if (loggingConfigurationJSON === null) {
      // do nothing
    } else {
      const cfg = JSON.parse(loggingConfigurationJSON) as Partial<LoggingConfiguration>;
      if (cfg.isInjectLoggingEnabled === true) {
        loggingConfiguration.isInjectLoggingEnabled = true;
      } else if (cfg.isInjectLoggingTimedEnabled === true) {
        if (typeof cfg.isInjectLoggingTimeLimit === "number") {
          const now = (new Date()).getTime();
          if (now < cfg.isInjectLoggingTimeLimit) {
            loggingConfiguration.isInjectLoggingEnabled = true;
            loggingConfiguration.isInjectLoggingTimedEnabled = true;
            loggingConfiguration.isInjectLoggingTimeLimit = cfg.isInjectLoggingTimeLimit;
          }
        }
      }
    }
  }
  console.info((loggingConfiguration.isInjectLoggingEnabled) ? "configureLogging enabled" : "configureLogging disable");
}

export function enableLogging(limitMinutes: number = 10) {
  loggingConfiguration.isInjectLoggingEnabled = false;
  loggingConfiguration.isInjectLoggingTimedEnabled = true;
  const now = (new Date()).getTime();
  const limit = now + limitMinutes * 60 * 1000;
  loggingConfiguration.isInjectLoggingTimeLimit = limit;
  const loggingConfigurationJSON = JSON.stringify(loggingConfiguration);
  window.localStorage.setItem(localStorageKeyLoggingConfiguration, loggingConfigurationJSON);
  loggingConfiguration.isInjectLoggingEnabled = true;
}

export function disableLogging() {
  loggingConfiguration.isInjectLoggingEnabled = false;
  loggingConfiguration.isInjectLoggingTimedEnabled = false;
  loggingConfiguration.isInjectLoggingTimeLimit = 0;
  const loggingConfigurationJSON = JSON.stringify(loggingConfiguration);
  window.localStorage.setItem(localStorageKeyLoggingConfiguration, loggingConfigurationJSON);
  loggingConfiguration.isInjectLoggingEnabled = false;
}
