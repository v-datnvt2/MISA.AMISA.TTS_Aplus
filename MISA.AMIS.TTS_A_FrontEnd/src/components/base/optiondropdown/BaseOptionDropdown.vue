<template>
  <div class="option-dropdown">
    <div class="option-head flex">
      <div class="default-option fw-600" @click="optionOnClick">
        {{ defaultOptionText }}
      </div>
      <div
        ref="arrowBtn"
        class="option-icon icon-32 icon-solid-arrow"
        @click="arrowOnClick"
      ></div>
    </div>
  </div>
</template>

<script>
import ResourceVI from "../../../js/ResourceVI";
import { mixin as clickaway } from "vue-clickaway";

export default {
  mixins: [clickaway],

  name: "OptionDropdown",
  props: {
    optionItemValue: String,
    optionItemName: String,
  },
  data() {
    return {
      top: 0,
      defaultOptionText: "",
    };
  },
  methods: {
    arrowOnClick() {
      console.log(this.top);
      this.isShow = !this.isShow;
      this.top = this.isShow
        ? this.$refs.arrowBtn.getBoundingClientRect().top
        : 0;
      this.$emit("arrowOnClick", this.top, this.optionItemValue);
    },
    optionOnClick() {
      let action =
        ResourceVI.optionTexts[this.optionItemName][0].optionAction;
      this.$emit("optionOnClick", action, this.optionItemValue);
    },
  },
  mounted() {
    this.options = ResourceVI.optionTexts[this.optionItemName];
    this.defaultOptionText =
      ResourceVI.optionTexts[this.optionItemName][0].optionText;
  },
};
</script>

<style scoped>
@import "../../../css/base/optiondropdown.css";
</style>
