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


  <div class="page-style">
        
    <div class="page-head-style">
      <div class="head-style">
        <img src="https://cmf420.com/wp-content/uploads/2022/05/Stoned-Island-Tee-White2.jpg" class="head-logo-style"/>
      </div>
      <div class="splitter-style"></div>
    </div>

    <div class="store-view-style">
        <StoreView></StoreView>
    </div>
    
  </div>

</template>

<style scoped>

* {
  padding: 0;
  margin: 0;
  //box-sizing: border-box;
}

.page-style {
  flex-direction: column;
  //margin-top: 1%;
  //margin-left: 1%;
  //min-height: 100vh;
  flex: 100%;
}

.page-head-style {
flex: 10%;
}

.head-style {
  display: inline-grid;
}

.head-logo-style {
  height: 60px;
  weight: 60px;
}

.splitter-style {

}

.store-view-style {
  background-color: black;
  flex-grow: 1;
}

</style>