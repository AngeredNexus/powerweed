<template>

  <div>

    <div
        class="flex flex-col pt-8 m-4 justify-center items-center xl:max-w-[100vw] backdrop-blur-sm bg-white/30 rounded">

      <div v-if="!isOnSubmit">

        <div class="text-amber-300">
          <p>BUY 5-9gr : 1st lvl discount</p>
          <p>BUY 10-49gr: 2nd lvl discount</p>
          <p>BUY 49+gr : 3rd lvl discount</p>
        </div>


        <div class="flex flex-col p-2">
          <label class="text-amber-300" for="firstName">Firstname*</label>
          <input id="firstName" v-model="order.firstname" type="text" required/>
        </div>

        <div class="flex flex-col p-2">
          <label class="text-amber-300" for="lastName">Lastname*</label>
          <input id="lastName" v-model="order.lastname" type="text" required/>
        </div>

        <div class="flex flex-col p-2">
          <label class="text-amber-300" for="address">Shipping Address*</label>
          <input id="address" v-model="order.address" type="text" required/>
        </div>

        <div class="flex flex-col p-2">
          <label class="text-amber-300" for="phone">Phone number*</label>
          <input id="phone" v-model="order.phoneNumber" type="number" required/>
        </div>

        <div class="flex flex-col p-2">
          <label class="text-amber-300" for="comment">Comment</label>
          <textarea id="comment" v-model="order.comment"/>
        </div>
      </div>

      <div v-if="isSubmitionEnabled" class="flex pt-1 m-4 h-8 bg-amber-300 border-amber-300 rounded-lg justify-center">

        <div class="grow text-black justify-center text-center">
          <button class="w-full" type="submit" @click="submitForm">ORDER</button>
        </div>

      </div>

      <div v-else class="grow">

        <Login :is-login-visible="isOnSubmit" @loginUpdated="onLoginUpdated"/>

      </div>
    </div>
  </div>
</template>


<script>


import {defineComponent} from "vue";
import back_repo from "@/repo/v1/backend-repo";
import {DxTextBox} from "devextreme-vue";
import Login from "@/common/components/login.vue";

const repo = back_repo("store");

export default defineComponent({
  name: "customerOrderView",
  props: ["orderItems", "tgsh"],
  components: {
    DxTextBox,
    Login
  },
  data() {
    return {
      order: {
        firstname: '',
        lastname: '',
        address: '',
        phoneNumber: '',
        comment: '',
        items: [],
        hash: ""
      },
      isOnSubmit: false
    };
  },
  computed: {
    isSubmitionEnabled() {

      let isOrderFilled =
          this.order.firstname !== '' &&
          this.order.lastname !== '' &&
          this.order.address !== '' &&
          this.order.phoneNumber !== '';

      let norm = JSON.parse(JSON.stringify(this.orderItems));
      let itemsCount = norm.length;

      console.log(isOrderFilled);
      console.log(norm);
      console.log(itemsCount);

      return !this.isOnSubmit && isOrderFilled && itemsCount > 0;
    }
  },
  methods: {
    async submitForm() {
      this.isOnSubmit = true;
    },
    async onLoginUpdated(loginObject) {
      
      if(!loginObject.isCodeConfirmed)
      {
        this.isOnSubmit = false;
        return;
      }
      
      let orderItems = this.orderItems.map(item => {
        return {
          weedId: item.id,
          amount: item.count
        }
      });

      let order = this.order;
      order.items = orderItems;
      order.hash = loginObject.hash;
      
      await repo.post("order", order);

      this.isOnSubmit = false;

      this.$emit("ordered");
    }
  }
});
</script>

<style scoped>
.order-form {
  /* add your styles here */
}
</style>