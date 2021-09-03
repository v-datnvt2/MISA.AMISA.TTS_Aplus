<template>
  <div class="menu-left left" :class="{ minimized: contentExpanded }">
    <div class="menu-item">
      <div class="menu-item-icon icon-dashboard"></div>
      <div class="menu-item-text" :class="{ minimized: contentExpanded }">
        Tổng quan
      </div>
    </div>
    <div class="menu-item">
      <div class="menu-item-icon icon-cash"></div>
      <div class="menu-item-text" :class="{ minimized: contentExpanded }">
        Báo cáo
      </div>
    </div>
    <div class="menu-item">
      <div class="menu-item-icon icon-buy"></div>
      <div class="menu-item-text" :class="{ minimized: contentExpanded }">
        Mua hàng
      </div>
    </div>
    <router-link to="/dict/employee" exact-path class="menu-item">
      <div class="menu-item-icon icon-employee"></div>
      <div class="menu-item-text" :class="{ minimized: contentExpanded }">
        Danh mục nhân viên
      </div>
    </router-link>
    <div class="menu-item">
      <div class="menu-item-icon icon-setting"></div>
      <div class="menu-item-text" :class="{ minimized: contentExpanded }">
        Thiết lập hệ thống
      </div>
    </div>
    <div
      class="toggle-menu icon-white-prev"
      :class="{ closed: contentExpanded }"
      @click="toggleMenuLeft"
    ></div>
  </div>
</template>

<script>
import "../../css/layout/menu.css";
export default {
  name: "TheMenu",
  data() {
    return {
      contentExpanded: false,
    };
  },
  methods: {
    toggleMenuLeft() {
      this.contentExpanded = !this.contentExpanded;
      this.$emit("toggleMenuLeft", this.contentExpanded);
    },
  },
};
</script>
