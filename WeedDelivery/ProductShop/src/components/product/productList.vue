<template>
  <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
    <div
        v-for="product in products"
        :key="product.id"
        class="flex flex-col items-center bg-white rounded-lg shadow-lg overflow-hidden hover:shadow-xl transition-shadow duration-300 cursor-pointer"
        @click="selectProduct(product)"
    >
      <img :src="product.images[0]" alt="Product Image" class="w-full h-48 object-cover">
      <div class="flex justify-between w-full p-4">
        <span class="font-semibold">{{ product.name }}</span>
        <span class="text-gray-700">{{ product.price }} ₽</span>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'ProductList',
  props: {
    products: {
      type: Array,
      required: true
    }
  },
  methods: {
    selectProduct(product) {
      this.$emit('select-product', product);
    }
  }
};
</script>
