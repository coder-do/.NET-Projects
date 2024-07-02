<template>
  <solar-modal>
    <template v-slot:header>
      Add New Customer
    </template>
    <template v-slot:body>
      <ul class="newCustomer">
        <li>
          <label for="firstName">First Name</label>
          <input type="text" id="firstName" v-model="customer.firstName" />
        </li>
        <li>
          <label for="lastName">Last Name</label>
          <input type="text" id="lastName" v-model="customer.lastName" />
        </li>
        <li>
          <label for="address1">Address Line 1</label>
          <input type="text" id="address1" v-model="customer.primaryAddress.address1" />
        </li>
        <li>
          <label for="address2">Address Line 2</label>
          <input type="text" id="address2" v-model="customer.primaryAddress.address2" />
        </li>
        <li>
          <label for="city">City</label>
          <input type="text" id="city" v-model="customer.primaryAddress.city" />
        </li>
        <li>
          <label for="state">State</label>
          <input type="text" id="state" v-model="customer.primaryAddress.state" />
        </li>
        <li>
          <label for="postalCode">Postal Code</label>
          <input type="text" id="postalCode" v-model="customer.primaryAddress.postalCode" />
        </li>
      </ul>
    </template>
    <template v-slot:footer>
      <solar-button @button:click="save">
        Save Received Shipment
      </solar-button>
      <solar-button @button:click="close">
        Close
      </solar-button>
    </template>
  </solar-modal>
</template>

<script lang="ts">
import { PropType } from "vue";
import { Options, Vue, prop } from "vue-class-component";
import SolarButton from "@/components/SolarButton.vue";
import SolarModal from "./SolarModal.vue";
import { IProduct, IProductInventory } from "@/types/Product";
import { IShipment } from "@/types/Shipment";
import { ICustomer } from "@/types/Customer";

class Props {
  // inventory = prop({
  //   required: true,
  //   type: Array as PropType<IProductInventory[]>
  // });
}

@Options({
  name: "NewCustomerModal",
  components: { SolarButton, SolarModal },
})
export default class NewCustomerModal extends Vue.with(Props) {
  customer: ICustomer = {
    id: +(new Date().valueOf().toString().substring(9)),
    createdOn: new Date(),
    updatedOn: new Date(),
    firstName: '',
    lastName: '',
    primaryAddress: {
      address1: '', address2: '', city: '', country: '', createdOn: new Date(),
      updatedOn: new Date(), id: this.getRandomInt(1, 3),
      postalCode: '', state: ''
    }
  };

  getRandomInt(min: number, max: number) {
    min = Math.ceil(min);
    max = Math.floor(max);
    return Math.floor(Math.random() * (max - min + 1)) + min;
  }

  save() {
    this.$emit('save:customer', this.customer);
  }

  close() {
    this.$emit('close');
  }
}
</script>

<style scoped lang="scss">
.newCustomer {
  display: flex;
  flex-wrap: wrap;
  list-style: none;
  padding: 0;
  margin: 0;

  input {
    width: 80%;
    height: 1.8rem;
    margin: 0.8rem;
    margin-right: 2rem;
    font-size: 1.1rem;
    line-height: 1.3rem;
    padding: 0.2rem;
    color: #555;
  }

  label {
    font-weight: bold;
    margin: 0.8rem;
    display: block;
  }
}
</style>