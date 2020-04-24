import { Injectable } from '@angular/core';
import { BaseApiService } from '../base-api.service';
import { Quotation } from 'app/models/Quotation';

@Injectable({
  providedIn: 'root'
})
export class PayOrderService {

  constructor(private baseAPI: BaseApiService) { }

  public insertQuotation(pQuotation: Array<Quotation>) {
    const vUrl = `${
      this.baseAPI.webserviceurl
    }/Quotations`;
    return this.baseAPI.http
      .post(vUrl, pQuotation ,this.baseAPI.getToken())
      .catch(this.baseAPI.errorHandler);
  }
}
