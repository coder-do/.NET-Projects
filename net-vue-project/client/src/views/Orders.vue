<template>
  <div>
    <h1 id="ordersTitle">
      Sales Orders
    </h1>
    <table id="sales-orders" class="table" v-if="orders.length">
      <thead>
        <tr>
          <th>CustomerId</th>
          <th>OrderId</th>
          <th>Order Total</th>
          <th>Order Status</th>
          <th>Mark Complete</th>
        </tr>
      </thead>
      <tr v-for="order in orders" :key="order.id">
        <td>
          {{ order.customer.id }}
        </td>
        <td>
          {{ order.id }}
        </td>
        <td>
          {{ $filters.price(getTotal(order)) }}
        </td>
        <td :class="{ green: order.isPaid }">
          {{ getStatus(order.isPaid) }}
        </td>
        <td>
          <div v-if="!order.isPaid" class="lni lni-checkmark-circle order-complete"
            @click="markOrderComplete(order.id)">

          </div>
        </td>
      </tr>
    </table>
    <hr />
  </div>
</template>

<script lang="ts">
import { PropType } from "vue";
import { Options, Vue, prop } from "vue-class-component";
import { IProduct, IProductInventory } from "@/types/Product";
import SolarButton from "@/components/SolarButton.vue";
import { ISalesOrder } from "@/types/Order";
import NewProductModal from "@/components/modals/NewProductModal.vue";
import ShipmentModal from "@/components/modals/ShipmentModal.vue";
import OrderService from '@/services/order-service';

class Props {
  inventory = prop({
    required: false,
    type: Array as PropType<IProductInventory[]>
  });
}

const orderService = new OrderService();
// const productService = new ProductService();

@Options({
  name: "Orders",
  components: { SolarButton, NewProductModal, ShipmentModal },
})
export default class Orders extends Vue.with(Props) {
  orders: ISalesOrder[] = [];

  async initialize() {
    this.orders = await orderService.getOrders();
  }

  async created() {
    await this.initialize();
  }

  async markOrderComplete(id: number) {
    await orderService.makeOrderComplete(id);
    await this.initialize();
  }

  getTotal(order: ISalesOrder) {
    return order?.salesOrderItems?.reduce((sum, cur) => {
      const price = cur?.product?.price;
      const quantity = cur?.quantity;
      return sum + (price && quantity ? price * quantity : 0);
    }, 0) ?? 0;
  }

  getStatus(isPaid: boolean) {
    return isPaid ? 'Paid Fully' : 'Not Paid';
  }
}
</script>

<style scoped lang="scss">
@import "@/scss/global";

.order-complete {
  color: $solar-green;
  font-size: 1.3rem;
  cursor: pointer;
}

.green {
  color: $solar-green;
}
</style>
