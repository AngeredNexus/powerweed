<template>

  <div class="store-view-style">
    
    <template v-for="item in items">
      <weed-item-view
          :item.sync="item"
      ></weed-item-view>
    </template>
    
  </div>
  
</template>

<script>
import weedItemView from "../../../common/components/weed-item-view.vue";
import {defineComponent} from "vue";

export default defineComponent({
  name: "storeView",
  components: {
    weedItemView,
  },
  data() {
    return {
      items: [
        {
          name: "Wedding Gelatto",
          strainType: "Indica",
          photoUrl: "https://i.ibb.co/QQ9pY3S/pf120x150.png"
        },
        {
          name: "Garlic Bread",
          strainType: "Indica dominant",
          photoUrl: "https://i.ibb.co/dfm5Jgy/gdp120x150.png"
        },
        {
          name: "MAC1",
          strainType: "Sativa",
          photoUrl: "https://i.ibb.co/T48Jd7v/mac1120x150.png"
        },
      ]
    }
  },
  methods: {
    async updateItems()
    {
      // let repo = new CustomerBackController();
      // let itemsResp = await repo.POST_SEARCH("search", {});
      // this.items = itemsResp.data;
    }
  },
  async mounted() {
    await this.updateItems();  }
})


</script>

<style scoped>

.store-view-style {
  display: flex;
  flex-flow: row wrap;
}

</style>