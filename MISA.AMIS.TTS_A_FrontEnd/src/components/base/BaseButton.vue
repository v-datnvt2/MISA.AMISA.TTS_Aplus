<template>
    <button
      class="button"
      :class="subClass"
      @click="$emit('btnClick')"
      ref="myButton"
    >
      {{ buttonText }}
    </button>
</template>
<script>
export default {
  name: "Button",
  props: {
    buttonText: String,
    subClass: String,
    reFocus: Boolean,
  },
  watch: {
    /**
     * ReFocus thay đổi khi  component cha ẩn hiện, watch Refocus để thực hiện focus button
     * datnvt 08/08
     **/
    reFocus: function () {
      this.$refs.myButton.focus();
      // console.log("focus on",this.buttonText)
    },
  },
};
</script>
