// src/main.js
import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";
import store from "./store";
import { price } from "./filters";
import { date } from "./filters";

const app = createApp(App);

app.config.globalProperties.$filters = {
  price,
  date
};

app.use(store).use(router).mount("#app");
