import { IProduct, IProductInventory } from "@/types/Product";
import { IShipment } from "@/types/Shipment";
import axios, { AxiosResponse } from "axios";

export default class OrderService {
  API_URL = process.env.VUE_APP_API_URL

  public async getOrders(): Promise<any> {
    const res = await axios.get(`${this.API_URL}/orders`);

    return res.data;
  }

  public async makeOrderComplete(id: number): Promise<any> {
    const res = await axios.patch(`${this.API_URL}/orders/complete/${id}`);

    return res.data;
  }
}