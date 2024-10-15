export const CollectionMapConst = {
    'ao': 1,
    'quan': 2,
    'chan-vay': 3,
    'ao-khoac': 4
};

/**
 * Hàm nhận vào giá trị và trả về chuỗi tương ứng
 * @param value - Giá trị cần tìm (1 đến 4)
 * @returns - Chuỗi tương ứng với giá trị
 */
export function getCollectionNameById(value: number): string | null {
    // Duyệt qua các key-value trong CollectionMapConst
    for (const key in CollectionMapConst) {
        if (CollectionMapConst[key] === value) {
            return key;  // Trả về key tương ứng với value
        }
    }
    return null;  // Nếu không tìm thấy, trả về null
}