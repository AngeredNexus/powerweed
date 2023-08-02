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
  computed: {
    redirectLoginUrl()
    {
      return `${process.env.APP_API_HOST}/api/v1/login`;
    },
    botname()
    {
      return `${process.env.APP_BOT_NAME}`;
    }
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

        <!--      Disable on test mb        -->
        
        <telegram-login-temp
            mode="redirect"
            :telegram-login="botname"
            :redirect-url="redirectLoginUrl"
        />
        
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