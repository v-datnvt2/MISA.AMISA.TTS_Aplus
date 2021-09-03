<template>
  <div class="top-cover" v-if="showLoadingScreen">
    <div class="top-cover-background"></div>
    <div class="lds-roller">
      <div></div>
      <div></div>
      <div></div>
      <div></div>
      <div></div>
      <div></div>
      <div></div>
      <div></div>
    </div>
  </div>
</template>
<script>
export default {
  name: "LoadingScreen",
  props: {
    showLoadingScreen: Boolean,
  },
};
</script>
<style scoped>
@import "../../css/base/loading.css";
</style>
