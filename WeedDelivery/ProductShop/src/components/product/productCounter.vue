<template>
  <div class="flex items-center justify-between w-full max-w-xs mt-6 p-4 bg-white rounded-lg shadow-lg">
    <!-- Количество продукта -->
    <span class="text-lg font-semibold text-gray-800">{{ $t('basket.amount') }} {{ quantity }}</span>

    <!-- Кнопки управления -->
    <div class="flex space-x-4">
      <button
          @click="decrease"
          class="bg-red-500 text-white px-4 py-2 rounded hover:bg-red-600 transition duration-300"
      >
        &minus;
      </button>
      <button
          @click="increase"
          class="bg-green-500 text-white px-4 py-2 rounded hover:bg-green-600 transition duration-300"
      >
        &plus;
      </button>
    </div>
  </div>
</template>

<script>
export default {
  name: 'ProductCounter',
  props: {
    productId: {
      type: String,
      required: true
    },
    initialQuantity: {
      type: Number,
      default: 0
    }
  },
  data() {
    return {
      quantity: this.initialQuantity
    };
  },
  watch: {
    quantity(newQuantity) {
      this.$parent.$emit('updateQuantity', { productId: this.productId, quantity: newQuantity });
    }
  },
  methods: {
    decrease() {
      if (this.quantity > 1) {
        this.quantity--;
      }
    },
    increase() {
      this.quantity++;
    }
  }
}
</script>
