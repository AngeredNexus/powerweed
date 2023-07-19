<template>
  <div>
    
    <button v-for="(data, index) in mainCategories" :key="index">
      {{ data.name }}
    </button>
    
  </div>
  
</template>

<script>
import {defineComponent} from "vue";

export default defineComponent({
  name: "",
  data() {
    return {
      mainCategories: []
    }
  }
})
</script>

<style scoped>

</style>