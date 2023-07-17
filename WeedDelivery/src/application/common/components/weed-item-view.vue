<script lang="ts">
import {Vue, Component, Prop} from 'vue-property-decorator';
import WeedItem from "@/application/models/weed-item";
import {DxGallery, DxNumberBox} from "devextreme-vue";

@Component({
  name: "weedItemView",
  components: {
    DxGallery,
    DxNumberBox
  }
})
export default class weedItemView extends Vue {
  @Prop({default: null}) item: WeedItem
}
</script>

<template>

  <div>
    <DxGallery
        :data-source="[item.PhotoUrl]"
        :height="60"
        :weight="60">
    </DxGallery>
    <p>{{item.Name}}</p>
    <p>{{item.StrainType}}</p>
    <DxNumberBox
        :value="0"
        :min="0"
        :max="50"
        :show-spin-buttons="true"
        :input-attr="{ 'aria-label': 'Min And Max' }"
        :weight="25"
        :height="20"
    />
  </div>

</template>

<style scoped>

</style>