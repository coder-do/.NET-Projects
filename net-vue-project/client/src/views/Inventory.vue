<template>
  <div class="inventory-container">
    <h1 id="inventoryTitle">
      Inventory Dashboard
    </h1>
    <hr />

    <div class="inventory-actions">
      <solar-button @click.native="showNewProductModal" id="addNewBtn">
        Add New Item
      </solar-button>
      <solar-button @click.native="showShipmentModal" id="receiveShipmentBtn">
        Receive Shipment
      </solar-button>
    </div>

    <table id="inventorytable" class="table">
      <tr>
        <th>Item</th>
        <th>Quantity On-hand</th>
        <th>Unit Price</th>
        <th>Taxable</th>
        <th>Delete</th>
      </tr>

      <tr v-for="item in inventory" :key="item.id">
        <td>
          {{ item.product.name }}
        </td>
        <td v-bind:class="`${applyColor(item.quantityOnHand, item.idealQuantity)}`">
          {{ item.quantityOnHand }}
        </td>
        <td>
          {{ $filters.price(item.product.price) }}
        </td>
        <td>
          <span v-if="item.product.isTaxable"> Yes </span>
          <span v-else> No </span>
        </td>
        <td>
          <div class="lni lni-cross-circle product-archive" @click="archiveProduct(item.id)"></div>
        </td>
      </tr>
    </table>

    <new-product-modal v-if="isProductOpen" @close="closeModals" @save:product="saveNewProduct" />
    <shipment-modal v-if="isShipmentOpen" :inventory="inventory" @close="closeModals"
      @save:shipment="saveNewShipment" />
  </div>
</template>

<script lang="ts">
import { PropType } from "vue";
import { Options, Vue, prop } from "vue-class-component";
import { IProduct, IProductInventory } from "@/types/Product";
import SolarButton from "@/components/SolarButton.vue";
import { IShipment } from "@/types/Shipment";
import NewProductModal from "@/components/modals/NewProductModal.vue";
import ShipmentModal from "@/components/modals/ShipmentModal.vue";
import InventoryService from '@/services/inventory-service'
import ProductService from '@/services/product-service';

class Props {
  inventory = prop({
    required: false,
    type: Array as PropType<IProductInventory[]>
  });
}

const inventoryService = new InventoryService();
const productService = new ProductService();

@Options({
  name: "Inventory",
  components: { SolarButton, NewProductModal, ShipmentModal },
})
export default class Inventory extends Vue.with(Props) {
  isProductOpen = false;
  isShipmentOpen = false;
  inventory: IProductInventory[] = [];

  applyColor(current: number, target: number) {
    if (current <= 0) {
      return 'red';
    } if (Math.abs(target - current) > 8) {
      return 'yellow';
    }
    return 'green';
  }

  async archiveProduct(id: number) {
    await productService.archive(id);
    this.isProductOpen = false;
    await this.initialize();
  }

  async created() {
    await this.initialize();
  }

  async fetchData() {
    return await inventoryService.getInventory();
  }

  async initialize() {
    this.inventory = await this.fetchData();
  }

  async saveNewProduct(product: IProduct) {
    await productService.save(product);
    this.isProductOpen = false;
    await this.initialize();
  }

  async saveNewShipment(shipment: IShipment) {
    await inventoryService.updateInventoryQuantity(shipment);
    this.isShipmentOpen = false;
    await this.initialize();
  }

  showShipmentModal() {
    this.isShipmentOpen = true;
    this.isProductOpen = false;
  }

  showNewProductModal() {
    this.isProductOpen = true;
    this.isShipmentOpen = false;
  }

  closeModals() {
    this.isProductOpen = false;
    this.isShipmentOpen = false;
  }
}
</script>

<style scoped lang="scss">
@import "@/scss/global";

.green {
  font-weight: bold;
  color: $solar-green;
}

.yellow {
  font-weight: bold;
  color: $solar-yellow;
}

.red {
  font-weight: bold;
  color: $solar-red;
}

.inventory-actions {
  display: flex;
  margin-bottom: 0.8rem;
}

.product-archive {
  cursor: pointer;
  font-weight: bold;
  font-size: 1.2rem;
  color: $solar-red;
}
</style>
