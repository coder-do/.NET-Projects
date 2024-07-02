<template>
  <solar-modal>
    <template v-slot:header>
      Add New Product
    </template>
    <template v-slot:body>
      <ul class="newProduct">
        <li>
          <label for="isTaxable">Is this product taxable?</label>
          <input id="isTaxable" type="checkbox" v-model="newProduct.isTaxable" />
        </li>
        <li>
          <label for="productName">Name</label>
          <input id="productName" type="text" v-model="newProduct.name" />
        </li>
        <li>
          <label for="Pdescription">Description</label>
          <input id="Pdescription" type="text" v-model="newProduct.description" />
        </li>
        <li>
          <label for="productPrice">Price (USD)</label>
          <input id="productPrice" type="number" v-model="newProduct.price" />
        </li>
      </ul>
    </template>
    <template v-slot:footer>
      <solar-button @click.native="save">
        Save Product
      </solar-button>
      <solar-button @click.native="close">
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

class Props {

}

@Options({
  name: "NewProductModal",
  components: { SolarButton, SolarModal },
})
export default class NewProductModal extends Vue.with(Props) {
  qtyReceived: number = 0;
  newProduct: IProduct = {
    id: +(new Date().valueOf().toString().substring(9)),
    name: 'Some Product',
    description: 'Some description',
    price: 1000,
    createdOn: new Date(),
    updatedOn: new Date(),
    isTaxable: true,
    isArchived: false
  }

  save() {
    this.$emit('save:product', this.newProduct);
  }

  close() {
    this.$emit('close');
  }
}
</script>

<style scoped lang="scss">
.newProduct {
  list-style: none;
  margin: 0;
  padding: 0;
}

input {
  width: 100%;
  height: 1.8rem;
  margin-bottom: 1.2rem;
  font-size: 1.1rem;
  line-height: 1.3rem;
  padding: 0.2rem;
  color: #555;
}

label {
  font-weight: bold;
  display: block;
  margin-bottom: 0.3rem;
}
</style>