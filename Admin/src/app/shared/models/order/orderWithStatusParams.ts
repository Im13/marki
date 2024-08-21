import { OrderParams } from "./orderParams";

export class OrderWithStatusParams extends OrderParams {
  statusId = 0;

  constructor(statusId: number) {
    super();
    this.statusId = statusId;
  }
}

