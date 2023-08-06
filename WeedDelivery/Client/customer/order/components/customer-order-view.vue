<template>

  <div>
    <div class="flex flex-col pt-8 m-4 justify-center items-center xl:max-w-[100vw] backdrop-blur-sm bg-white/30 rounded">

      <div class="text-amber-300">
        <p>BUY 1-4gr: 400฿ + 150฿ Delivery</p>
        <p>BUY 5-9gr: 350฿ + 50฿ Delivery</p>
        <p>BUY 10-49gr: 300฿ + 50฿ Delivery</p>
        <p>BUY 49+gr: 250฿ + 50฿ Delivery</p>
      </div>

      <div class="flex flex-col p-2">
        <label class="text-amber-300" for="firstName">Firstname:</label>
        <input id="firstName" v-model="order.firstname" type="text" required/>
      </div>

      <div class="flex flex-col p-2">
        <label class="text-amber-300" for="lastName">Lastname:</label>
        <input id="lastName" v-model="order.lastname" type="text" required/>
      </div>

      <div class="flex flex-col p-2">
        <label class="text-amber-300" for="address">Shipping Address:</label>
        <input id="address" v-model="order.address" type="text" required/>
      </div>

      <div class="flex flex-col p-2">
        <label class="text-amber-300" for="phone">Phone number</label>
        <input id="phone" v-model="order.phoneNumber" type="number" required/>
      </div>

      <div class="flex flex-col p-2">
        <label class="text-amber-300" for="comment">Comment</label>
        <input id="comment" v-model="order.comment" type="text" required/>
      </div>

    </div>

    <div class="flex pt-1 m-4 h-8 bg-amber-300 border-amber-300 rounded-lg justify-center">
      <div class="grow text-black justify-center text-center">
        <button class="w-full" type="submit" @click="submitForm">ORDER</button>
      </div>

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
        comment: '',
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

      console.log("Order done!");
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