import { inject, Service } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";
import { Project, ResponseAPI } from '@src/generated/Brimborium.Angular.API';
import { getHubProxyFactory, getReceiverRegister } from '@src/generated/TypedSignalR.Client';
import { AccessServiceSignalrEventPublisher } from './access-service-signalr-event-publisher';
import type { Disposable } from '@src/generated/TypedSignalR.Client';
import type { IHubContract } from '@src/generated/TypedSignalR.Client/Brimborium.Angular.API';

@Service({
  autoProvided: true,
  factory: () => inject(AccessServiceSignalrFactory).instance
})
export class AccessServiceSignalr implements IHubContract {
  constructor() { }

  public get hubProxy(): IHubContract { throw new Error('Method not implemented.'); }
  public getHubProxy(): IHubContract { throw new Error('Method not implemented.'); }

  public update(project: Project): Promise<ResponseAPI<Project>> { throw new Error('Method not implemented.'); }

  public dispose() { throw new Error('Method not implemented.'); }
}

export class AccessServiceSignalrImpl extends AccessServiceSignalr implements IHubContract {
  public connection: HubConnection | undefined = undefined;
  private _hubProxy: IHubContract | undefined;
  private _receiverDisposable: Disposable | undefined;

  constructor(
    public receiver: AccessServiceSignalrEventPublisher
  ) {
    super();
  }

  public override get hubProxy(): IHubContract {
    return this._hubProxy ?? this.getHubProxy();
  }

  public override dispose() {
    this._hubProxy = undefined;
    this._receiverDisposable?.dispose();
    this._receiverDisposable = undefined;
    this.connection = undefined;
  }

  public override getHubProxy(): IHubContract {
    if (undefined === this.connection || undefined === this._hubProxy) {
      const connection = this.connection = new HubConnectionBuilder()
        .withUrl("/hub/api")
        .build();
      connection.onclose = () => { this.dispose(); };
      const receiverDisposable = getReceiverRegister("IClientContract").register(connection, this.receiver);
      const hubProxy = getHubProxyFactory("IHubContract").createHubProxy(connection);
      this.connection = connection;
      this._receiverDisposable = receiverDisposable;
      return this._hubProxy = hubProxy;
    } else {
      return this._hubProxy;
    }
  }

  /* IHubContract */

  public override async update(project: Project): Promise<ResponseAPI<Project>> {
    const hubProxy = this.getHubProxy();
    const result = await hubProxy.update(project);
    await this.receiver.onUpdated(project);
    return result;
  }
}

@Service()
export class AccessServiceSignalrFactory {
  private _instance: AccessServiceSignalr | undefined = undefined;
  public get instance(): AccessServiceSignalr {
    return (null == this._instance)
      ? (this._instance = new AccessServiceSignalrImpl(inject(AccessServiceSignalrEventPublisher)))
      : (this._instance);
  }
  public set instance(value: AccessServiceSignalr | undefined) {
    this._instance = value;
  }
}
