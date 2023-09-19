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
  
  <div v-if="isVisible">

    <div class="flex flex-col m-2">
      <h4 class="grow justify-center font-semibold text-amber-900 dark:text-amber-300">
        Please confirm order by passing personal code, which acceptable by messenger
      </h4>
    </div>

    <div class="flex flex-col p-2 h-12 w-12">
      <img src="@/assets/media/msgr_tg.svg" @click="onMessengerClick"/>
    </div>

    <div class="flex flex-col p-2">
      <input id="code" v-model="code" type="text" required/>
    </div>

    <div class="flex flex-row p-2">
      <button @click="loginClick" class="grow ml-1 mr-2 bg-amber-300 border-amber-300 rounded-lg justify-center">ACCEPT</button>
      <button @click="onDeclineClick" class="grow mr-1 ml-2 bg-amber-300 border-amber-300 rounded-lg justify-center">DECLINE</button>
    </div>
  </div>
  
<!--<div v-if="isLoginVisible" -->
<!--     data-modal-target="defaultModal" -->
<!--     data-modal-toggle="defaultModal">-->

<!--  &lt;!&ndash; Main modal &ndash;&gt;-->
<!--  <div id="defaultModal" tabindex="-1" aria-hidden="true" class="grow top-0 left-0 right-0 z-50 hidden w-full p-4 overflow-x-hidden overflow-y-auto md:inset-0 h-[calc(100%-1rem)] max-h-full">-->
<!--    <div class="relative w-full max-w-2xl max-h-full">-->
<!--      &lt;!&ndash; Modal content &ndash;&gt;-->
<!--      <div class="relative bg-white rounded-lg shadow dark:bg-gray-700">-->
<!--        &lt;!&ndash; Modal header &ndash;&gt;-->
<!--        <div class="flex items-start justify-between p-4 border-b rounded-t dark:border-gray-600">-->
<!--          -->
<!--          <button type="button" class="text-gray-400 bg-transparent hover:bg-gray-200 hover:text-gray-900 rounded-lg text-sm w-8 h-8 ml-auto inline-flex justify-center items-center dark:hover:bg-gray-600 dark:hover:text-white" data-modal-hide="defaultModal">-->
<!--            <svg class="w-3 h-3" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 14 14">-->
<!--              <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="m1 1 6 6m0 0 6 6M7 7l6-6M7 7l-6 6"/>-->
<!--            </svg>-->
<!--            <span class="sr-only">Close modal</span>-->
<!--          </button>-->
<!--        </div>-->
<!--        &lt;!&ndash; Modal body &ndash;&gt;-->
<!--        <div class="p-6 space-y-6">-->
<!--         -->
<!--        </div>-->
<!--        &lt;!&ndash; Modal footer &ndash;&gt;-->
<!--        <div class="flex items-center p-6 space-x-2 border-t border-gray-200 rounded-b dark:border-gray-600">-->
<!--          <button data-modal-hide="defaultModal" type="button" class="text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800">I accept</button>-->
<!--          <button data-modal-hide="defaultModal" type="button" class="text-gray-500 bg-white hover:bg-gray-100 focus:ring-4 focus:outline-none focus:ring-blue-300 rounded-lg border border-gray-200 text-sm font-medium px-5 py-2.5 hover:text-gray-900 focus:z-10 dark:bg-gray-700 dark:text-gray-300 dark:border-gray-500 dark:hover:text-white dark:hover:bg-gray-600 dark:focus:ring-gray-600">Decline</button>-->
<!--        </div>-->
<!--      </div>-->
<!--    </div>-->
<!--  </div>-->
<!--</div>-->
  
</template>

<style>

</style>