<template>
  <div class="inputbox">
    <div class="field-input-icon">
      <input
        type="text"
        :class="[subClass]"
        :id="id"
        :placeholder="placeHolder"
        :required="required == 'true' ? true : false"
        @input="onInput($event.target.value)"
        v-model="modelData"
      />
      <div class="input-icon icon-16" :class="[iconName]"></div>
      <!-- <div
        class="clearchoice"
        :class="{ 'd-none': isEmpty }"
        @click="clearInputOnClick"
      ></div> -->
    </div>
  </div>
</template>
<script>
export default {
  name: "FieldInputIcon",
  data() {
    return {
      modelData: "",
      isEmpty: true,
    };
  },
  props: {
    subClass: String,
    iconName: String,
    id: String,
    value: String,
    fieldName: String,
    tabIndex: String,
    originValue: String,
    dataType: String,
    placeHolder: String,
    required: String,
  },
  methods: {
    /**
     * Khi input, emit sự kiện lên component cha cùng với giá trị hiện tại và tên của ô nhập dữ liệu
     * Nguyễn Hùng 30/07
     */
    onInput(value) {
      this.$emit("input", value);
      if (this.modelData.length > 0) this.isEmpty = false;
    },
    clearInputOnClick() {
      console.log("clearing");
      this.modelData = "";
      this.isEmpty = true;
      this.$emit("input", "");
    },
  },
  watch: {
    originValue: function() {
      try {
        this.modelData = this.originValue;
      } catch (error) {
        console.log("invalid data");
      }
    },
  },
};
</script>
