import { Service } from "@angular/core";
import { EventPublisher } from "./event-publisher";
import type { IClientContract } from "@src/generated/TypedSignalR.Client/Brimborium.Angular.API";
import type { Project } from "@src/generated/Brimborium.Angular.API";

@Service()
export class AccessServiceSignalrEventPublisher extends EventPublisher<IClientContract> implements IClientContract {
  constructor() { super(); }

  async onUpdated(project: Project): Promise<void> {
    const listReceiver = this.listReceiver;
    for (const item of listReceiver) {
      await item.onUpdated(project);
    }
  }
}
