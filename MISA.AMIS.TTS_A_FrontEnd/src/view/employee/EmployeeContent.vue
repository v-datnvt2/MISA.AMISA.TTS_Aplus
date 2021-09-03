<template>
  <div class="content-grid" :class="{ contentExpanded: contentExpanded }">
    <div class="content" :class="{ expanded: contentExpanded }">
      <div class="content-header" :class="{ expanded: contentExpanded }">
        <div class="title">Nhân viên</div>

        <div class="head-button-zone">
          <ButtonIcon
            subClass="btn-cancel"
            iconName="icon-document"
            buttonText="Nhân bản"
            @btnClick="$refs.ctable.btnReplicateOnClick()"
            v-if="totalSelected > 0"
          />

          <ButtonIcon
            subClass="danger"
            iconName="icon-toast-danger"
            buttonText="Xóa dữ liệu"
            @btnClick="$refs.ctable.btnDeleteOnClick()"
            v-if="totalSelected > 0"
          />

          <Button
            subClass="add-entity fw-b"
            buttonText="Thêm mới nhân viên"
            @btnClick="showAddForm"
          />
        </div>
      </div>
      <div class="toolbar" :class="{ expanded: contentExpanded }">
        <div class="toolbar-left">
          <!-- <ButtonIcon @btnClick="reloadTable" iconName="icon-24 icon-refresh" /> -->
        </div>
        <div class="toolbar-right">
          <div class="filter">
            <FieldInputIcon
              subClass="searchfield italic"
              iconName="icon-search"
              placeHolder="Tìm theo Mã, Tên hoặc Số điện thoại"
              v-model="searchboxFilter"
              @input="callFilterFunction"
            />
            <ButtonIcon @btnClick="reloadTable" iconName="icon-24 icon-refresh" />
            <ButtonIcon @btnClick="reloadTable" iconName="icon-24 icon-excel" />
          </div>
        </div>
      </div>
      <Table
        ref="ctable"
        entity="Employee"
        myurl="Employees"
        :contentExpanded="contentExpanded"
        :warningResponse="warningResponse"
        :thList="thList"
        :pageSize="pageSize"
        :pageNumber="pageNumber"
        :filters="filters"
        :filterUpdate="filterUpdate"
        @optionOnClick="optionOnClick"
        @checkOnItem="checkOnItem"
        @callReloadTable="reloadTable"
        @onTableLoadDone="onTableLoadDone"
      />
      <PageNavigation
        :contentExpanded="contentExpanded"
        :totalRecord="totalRecord"
        :totalPage="totalPage"
        :pageNumber="pageNumber"
        :entityClass="'nhân viên'"
        @onUpdatePagingInfo="onUpdatePagingInfo"
      />
    </div>
    <AddEmployeeForm
      :isHidden="isHidden"
      myurl="Employees"
      :employeeID="entityID"
      :formMode="formMode"
      :toggleForm="toggleForm"
      :warningResponse="warningResponse"
      :departments="departments"
      :updateDropdown="updateDropdown"
      @callReloadTable="reloadTable"
      @hideAddForm="hideAddForm"
    />
  </div>
</template>

