<script>
import weedItemView from "../../../common/components/weed-item-view.vue";
import {defineComponent} from "vue";
import backendRepo from "../../../repo/v1/backend-repo";
import {DxButton} from "devextreme-vue";

const repository = backendRepo("store");

let items = [];

export default defineComponent({
  name: "storeView",
  components: {
    weedItemView,
    DxButton
  },
  data() {
    return {
      items,
    }
  },
  computed: {
    ordering() {
      return this.items.filter(i => i.count > 0);
    },
    totalOrderingCount() {

      let sum = 0;

      this.ordering.forEach(x => sum += x.count);

      return sum;
    },
    isHaveChoice() {
      return this.ordering.some(p => p.count > 0);
    },
    actualPrice() {

      let totalCount = this.totalOrderingCount;

      if (totalCount < 50) {
        if (totalCount < 10) {
          if (totalCount < 5)
            return 400;
          return 350;
        }
        return 300;
      }
      return 250;
    },
    actualDeliveryPrice() {
      return this.totalOrderingCount >= 5 ? 50 : 150;
    },
    totalPriceFull(){
      let sum = 0;
      this.ordering.forEach(x => sum += x.price * x.count);
      return sum;
    },
    totalPrice() {
      
      if(this.isDiscounted)
      {
        return this.actualPrice * this.totalOrderingCount + this.actualDeliveryPrice;  
      }

      return this.totalWithoutDiscount;
      
    },
    totalWithoutDiscount() {
      return this.totalPriceFull + 150;
    },
    isDiscounted() {
      return this.totalOrderingCount > 4;
    },
    totalPriceStyle() {
      return this.isDiscounted ? "font-bold underline" : "font-bold";
    }
  },
  methods: {
    async updateItems() {
      this.items = [];
      let itemsResp = repository.get("search-all", {}).then(x => this.items = x.data.items);
    },
    onItemCounterChanged(id, value) {

      console.log(`id: ${id}; value: ${value}`)

      let itemsReactiveCopy = this.items;
      itemsReactiveCopy.find(i => i.id === id)["count"] = value;

      this.items = itemsReactiveCopy;

    },
    onMakeOrderClick() {

      let updObj = {}

      this.$emit("makeOrderClicked", this.ordering);
    }
  },
  mounted() {
    this.updateItems();
  }
})


</script>

<style>

</style>


<template>
  <div>

    <div class="flex flex-col justify-center text-center items-center">
      <p class="text-amber-300">Delivery in 60-90 mins!</p>
      <p class="text-amber-300">Доставка в течении 60-90 минут!</p>
      <p class="text-amber-400 text-lg">Take 5+g ➟ All weed price 350฿\g</p>
    </div>

    <div v-if="items.length > 0" id="content" class="">
      <div id="storeItems"
           class="flex flex-wrap gap-x-8 gap-y-10 my-4 mx-auto justify-center items-center xl:max-w-[75vw]">
        <weed-item-view v-for="item in items" :item="item" :price="actualPrice"
                        v-on:valueChanged="onItemCounterChanged"/>
      </div>
    </div>


    <div v-if="isHaveChoice" class="fixed min-w-[100vw] bottom-0 z-10 ">

      <div class=" w-full bg-white border text-center backdrop-blur-sm">

        <div class="flex pl-4 space-x-4 content-center">
          <div class="flex justify-center py-5 grow space-x-4">
            <p :class=totalPriceStyle>{{ totalPrice }}฿</p>
            <p v-if=isDiscounted class="text-red-400 line-through font-bold ">{{ totalWithoutDiscount }}฿</p>
            <p class="pl-3">{{ totalOrderingCount }}g.</p>
          </div>

          <div class="py-4 pr-4">
            <div class="w-24 border-amber-400 bg-amber-300 border-2 rounded-lg h-10">
              <DxButton class="pt-2 text-black"
                        @click="onMakeOrderClick">
                BUY
              </DxButton>
            </div>
          </div>
        </div>

      </div>

    </div>

  </div>
</template>