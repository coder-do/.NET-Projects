<template>
  <div>
    <h1 id="invoiceTitle">Create Invoice</h1>
    <hr />
    <div class="invoice-step" v-if="invoiceStep === 1">
      <h2>Step 1: Select Customer</h2>
      <div v-if="customers.length" class="invoice-step-detail">
        <label for="customer">Customer: </label>
        <select id="customer" class="invoiceCustomers" v-model="selectedCustomerId">
          <option value="" disabled>Please select a customer</option>
          <option v-for="c in customers" :value="c.id" :key="c.id">{{ c.firstName + ' ' + c.lastName }}</option>
        </select>
      </div>
    </div>
    <div class="invoice-step" v-if="invoiceStep === 2">
      <h2>Step 2: Create Order</h2>
      <div v-if="inventory.length" class="invoice-step-detail">
        <label for="product">Product: </label>
        <select id="product" class="invoiceLineItem" v-model="newItem.product">
          <option value="" disabled>Please select a product</option>
          <option v-for="c in inventory" :value="c.product" :key="c.product.id">{{ c.product.name }}
          </option>
        </select>
        <label for="quantity">Quantity:</label>
        <input type="number" min="0" v-model="newItem.quantity" id="quantity" />
      </div>

      <div class="invoice-item-actions">
        <solar-button :disabled="!newItem.product || !newItem.quantity" @click.native="addLineItem">
          Add Line Item
        </solar-button>
        <solar-button :disabled="!lineItems.length" @click.native="finalizeOrder">
          Finalize Order
        </solar-button>
      </div>

      <div class="invoice-order-list" v-if="lineItems.length">
        <div class="runningTotal">
          <h3>Running Total:</h3>
          {{ $filters.price(runningTotal) }}
        </div>
        <hr />
        <table class="table">
          <thead>
            <tr>
              <th>Product</th>
              <th>Description</th>
              <th>Qty</th>
              <th>Price</th>
              <th>Subtotal</th>
            </tr>
          </thead>
          <tr v-for="l in lineItems" :key="`index_${l.product?.id}`">
            <td>{{ l.product?.name }}</td>
            <td>{{ l.product?.description }}</td>
            <td>{{ l.quantity }}</td>
            <td>{{ l.product?.price }}</td>
            <td>{{ $filters.price(l.product?.price ?? 0 * l.quantity) }}</td>
          </tr>
        </table>
      </div>
    </div>
    <div class="invoice-step" v-if="invoiceStep === 3">
      <h2>Step 3: Review and Submit</h2>
      <solar-button @click.nativ="submitInvoice">Submit Invoice</solar-button>
      <hr />

      <div class="invoice-step-detail" id="invoice" ref="invoice">
        <div class="invoice-logo">
          <img src="../assets/images/solar-logo.avif" alt="ddd" id="imgLogo">

          <h3>1337 Solar Lane</h3>
          <h3>San Somewhere, CA 90000</h3>
          <h3>USA</h3>

          <div class="invoice-order-list" v-if="lineItems.length">
            <div class="invoice-header">
              <h3>Invoice: {{ $filters.date(new Date()) }}</h3>
              <h3>Customer: {{ this.selectedCustomer?.firstName + ' ' + this.selectedCustomer?.lastName }}</h3>
              <h3>Address: {{ this.selectedCustomer?.primaryAddress.address1 }}</h3>
              <h3 v-if="this.selectedCustomer?.primaryAddress.address2">
                {{ this.selectedCustomer.primaryAddress.address2 }}
              </h3>
              <h3>
                {{ this.selectedCustomer?.primaryAddress.city }}
                {{ this.selectedCustomer?.primaryAddress.state }}
                {{ this.selectedCustomer?.primaryAddress.postalCode }}
              </h3>
              <h3>
                {{ this.selectedCustomer?.primaryAddress.country }}
              </h3>
            </div>

            <table class="table">
              <thead>
                <tr>
                  <th>Product</th>
                  <th>Description</th>
                  <th>Qty</th>
                  <th>Price</th>
                  <th>Subtotal</th>
                </tr>
              </thead>
              <tr v-for="l in lineItems" :key="`index_${l.product?.id}`">
                <td>{{ l.product?.name }}</td>
                <td>{{ l.product?.description }}</td>
                <td>{{ l.quantity }}</td>
                <td>{{ l.product?.price }}</td>
                <td>{{ $filters.price(l.product?.price ?? 0 * l.quantity) }}</td>
              </tr>
              <tfoot>
                <tr>
                  <td colspan="4" class="due">Balance due upon receipt:</td>
                  <td class="price-final">{{ $filters.price(runningTotal) }}</td>
                </tr>
              </tfoot>
            </table>
          </div>
        </div>
      </div>
    </div>
    <hr />

    <div class="invoice-steps-actions">
      <solar-button @click.native="prev" :disabled="!canGoPrev">Previous</solar-button>
      <solar-button @click.native="next" :disabled="!canGoNext">Next</solar-button>
      <solar-button @click.native="startOver">Next</solar-button>
    </div>
  </div>
</template>

