import { ICustomer } from "@/types/Customer";
import { IProduct, IProductInventory } from "@/types/Product";
import { IServiceResponse } from "@/types/ServiceResponse";
import { IShipment } from "@/types/Shipment";
import axios, { AxiosResponse } from "axios";

export default class CustomerService {
  API_URL = process.env.VUE_APP_API_URL

  public async getCustomers(): Promise<ICustomer[]> {
    const res = await axios.get(`${this.API_URL}/customers`);

    return res.data;
  }

  public async addCustomer(customer: ICustomer): Promise<IServiceResponse<ICustomer>> {
    const res = await axios.post(`${this.API_URL}/customers`, customer);

    return res.data;
  }

  public async deleteCustomer(id: number): Promise<boolean> {
    const res = await axios.delete(`${this.API_URL}/customers/${id}`);

    return res.data;
  }
}