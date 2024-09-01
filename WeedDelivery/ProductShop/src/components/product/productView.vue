<template>
  <div class="product-detail">
    <div class="product-image">
      <img :src="product.images[currentImageIndex]" @click="nextImage" />
    </div>
    <div class="product-info">
      <h2>{{ product.name }}</h2>
      <p>THC: {{ product.thcContent }}%</p>
      <p>Price: {{ product.price }} ₽</p>
      <div class="quantity-selector">
        <button @click="decreaseQuantity">-</button>
        <input type="number" v-model="quantity" min="0" />
        <button @click="increaseQuantity">+</button>
      </div>
      <button @click="addToCart">Add to Cart</button>
    </div>
  </div>
</template>

<script>
export default {
  name: 'ProductView',
  props: {
    product: {
      type: Object,
      required: true
    }
  },
  data() {
    return {
      quantity: 1,
      currentImageIndex: 0
    };
  },
  methods: {
    nextImage() {
      this.currentImageIndex = (this.currentImageIndex + 1) % this.product.images.length;
    },
    decreaseQuantity() {
      if (this.quantity > 0) {
        this.quantity--;
      }
    },
    increaseQuantity() {
      this.quantity++;
    },
    addToCart() {
      this.$emit('update-quantity', {
        productId: this.product.id,
        quantity: this.quantity
      });
    }
  }
};
</script>
