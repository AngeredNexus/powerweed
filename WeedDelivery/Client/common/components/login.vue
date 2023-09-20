<script>

import {defineComponent} from "vue";
import back_repo from "@/repo/v1/backend-repo";

const repo = back_repo("auth")
export default defineComponent({

  name: "login",
  components: {
  },
  props: ['isLoginVisible'],
  data() {
    return {
      code: "",
      isVisible: this.isLoginVisible
    }
  },
  methods: {
    async loginClick(e)
    {
      
      let req = {
        idCode: this.code
      }
      
      let resp = await repo.post("confirm", req).then(x => x);
      
      console.log(resp)
      
      // ERROR REQ
      if(resp.status !== 200)
      {
        console.log("NOPE");
        return;
      }
      
      let authData = resp.data;
      
      // NOT AUTH
      if(!authData.isCodeConfirmed)
      {
        console.log("NOPE1");
        return;
      }

      this.isVisible = false;
      this.$emit("loginUpdated", authData);
    },
    onMessengerClick(e)
    {
      window.open('https://telegram.me/si_main_bot?start');
    },
    onDeclineClick(e)
    {
      this.isVisible = false;

      let authData = {
        hash: "",
        isCodeConfirmed: false
      }

      this.$emit("loginUpdated", authData);
    }
  }

})

</script>

<template>
  
  <div v-if="isVisible" class="flex flex-col items-center">

    <div class="flex-col m-2">
      <h6 class="grow justify-center font-semibold text-amber-900 dark:text-amber-300">
        Please confirm order by passing personal code, which acceptable by command "/passwd" in messenger
      </h6>
      <h6 class="grow justify-center font-semibold text-amber-400 dark:text-amber-300">
        Пожалуйста, подтвердите заказ, введя персональный код(доступен по команде "/passwd" в мессенджере)
      </h6>
    </div>

    <div class="grow flex-col justify-center grow p-2 h-12 w-12">
      <img src="@/assets/media/msgr_tg.svg" @click="onMessengerClick"/>
    </div>

    <div class="grow flex-col p-2">
      <input id="code" v-model="code" type="text" required/>
    </div>

    <div class="grow flex-row p-2">
      <button @click="loginClick" class="px-6 ml-1 mr-2 bg-amber-300 border-amber-300 rounded-lg justify-center">ACCEPT</button>
      <button @click="onDeclineClick" class="px-6 mr-1 ml-2 bg-amber-300 border-amber-300 rounded-lg justify-center">DECLINE</button>
    </div>
  </div>
  
</template>

<style>

</style>