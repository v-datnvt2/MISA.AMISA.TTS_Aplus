<template>
  <div class="table-background">
    <div
      id="gridEmployee"
      class="table-wrapper "
      :class="{ expanded: contentExpanded }"
    >
      <table>
        <thead>
          <tr>
            <th
              v-for="(thItem, thIndex) in thList"
              :key="thIndex"
              :class="[
                thItem.thClass,
                { '.bd-bottom-0': true },
                { 'bd-left-dot': thIndex > 0 },
              ]"
            >
              <div
                @click="selectAllTR"
                :class="{ 'icon-checkedd': allSelected }"
                v-if="thItem.thClass == 'checkboxdiv'"
              >
                <Checkbox @cbclick="selectThisTR(entity[itemId])" />
              </div>

              {{ thItem.fieldText }}
            </th>
          </tr>
          <span class="thspan"></span>
        </thead>
        <tbody>
          <tr
            v-for="(entity, entityIndex) in entities"
            :key="entity[itemId]"
            :class="[isSelected.has(entity[itemId]) ? 'checked' : '']"
            @dblclick="dbClickOnTR(entity[itemId])"
          >
            <td
              v-for="(thItem, thIndex) in thList"
              :key="thIndex"
              :class="[thItem.thClass, { 'bd-left-dot': thIndex > 0 }]"
            >
              <!-- <div
                class="trcheckbox"
                v-if="thItem.thClass == 'checkboxdiv'"
              > -->
              <Checkbox
                @cbclick="selectThisTR(entity[itemId])"
                v-if="thItem.thClass == 'checkboxdiv'"
                :isChecked="isSelected.has(entity[itemId])"
              />
              <!-- </div> -->

              <div v-else-if="thItem.fieldName == 'No'">
                {{ (pageNumber - 1) * pageSize + entityIndex + 1 }}
              </div>
              <div v-else-if="thItem.fieldName == 'optiondd'">
                <OptionDropdown
                  :optionItemName="'employee'"
                  :defaultOptionText="'Sửa'"
                  :optionItemValue="entity[itemId]"
                  @arrowOnClick="arrowOnClick"
                  @optionOnClick="optionOnClick"
                />
              </div>
              <div v-else>
                {{ entity[thItem.fieldName] }}
              </div>
            </td>
            <td></td>
          </tr>
        </tbody>
      </table>
    </div>
    <OptionMenu
      :mousePos="mousePos"
      :selectedEntityId="selectedEntityId"
      :optionEntity="'employee'"
      @optionOnClick="optionOnClick"
    />
  </div>
</template>

