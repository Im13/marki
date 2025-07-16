import { OrderStatus } from "../_models/orderStatus";

export const ORDER_STATUSES: OrderStatus[] = [
    { id: 1, status: 'Mới' },
    { id: 2, status: 'Chờ hàng' },
    { id: 3, status: 'Ưu tiên xuất đơn' },
    { id: 4, status: 'Đã xác nhận' },
    { id: 5, status: 'Gửi hàng đi' },
    { id: 6, status: 'Huỷ đơn' },
    { id: 7, status: 'Xoá đơn' },
    { id: 8, status: 'Từ chối nhận hàng' },
    { id: 9, status: 'Đang hoàn' },
    { id: 10, status: 'Đã hoàn toàn bộ' },
    { id: 11, status: 'Hoàn thành' },
    { id: 12, status: 'Tạo trùng lặp' }
];