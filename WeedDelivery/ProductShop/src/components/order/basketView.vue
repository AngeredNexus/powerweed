<template>
  <div class="cart-modal">
    <h2>Корзина</h2>
    <ul>
      <li v-for="item in cartItems" :key="item.id">
        <div>{{ item.name }} ({{ item.quantity }})</div>
        <div>{{ item.price }} ₽</div>
      </li>
    </ul>
    <div class="cart-total">
      <p>Итого: {{ cartPrice }} ₽</p>
      <p v-if="isHaveDiscount">С учетом скидки: {{ discountedPrice }} ₽</p>
    </div>
    <button @click="$emit('checkout')">Оформить заказ</button>
    <button @click="$emit('close-cart')">Закрыть</button>
  </div>
</template>

<script>
export default {
  name: 'BasketView',
  props: {
    cartItems: {
      type: Array,
      required: true
    },
    cartPrice: {
      type: Number,
      required: true
    },
    discountedPrice: {
      type: Number,
      required: false
    }
  },
  computed: {
    isHaveDiscount() {
      return this.discountedPrice < this.cartPrice;
    }
  }
};
</script>
