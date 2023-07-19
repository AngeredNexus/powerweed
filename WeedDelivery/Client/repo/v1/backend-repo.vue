<script>
import {defineComponent} from 'vue';
import baseRepo from '../base-repo.vue';

export default defineComponent({
  name: "backend-repo",
  props: ['backendUrl'],
  
  components: {
    baseRepo
  },
  
  data() {
    return {
      baseUrl: "http://localhost:55525/api/v1"  
    }
  },

  methods: {
    async post(url, data){
      
        let full_url = `${this.backendUrl}/${url}`;        
        return await this.$refs.repo.post(full_url, data);
        
    },
    async get(url, params){

      let full_url = `${this.backendUrl}/${url}`;
      return await this.$refs.repo.get(full_url, params);
      
    },
  },
})
</script>

<template>

  <backend-repo :baseUrl="backendUrl" ref="repo"/>
  
</template>

<style scoped>

</style>