import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'thousandSeparator'
})
export class ThousandSeparatorPipe implements PipeTransform {
  transform(value: number | string | null | undefined): string {
    if (value === null || value === undefined || value === '') return '0';
    
    // Chuyển value về string và loại bỏ dấu phẩy nếu có
    const numberStr = value.toString().replace(/,/g, '');
    
    // Kiểm tra xem có phải số hợp lệ không
    const number = Number(numberStr);
    if (isNaN(number)) return '0';
    
    // Format số với dấu phẩy ngăn cách hàng nghìn
    return new Intl.NumberFormat('vi-VN').format(number);
  }
}
