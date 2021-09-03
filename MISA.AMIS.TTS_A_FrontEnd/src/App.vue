<template>
  <div id="app">
    <Menu />
    <div class="main-grid">
      <Header />
      <div class="router-slot">
        <router-view></router-view>
      </div>
    </div>
    <LoadingScreen :showLoadingScreen="showLoadingScreen" />
    <PopupMessage />
    <ToastMessage
      :isHiddenToastMessage="isHiddenToastMessage"
      :toastMessage="toastMessage"
      @close="closeToastMessage"
    />
  </div>
</template>

<script>
import { eventBus } from "./main.js";
import LoadingScreen from "./components/base/BaseLoadingScreen.vue";
import ToastMessage from "./components/base/BaseToastMessage.vue";
import PopupMessage from "./components/base/BasePopupMessage.vue";
import Menu from "./components/layout/menu/TheMenu.vue";
import Header from "./components/layout/TheHeader.vue";
// import Content from "./components/layout/TheContent.vue";
export default {
  name: "App",
  components: {
    LoadingScreen,
    Menu,
    Header,
    // Content,
    ToastMessage,
    PopupMessage,
  },
  data() {
    return {
      showLoadingScreen: true,
      isHiddenWarning: true,
      isHiddenToastMessage: true,
      toastMessage: {},
      action: "",
      popupMessage: {},
    };
  },
  methods: {
    closeToastMessage() {
      this.isHiddenToastMessage = true;
    },
  },
  created() {
    let vm = this;

    eventBus.$on("showLoadingScreen", () => {
      vm.showLoadingScreen = true;
    });

    eventBus.$on("hideLoadingScreen", () => {
      vm.showLoadingScreen = false;
    });
  },
};
</script>

<style>
@import "./css/main.css";
</style>
