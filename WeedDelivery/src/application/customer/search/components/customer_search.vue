<template>
  <div>
    
    <button v-for="(data, index) in mainCategories" :key="index">
      {{ data.name }}
    </button>
    
  </div>
  
</template>

<script lang="ts">
import {Vue, Component} from 'vue-property-decorator';
import {WeedCategory} from '../../../common/models/WeedCategory';
@Component({
  data() {
    return {
      mainCategories: []
    }
  }
})

export default class customer_search extends Vue
{

}
</script>

<style scoped>

</style>