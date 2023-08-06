<script>

import {defineComponent} from "vue";
import back_repo from "@/repo/v1/backend-repo";

import StoreView from "./customer/view/components/store-view.vue";
import {DxGallery} from "devextreme-vue";
import CustomerOrderView from "@/customer/order/components/customer-order-view.vue";
import {telegramLoginTemp} from 'vue3-telegram-login';
import {getCookie} from "@/utils/utils";

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
    },
    onOrderDone(){
      this.isOrdering = false;
      console.log(this.isOrdering);
    }
  },
  computed: {
    redirectLoginUrl() {
      return `${process.env.APP_API_HOST}/api/v1/auth/login`;
    },
    botname() {
      return `${process.env.APP_BOT_NAME}`;
    }
  },
  async mounted() {
    
    let resp = await repo.get("auth").then(x => x);

    this.isAuthedTg = resp.data.isAuthSuccess;

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
      </ul>
    </div>

    <div v-if="!isAuthedTg" class="flex w-full justify-center pt-14">

      <telegram-login-temp  mode="redirect"
                           :telegram-login="botname"
                           :redirect-url="redirectLoginUrl"
      />

    </div>
    
    <div v-else>
      <div v-if="!isOrdering" class="pb-12 pt-6">
        <StoreView @makeOrderClicked="onMakeOrder"/>
      </div>

      <div v-else class="">
        <CustomerOrderView :orderItems="order" @ordered="onOrderDone"/>
      </div>
    </div>

  </div>

</template>