<template>
  <div class="row justify-content-between">

   
    
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator';
import { HelloApi } from './HelloApi.vue';
import { store, signout } from '../../shared';
import {reactive} from "vue";

const mainCategories = reactive<Category[]>([
  {name: "По времени суток", value: 1, hasSubcategories: true, subCategories: []},
  {name: "Под занятие", value: 2, hasSubcategories: true, subCategories: []},
  {name: "По силе", value: 3, hasSubcategories: true, subCategories: []},
  {name: "По генетике", value: 4, hasSubcategories: true, subCategories: []}
]);

@Component({
  components: {
    HelloApi,
  },
})
export class Home extends Vue {
  @Prop({ default: '' }) name: string;
  signout = signout;

  get userSession() {
    return store.userSession;
  }

  get mainCategories() {
    return mainCategories;
  }
  
}
export default Home;
</script>
