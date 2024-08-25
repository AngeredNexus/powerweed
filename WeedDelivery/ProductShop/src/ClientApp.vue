<template>
  <div id="app">
    <router-view v-if="mainPageMounted"/>
  </div>
</template>

<script>
import ShopView from "./components/shop/shopView.vue";
import AuthAwait from "./components/auth/authAwait.vue";
import GuestView from "./components/auth/guest.vue";

import router from "./components/router/router";

export default {
  name: 'app',
  data() {
    return {
      mainPageMounted: false
    }
  },
  created() {
    this.mainPageMounted = true;
    router.push({name: "login"});
  },
  components: {
    ShopView,
    GuestView,
    AuthAwait
  }
};
</script>

