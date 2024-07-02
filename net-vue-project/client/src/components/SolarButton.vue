<template>
  <div class="btn-link">
    <button :disabled="disabled" v-if="link" @click="visitRoute"
      :class="['solar-button', { 'full-width': isFullWidth }]">
      <slot> </slot>
    </button>
    <button :disabled="disabled" v-else @click="onClick" :class="['solar-button', { 'full-width': isFullWidth }]">
      <slot> </slot>
    </button>
  </div>
</template>

<script lang="ts">
import { PropType } from "vue";
import { Options, Vue, prop } from "vue-class-component";

class Props {
  link = prop({
    required: false,
    type: String as PropType<string | undefined>
  });
  isFullWidth = prop({
    required: false,
    default: false,
    type: Boolean as PropType<boolean>
  });
  disabled = prop({
    required: false,
    default: false,
    type: Boolean as PropType<boolean>
  });
}

@Options({
  name: "SideMenu",
  components: {},
})
export default class SolarButton extends Vue.with(Props) {
  visitRoute() {
    this.$router.push(this.link || '#');
  }
  onClick() {
    this.$emit('button:click');
  }
}
</script>

<style scoped lang="scss">
@import '@/scss/global';

.solar-button {
  background: lighten($solar-blue, 10%);
  color: white;
  padding: 0.8rem;
  transform: background-color 0.5s;
  margin: 0.3rem 0.2rem;
  display: inline-block;
  cursor: pointer;
  font-size: 1rem;
  min-width: 100px;
  border: none;
  border-bottom: 2px solid darken($solar-blue, 20%);
  border-radius: 3px;

  &:hover {
    background: lighten($solar-blue, 20%);
    transition: background-color 0.5s;
  }

  &:disabled {
    background: lighten($solar-blue, 15%);
    border-bottom: 2px solid darken($solar-blue, 20%);
  }

  &:active {
    background: $solar-yellow;
    border-bottom: 2px solid lighten($solar-yellow, 20%);
  }
}

.full-width {
  display: block;
  width: 100%;
}
</style>
