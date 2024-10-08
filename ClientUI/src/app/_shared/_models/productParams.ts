export class ProductParams {
    pageIndex = 1;
    pageSize = 20;
    search = '';
    typeId: number;

    constructor(typeId?: number) {
      this.typeId = typeId || 0;
    }
}
