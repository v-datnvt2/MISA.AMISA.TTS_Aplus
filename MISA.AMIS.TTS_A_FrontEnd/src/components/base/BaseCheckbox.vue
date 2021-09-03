<template>
  <div class="checkbox">
    <div
      class="checkbox-outline bd-rd-2"
      :class="[checked ? 'checked rot-0' : 'rot-m90']"
    >
      <div
        class="checkbox-box bd-rd-2"
        :class="{ 'icon-check': checked }"
        @click="checkboxOnClick"
      ></div>
    </div>
    <div class="checkbox-text" v-if="checkboxText">{{ checkboxText }}</div>
  </div>
</template>

<script>
export default {
  name: "Checkbox",
  props: {
    checkboxId: Number,
    isChecked: Boolean,
    checkboxText: String,
  },
  data() {
    return {
      checked: false,
    };
  },
  methods: {
    checkboxOnClick() {
      this.checked = !this.checked;
      console.log("checkon bõx", this.checkboxId);
      this.$emit("checkABox", this.checkboxId);
    },
  },
  watch:{
    isChecked:function(){
      this.checked= this.isChecked
    }
  }
};
</script>

<style scoped>
@import "../../css/base/checkbox.css";
</style>