<script>
import axios from "axios";
import FormatFn from "../../js/FormatFunction";
import { eventBus } from "../../main.js";
import OptionDropdown from "./optiondropdown/BaseOptionDropdown.vue";
import Checkbox from "./BaseCheckbox.vue";
import OptionMenu from "./optiondropdown/BaseOptionMenu.vue";
import Constant from "../../api/config/APIConfig.js";
export default {
  name: "BaseTable",
  props: {
    warningResponse: String,
    contentExpanded: Boolean,
    thList: Array,

    //page navigation props
    pageNumber: Number,
    pageSize: Number,
    filters: Object,
    filterUpdate: Boolean,
    myurl: String,
    entity: String,
  },
  components: {
    OptionDropdown,
    OptionMenu,
    Checkbox,
  },
  data() {
    return {
      entities: [],
      selectedEntityId: "",
      isSelected: new Set(),
      allSelected: false,
      totalRecord: 1,
      mousePos: 0,
      itemId: `${this.entity}ID`,
    };
  },
  methods: {
    arrowOnClick(top, selectedEntityId) {
      this.mousePos = top;
      this.selectedEntityId = selectedEntityId;
    },

    closeOptionMenu() {
      this.mousePos = 0;
    },

    optionOnClick(action, selectedEntityId) {
      this.closeOptionMenu();
      this.$emit("optionOnClick", action, selectedEntityId);
    },
    /**
     * Gọi API lấy tất cả danh sách nhân viên , hiển thị dữ liệu
     * Nguyễn Hùng 30/07
     */
    loadTableData() {
      let vm = this,
        searchboxFilter = vm.filters.searchboxFilter,
        departmentFilter = vm.filters.departmentFilter;

      vm.entities = {};

      let filterUrl = `${Constant.LocalUrl}/${vm.myurl}/Filter?pageSize=${vm.pageSize}&pageNumber=${vm.pageNumber}`;
      if (searchboxFilter) {
        filterUrl += `&searchKey=${searchboxFilter.trim()}`;
      }
      if (departmentFilter && departmentFilter != "-1") {
        filterUrl += `&departmentID=${departmentFilter}`;
      }

      axios
        .get(filterUrl)
        .then((response) => {
          if (response) {
            vm.entities = [];

            if (response.status == 200) {
              let resData = response.data;
              vm.totalRecord = resData.TotalRecord;

              console.table(vm.entity, resData.Entities);
              vm.entities = FormatFn.formatTableData(
                resData.Entities,
                vm.entity
              );

              eventBus.$emit("LoadDataSuccess");

              //Emit thông tin trang hiện tại lên Content, phân trang
              this.$emit(
                "onTableLoadDone",
                resData.TotalRecord,
                resData.TotalPage
              );
              vm.checkTotalSelected();
            } else if (response.status == 204) {
              let toastMessage = {};
              toastMessage.toastType = "Inform";
              toastMessage.toastText = `Không có dữ liệu phù hợp`;
              this.$emit("showToastMessage", toastMessage);
              this.$emit("onTableLoadDone", 0, 1);
            }
          }
          eventBus.$emit("hideLoadingScreen");
        })
        .catch((error) => {
          let toastMessage = {};
          toastMessage.toastType = "Danger";
          toastMessage.toastText = `Cập nhật dữ liệu thất bại`;
          this.$emit("showToastMessage", toastMessage);
          console.log(error);
          eventBus.$emit("hideLoadingScreen");
        });
    },

    /**
     * Sư kiện click vào ô checkbox thứ i
     * Đổi trạng thái của phần tử thứ  i trong mảng đánh dấu true: chọn /false: bỏ chọn
     * Nguyễn Hùng 30/07
     */
    selectThisTR(entityId) {
      let tmpSelectedSet = this.isSelected;
      if (tmpSelectedSet.has(entityId)) {
        tmpSelectedSet.delete(entityId);
      } else {
        tmpSelectedSet.add(entityId);
      }
      this.isSelected = new Set(tmpSelectedSet);
      // this.$set(this.isSelected, entityIndex, !this.isSelected[entityIndex]);

      let totalSelected = this.checkTotalSelected();

      this.$emit("checkOnItem", totalSelected);
    },

    /**
     * Hàm đếm tổng số hàng được check trong bảng
     * Return số hàng được check trong bảng
     */
    checkTotalSelected() {
      let vm = this,
        isAllSelected = true,
        selectedCount = vm.isSelected.size;
      vm.entities.forEach((entity) => {
        if (!vm.isSelected.has(entity[vm.itemId])) {
          isAllSelected = false;
        }
      });

      vm.allSelected = isAllSelected;
      return selectedCount;
    },

    /**
     * Khi click vào ô checkbox trên TH
     * Đổi trạng thái được chọn của tất cả phần tử  trong mảng đánh dấu true: chọn /false: bỏ chọn
     * Nguyễn Hùng 31/07
     */
    selectAllTR() {
      let vm = this,
        tmpSelectedSet = vm.isSelected;
      vm.allSelected = !vm.allSelected;
      if (vm.allSelected) {
        vm.entities.forEach((entity) => {
          tmpSelectedSet.add(entity[vm.itemId]);
        });
      } else {
        vm.entities.forEach((entity) => {
          tmpSelectedSet.delete(entity[vm.itemId]);
        });
      }
      vm.isSelected = tmpSelectedSet;
      vm.checkTotalSelected();
      // vm.$emit("checkOnItem", vm.checkTotalSelected());
    },

    /**
    Tạo nội dung và hiển thị popup xác nhận khi bấm nút nhân bản
    Emit nội dung lên component trên
     */
    btnReplicateOnClick() {
      //nếu hàng này dược chọn
      let message = {},
        totalChecked = this.checkTotalSelected(),
        action = "Replicate";
      message["warningType"] = "Warning";
      message["text-before"] = "Bạn có muốn nhân bản ";
      message["text-target"] = totalChecked + " nhân viên";
      message["text-after"] = "đã chọn không ?";
      message["confirm-button-text"] = "Xác nhận";
      this.$emit("showPopupMessage", message, action);
    },


    /**
    Tạo nội dung và hiển thị popup xác nhận khi bấm nút xóa bản ghi
    Emit nội dung lên component trên
     */
    btnDeleteOnClick() {
      //nếu hàng này dược chọn
      let message = {},
        totalChecked = this.checkTotalSelected(),
        action = "Delete";
      message["warningType"] = "Danger";
      message["text-before"] = "Bạn có muốn xóa dữ liệu của ";
      message["text-target"] = totalChecked + " nhân viên";
      message["text-after"] = "trong hệ thống ?";
      message["confirm-button-text"] = "Xóa";
      this.$emit("showPopupMessage", message, action);
    },

    /**
     * Khi doubleclick vào 1 hàng.
     * Nếu hàng đó được check thì hiện popup hỏi xóa
     * Nếu hàng đó không check  thì gọi hàm hiển thị form detail
     * Nguyễn Hùng 30/07
     */
    dbClickOnTR(itemId) {
      //nếu hàng này không được chọn, emit sự kiện để hiển thị form detail của item
      this.optionOnClick('ReqEdit',itemId)
    },

    /**
     * Duyệt danh sách những TR được chọn
     * Lấy ID của entity và gọi API xóa dữ liệu tương ứng
     * Nguyễn Hùng 30/07
     */
    deleteConfirmed() {
      let vm = this,
        hasError = false,
        pendingItems = [];

      for (let deleteId of vm.isSelected) {
        pendingItems.push(deleteId);
      }
      axios
        .post(`${Constant.LocalUrl}/${vm.myurl}/Multiple/Delete`, pendingItems)
        .then((response) => {
          console.log(response);
          if (!hasError) {
            // Tạo toast message thông báo thành công.
            let toastMessage = {};
            toastMessage.toastType = "Notify";
            toastMessage.toastText = ` Hoàn thành  xóa ${pendingItems.length} bản ghi`;
            this.$emit("showToastMessage", toastMessage);
            this.loadTableData();
          }
        })
        .catch((error) => {
          console.log(error);
          //Tạo ToastMessage hiển thị lỗi
          let toastMessage = {};
          toastMessage.toastType = "Danger";
          toastMessage.toastText = `Quá trình xóa không hoàn tất, dữ liệu không thay đổi`;
          this.$emit("showToastMessage", toastMessage);
          hasError = true;
        });
    },

    /**
     * Duyệt danh sách những TR được chọn
     * Lấy ID của entity và gọi API xóa dữ liệu tương ứng
     * Nguyễn Hùng 30/07
     */
    async replicateConfirmed() {
      let vm = this,
        totalReplicated = 0,
        hasError = false;

      for (let index = 0; index < vm.isSelected.length; index++) {
        let value = vm.isSelected[index];
        if (value) {
          //TODO
          //Gọi API lấy mã đối tượng mới.
          setInterval(async () => {
            let newEntityCode = await vm.getNewEmployeeCode();

            //Gọi API lấy thông tin của đối tượng cần nhân bản
            let replicateId = vm.entities[index][vm.itemId],
              replicateEntity = await vm.getEntityById(replicateId);

            replicateEntity.EmployeeCode = newEntityCode;

            //Gọi API lưu dữ liệu nhân bản
            let repResult = await vm.saveEntity(replicateEntity);
            vm.loadTableData();
            totalReplicated += repResult;
          }, 1000);
        }
      }
      //Nếu không có lỗi xảy ra , tạo message thông báo thành công và hiển thị
      if (!hasError) {
        let toastMessage = {};
        toastMessage.toastType = "Notify";
        toastMessage.toastText = ` Hoàn thành  nhân bản ${totalReplicated} bản ghi`;
        this.$emit("showToastMessage", toastMessage);
        this.$emit("checkOnItem", 0);
      }
    },

    /**
     * Gọi API lấy mã nhân viên mới và trả về mã đó
     */
    getNewEmployeeCode() {
      let vm = this;
      return new Promise((resolve) => {
        axios
          .get(`${Constant.LocalUrl}/${vm.myurl}/NewCode`)
          .then((response) => {
            // console.log(response.data, Number(new Date()));
            resolve(response.data);
          })
          .catch((error) => {
            console.log(error);
            // vm.generateToastMessage(
            //   "Không thể lấy được mã nhân viên mới, lỗi server",
            //   "Danger"
            // );
          });
      });
    },

    /**
     * Gọi API lấy đối tượng theo ID
     * Trả về object chứa đối thông tin đối tượng
     */
    getEntityById(replicateId) {
      let vm = this;
      return new Promise((resolve) => {
        axios
          .get(`${Constant.LocalUrl}/${vm.myurl}/${replicateId}`)
          .then((res) => {
            let newEntity = res.data;
            newEntity["EmployeeCode"] = "";
            // console.log(newEntity, Number(new Date()));
            resolve(newEntity);
          })
          .catch((error) => {
            console.log(error);
            // vm.generateToastMessage(
            //   "Không thể lấy được thông tin nhân viên",
            //   "Danger"
            // );
          });
      });
    },

    /**
     * GỌi API thêm mới bản ghi
     * Param{entity} : object chứa thông tin đối tượng cần lưu
     * trả về kết quả thực hiện : thành công=1, thất bại :0
     */
    saveEntity(entity) {
      let vm = this;
      return new Promise((resolve) => {
        axios
          .post(`${Constant.LocalUrl}/${vm.myurl}/`, entity)
          .then(() => {
            // vm.generateToastMessage("Thêm mới dữ liệu thành công", "Notify");
            resolve(1);
          })
          .catch((error) => {
            console.log(error);
            resolve(0);
            // vm.generateToastMessage("Lỗi khi thêm mới", "Danger");
          });
      });
    },
  },
  watch: {
    /**
     * Kiểm tra thông điệp khi bấm vào popup xóa, nếu đồng ý, thì gọi hàm xóa
     * Nguyễn Hùng 30/07
     */
    warningResponse: function() {
      let response = this.warningResponse.split("_");
      switch (response[0]) {
        case "ConfirmDelete":
          this.deleteConfirmed();
          break;
        case "ConfirmReplicate":
          this.replicateConfirmed();
          break;
      }
    },
    filterUpdate: function() {
      this.loadTableData();
    },
  },
  mounted() {
    this.loadTableData();
  },
};
</script>
