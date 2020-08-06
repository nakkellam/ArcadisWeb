import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'sumquantity'
})
export class SumTotalPipe implements PipeTransform {
    transform(items: any[], attr: string): any {
        return items.reduce((a, b) => a + (b["cost"] * b["quantity"]), 0);
    }
}
