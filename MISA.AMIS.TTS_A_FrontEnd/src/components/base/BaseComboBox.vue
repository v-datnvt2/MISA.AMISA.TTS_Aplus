<template>
  <div
    class="combobox-zone"
    :class="[subClass, hasError ? 'error ' : '']"
    v-on-clickaway="cbxOnClickAway"
  >
    <label
      >{{ labelText }}
      <span v-if="isRequired"
        ><span class="fw-400 required"> * </span></span
      ></label
    >
    <div
      :id="id"
      :class="[
        'combobox mt-4',
        subClass,
        closed ? '' : 'cb-active',
        hasError ? 'missing' : '',
      ]"
      ref="currentCombobox"
      v-on:keyup="keyupOnItem"
      v-on:keydown="keydownOnItem"
      style="z-index:2"
    >
      <div class="choice flex" style="z-index:5">
        <input
          type="text"
          :tabindex="tabIndex"
          class="divtext"
          style="z-index:5"
          @focus="openItems()"
          @input="cbxOnInput()"
          @blur="inputOnBlur()"
          v-model="inputValue"
          :disabled="textDisable ? true : false"
        />
        <div class="arrow-zone" @click="toggleItems()">
          <div :class="['icon-32 icon-solid-arrow']"></div>
        </div>
      </div>

      <div
        class="itemwrapper"
        :class="[cbDirection, { mustshow: !closed }]"
        style="z-index:10"
      >
        <div
          ref="item"
          v-for="(item, itemIndex) in items"
          :class="[
            'item',
            { selected: item[itemId] == currentId },
            { highlighted: itemIndex == currentFocus },
          ]"
          :key="itemIndex"
          @click="[clickItem(itemIndex), $refs.currentCombobox.blur()]"
          style="z-index:10"
        >
          {{ item[itemName] }}
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import { mixin as clickaway } from "vue-clickaway";
import ResourceVI from "../../js/ResourceVI";
import Constant from "../../api/config/APIConfig";
import FormatFn from "../../js/FormatFunction";
export default {
  mixins: [clickaway],
  name: "BaseCombobox",
  components: {
  },
  props: {
    labelText: String,
    isRequired: Boolean,
    id: String,
    tabIndex: String,
    subClass: String,
    itemId: String,
    itemName: String,
    cbDirection: String,
    textDisable: Boolean,
    myurl: String,
    defaultName: String,
    originValue: String,
    inputItems: Array,
    updateCombobox: Boolean,
  },
  data() {
    return {
      items: [],
      currentId: "-1",
      closed: true,
      defaultId: "-1",
      currentFocus: -1,
      inputValue: null,
      errorMessage: "",
      hasError: false,
    };
  },
  methods: {
    cbxOnClickAway() {
      this.closed = true;
    },

    cbxOnInput() {
      let vm = this;
      vm.openItems();
      if (vm.inputValue) {
        console.log("val", vm.inputValue);
        for (let i = 0; i < vm.items.length; i++) {
          let itemName = vm.items[i][vm.itemName];
          if (FormatFn.includeIgnoreCase(itemName, vm.inputValue)) {
            vm.currentFocus = i;
            break;
          } else {
            vm.currentFocus = -1;
          }
        }
      } else {
        vm.currentFocus = -1;
        vm.inputValue = null;
        console.log("nullorempy");
      }
    },

    inputOnBlur() {
      let vm = this,
        hasMatchItem = false;

      for (let i = 0; i < vm.items.length; i++) {
        let itemName = vm.items[i][vm.itemName];
        if (FormatFn.includeIgnoreCase(itemName, vm.inputValue)) {
          vm.clickItem(i);
          hasMatchItem = true;
          break;
        }
      }

      if (!hasMatchItem) {
        vm.currentId = vm.defaultId;
        vm.inputValue = vm.defaultName;
        vm.hasError = true;
        vm.$emit("input", null);
      }
    },

    validateInput() {
      let vm = this;
      if (vm.isRequired) {
        if (vm.currentId == "-1" || !vm.currentId) {
          vm.hasError = true;
          return false;
        } else {
          vm.hasError = false;
        }
      }
      return true;
    },
    resetFieldInput() {
      (this.currentId = null), (this.inputValue = "");
    },

    /**
     * preventDefault các nút trên combobox
     * datnvt  30/07
     * Modifed 08/08
     */
    keydownOnItem(event) {
      let vm = this,
        preventKeys = "ArrowUp ArrowDown Escape Enter Delete",
        tmpVal = event.target.value;
      if (preventKeys.search(event.code) >= 0) {
        event.preventDefault();
      }
      vm.inputValue = tmpVal;

      switch (event.key) {
        //Trường hợp bấm lên xuống chọn item
        case "ArrowDown":
          if (vm.currentFocus < vm.items.length - 1 && !vm.closed) {
            vm.currentFocus = vm.currentFocus + 1;
          }
          break;
        case "ArrowUp":
          if (vm.currentFocus > 0 && !vm.closed) {
            vm.currentFocus = vm.currentFocus - 1;
          }
          break;

        //Trường hợp bấm Enter chọn item đang focus
        case "Enter":
          if (vm.currentFocus >= 0) {
            vm.clickItem(vm.currentFocus);
          }
          break;
        //Trường hợp bấm Tab
        case "Tab":
          vm.closeCombobox();
          break;

        //Trường hợp bấm Esc để đóng combobox
        case "Escape":
          vm.closeCombobox();
          break;
      }

      // console.log(vm.currentFocus);
    },

    /**
     * Sự kiện keyup trên combobox
     * Thực hiện hành động tương ứng
     * datnvt  30/07
     * Modifed 08/08
     */
    keyupOnItem() {
      // console.log("keyup", event.target.value);
      // if (event.code != "Space" && event.code != "Tab") {
      //   event.preventDefault();
      // }
      // alert("up"+ event.target);
      // let vm = this;
      // switch (event.key) {
      //   //Trường hợp bấm lên xuống chọn item
      //   case "ArrowDown":
      //     if (vm.currentFocus < vm.items.length - 1 && !vm.closed) {
      //       vm.currentFocus = vm.currentFocus + 1;
      //     }
      //     break;
      //   case "ArrowUp":
      //     if (vm.currentFocus > 0 && !vm.closed) {
      //       vm.currentFocus = vm.currentFocus - 1;
      //     }
      //     break;
      //   //Trường hợp bấm Enter chọn item đang focus
      //   case "Enter":
      //     if (vm.currentFocus >= 0) {
      //       vm.clickItem(vm.currentFocus);
      //     }
      //     break;
      //   //Trường hợp bấm Esc để đóng combobox
      //   case "Escape":
      //     vm.closeCombobox();
      //     break;
      // }
    },

    /**
     * Kiểm tra class của combobox
     * Gọi API lấy dữ liệu combobox
     * Hiển thị dữ liệu
     * Nguyễn Hùng 30/07
     */
    loadComboboxData() {
      let vm = this;
      vm.items = [];

      if (vm.myurl) {
        axios
          .get(`${Constant.LocalUrl}/${vm.myurl}`)
          .then((response) => {
            vm.items = response.data;
          })
          .catch((error) => {
            console.log(error);
          });
      } else {
        if (vm.inputItems.length > 0) {
          vm.items = vm.inputItems;
        }
        if (vm.itemName == "GenderName") {
          vm.items = ResourceVI.Enums.Gender;
        }
      }
    },

    /**
     * Sự kiện bấm vào text / arrow của combobox,
     * Hiển thị danh sách lựa chọn
     * Nguyễn Hùng 30/07
     */
    toggleItems() {
      this.closed = !this.closed;
    },

    /**
     * Sự kiện bấm vào text / arrow của combobox,
     * Hiển thị danh sách lựa chọn
     * Nguyễn Hùng 30/07
     */
    openItems() {
      this.closed = false;
    },

    /**
     * Đóng combobox, hủy focus ở các item trong combobox
     * Nguyễn Hùng 30/07
     * Modified 08/08
     */
    closeCombobox() {
      this.closed = true;
      this.currentFocus = -1;
    },

    /**
     * Đóng combobox, hủy focus ở các item trong combobox
     * Nguyễn Hùng 30/07
     * Modified 08/08
     */
    openCombobox() {
      this.closed = false;
      console.log("closeCombobox", this.closed);
    },

    /**
     * Sự kiện bấm vào 1 lựa chọn
     * Emit sự kiện chứa giá trị và id của lựa chọn vừa bấm cho components cha
     * Nguyễn Hùng 30/07
     */
    clickItem(itemIndex) {
      let vm = this,
        item = vm.items[itemIndex];

      vm.currentId = item[vm.itemId];
      vm.inputValue = item[vm.itemName];
      vm.closed = true;
      vm.currentFocus = -1;
      vm.hasError = false;

      let digitValue = Number(vm.currentId);
      if (!isNaN(digitValue)) {
        vm.$emit("input", digitValue);
      } else {
        vm.$emit("input", vm.currentId);
      }
    },

    /**
     * Khởi tạo các giá trị cho combobox
     * Nếu là thêm mới thì làm trống các ô
     * Nếu là xem thông tin thì hiển thị thông tin hiện tại
     */
    initChoice() {
      let vm = this;
      if ((vm.originValue + "").length > 0) {
        vm.items.forEach((item) => {
          if (vm.originValue == item[vm.itemId]) {
            vm.inputValue = item[vm.itemName];
            vm.currentId = item[vm.itemId];
          }
        });
      } else {
        vm.currentId = "-1";
        vm.inputValue = "  ";
      }
    },
  },
  mounted() {
    this.loadComboboxData();
    if (this.defaultName) {
      this.inputValue = this.defaultName;
    }

    // this.initChoice();
  },
  watch: {
    originValue: function() {
      this.initChoice();
    },
    updateCombobox: function() {
      this.loadComboboxData();
      this.initChoice();
    },
  },
};
</script>
<style scoped>
@import "../../css/base/combobox.css";
@import "../../css/common/format.css";
</style>
