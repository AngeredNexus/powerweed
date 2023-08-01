<script>

import {defineComponent} from "vue";
import back_repo from "@/repo/v1/backend-repo";

import StoreView from "./customer/view/components/store-view.vue";
import {DxGallery} from "devextreme-vue";
import CustomerOrderView from "@/customer/order/components/customer-order-view.vue";
import { telegramLoginTemp } from 'vue3-telegram-login';

const repo = back_repo("auth")

export default defineComponent({

  name: "ClientApp",
  components: {
    StoreView,
    DxGallery,
    CustomerOrderView,
    telegramLoginTemp 
  },
  data() {
    return {
      isAuthedTg: false,
      isOrdering: false,
      order: []
    }
  },
  methods: {
    onMakeOrder(order) {
      this.order = order;
      this.isOrdering = true;
    },
    onLoginCallback(user) {
      console.log(user);
    }
  },
  mounted() {
    let tgAuthUser = {
      id: "924989531",
      username: "thenexofficial"
    }

    repo.post("login-test", tgAuthUser);
    
  }
})

</script>

<template>


  <div id="app" class="b">

    <div class="">
      <ul class="flex items-stretch bg-white/20 p-2">
        <li class="w-16 flex-none select-none cursor-pointer inline-block hover:text-a1 font-bold"
            style="background-image: url('/dist/static/media/smokeisland.svg');">
        </li>
        <li class="p-4 select-none grow cursor-pointer inline-block hover:text-a1 font-bold">
          <div class="text-amber-300 text-center text-2xl my-auto [text-shadow:_0_2px_0_rgb(0_0_0_/_40%)]">SMOKE
            ISLAND
          </div>
        </li>

<!--        <telegram-login-temp-->
<!--            mode="redirect"-->
<!--            telegram-login="samplebot"-->
<!--            redirect-url="http://localhost:55525/api/v1/login"-->
<!--        />-->
        
      </ul>
    </div>

    <div v-if="!isOrdering" class="pb-12 pt-6">
      <StoreView @makeOrderClicked="onMakeOrder"/>
    </div>

    <div v-else class="">
      <CustomerOrderView :orderItems="order"/>
    </div>

  </div>

</template>