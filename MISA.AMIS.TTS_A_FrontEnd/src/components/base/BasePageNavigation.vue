<template>
  <div>
    <div class="page-navigation" :class="{ expanded: contentExpanded }">
      <div class="number-total-info">
        <p>
          Hiển thị
          <span class="number-total">
            {{ (currentPageNumber - 1) * pageSize + 1 }}-
            {{
              Math.min(
                (currentPageNumber - 1) * pageSize + pageSize,
                totalRecord
              )
            }}
            /{{ totalRecord }}</span
          >
          {{ entityClass }}
        </p>
      </div>

      <div class="per-page-info">
        <ComboBox
          tabIndex="0"
          subClass="w-100p"
          cbDirection="bottom-32"
          itemId="pageSizeValue"
          itemName="pageSizeText"
          :textDisable="true"
          :originValue="pageSize + ''"
          :inputItems="pageSizeOptions"
          :defaultId="pageSizeOptions[0]['pageSizeValue']"
          :defaultName="pageSizeOptions[0]['pageSizeText']"
          :updateCombobox="updateCombobox"
          v-model="pageSize"
        />
        <div class="navigation-button">
          <Button
            :subClass="
              currentPageNumber <= 1
                ? 'disabledText page-number white-hv'
                : 'page-number white-hv'
            "
            @btnClick="pageButtonOnClick('PrevPage')"
            :buttonText="'Trước'"
            :disabled="currentPageNumber <= 1"
          />
          <Button
            v-for="index in totalPage"
            :key="index"
            subClass="page-number white-hv "
            :class="[
              { selected: currentPageNumber == index },
              { 'd-none': index < leftLimit || index > rightLimit },
            ]"
            :buttonText="index + ''"
            @btnClick="pageButtonOnClick(index)"
          />

          <Button
            :subClass="
              currentPageNumber >= totalPage
                ? 'disabledText page-number white-hv'
                : 'page-number white-hv'
            "
            @btnClick="pageButtonOnClick('NextPage')"
            :buttonText="'Sau'"
            :disabled="currentPageNumber >= totalPage"
          />
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import Button from "./BaseButton.vue";
import ComboBox from "./BaseComboBox.vue";
import ResourceVI from "../../js/ResourceVI.js";
import { eventBus } from "../../main.js";
export default {
  name: "PageNavigation",
  props: {
    contentExpanded: Boolean,
    totalRecord: Number,
    totalPage: Number,
    pageNumber: Number,
    entityClass: String,
  },
  components: {
    Button,
    ComboBox,
  },
  data() {
    return {
      currentPageNumber: 1,
      pageSize: 20,
      updateCombobox: true,
      leftLimit: 0,
      rightLimit: 1,
      numberOfShowButton: 5,
      pageSizeOptions: [
        { pageSizeValue: 10, pageSizeText: "10 " + ResourceVI.pageSizeText },
        { pageSizeValue: 20, pageSizeText: "20 " + ResourceVI.pageSizeText },
        { pageSizeValue: 30, pageSizeText: "30 " + ResourceVI.pageSizeText },
        { pageSizeValue: 50, pageSizeText: "50 " + ResourceVI.pageSizeText },
        { pageSizeValue: 100, pageSizeText: "100 " + ResourceVI.pageSizeText },
      ],
    };
  },
  methods: {
    pageButtonOnClick(currentPageNumber) {
      let vm = this;
      // console.log("currentPageNumber:", currentPageNumber);
      switch (currentPageNumber) {
        case "FirstPage":
          vm.currentPageNumber = 1;
          break;
        case "PrevPage":
          if (vm.currentPageNumber > 1) {
            vm.currentPageNumber -= 1;
          }
          break;
        case "NextPage":
          if (vm.currentPageNumber < vm.totalPage) {
            vm.currentPageNumber += 1;
          }
          break;
        case "LastPage":
          vm.currentPageNumber = vm.totalPage;
          break;
        default:
          vm.currentPageNumber = currentPageNumber;
          break;
      }
      vm.onUpdatePagingInfo();
    },

    updatePageSize(newPageSize) {
      let vm = this;

      vm.pageSize = newPageSize;
      vm.onUpdatePagingInfo();
    },

    //Khi thực hiện thay đổi thông tin trang (trang hiện tại, số bản ghi/ trang)
    onUpdatePagingInfo() {
      let vm = this;

      vm.adjustCenterButtonNumber();
      setTimeout(() => {
        vm.updatePageList();
        vm.$emit("onUpdatePagingInfo", vm.currentPageNumber, vm.pageSize);
      }, 500);
    },

    /**
     * Cập nhật  số lượng các trang khi thay đổi số bản ghi/ trang,  tổng só bản ghi
     */
    updatePageList() {
      let vm = this;
      eventBus.$emit("showLoadingScreen");
      setTimeout(() => {
        eventBus.$emit("hideLoadingScreen");
      }, 3000);
      //Khi số trang thay đổi thì điều chỉnh lại trang nằm ở giữa
      vm.adjustCenterButtonNumber();
    },

    /**
     * Điều chỉnh số lượng button chọn trang hiển thị
     * Giữ cho trang hiện tại ở trung tâm
     **/
    adjustCenterButtonNumber() {
      let vm = this,
        leftCount = Math.ceil((vm.numberOfShowButton - 1) / 2);
      vm.currentPageNumber = Math.min(vm.currentPageNumber, vm.totalPage);
      //Đếm số  lượng button phía bên trái và phải của trang hiện tại để hiển thị
      if (vm.currentPageNumber - leftCount <= 0) {
        vm.leftLimit = 0;
        vm.rightLimit = vm.leftLimit + vm.numberOfShowButton;
      } else if (vm.currentPageNumber + leftCount >= vm.totalPage) {
        vm.rightLimit = vm.totalPage;
        vm.leftLimit = vm.rightLimit - vm.numberOfShowButton + 1;
      } else {
        vm.leftLimit = vm.currentPageNumber - leftCount;
        vm.rightLimit = vm.leftLimit + vm.numberOfShowButton - 1;
      }
    },

    /**
     * Tạo toastMessage thông báo tương ứng với sự kiện bấm vào nút chuyển trang
     * IS_FIRST_PAGE/IS_LAST_PAGE
     */
    getToastMessage(messageStatus) {
      let toastMessage = {};
      toastMessage.toastType = "Inform";

      switch (messageStatus) {
        case "IS_FIRST_PAGE":
          toastMessage.toastText = "Đây là trang đầu";
          return toastMessage;
        case "IS_LAST_PAGE":
          toastMessage.toastText = "Đây là trang đầu";
          return toastMessage;
      }
    },
  },
  watch: {
    totalRecord: function() {
      this.updatePageList();
    },
    totalPage: function() {
      this.updateCombobox = !this.updateCombobox;
      this.updatePageList();
    },
    pageSize: function(newPageSize) {
      this.updatePageSize(newPageSize);
    },
  },
};
</script>
