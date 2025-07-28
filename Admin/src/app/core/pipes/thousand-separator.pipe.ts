import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'thousandSeparator'
})
export class ThousandSeparatorPipe implements PipeTransform {
  transform(value: number | string | null | undefined): string {
    if (value === null || value === undefined || value === '') return '0';
    
    // Convert value to string and remove commas
    const numberStr = value.toString().replace(/,/g, '');

    // Check if it's a valid number
    const number = Number(numberStr);
    if (isNaN(number)) return '0';

    // Check if the number has a decimal part
    const hasDecimal = number % 1 !== 0;

    // If it's an integer, format normally
    if (!hasDecimal) {
      return new Intl.NumberFormat('vi-VN').format(number);
    }

    // If it has a decimal part, format with 1-2 decimal places
    return new Intl.NumberFormat('vi-VN', {
      minimumFractionDigits: 1,
      maximumFractionDigits: 2
    }).format(number);
  }
}
