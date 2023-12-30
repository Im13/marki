export interface Pagination {
  pageIndex: number;
  pageSize: number;
  count: number;
  totalItems: number;
}

export class PaginatedResult<T> {
  result: T;
  pagination: Pagination;
}

