<template>
  <div class="flex flex-col pt-8 m-2 justify-center items-center xl:max-w-[100vw] backdrop-blur-sm bg-white/30 rounded">    
      <div class="flex flex-col p-2">
        <label class="text-amber-300" for="firstName">Firstname:</label>
        <input id="firstName" v-model="order.firstname" type="text" required />
      </div>

    <div class="flex flex-col p-2">
      <label class="text-amber-300" for="lastName">Lastname:</label>
      <input id="lastName" v-model="order.lastname" type="text" required />
    </div>
    
      <div class="flex flex-col p-2">
        <label class="text-amber-300" for="address">Shipping Address:</label>
        <input id="address" v-model="order.address" type="text" required />
      </div>

      <div class="flex flex-col p-2">
        <label class="text-amber-300" for="phone">Phone number</label>
        <input id="phone" v-model="order.phoneNumber" type="number" required />
      </div>

      <div class="bg-white text-amber-300 border-x-4 border-y-6 grow">
        <button type="submit" @click="submitForm">Submit Order</button>
      </div>
  </div>
</template>


<script>


import {defineComponent} from "vue";
import back_repo from "@/repo/v1/backend-repo";

const repo = back_repo("store");

export default defineComponent({
  name: "customerOrderView",
  props: ["orderItems"],
  data() {
    return {
      order: {
        firstname: '',
        lastname: '',
        address: '',
        phoneNumber: '',
        items: []
      }
    };
  },
  methods: {
    async submitForm() {
      
      let orderItems = this.orderItems.map(item => {
        return {
          weedId: item.id,
          amount: item.count
        }
      });
      
      let order = this.order;
      order.items = orderItems;
      
      console.log(order)
      await repo.post("order", order);
      
    }
  }
});
</script>

<style scoped>
.order-form {
  /* add your styles here */
}
</style>