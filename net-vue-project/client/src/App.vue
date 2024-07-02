<template>
  <div id="app">
    <div class="app-menu">
      <side-menu />
    </div>
    <div class="app-content">
      <router-view />
    </div>
  </div>
</template>

<script lang="ts">
import { Vue, Options } from "vue-class-component";
import SideMenu from "@/components/SideMenu.vue";

@Options({
  components: { SideMenu },
})
export default class App extends Vue {

}
</script>

<style lang="scss">
@import "@/scss/global";

#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  display: flex;
  width: 100%;
}

.app-menu {
  position: fixed;
}

.app-content {
  width: 90%;
  padding: 1.2rem;
  margin: 1rem auto 1rem $menu-width;
}

* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

a {
  text-decoration: none;
}

hr {
  border: 1px solid #eee;
  margin: 1rem 0;
}

.table {
  width: 100%;
  max-width: 100%;
  margin-bottom: 2rem;
  border-collapse: collapse;
  width: 100%;

  tr {
    border-top: 1px solid #eee;
    border-bottom: 1px solid #eee;
    transition: background-color 0.3s;

    &:hover {
      background-color: #f5f5f5;
      transition: background-color 0.3s;
    }
  }

  td {
    padding: 1.2rem;
  }

  th {
    background-color: #fafafa;
    padding: 1.2rem;
    border-bottom: 1px solid $solar-blue;
  }
}
</style>
