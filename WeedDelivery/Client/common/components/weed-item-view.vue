<script>
import {defineComponent} from "vue";


export default defineComponent({
  name: "weedItemView",
  components: {},
  props: ["item", "grade"],
  data() {
    return {
      itemCount: 0
    }
  },
  methods: {
    onIncrementClick() {

      if (this.itemCount >= 100)
        return;

      this.itemCount += 1;
      this.callValueChanged();
    },
    onDecrementClick() {

      if (this.itemCount === 0)
        return;

      this.itemCount -= 1;
      this.callValueChanged();
    },
    callValueChanged() {
      this.$emit("valueChanged", this.item.id, this.itemCount)
    },
    getStringStrain(strain) {
      let strains = {
        0: "Unknown",
        1: "Indica",
        2: "Sativa",
        3: "Indica hybr.",
        4: "Sativa hybr.",
      }

      return strains[strain];

    }
  },
  computed: {
    getItemPhotoBgStyle() {
      let style = {
        "background-image": `url('${this.item.photoUrl}')`
      }

      return style;
    },
    currentPrice() {
      let discounted = this.item.price - (this.grade * this.item.discountStep);

      let currentPrice = this.item.hasDiscount ? discounted : this.item.price;
      return currentPrice;
    },
  }
})

</script>

<template>


  <div id="shopItem" class="flex flex-col basis-1\2">

    <!-- Name -->
    <div
        class="w-32 h-14 place-content-center justify-center text-center align-middle text-l text-amber-300 bg-black bg-opacity-60 backdrop-blur-m rounded">
      <span class="grow justify-center text-center align-middle lpx-1 mt-1 line-clamp-2 leading-6 [text-shadow:_0_1px_0_rgb(0_0_0_/_40%)]">{{ item.name }}</span>
    </div>
    
    <!-- Image and stats -->
    <div
        class="w-32 h-32 flex flex-col justify-between min-h-36 max-h-36 gap-0 text-amber-300 object-cover shadow-lg shadow-white/20 backdrop-blur-sm bg-white/30 rounded bg-no-repeat bg-cover bg-center"
        :style="getItemPhotoBgStyle">
      <div
          class="flex bg-black bg-opacity-40 text-center justify-center text-lg flex items-start backdrop-blur-sm rounded text-amber-300">
        <span class="lpx-1 mt-1 line-clamp-2 leading-6 [text-shadow:_0_1px_0_rgb(0_0_0_/_40%)]"></span>
      </div>

      <div class="flex flex-col py-[0.1em] text-right text-xs [text-shadow:_0_1px_0_rgb(0_0_0_/_40%)] text-black items-end">
        <span v-if="item.hasPcs" class="grow backdrop-blur-sm bg-white/60 rounded p-[0.1em] px-1">
          {{item.pcs}} pcs
        </span>
        <span class="backdrop-blur-sm bg-white/60 rounded p-[0.1em] px-2">
          THC: {{ item.thc }}{{ item.mark }}
        </span>
      </div>

    </div>

    <!-- Strain - Price -->
    <div class="text-sm text-center [text-shadow:_0_1px_0_rgb(0_0_0_/_40%)] text-white mt-1">
      <div id="itemCounter" class="flex flex-row items-center justify-between basis-1\2">
        <div class="">{{ getStringStrain(item.strainType) }}</div>
        <div class="">{{ currentPrice }}฿</div>
      </div>
    </div>

    <!-- COUNTER -->
    <div class="text-center [text-shadow:_0_1px_0_rgb(0_0_0_/_40%)] text-white mt-1 my-1">
      <div id="itemCounter" class="flex flex-row items-center justify-between">
        <div class="mx-2.5"><img src="@/assets/media/btn_minus.png" class="rounded object-cover"
                                 @click="onDecrementClick"/></div>
        <div class="">{{ itemCount }}</div>
        <div class="mx-2.5"><img src="@/assets/media/btn_plus.png" class="rounded object-cover"
                                 @click="onIncrementClick"/></div>
      </div>
    </div>
  </div>

</template>