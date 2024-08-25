<template>
  <div class="container mx-auto p-6">
    <div class="flex justify-between items-center mb-4">
      <h1 class="text-2xl font-bold">{{ $t('checkout.title') }}</h1>
      <button @click="goBack" class="text-blue-500 underline">{{ $t('checkout.back') }}</button>
    </div>

    <ul class="mt-4">
      <li v-for="item in cartItems" :key="item.id" class="flex justify-between py-2">
        <span>{{ item.name }}</span>
        <span>{{ item.price }} ₽</span>
      </li>
    </ul>

    <div class="mt-4">
      <input v-model="name" type="text" :placeholder="$t('checkout.namePlaceholder')" class="border p-2 w-full mb-2" />
      <input
          v-model="phone"
          type="text"
          :placeholder="$t('checkout.phonePlaceholder')"
          class="border p-2 w-full mb-2"
          @input="applyPhoneMask"
      />
      <input v-model="comment" type="text" :placeholder="$t('checkout.commentPlaceholder')" class="border p-2 w-full mb-2" />
      <address-input v-model="address" class="mb-2" :readonly="true"></address-input>
    </div>

    <div class="flex justify-end">
      <button @click="confirmOrder" :disabled="isSubmitting" class="bg-green-500 text-white py-2 px-4 rounded">
        {{ $t('checkout.confirmButton') }}
      </button>
    </div>

    <!-- Модальное окно статуса заказа -->
    <div v-if="isSubmitting" class="fixed inset-0 flex justify-center items-center bg-black bg-opacity-50 z-50">
      <div class="bg-white p-6 rounded shadow-lg text-center">
        <p>{{ statusMessage }}</p>
        <button @click="closeModal" class="mt-4 bg-blue-500 text-white py-2 px-4 rounded">
          {{ $t('checkout.okButton') }}
        </button>
      </div>
    </div>
  </div>
</template>

<script>
import AddressInput from './addressInput.vue';
import { mapGetters } from 'vuex';
import api from '@/common/services/api';

export default {
  name: 'CheckoutPage',
  components: {
    AddressInput
  },
  data() {
    return {
      name: '',
      phone: '',
      address: '',
      comment: '',
      isSubmitting: false,
      isSuccess: false
    };
  },
  computed: {
    ...mapGetters('cart', ['cartItems', 'totalPrice']),
    statusMessage(){
      return this.isSuccess 
          ? this.$t('checkout.successMessage')
          : this.$t('checkout.errorMessage');
    }
  },
  methods: {
    goBack() {
      this.$router.go(-1);
    },
    applyPhoneMask() {
      const phoneRegex = /^\+[1-9]\d{1,14}$/;
      this.phone = this.phone.replace(/[^+\d]/g, '').slice(0, 15);
      if (!phoneRegex.test(this.phone)) {
        this.phone = this.phone.replace(/[^\d+]/g, '');
      }
    },
    async confirmOrder() {
      
      this.isSubmitting = true;
      
      const orderData = {
        name: this.name,
        phone: this.phone,
        address: this.address,
        comment: this.comment,
        items: this.cartItems.map(item => ({id: item.id, amount: item.quantity}))
      };

      try {
        const response = await api.post('/order/place', orderData);
        this.isSuccess = response.status === 200;
        
        if(response.status === 200)
        {
          this.statusMessage = this.$t('checkout.successMessage');
          this.$store.dispatch('cart/clearCart');
        }
        
      } catch (error) {
        this.statusMessage = this.$t('checkout.errorMessage');
      }
    },
    closeModal() {
      this.statusMessage = '';
      this.isSubmitting = false;
      if (this.isSuccess) {
        this.$router.push({name: 'store'});
      }
    }
  }
};
</script>

<style scoped>
/* Добавьте любые стили, необходимые для оформления */
</style>
