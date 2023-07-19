<script>
import {DxGallery, DxNumberBox} from "devextreme-vue";
import {defineComponent} from "vue";


export default defineComponent({
  name: "weedItemView",
  components: {
    DxGallery,
    DxNumberBox
  },
  props: ["item"],
})

</script>

<template>

  <div class="item-flex">
    <DxGallery
        :data-source="[item.photoUrl]"
        :height="120"
        :weight="150">
    </DxGallery>
    <p>{{ item.name }}</p>
    <p>{{ item.strainType }}</p>
    <DxNumberBox
        :value="0"
        :min="0"
        :max="50"
        :show-spin-buttons="true"
        :show-clear-button="true"
        :weight="120"
        :height="50"
    />
  </div>

</template>

<style scoped>

.item-flex {
  flex: 45%;
}

</style>