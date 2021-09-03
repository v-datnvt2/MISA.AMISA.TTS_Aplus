<template>
  <div class="hello" :class="{ 'd-none': isHiddenWarning }">
    <div class="message-background"></div>
    <div class="message-grid bd-rd-3">
      <div class="message-content flex">
        <div class="message-icon icon-36" :class="icon"></div>
        <div class="message-text flex">
          <span class="text-name">{{ message.textName }}</span>
          <span class="text-value">{{ message.textValue }}</span>
          <span class="text-body">{{ message.textBody }}</span>
        </div>
      </div>
      <div class="message-footer flex">
        <div class="mess-footer-left">
          <Button
            v-if="btnCancelText"
            class="secondary fw-600"
            :buttonText="btnCancelText"
            @btnClick="messageBtnOnClick('cancel')"
          />
        </div>
        <div class="mess-footer-right flex">
          <Button
            v-if="btnnDeclineText"
            class="secondary fw-600"
            :buttonText="btnnDeclineText"
            @btnClick="messageBtnOnClick('decline')"
          />
          <Button
            class="primary fw-600 ml-8"
            :buttonText="btnConfirmText"
            @btnClick="messageBtnOnClick('confirm')"
          />
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import { eventBus } from "../../main.js";
import Button from "./BaseButton.vue";
export default {
  name: "PopupMessage",
  components: {
    Button,
  },
  data() {
    return {
      isHiddenWarning: true,
      action: "",
      warningType: "",
      btnnDeclineText: "Không",
      btnConfirmText: "Có",
      btnCancelText: "Hủy",
      icon: "icon-blue-question",
      message: {
        textName: "",
        textValue: "",
        textBody: "Dữ liệu chưa lưu sẽ bị mất, Tiếp tục đóng ?",
      },
    };
  },
  methods: {
    messageBtnOnClick(choice) {
      switch (this.action) {
        case "Notify":
          break;
        default:
          eventBus.$emit("AddFormResponse", this.action, choice);
          break;
      }
      this.isHiddenWarning = true;
    },
    showPopupMessage(message, action) {
      let vm = this;
      switch (action) {
        case "Notify":
          vm.btnConfirmText = "Đồng ý";
          vm.btnCancelText = "";
          vm.btnDeclineText = "";
          break;
        case "Confirm":
          vm.btnConfirmText = "Có";
          vm.btnCancelText = "";
          vm.btnDeclineText = "Không";
          break;
        default:
          vm.btnConfirmText = "Có";
          vm.btnCancelText = "Hủy";
          vm.btnDeclineText = "Không";
          break;
      }
      this.message = message;
      this.action = action;
      this.isHiddenWarning = false;
    },
  },
  created() {
    let vm = this;
    eventBus.$on("showPopupMessage", (message, action) => {
      vm.showPopupMessage(message, action);
    });
  },
  watch: {
    /**
     * watch kiểu của popup message, gán class tương ứng cho loại popup đó
     * các loại Danger/ Warning / Notify
     * NHH 30/07
     */
    // isHiddenWarning: function() {
    //   let vm = this;
    //   if (vm.message) {
    //     vm.warningType = vm.message.warningType.toLowerCase();
    //     vm.btnConfirmText = vm.message.btnConfirmText
    //       ? vm.message.btnConfirmText
    //       : "Có";
    //     vm.btnDeclineText = vm.message.btnConfirmText
    //       ? vm.message.btnConfirmText
    //       : "Không";
    //     vm.btnCancelText = vm.message.btnConfirmText
    //       ? vm.message.btnConfirmText
    //       : "Hủy";
    //   }
    // },
  },
};
</script>

<style scoped>
@import "../../css/base/message-popup.css";
</style>
