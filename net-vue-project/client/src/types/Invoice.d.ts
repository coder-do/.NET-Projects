import { IProduct } from "./Product";

export interface IInvoice {
  id: number;
  customerId: number;
  lineItems: ILineItem[];
  createdOn: Date;
  updatedOn: Date;
}

export interface ILineItem {
  id: number;
  product?: IProduct;
  quantity: number;
}