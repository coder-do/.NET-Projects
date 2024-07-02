import { IInvoice } from "@/types/Invoice";
import { IProductInventory } from "@/types/Product";
import { IServiceResponse } from "@/types/ServiceResponse";
import axios, { AxiosResponse } from "axios";

export default class InvoiceService {
  API_URL = process.env.VUE_APP_API_URL

  public async makeNewInvoice(invoice: IInvoice): Promise<IServiceResponse<IInvoice>> {
    let now = new Date();
    invoice.createdOn = now;
    invoice.updatedOn = now;
    const res = await axios.post(`${this.API_URL}/orders/invoice`, invoice);

    return res.data;
  }
}