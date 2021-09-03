<template>
  <div class="inputbox" :class="[hasError ? 'error' : '', subClass]">
    <label
      >{{ labelText }}
      <span v-if="isRequired"
        ><span class="fw-400 required"> * </span></span
      ></label
    >
    <input
      :type="inputType"
      class="field-input mt-4"
      :class="[hasError ? 'missing' : '']"
      :id="id"
      tabindex="0"
      :datatype="dataType"
      :placeholder="placeHolder"
      :maxlength="maxLength"
      @input="onInput($event.target.value)"
      @blur="validateInput($event.target.value)"
      v-model="modelData"
      ref="myself"
    />
  </div>
</template>

<script>
import ValidateFn from "../../js/ValidateFn";
import FormatFn from "../../js/FormatFunction";
import EntityModel from "../../js/model/EntityModel";
export default {
  name: "BaseInputLabel",
  components: {
  },
  props: {
    // Common props
    labelText: String,
    inputType: String,
    id: String,
    subClass: String,
    fieldName: String,
    tabIndex: String,
    originValue: String,
    dataType: String,
    placeHolder: String,
    isRequired: Boolean,
    maxLength: Number,

    //formattning props
    autoFocus: Boolean,
    isShowed: Boolean,
  },
  data() {
    return {
      modelData: "",
      isEmpty: true,
      hasError: false,
      errorMessage: "",
    };
  },
  methods: {
    /**
     * Khi input, emit sự kiện lên component cha cùng với giá trị hiện tại và tên của ô nhập dữ liệu
     * Nguyễn Hùng 30/07
     */
    onInput(value) {
      // this.validateInput();
      this.$emit("input", value);
    },
    /**'
     * Khi blur ô input, thực hiện validate định dạng dữ liệu
     * Hiển thị tooltip thông báo lôi nếu có
     */
    validateInput() {
      let vm = this;
      if (vm.isRequired) {
        let validateMessage = ValidateFn.validateInputFormat(
          vm.modelData,
          vm.dataType
        );
        if (validateMessage != "Correct") {
          vm.hasError = true;
          let propDisplayName = EntityModel.Employee[vm.fieldName];
          console.log(propDisplayName, validateMessage);
          vm.errorMessage = FormatFn.formatString(
            validateMessage,
            propDisplayName
          );
        } else {
          vm.hasError = false;
        }
      } else {
        vm.hasError = false;
      }
      // console.log(vm.fieldName, vm.hasError, vm.errorMessage);
      let res = !vm.hasError;
      return res;
    },

    /**
     * Khi bấm vào nút clear, xóa dữ liệu trong ô nhập
     */
    clearInputOnClick() {
      this.modelData = "";
      this.isEmpty = true;
    },

    /**
     * Thực hiện focus vào chính ô input.
     */
    doFocus() {
      if (this.autoFocus) {
        this.$refs.myself.focus();
      }
    },

    resetFieldInput() {
      this.clearInputOnClick();
      this.hasError = false;
    },
  },
  watch: {
    originValue: function() {
      try {
        if (this.originValue != "undefined") {
          this.modelData = this.originValue;
        }
        this.checkInputLength();
        this.hasError = false;
      } catch (error) {
        // console.log("invalid data");
      }
    },
    isShowed: function() {
      this.doFocus();
    },
  },
};
</script>

<style scoped>
@import "../../css/common/format.css";
@import "../../css/common/default.css";
</style>
