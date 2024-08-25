<template>
  <div class="relative min-h-screen">
    <!-- Шапка -->
    <header class="fixed top-0 left-0 w-full bg-gray-800 text-white shadow-md z-50">
      <div class="flex justify-between items-center p-4 max-w-screen-xl mx-auto">
        <div class="text-xl font-bold flex items-center">
          <Header/>
        </div>
      </div>
    </header>

    <!-- Контент -->
    <div class="pt-16 flex flex-col items-center p-6 bg-gray-100 rounded-lg shadow-lg">
      <img :src="imageSrc" alt="Image" class="w-48 h-48 object-cover rounded-full mb-4">
      <p class="text-lg font-semibold mb-2">{{ $t('guestView.caption') }}</p>
      <p class="text-lg font-semibold mb-2">{{ $t('guestView.message') }}</p>
      <button
          @click="authorize"
          :disabled="isCheckingAuth"
          class="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600"
      >
        {{ $t('guestView.authorizeButton') }}
      </button>
    </div>
  </div>
</template>

<script>
import api from '../../common/services/api.js';
import router from "../../components/router/router";
import Header from "@/components/header.vue";

export default {
  name: 'guestView',
  components: {Header},
  data() {
    return {
      imageSrc: '/powerweed.logo.png',
      isCheckingAuth: false
    };
  },
  methods: {
    async checkAuth() {
      try {
        const response = await api.get('/athy/check');
        return true;
      } catch (error) {
        return false;
      }
    },
    async authorize() {
      try {
        const response = await api.get('/athy/request');
        const code = response.data;
        await router.push({name: 'loginWait', query: {code}});
      } catch (error) {
        console.error('Ошибка авторизации:', error);
      }
    }
  },
  async mounted() {
    this.isCheckingAuth = true;
    if (await this.checkAuth())
      await router.push({name: 'store'});
    this.isCheckingAuth = false;
  }
};
</script>
