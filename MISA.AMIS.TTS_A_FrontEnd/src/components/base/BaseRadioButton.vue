<template>
  <div class="rbutton flex-col" :class="[subClass]">
    <label>{{ labelText }}</label>
    <div class="radio-item-wrapper flex">
      <div
        class="radio-item flex"
        v-for="(radioItem, radioIndex) in items"
        :key="radioIndex"
      >
        <div
          class="rbutton-box"
          tabindex="0"
          :class="[{ 'icon-radio': radioIndex == currentValue }]"
          @click="checkitem(radioIndex)"
          @keydown="radioItemOnKeydown($event, radioIndex)"
        ></div>
        <div class="ml-10 mr-20">{{ radioItem }}</div>
      </div>
    </div>
  </div>
</template>
<script>
export default {
  name: "RadioButton",
  props: {
    labelText: String,
    subClass: String,
    inputItems: String,
    originValue: String,
  },
  data() {
    return {
      currentValue: -1,
      items: ["Nam", "Nữ"],
    };
  },
  methods: {
    checkitem(radioIndex) {
      if (this.currentValue == radioIndex) {
        this.currentValue = null;
      } else {
        this.currentValue = radioIndex;
      }
      this.$emit("input", this.currentValue);
    },

    radioItemOnKeydown(event, radioIndex) {
      if (event.key == "Enter" || event.key == " ") {
        this.checkitem(radioIndex);
      }
    },
  },
  watch: {
    originValue: function() {
      this.currentValue = this.originValue;
    },
  },
};
</script>

<style scoped>
.rbutton {
  height: 100%;
}

.rbutton label {
  font-family: "notosans-regular";
  font-weight: bold;
}

.radio-item-wrapper {
  height: 32px;
  margin-top: 4px;
  position: relative;
}
.radio-item {
  margin-top: 8px;
}
.rbutton-box {
  height: 18px;
  width: 18px;

  border-radius: 50%;
  box-sizing: border-box;
  border: 1px solid green;
  background-size: 20px 20px;
  background-position: center;
}

.rbutton-box:focus-visible {
  outline: 1px solid #2ca01c;
  padding: 2px;
}
</style>
