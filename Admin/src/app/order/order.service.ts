import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { read, utils } from "xlsx";
import * as XLSX from 'xlsx';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  baseApiUrl = environment.apiUrl;
  arrayBuffer: any;
  data: any;

  constructor(private http: HttpClient) { }

  uploadShopeeOrdersFile(excelFile: FormData) {
    return this.http.post(this.baseApiUrl + 'shopee/create-orders', excelFile);
  }

  handleExcelFile(file: File) {
    const fileReader = new FileReader();

    fileReader.onload = (e: any) => {
      /* read workbook */
      const ab: ArrayBuffer = e.target.result;
      const wb: XLSX.WorkBook = read(ab);

      /* grab first sheet */
      const wsName: string = wb.SheetNames[0];
      const ws: XLSX.WorkSheet = wb.Sheets[wsName];

      /* save data */
      this.data = utils.sheet_to_json(ws, {header: 1});

      console.log(this.data);
    }

    fileReader.readAsArrayBuffer(file);
  }
}
