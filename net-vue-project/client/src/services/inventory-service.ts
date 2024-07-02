import { IProductInventory } from "@/types/Product";
import { IShipment } from "@/types/Shipment";
import axios, { AxiosResponse } from "axios";

export default class InventoryService {
  API_URL = process.env.VUE_APP_API_URL

  public async getInventory(): Promise<IProductInventory[]> {
    const res = await axios.get(`${this.API_URL}/inventory`);

    return res.data;
  }

  public async updateInventoryQuantity(shipment: IShipment): Promise<IProductInventory[]> {
    const res = await axios.patch(`${this.API_URL}/inventory/`, shipment);

    return res.data;
  }
}