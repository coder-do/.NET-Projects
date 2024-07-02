<template>
  <div>
    <h1 id="customersTitle">Manage Customers </h1>
  </div>
  <hr />
  <div class="customer-actions">
    <solar-button @click.native="showCustomerModal">
      Add Customer
    </solar-button>
  </div>
  <table id="customers" class="table">
    <tr>
      <th>Customer</th>
      <th>Address</th>
      <th>State</th>
      <th>Since</th>
      <th>Delete</th>
    </tr>
    <tr v-for="customer in customers">
      <td>
        {{ customer.firstName + ' ' + customer.lastName }}
      </td>
      <td>
        {{ customer.primaryAddress.address1 + ' ' + customer.primaryAddress.address2 }}
      </td>
      <td>
        {{ customer.primaryAddress.state }}
      </td>
      <td>
        {{ $filters.date(customer.createdOn) }}
      </td>
      <td>
        <div class="lni lni-cross-circle product-archive" @click="deleteCustomer(customer.id)"></div>
      </td>
    </tr>
  </table>
  <new-customer-modal v-if="customerModalOpen" @save:customer="saveCustomer" @close="closeModal" />
</template>

<script lang="ts">
import { PropType } from "vue";
import { Options, Vue, prop } from "vue-class-component";
import SolarButton from "@/components/SolarButton.vue";
import { ICustomer } from "@/types/Customer";
import NewCustomerModal from "@/components/modals/NewCustomerModal.vue";
import ShipmentModal from "@/components/modals/ShipmentModal.vue";
import CustomerService from '@/services/customer-service';

class Props {
  // inventory = prop({
  //   required: false,
  //   type: Array as PropType<IProductInventory[]>
  // });
}

const customerService = new CustomerService();
// const productService = new ProductService();

@Options({
  name: "Customers",
  components: { SolarButton, NewCustomerModal, ShipmentModal },
})
export default class Customers extends Vue.with(Props) {
  customers: ICustomer[] = [];
  customerModalOpen = false;

  async created() {
    await this.initialize();
  }

  closeModal() {
    this.customerModalOpen = false;
  }

  async fetchData() {
    return await customerService.getCustomers();
  }

  async initialize() {
    this.customers = await this.fetchData();
  }

  showCustomerModal() {
    this.customerModalOpen = true
  }

  async saveCustomer(cust: ICustomer) {
    await customerService.addCustomer(cust);
    this.closeModal();
    await this.initialize();
  }

  async deleteCustomer(id: number) {
    await customerService.deleteCustomer(id);
    await this.initialize();
  }
}
</script>

<style scoped lang="scss">
@import "@/scss/global";

.product-archive {
  cursor: pointer;
  font-weight: bold;
  font-size: 1.2rem;
  color: $solar-red;
}
</style>
