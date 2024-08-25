import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import ViteDotNet from 'vite-dotnet'
import vueI18n from '@intlify/unplugin-vue-i18n/vite';
import path from 'path';

export default defineConfig({
  plugins: [
    vue(),
    vueI18n({
      include: path.resolve(__dirname, './src/common/locale/locales/**'),
    }),
    ViteDotNet('src/main.js', 5178),
  ],
  resolve: {
    alias: {
      'vue$': 'vue/dist/vue.esm-bundler.js',
      '@': path.resolve(__dirname, 'src'),
    },
    extensions: ['.js'],
  },
  build: {
    outDir: path.join(__dirname, 'wwwroot/dist'),
    sourcemap: "production",
  },
  define: {
    'process.env.APP_API_HOST': JSON.stringify(process.env.APP_API_HOST),
  },
});
