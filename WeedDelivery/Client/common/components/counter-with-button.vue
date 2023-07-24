<script>
import {defineComponent} from 'vue'
import {DxButton, DxNumberBox} from "devextreme-vue";

export default defineComponent({
  name: "counterWithButton",
  components: {
    DxButton,
    DxNumberBox
  },
  data() {
    return {
      value: 0
    }
  },
  computed: {
    isAdded() {
      return this.value > 0;
    }
  },
  methods: {
    decrementClick() {
      this.value = this.value - 1;
    },
    incrementClick() {
      this.value = this.value + 1;
    }
  }
})
</script>

<template>

  <div>
    
    <div v-if="!isAdded">
      
      <DxButton
      :width="120"
      :height="50"
      @click="incrementClick"
      >Добавить</DxButton>
      
    </div>
    
    <div v-else class="counter-style">
      
      <DxButton
          :width="50"
          :height="50"
          @click="decrementClick"
      >-</DxButton>
      <DxNumberBox
          class="counter-number-box-style"
          :width="80"
          :height="50"
          :max="100"
          :min="0"
          :value="value"
      ></DxNumberBox>
      <DxButton
          :width="50"
          :height="50"
          @click="incrementClick"
      >+</DxButton>
      
    </div>
    
  </div>
  
</template>

<style scoped>

.counter-style {
  display: inline-grid;
  grid-template-columns: 8% 20% 10%;
  align-content: start;
}

</style>

<style>

.counter-number-box-style .dx-texteditor-input {
  width: 30px;
}

</style>