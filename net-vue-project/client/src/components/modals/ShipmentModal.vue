<template>
  <solar-modal>
    <template v-slot:header>
      Receive Shipment
    </template>
    <template v-slot:body>
      <label for="product">Product Received:</label>
      <select v-model="selectedProduct" class="shipmentItems" id="product">
        <option disabled value="">Please select one</option>
        <option v-for="item in inventory" :value="item" :key="item.product.id">
          {{ item.product.name }}
        </option>
      </select>
      <label for="qtyReceived">Quantity Received</label>
      <input type="number" id="qtyReceived" v-model="qtyReceived" />
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

class Props {
  inventory = prop({
    required: true,
    type: Array as PropType<IProductInventory[]>
  });
}

@Options({
  name: "ShipmentModal",
  components: { SolarButton, SolarModal },
})
export default class ShipmentModal extends Vue.with(Props) {
  qtyReceived: number = 0;
  selectedProduct: IProduct = {
    id: 1,
    name: 'Some Product',
    description: 'Some description',
    price: 1000,
    createdOn: new Date(),
    updatedOn: new Date(),
    isTaxable: true,
    isArchived: false
  }

  save() {
    let shipment: IShipment = {
      productId: this.selectedProduct.id,
      adjustment: this.qtyReceived
    };

    this.$emit('save:shipment', shipment);
  }

  close() {
    this.$emit('close');
  }
}
</script>

<style scoped lang="scss"></style>