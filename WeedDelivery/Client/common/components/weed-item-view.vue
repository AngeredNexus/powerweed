<script>
import {DxGallery} from "devextreme-vue";
import {defineComponent} from "vue";
import CounterWithButton from "@/common/components/counter-with-button.vue";


export default defineComponent({
  name: "weedItemView",
  components: {
    DxGallery,
    CounterWithButton,
  },
  props: ["item", "count"],
  data() {
    return {
      isGalleryVisible: false,
      images: ['https://i.ibb.co/QQ9pY3S/pf120x150.png', 'https://i.ibb.co/dfm5Jgy/gdp120x150.png']
    }
  },
  methods: {
    onGalleryClicked() {
      this.isGalleryVisible = true;
    },
    onIncrementClick(){
      
      if(this.count >= 100)
        return;
      
      this.count += 1;
    },
    onDecrementClick(){
      
      if(this.count === 0)
        return;
      
      this.count -= 1;
    }
  },
  computed: {
    getItemPhotoBgStyle() {
      let style = {
        "background-image": `url('${this.item.item.photoUrl}')`
      }
      
      return style;
    }
  }
})

</script>

<template>
  
  <div>

    <div id="shopItem">
      <div class="flex flex-col justify-between gap-0 text-amber-200 object-cover shadow-lg shadow-white/20 backdrop-blur-sm bg-white/30 rounded border-0 border-white/15 bg-no-repeat bg-cover bg-center h-32 w-32" :style="getItemPhotoBgStyle">
        <div class="text-center justify-center text-lg [text-shadow:_0_1px_0_rgb(0_0_0_/_40%)]  flex items-start">
          <span class="line-clamp-2 leading-6">{{item.item.name}}</span>
        </div>
        <div class="text-right pr-1 text-xs [text-shadow:_0_1px_0_rgb(0_0_0_/_40%)] items-end">THC: 20%</div>
      </div>
      <div class="text-center [text-shadow:_0_1px_0_rgb(0_0_0_/_40%)] text-white mt-1">
        <div id="itemCounter" class="flex flex-row items-center justify-center">
          <div class="mx-2.5"><p class="rounded" @click="onDecrementClick">◄</p></div>
          <div class="">{{count}}</div>
          <div class="mx-2.5"><p class="rounded" @click="onIncrementClick">►</p></div>
        </div>
      </div>
    </div>
    
  </div>

</template>