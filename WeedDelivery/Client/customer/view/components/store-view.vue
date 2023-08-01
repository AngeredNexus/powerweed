<template>
  <div>
    
    <div v-if="items.length > 0" id="content" class="">
      <div id="storeItems" class="flex flex-wrap gap-x-8 gap-y-10 my-4 mx-auto justify-center items-center xl:max-w-[75vw]">
        <weed-item-view v-for="item in items" :item="item" v-on:valueChanged="onItemCounterChanged"/>
      </div>
    </div>

    
    <div v-if="isHaveChoice">
      <div class="fixed min-w-[100vw] bottom-0 z-10">
        <DxButton class="min-w-[100vw] py-3 rounded bg-white text-center"
        @click="onMakeOrderClick">{{ totalPrice }}฿</DxButton>
      </div>
    </div>

  </div>
</template>

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
    isHaveChoice() {
      return this.ordering.some(p => p.count > 0);
    },
    totalPrice() {
      return 1000;
      // return this.ordering.reduce((x, y) => x + y);
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
    onMakeOrderClick(){
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