<script>
import axios from "axios";
import { eventBus } from "../../main.js";
import Button from "../../components/base/BaseButton.vue";
import ButtonIcon from "../../components/base/BaseButtonIcon.vue";
import FieldInputIcon from "../../components/base/BaseFieldInputIcon.vue";
import PageNavigation from "../../components/base/BasePageNavigation.vue";
import Table from "../../components/base/BaseTable.vue";
import AddEmployeeForm from "./EmployeeDetail.vue";
export default {
  name: "EmployeePage",
  components: {
    Button,
    ButtonIcon,
    FieldInputIcon,
    Table,
    AddEmployeeForm,
    PageNavigation,
  },
  props: {
    //content props
    contentExpanded: Boolean,
  },
  data() {
    return {
      //filter props
      searchboxFilter: "",
      updateTime: 0,
      //dropDown props:
      departments: [],
      positions: [],
      updateDropdown: true,
      //pageNavigation props
      pageNumber: 1,
      totalPage: 1,
      totalRecord: 1,
      pageSize: 20,
      filters: {},
      filterUpdate: true,
      //table props
      thList: [],
      //form props
      isHidden: true,
      toggleForm: true,
      totalSelected: 0,
      entityID: "",
      formMode: -1,

      //popup props
      popupMessage: {},
      action: "",
      isHiddenWarning: true,
      warningResponse: "",

      //toastmessage props
      isHiddenToastMessage: true,
      toastMessage: {},
    };
  },
  methods: {
    reloadTable() {
      this.$refs.ctable.loadTableData();
    },
    /**
     * Đổi giá trị biến để hiển thị form thông tin, thêm mới
     * gán formmode= 0 ( 0: thêm mới ,  1: cập nhật)
     * Nguyễn Hùng 30/07
     */
    showAddForm() {
      this.isHidden = false;
      this.formMode = 0;
      this.toggleForm = !this.toggleForm;
    },

    /**
     * Đổi giá trị biến để ẩn form thông tin
     * Nguyễn Hùng 30/07
     */
    hideAddForm() {
      this.isHidden = true;
      this.formMode = -1;
    },

    /**
     * Đổi giá trị biến để hiển thị form thông tin, cập nhật
     * gán formmode= 1 ( 0: thêm mới ,  1: cập nhật)
     * Nguyễn Hùng 30/07
     */
    dbClickOnTR(entityID) {
      this.entityID = entityID;
      this.formMode = 1;
      this.isHidden = false;
      this.toggleForm = !this.toggleForm;
    },

    optionOnClick(action, selectedEntityId) {
      let vm = this;
      console.log("prepare", action, selectedEntityId);
      switch (action) {
        case "ReqAdd":
          vm.formMode = 0;
          break;
        case "ReqEdit":
          vm.formMode = 1;
          break;
        case "ReqDuplicate":
          vm.formMode = 2;
          break;
      }
      if (this.formMode >= 1 && this.formMode <= 3) {
        vm.entityID = selectedEntityId;
        vm.isHidden = false;
        vm.toggleForm = !vm.toggleForm;
      }
    },

    /**
     * Thực hiện cập nhật số lượng item được check khi check vào checkbox, table Emit.
     */
    checkOnItem(selectedCount) {
      this.totalSelected = selectedCount;
    },

    /**
     * Nghe sự kiện bấm vào nút chuyển trang
     * Gọi component table tải trang tương ứngo
     **/
    onUpdatePagingInfo(pageNumber, pageSize) {
      this.pageNumber = pageNumber;
      this.pageSize = pageSize;
      this.callFilterFunction();
    },

    /**
     * Nghe sự kiện bấm vào nút chuyển trang
     * Gọi component table tải trang tương ứng
     **/
    callFilterFunction() {
      let filters = {
        searchboxFilter: this.searchboxFilter,
      };
      this.filters = filters;

      let currentTime = Number(new Date());
      let to = null;

      console.log(currentTime - this.updateTime);
      if (currentTime - this.updateTime > 1000) {
        eventBus.$emit("showLoadingScreen");
        this.filterUpdate = !this.filterUpdate;
        this.updateTime = Number(new Date());
        // currentTime=  Number(new Date());
      } else {
        clearTimeout(to);
        to = setTimeout(() => {
          this.filterUpdate = !this.filterUpdate;
          this.updateTime = Number(new Date());
        }, 1000);
      }
    },

    onTableLoadDone(totalRecord, totalPage) {
      this.totalRecord = totalRecord;
      this.totalPage = totalPage;
      this.pageNumber = Math.min(totalPage, this.pageNumber);
    },

    exportFile() {
      let vm = this,
        searchboxFilter = vm.filters.searchboxFilter;
      let isAllPage = false,
        propNames = [
          "EmployeeCode",
          "FullName",
          "Gender",
          "DateOfBirth",
          "PositionName",
          "DepartmentName",
          "BankAccountNumber",
          "BankName",
        ];
      vm.entities = {};

      let filterUrl =
        `https://localhost:5001/api/v1/Employees/Export?` +
        `pageSize=${vm.pageSize}&pageNumber=${vm.pageNumber}&isAllPage=${isAllPage}`;

      if (searchboxFilter) {
        filterUrl += `&searchKey=${searchboxFilter.trim()}`;
      }

      axios
        // add responseType
        .post(filterUrl, propNames, {
          responseType: "blob",
        })
        .then((response) => {
          if (response.status == 200) {
            const url = window.URL.createObjectURL(new Blob([response.data]));
            const a = document.createElement("a");
            a.href = url;
            const filename = `file.xlsx`;
            a.setAttribute("download", filename);
            document.body.appendChild(a);
            a.click();
            a.remove();
          } else {
            let toastMessage = {};
            toastMessage.toastType = "Inform";
            toastMessage.toastText = `Không có dữ liệu phù hợp`;
            console.log(toastMessage);
          }
        })
        .catch((error) => {
          console.log(error);
        });
    },
  },
  created() {
    eventBus.$on("save", (choice) => {
      console.log("popupchoice", choice);
    });
  },
  mounted() {
    this.pageNumber = 1;
    this.totalPage = 1;
    this.totalRecord = 1;
    this.pageSize = 20;

    this.thList = [
      {
        fieldName: "No",
        dataType: "Checkbox",
        fieldText: "",
        thClass: "checkboxdiv",
      },
      {
        fieldName: "EmployeeCode",
        dataType: "",
        fieldText: "MÃ NHÂN VIÊN",
        thClass: "a-left minw-150",
      },
      {
        fieldName: "FullName",
        dataType: "",
        fieldText: "TÊN NHÂN VIÊN",
        thClass: "a-left minw-250",
      },
      {
        fieldName: "GenderName",
        dataType: "",
        fieldText: "GIỚI TÍNH",
        thClass: "a-left minw-100",
      },
      {
        fieldName: "DateOfBirth",
        dataType: "Date",
        fieldText: "NGÀY SINH",
        thClass: "a-center minw-150",
      },
      {
        fieldName: "IdentityNumber",
        dataType: "",
        fieldText: "SỐ CMND",
        thClass: "a-left minw-200",
      },
      {
        fieldName: "PositionName",
        dataType: "",
        fieldText: "CHỨC DANH",
        thClass: "a-left minw-250",
      },
      {
        fieldName: "DepartmentName",
        dataType: "",
        fieldText: "TÊN ĐƠN VỊ",
        thClass: "a-left minw-250",
      },
      {
        fieldName: "BankAccountNumber",
        dataType: "",
        fieldText: "SỐ TÀI KHOẢN",
        thClass: "a-left minw-150",
      },
      {
        fieldName: "BankName",
        dataType: "Number",
        fieldText: "TÊN NGÂN HÀNG",
        thClass: "a-left minw-250",
      },
      {
        fieldName: "BankBranch",
        dataType: "",
        fieldText: "CHI NHÁNH TK NGÂN HÀNG",
        thClass: "a-left minw-250",
      },
      {
        fieldName: "optiondd",
        dataType: "",
        fieldText: "CHỨC NĂNG",
        thClass: "a-left minw-120",
      },
    ];
  },
};
</script>

<style scoped>
@import "../../css/layout/content.css";
.footer .navigation-button .button {
  width: 32px !important;
  min-width: 32px !important;
}
</style>
