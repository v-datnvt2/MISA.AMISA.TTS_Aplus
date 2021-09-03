<template>
  <div>
    <div class="menu-left left" :class="{ minimized: contentExpanded }">
      <div class="menu-left-header">
        <div class="icon-24 menu-toggle icon-toggle"></div>
        <img class="logo icon-logo" src="https://actappg1.misacdn.net/img/Logo_Module_TiengViet_White.66947422.svg">
      </div>
      <MenuItem
        icon="icon-dashboard"
        :contentExpanded="contentExpanded"
        menuItemText="Tổng quan"
      />
      <MenuItem
        icon="icon-cash"
        :contentExpanded="contentExpanded"
        menuItemText="Tiền mặt"
      />
      <MenuItem
        icon="icon-deposit"
        :contentExpanded="contentExpanded"
        menuItemText="Tiền gửi"
      />
      <MenuItem
        icon="icon-buy"
        :contentExpanded="contentExpanded"
        menuItemText="Mua hàng"
      />
      <MenuItem
        icon="icon-sell"
        :contentExpanded="contentExpanded"
        menuItemText="Bán hàng"
      />
      <MenuItem
        icon="icon-invoice"
        :contentExpanded="contentExpanded"
        menuItemText="Quản lý hóa đơn"
      />
      <MenuItem
        icon="icon-warehouse"
        :contentExpanded="contentExpanded"
        menuItemText="Kho"
      />
      <MenuItem
        icon="icon-tool"
        :contentExpanded="contentExpanded"
        menuItemText="Công cụ dụng cụ"
      />
      <MenuItem
        icon="icon-assets"
        :contentExpanded="contentExpanded"
        menuItemText="Tài sản cố định"
      />
      <MenuItem
        icon="icon-tax"
        :contentExpanded="contentExpanded"
        menuItemText="Thuế"
      />
      <MenuItem
        icon="icon-price"
        :contentExpanded="contentExpanded"
        menuItemText="Giá thành"
      />
      <MenuItem
        icon="icon-sumary"
        :contentExpanded="contentExpanded"
        menuItemText="Tổng hợp"
      />
      <MenuItem
        icon="icon-budget"
        :contentExpanded="contentExpanded"
        menuItemText="Ngân sách"
      />
      <MenuItem
        icon="icon-report"
        :contentExpanded="contentExpanded"
        menuItemText="Báo cáo"
      />
      <MenuItem
        icon="icon-analysis"
        :contentExpanded="contentExpanded"
        menuItemText="Phân tích tài chính"
      />
    </div>
  </div>
</template>

<script>
import "../../../css/layout/menu.css";
import MenuItem from "./MenuItem.vue";
export default {
  name: "TheMenu",
  components: {
    MenuItem,
  },
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
