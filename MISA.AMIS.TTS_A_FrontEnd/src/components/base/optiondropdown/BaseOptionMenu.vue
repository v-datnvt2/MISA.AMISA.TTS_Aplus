<template>
  <div
    class="option-wrapper bd-rd-2"
    :class="{ 'd-none': !isShow }"
    :style="optionStyle"
  >
    <div
      v-for="(option, index) in options"
      :key="index"
      class="option-item"
      :class="{ 'd-none': index == 0 }"
      @click="optionItemOnClick(option.optionId)"
    >
      {{ option.optionText }}
    </div>
  </div>
</template>

<script>
import ResourceVI from "../../../js/ResourceVI";
export default {
  name: "OptionItem",
  props: {
    selectedEntityId: String,
    optionEntity: String,
    mousePos: Number,
  },
  data() {
    return {
      options: [],
      isShow: false,
      optionStyle: "",
    };
  },
  methods: {
    optionItemOnClick(optionId) {
      this.$emit(
        "optionOnClick",
        this.options[optionId].optionAction,
        this.selectedEntityId
      );
      this.isShow = false;
    },
  },
  mounted() {
    this.options = ResourceVI.optionTexts[this.optionEntity];
  },
  watch: {
    itemId: function() {},
    mousePos: function() {
      if (this.mousePos > 0) {
        this.isShow = true;
        this.optionStyle = `top: ${this.mousePos - 15}px`;
      }else{
        this.isShow= false;
      }
    },
  },
};
</script>

<style scoped>
@import "../../../css/base/optiondropdown.css";
</style>
