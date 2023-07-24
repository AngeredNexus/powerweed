<template>

  <!--  <div v-for="item in items">-->
  <!--      <weed-item-view-->
  <!--          :item.sync="item"-->
  <!--      ></weed-item-view>-->
  <!--  </div>-->

  <div>

    <div id="content" class="">
      <div id="storeItems" class="flex flex-wrap gap-x-4 gap-y-8 my-4 mx-auto justify-center items-center">

        <weed-item-view v-for="item in items" :item.sync="item" :count.sync="item.count"/>

      </div>
    </div>
  </div>
</template>

<script>
import weedItemView from "../../../common/components/weed-item-view.vue";
import {defineComponent} from "vue";
import backendRepo from "../../../repo/v1/backend-repo";

export default defineComponent({
  name: "storeView",
  components: {
    weedItemView,
  },
  data() {
    return {
      repository: backendRepo("store"),
      items: []
    }
  },
  methods: {
    async updateItems() {
      
      let itemsResp = await this.repository.get("search-all", {});
      
      let wrapped = this.wrapItemsToStore(itemsResp.data.items);
      console.log(JSON.stringify(wrapped));
      
      this.items = wrapped;
    },
    wrapItemsToStore(items){
      
      return items.map(function (item_orig) {
          return {
            item: item_orig,
            count: 0
          }
      });
      
    }
  },
  async mounted() {
    await this.updateItems();
  }
})


</script>