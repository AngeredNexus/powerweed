<template>
<div>
    <nav class="navbar navbar-expand-lg navbar-dark">
        <div class="container">
            <router-link class="navbar-brand" to="/" exact>
                <span class="align-middle">WeedDelivery</span>
            </router-link>
            <navbar :items="navItems" :attributes="store.userAttributes" />
        </div>
    </nav>

    <div id="content" class="container mt-4">
      <router-view></router-view>
    </div>
</div>
</template>

<script lang="ts">
import Vue, {reactive} from 'vue';
import { Component, Prop } from 'vue-property-decorator';
import { bus, store } from './shared';
import { Button } from "@servicestack/vue";



const navItems = [
  { "href":"/",         "label": "Подбор",     "exact":true },
  { "href":"/about",    "label": "Категории" },
  { "href":"/signin",    "label": "Профи" },
  // { "href":"/signin",   "label": "Sign In",  "hide":"auth" },
  // { "href":"/profile",  "label": "Profile",  "show":"auth" },
  // { "href":"/admin",    "label": "Admin",    "show":"role:Admin" }
]

@Component({
  data() {
    return {
    }
  }
})

export class App extends Vue {
  get store() { return store }
  get navItems() { return navItems }
}
export default App
</script>
