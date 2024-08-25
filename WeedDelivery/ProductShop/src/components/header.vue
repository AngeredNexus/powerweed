<template>
  <header class="bg-gray-800 text-white shadow-md py-4 px-6">
    <div class="container mx-auto flex items-center justify-between">
      <!-- Логотип -->
      <div class="flex items-center space-x-4">
        <img src="/powerweed.logo.png" alt="Logo" class="h-8">
        <LanguageSwitcher />
      </div>
      <!-- Можно добавить другие элементы шапки здесь -->
    </div>
  </header>
</template>

<script>
import LanguageSwitcher from './languageSwitcher.vue';

export default {
  name: 'Header',
  components: {
    LanguageSwitcher,
  },
};
</script>

<style scoped>
header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}
</style>