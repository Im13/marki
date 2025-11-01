export const CollectionMapConst = {
    'all': 0,        
    'tops': 1,          
    'bottoms': 2,      
    'dresses': 3,       
    'outerwear': 4,    
    
    // Giữ lại tên tiếng Việt để backward compatible (nếu cần)
    'ao': 1,
    'quan': 2,
    'chan-vay': 3,
    'ao-khoac': 4
};

/**
 * Hàm nhận vào giá trị và trả về chuỗi tương ứng
 * @param value - Giá trị cần tìm (0 đến 4)
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

/**
 * Lấy display name cho collection
 * @param slug - Slug của collection (vd: 'tops', 'bottoms')
 * @returns - Tên hiển thị
 */
export function getCollectionDisplayName(slug: string): string {
    const displayNames: { [key: string]: string } = {
        'all': 'Tất cả sản phẩm',
        'tops': 'Áo',
        'bottoms': 'Quần',
        'dresses': 'Váy/Đầm',
        'outerwear': 'Áo khoác',
        'new-arrivals': 'Hàng mới về'
    };
    return displayNames[slug] || slug;
}
