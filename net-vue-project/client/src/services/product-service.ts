import { IProduct, IProductInventory } from "@/types/Product";
import { IShipment } from "@/types/Shipment";
import axios, { AxiosResponse } from "axios";

export default class ProductService {
  API_URL = process.env.VUE_APP_API_URL

  public async archive(id: number): Promise<any> {
    const res = await axios.patch(`${this.API_URL}/products/${id}`);

    return res.data;
  }

  public async save(product: IProduct): Promise<any> {
    const res = await axios.post(`${this.API_URL}/products/`, product);

    return res.data;
  }
}