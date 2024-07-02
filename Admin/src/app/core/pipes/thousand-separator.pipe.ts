import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'thousandSeparator'
})
export class ThousandSeparatorPipe implements PipeTransform {

  transform(value: number, ...args: unknown[]): string {
    return value.toLocaleString('de-DE');
  }

}
