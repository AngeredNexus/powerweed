<script>

import {defineComponent} from "vue";


import StoreView from "./customer/view/components/store-view.vue";
import {DxGallery} from "devextreme-vue";

export default defineComponent({

  name: "ClientApp",

  components: {
    StoreView,
    DxGallery
  },
  mounted() {
  },

  data() {
    return {
      items: ["https://img.freepik.com/premium-photo/marijuana-leaf-abstract-background-psychedelic-weed-cannabis_691560-5944.jpg?w=2000",
        "https://s.hdnux.com/photos/01/33/37/40/23984163/5/rawImage.jpg",
        "https://www.mercurynews.com/wp-content/uploads/2022/12/image1-4.jpg?w=1024"]
    }
  }

})

</script>

<template>


  <div id="app" class="bg-black b">
        
    <div>
      <ul class="flex items-stretchbg bg-white border-b-2 border-b3 bg-white">
        <li class="p-4 pl-4 select-none cursor-pointer inline-block hover:text-a1 font-bold hover:bg-b2 border-b-2 border-b1 hover:border-a1">
          <a class="transition-all duration-300 ease-in-out" href="/profile">SHOP</a>
        </li>
        <li class="p-4 select-none cursor-pointer inline-block hover:text-a1 font-bold hover:bg-b2 border-b-2 border-b1 hover:border-a1">
          <a class="transition-all duration-300 ease-in-out" href="/social">CONTACT</a>
        </li>
      </ul>
    </div>

    <div>
        <StoreView></StoreView>
    </div>
    
  </div>

</template>