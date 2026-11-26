import { inject, InjectionToken, Service } from '@angular/core';

@Service({
  autoProvided: true,
  factory: () => inject(AccessServiceRestFactory).instance
})
export class AccessServiceRest {
  test(): string { throw new Error("abstract"); }
}


export class AccessServiceRestImpl extends AccessServiceRest {
  override test(): string {
    return "real";
  }
}


@Service()
export class AccessServiceRestFactory {
  private _instance: AccessServiceRest | undefined = undefined;
  public get instance(): AccessServiceRest {
    return (null == this._instance) ? (new AccessServiceRestImpl()) : (this._instance);
  }
  public set instance(value: AccessServiceRest | undefined) {
    this._instance = value;
  }
}
