export interface Pagination<T> {
  pageIndex: number;
  pageSize: number;
  count: number;
  totalItems: number;
  data: T;
}