<script lang="ts">
import { PropType } from "vue";
import { Options, Vue, prop } from "vue-class-component";
import { IProduct, IProductInventory } from "@/types/Product";
import SolarButton from "@/components/SolarButton.vue";
import NewProductModal from "@/components/modals/NewProductModal.vue";
import ShipmentModal from "@/components/modals/ShipmentModal.vue";
import InvoiceService from "@/services/invoice-service";
import { IInvoice, ILineItem } from "@/types/Invoice";
import { ICustomer } from "@/types/Customer";
import CustomerService from "@/services/customer-service";
import InventoryService from "@/services/inventory-service";
import { jsPDF } from 'jspdf';
import html2canvas from 'html2canvas';

class Props {
  inventory = prop({
    required: false,
    type: Array as PropType<IProductInventory[]>
  });
}

const invoiceService = new InvoiceService();
const customerService = new CustomerService();
const inventoryService = new InventoryService();

@Options({
  name: "CreateInvoice",
  components: { SolarButton, NewProductModal, ShipmentModal },
})
export default class CreateInvoice extends Vue.with(Props) {
  invoiceStep: number = 1;
  invoice: IInvoice = {
    id: 1,
    createdOn: new Date(),
    updatedOn: new Date(),
    customerId: 0,
    lineItems: [{ quantity: 1, id: 1 }]
  };
  customers: ICustomer[] = [];
  selectedCustomerId: number = 0;
  inventory: IProductInventory[] = [];
  lineItems: ILineItem[] = [];
  newItem: ILineItem = { id: 1, quantity: 3, product: undefined };

  get selectedCustomer() {
    return this.customers.find(e => e.id === this.selectedCustomerId)
  }

  async submitInvoice() {
    this.invoice = {
      id: +(new Date().valueOf().toString().substring(9)),
      customerId: this.selectedCustomerId,
      lineItems: this.lineItems,
      createdOn: new Date(),
      updatedOn: new Date(),
    };

    await invoiceService.makeNewInvoice(this.invoice);

    this.downloadPDF();
    this.$router.push('/orders')
  }

  downloadPDF() {
    const doc = new jsPDF('p', 'pt', 'a4', true);
    const invoiceEl = document.getElementById('invoice');
    const width = (this.$refs.invoice as HTMLElement)?.clientWidth;
    const height = (this.$refs.invoice as HTMLElement)?.clientHeight;
    html2canvas(invoiceEl!).then((c: any) => {
      let image = c.toDataURL('image/png');
      doc.addImage(image, 'PNG', 0, 0, width * 0.55, height * 0.55);
      doc.save('invoice');
    });
  }

  addLineItem() {
    let newItem: ILineItem = {
      id: this.newItem.product?.id || 1,
      product: this.newItem.product,
      quantity: +this.newItem.quantity
    };

    let existingItems = this.lineItems.map(e => e.product?.id);

    if (existingItems.includes(newItem.product?.id)) {
      let lineItem = this.lineItems.find(e => e.product?.id === newItem.product?.id);
      if (lineItem) {
        lineItem.quantity = lineItem.quantity += newItem.quantity;
      }
    } else {
      this.lineItems.push(newItem);
    }

    this.newItem = { product: undefined, quantity: 0, id: 1 };
  }

  finalizeOrder() {
    this.invoiceStep = 3;
  }

  startOver() {
    this.invoice = { customerId: 0, lineItems: [], createdOn: new Date(), updatedOn: new Date(), id: 1 };
    this.invoiceStep = 1;
  }

  get runningTotal() {
    // return this.lineItems.reduce((a, b) => a + (b['product']['price'] * b['quantity']), 0);
    return this.lineItems?.reduce((a, b) => {
      const price = b?.product?.price ?? 0;
      const quantity = b?.quantity ?? 0;
      return a + (price * quantity);
    }, 0) ?? 0;
  }

  get canGoPrev() {
    return this.invoiceStep !== 1;
  }

  get canGoNext() {
    if (this.invoiceStep === 1) {
      return this.selectedCustomerId !== 0;
    }

    if (this.invoiceStep === 2) {
      return this.lineItems.length;
    }

    if (this.invoiceStep === 3) {
      return false;
    }

    return false;
  }

  prev() {
    if (this.invoiceStep === 1) {
      return;
    }
    this.invoiceStep -= 1;
  }

  next() {
    if (this.invoiceStep === 3) {
      return;
    }
    this.invoiceStep += 1;
  }

  async created() {
    await this.initialize();
  }
  async initialize() {
    this.customers = await customerService.getCustomers();
    this.inventory = await inventoryService.getInventory();
  }
}
</script>

<style scoped lang="scss">
@import "@/scss/global";

.invoice-step-actions {
  display: flex;
  width: 100%;
}

.invoice-step-detail {
  margin: 1.2rem;
}

.invoice-order-list {
  margin-top: 1.2rem;
  padding: 0.8rem;

  .line-item {
    display: flex;
    padding: 0.8rem;
    border-bottom: 1px dashed #ccc;
  }

  .item-col {
    flex-grow: 1;
  }
}

.invoice-items-actions {
  display: flex;
}

.price-pre-tax {
  font-weight: bold;
}

.price-final {
  font-weight: bold;
  color: $solar-green;
}

.due {
  font-weight: bold;
}

.invoice-header {
  width: 100%;
  margin-bottom: 1.2rem;
}

.invoice-logo {
  padding-top: 1.4rem;
  text-align: center;

  img {
    width: 280px;
  }
}
</style>
