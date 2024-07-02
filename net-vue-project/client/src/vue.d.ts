// src/vue.d.ts
import { ComponentCustomProperties } from 'vue';
import { price } from './filters';
import { date } from './filters';

declare module '@vue/runtime-core' {
  interface ComponentCustomProperties {
    $filters: {
      price: typeof price;
      date: typeof date;
    };
  }
}
