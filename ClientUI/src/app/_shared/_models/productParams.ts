export class ProductParams {
    pageIndex = 1;
    pageSize = 40;
    search = '';
    typeId: number;

    constructor(typeId?: number) {
      this.typeId = typeId || 0;
    }
}
