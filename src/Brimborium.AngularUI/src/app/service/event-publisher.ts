import { Disposable } from "@src/generated/TypedSignalR.Client";

export class EventPublisher<R> implements Disposable {
  public listReceiver: R[] = [];

  constructor() { }

  public dispose(): void {
    this.listReceiver.splice(0, this.listReceiver.length);
  }

  public add(receiver: R): Disposable {
    this.listReceiver = [...this.listReceiver, receiver];
    return { dispose: () => { this.listReceiver = this.listReceiver.filter(item => (item !== receiver)); } }
  }

  protected async publishEvent<V>(value: V, fn: ((receiver: R, value: V) => Promise<void>)): Promise<void> {
    const listReceiver = this.listReceiver;
    for (const item of listReceiver) {
      await fn(item, value);
    }
  }
}
