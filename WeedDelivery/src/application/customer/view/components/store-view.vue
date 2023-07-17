<template>

  <div class="store-view-style">
    
    <template v-for="item in items">
      <weed-item-view
          :item.sync="item"
      ></weed-item-view>
    </template>
    
  </div>
  
</template>

<script lang="ts">
import {Vue, Component, Prop} from 'vue-property-decorator';
import WeedItem from "@/application/models/weed-item";
import weedItemView from "@/application/common/components/weed-item-view.vue";
import {CustomerBackController} from "@/application/repo/v1/customer/CustomerBackController";

@Component({
  name: "storeView",
  components: {
    weedItemView,
  },
  data() {
    return {
      items: [WeedItem]
    }
  },
  methods: {
    async updateItems()
    {
      let repo = new CustomerBackController();
      let itemsResp = await repo.POST_SEARCH("", null);
      this.items = itemsResp.data;
    }
  },
  async mounted() {
    await this.updateItems();
  }
})
export default class storeView extends Vue
{
}

</script>

<style scoped>

#store-view-style {
  display: flex;
  flex-flow: row wrap;
}

</style>