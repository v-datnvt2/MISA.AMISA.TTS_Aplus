<template>
  <div :class="{ 'd-none': isHidden }">
    <div class="modal-background"></div>
    <div class="modal-zone" entityId="manv">
      <div class="modal">
        <div class="md-header">
          <div class="flex">
            <div class="title">Thông tin nhân viên</div>
            <Checkbox :checkboxText="'Là khách hàng'" />
            <Checkbox :checkboxText="'Là nhà cung cấp'" />
          </div>

          <div class="flex">
            <div class="btn icon-24 icon-question" />
            <div class="btn icon-24 icon-close" @click="btnCloseOnClick" />
          </div>
        </div>
        <div class="md-content-grid" ref="detail">
          <div class="md-content-row h-50p flex">
            <div class="md-content-col pd-r-26">
              <div class="md-input-row flex">
                <FieldInputLabel
                  labelText="Mã"
                  :isRequired="true"
                  :maxLength="10"
                  :autoFocus="true"
                  :isShowed="isShowed"
                  dataType="EmployeeCode"
                  subClass="w-40p pd-r-6"
                  v-model="employee.EmployeeCode"
                  :originValue="employee.EmployeeCode"
                  :validate="triggerValidate"
                  ref="validateFieldCode"
                />

                <FieldInputLabel
                  labelText="Tên"
                  :isRequired="true"
                  :maxLength="100"
                  dataType="PersonName"
                  subClass="w-60p"
                  v-model="employee.FullName"
                  :originValue="employee.FullName"
                  ref="validateFieldFullName"
                />
              </div>
              <div class="md-input-row flex">
                <ComboBox
                  labelText="Đơn vị"
                  subClass="w-100p"
                  itemId="DepartmentID"
                  itemName="DepartmentName"
                  myurl="Departments"
                  ddDirection="top-32"
                  :isRequired="true"
                  :selectedId="'0'"
                  v-model="employee.DepartmentID"
                  :originValue="employee.DepartmentID"
                  :updateCombobox="updateCombobox"
                  ref="validateFieldDep"
                />
              </div>
              <div class="md-input-row flex">
                <FieldInputLabel
                  labelText="Chức danh"
                  :maxLength="100"
                  v-model="employee.PositionName"
                  :originValue="employee.PositionName"
                  :validate="triggerValidate"
                />
              </div>
            </div>

            <div class="md-content-col">
              <div class="md-input-row flex">
                <FieldInputLabel
                  labelText="Ngày sinh"
                  :inputType="'date'"
                  subClass="w-40p pd-r-6"
                  v-model="employee.DateOfBirth"
                  :originValue="employee.DateOfBirth"
                  :validate="triggerValidate"
                />
                <RadioButton
                  labelText="Giới tính"
                  subClass="w-60p pd-l-10"
                  v-model="employee.Gender"
                  :originValue="employee.Gender + ''"
                />
              </div>
              <div class="md-input-row flex">
                <FieldInputLabel
                  labelText="Số CMND"
                  :maxLength="100"
                  subClass="w-60p pd-r-6"
                  v-model="employee.IdentityNumber"
                  :originValue="employee.IdentityNumber"
                  :validate="triggerValidate"
                />
                <FieldInputLabel
                  labelText="Ngày cấp"
                  :maxLength="100"
                  :inputType="'date'"
                  subClass="w-40p"
                  v-model="employee.IdentityDate"
                  :originValue="employee.IdentityDate"
                  :validate="triggerValidate"
                />
              </div>
              <div class="md-input-row flex">
                <FieldInputLabel
                  labelText="Nơi cấp"
                  :maxLength="100"
                  v-model="employee.IdentityPlace"
                  :originValue="employee.IdentityPlace"
                />
              </div>
            </div>
          </div>

          <div class="md-content-row h-50p mt-20 flex-col">
            <div class="md-content-row">
              <div class="md-input-row">
                <FieldInputLabel
                  labelText="Địa chỉ"
                  :maxLength="255"
                  v-model="employee.Address"
                  :originValue="employee.Address"
                />
              </div>
            </div>
            <div class="md-content-row flex">
              <div class="md-content-col flex">
                <div class="md-input-row w-50p pd-r-6">
                  <FieldInputLabel
                    labelText="ĐT di động"
                    :maxLength="255"
                    v-model="employee.MobilePhoneNumber"
                    :originValue="employee.MobilePhoneNumber"
                  />
                </div>
                <div class="md-input-row w-50p pd-r-6">
                  <FieldInputLabel
                    labelText="ĐT cố định"
                    :maxLength="255"
                    v-model="employee.LandlinePhoneNumber"
                    :originValue="employee.LandlinePhoneNumber"
                  />
                </div>
              </div>
              <div class="md-content-col">
                <div class="md-input-row  w-50p">
                  <FieldInputLabel
                    labelText="Email"
                    :maxLength="255"
                    v-model="employee.Email"
                    :originValue="employee.Email"
                    :validate="triggerValidate"
                  />
                </div>
              </div>
            </div>
            <div class="md-content-row flex">
              <div class="md-content-col flex">
                <div class="md-input-row w-50p pd-r-6">
                  <FieldInputLabel
                    labelText="Tài khoản ngân hàng"
                    :maxLength="255"
                    v-model="employee.BankAccountNumber"
                    :originValue="employee.BankAccountNumber"
                  />
                </div>
                <div class="md-input-row w-50p pd-r-6">
                  <FieldInputLabel
                    labelText="Tên ngân hàng"
                    :maxLength="255"
                    v-model="employee.BankName"
                    :originValue="employee.BankName"
                  />
                </div>
              </div>
              <div class="md-content-col">
                <div class="md-input-row  w-50p">
                  <FieldInputLabel
                    labelText="Chi nhánh"
                    :maxLength="255"
                    v-model="employee.BankBranch"
                    :originValue="employee.BankBranch"
                  />
                </div>
              </div>
            </div>
          </div>
        </div>
        <div class="md-footer">
          <div class="footer-top"></div>
          <div class="footer-bottom">
            <Button
              subClass="secondary fw-700 cancel"
              buttonText="Hủy"
              @btnClick="btnCancelOnClick"
            />
            <div class="flex">
              <Button
                subClass="secondary fw-700"
                buttonText="Cất"
                @btnClick="btnSaveOnClick('SaveThenClose')"
              />
              <Button
                subClass="primary fw-700 ml-10  btn-save "
                buttonText="Cất và Thêm"
                @btnClick="btnSaveOnClick('SaveThenAdd')"
              />
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import FormatFn from "../../js/FormatFunction.js";
import Checkbox from "../../components/base/BaseCheckbox.vue";
import RadioButton from "../../components/base/BaseRadioButton.vue";
import FieldInputLabel from "../../components/base/BaseFieldInputLabel.vue";
// import FieldInputCurrency from "../../components/base/BaseFieldInputCurrency.vue";
import ComboBox from "../../components/base/BaseComboBox.vue";
// import ButtonIcon from "../../components/base/BaseButtonIcon.vue";
import Button from "../../components/base/BaseButton.vue";
import { eventBus } from "../../main.js";
import ResourceVI from "../../js/ResourceVI.js";
import Constant from "../../api/config/APIConfig.js";
export default {
  mixins: [FormatFn],
  name: "EmployeeForm",
  components: {
    ComboBox,
    FieldInputLabel,
    // ButtonIcon,
    Button,
    Checkbox,
    RadioButton,
    // FieldInputCurrency,
  },
  data() {
    return {
      originEntity: {},
      employee: {},
      isShowed: true,
      updateCombobox: true,
      triggerValidate: true,
      errorFields: [],
      // EmployeeCode: "",
    };
  },
  props: {
    isHidden: Boolean,
    employeeID: String,
    formMode: Number,
    toggleForm: Boolean,
    warningResponse: String,
    thList: Object,
    myurl: String,
  },
  methods: {
    //#region Sự kiện trên các nút / components

    keydownOnModal(event) {
      if (event.code == "Escape") {
        this.requestCloseForm();
      }
      console.log(event.code);
    },

    /**
     * Hàm thực thi cho sự kiện input emit bởi ô input
     * lấy nội dung + tên thuộc tính của ô input đó, gán vào thuộc tính tương ứng của entity
     * Nguyễn Hùng 30/07
     */
    userInput(text, fieldName) {
      this.employee[fieldName] = text;
      console.log(text, fieldName);
    },

    /**
     * Thực thi cho sự kiện do nút save emit .
     * Kiểm tra trạng thái form thêm mới (0) hay cập nhật (1)
     * Gọi API tương ứng để lưu dữ liệu
     * todo validate
     *  Nguyễn Hùng 30/07
     */
    btnSaveOnClick(formAction) {
      let vm = this,
        currentErrorFields = vm.validateFormData();

      //Nếu form đang ở chế độ sửa thông tin
      if (vm.formMode == 1) {
        formAction = formAction.replace("Save", "Update");
      }

      if (currentErrorFields.length == 0) {
        //Nếu không có lỗi xảy ra
        let popupMessage = {
          warningType: "Notify",
          textBody: FormatFn.formatString(
            ResourceVI.PopupMessage[formAction],
            vm.employee.FullName
          ),
        };
        console.log("step 1 validate OK");
        eventBus.$emit("showPopupMessage", popupMessage, formAction);
      } else {
        //Nếu có lỗi xảy ra
        console.log("step 1 validate ERROP");
        vm.createToastMessage("ValidateFailed", false);
      }
    },

    btnCloseOnClick() {
      console.log("Btn CLose click");
      let isModifiedForm = this.checkModifiedEntity(),
        formAction = "SaveThenClose";

      if (isModifiedForm) {
        let popupMessage = {
          warningType: "Notify",
          textBody: ResourceVI.PopupMessage["CloseModifedForm"],
        };
        console.log("step 0 modified");
        eventBus.$emit("showPopupMessage", popupMessage, formAction);
      } else {
        this.resetEntityData();
        this.$emit("hideAddForm");
      }
    },

    btnCancelOnClick() {
      this.$emit("hideAddForm");
      console.log("Btn cancel click");
    },

    //#endregion sự kiện trên các nút/ components

    //#region các hàm thực hiện hành động tương ứng với sự kiện các nút/components

    checkModifiedEntity() {
      let vm = this;
      for (let key in vm.employee) {
        if (vm.employee[key] != vm.originEntity[key]) return true;
      }
      console.log("unmodified");
      return false;
    },

    validateFormData() {
      let vm = this,
        currentErrorFields = [];

      for (let [key] of Object.entries(vm.$refs)) {
        if (key.includes("validateField") && !vm.$refs[key].validateInput()) {
          currentErrorFields.push(key);
        }
      }
      vm.errorFields = currentErrorFields;
      return currentErrorFields;
    },

    focusOnFirstErrorField() {
      let vm = this;
      vm.errorFields.forEach((refName) => {
        vm.$refs[refName].focusOnField();
      });
    },

    doSaveEntity(formAction) {
      let vm = this,
        isValid = true;
      console.log(vm.employee);
      axios
        .post(`${Constant.LocalUrl}/${vm.myurl}/`, vm.employee)
        .then((response) => {
          vm.processSaveResponse("AddSuccess", isValid, response);
          vm.processAfterSave(formAction);
        })
        .catch((error) => {
          console.log("step 7 get error", formAction);
          isValid = false;
          vm.processSaveResponse("AddFailed", isValid, error);
        });
    },

    doUpdateEntity(formAction) {
      let vm = this,
        isValid = true;
      console.log("step 6 call API", vm.employee);
      axios
        .put(`${Constant.LocalUrl}/${vm.myurl}/${vm.employeeID}`, vm.employee)
        .then((response) => {
          console.log("step 7 get response", vm.employee);
          vm.processSaveResponse("UpdateSuccess", isValid, response);
          vm.processAfterSave(formAction);
        })
        .catch((error) => {
          console.log("step 7 get error", formAction);
          isValid = false;
          vm.processSaveResponse("UpdateFailed", isValid, error);
        });
    },

    getNewCode
    () {
      let vm = this;
      axios
        .get(`${Constant.LocalUrl}/${vm.myurl}/NewCode`)
        .then((response) => {
          // let newEntity = {
          //   EmployeeCode: "",
          //   PositionId: "",
          //   DepartmentID: "",
          //   Gender: "",
          //   WorkStatus: "",
          // };
          // newEntity.EmployeeCode = response.data;
          vm.$set(vm.employee, "EmployeeCode", response.data);
          // vm.employee.EmployeeCode = response.data;
          vm.originEntity = {};
          Object.assign(vm.originEntity, vm.employee);
          console.log(vm.employee.EmployeeCode);
          return response.data;
        })
        .catch((error) => {
          console.log(error);
          vm.createToastMessage("GetNewCodeFailed", false);
        });
    },

    getEntityInfo() {
      let vm = this;
      axios
        .get(`${Constant.LocalUrl}/${vm.myurl}/${vm.employeeID}`)
        .then((response) => {
          let foundEmployee = response.data;
          console.log(foundEmployee);
          //Thực hiện định dạng dữ liệu nhận từ API
          for (let key in foundEmployee) {
            if (vm.isaDate(foundEmployee[key])) {
              foundEmployee[key] = vm.formatDateYMD(foundEmployee[key]);
            }
          }
          vm.employee = foundEmployee;
          vm.originEntity = {};
          Object.assign(vm.originEntity, foundEmployee);
        })
        .catch((error) => {
          console.log(error);
        });
    },

    /**
     * Khởi tạo nội dung message
     * Emit sự kiện để gọi hàm hiển thị popup khi bấm nút thoát
     */
    requestCloseForm() {
      let action = "CloseForm",
        message = {
          warningType: "Warning",
          textName: "",
          textValue: "",
          textBody: "Dữ liệu đã được thay đổi, bạn có muốn cất ?",
          btnConfirmText: "Có",
          btnDeclineText: "Không",
          btnCancelText: "Hủy",
        };
      eventBus.$emit("showPopupMessage", message, action);
    },

    resetEntityData() {
      let vm = this,
        newEntity = {};
      vm.employee = newEntity;

      //Xóa dữ liệu trên các ô input
      for (let key of Object.entries(vm.$refs)) {
        if (key.includes("validateField")) {
          console.log(key);
          vm.$refs[key].validateInput();
          vm.$refs[key].resetFieldInput();
        }
      }
    },

    processAfterSave(formAction) {
      this.resetEntityData();
      if (formAction.includes("Close")) {
        this.$emit("hideAddForm");
      }else{
        this.getNewCode
        ();
      }
    },
    //#endregion

    //#region các hàm tạo toast message  , popup

    processSaveResponse(actionResult, isValid, response) {
      let vm = this,
        responseData = {};
      console.log("proccesResponse: ", actionResult, isValid, response);
      if (isValid) {
        vm.createToastMessage(actionResult, isValid, responseData);
      } else {
        responseData = response.response.data;
        vm.createPopupMessage(actionResult, isValid, responseData);
      }
    },

    createPopupMessage(actionResult, isValid, responseData) {
      let warningType = "",
        textBody = "";
      console.log("Genpopup", actionResult, responseData);

      if (isValid) {
        warningType = "Notify";
        textBody = responseData.userMsg
          ? responseData.userMsg
          : ResourceVI.actionResults.Success[actionResult].userMsg;
      } else {
        warningType = "Notify";
        textBody = responseData.userMsg
          ? responseData.userMsg
          : ResourceVI.actionResults.Success[actionResult].userMsg;
      }

      let popupMessage = { warningType, textBody };
      eventBus.$emit("showPopupMessage", popupMessage, warningType);
    },

    createToastMessage(actionResult, isValid, responseData) {
      let toastMessage = {};
      console.log(actionResult, responseData);
      if (responseData) {
        toastMessage.toastText = responseData.userMsg
          ? responseData.userMsg
          : ResourceVI.UserMsg[actionResult];
      } else {
        toastMessage.toastText = ResourceVI.UserMsg[actionResult];
      }

      toastMessage.toastType = isValid ? "notify" : "danger";
      eventBus.$emit("showToastMessage", toastMessage);
    },

    //#endregion

    processAddFormResponse(action, choice) {
      let vm = this;
      if (choice == "confirm") {
        console.log("step 4confirmsave ");
        if (vm.formMode == 0) {
          vm.doSaveEntity(action);
        } else {
          vm.doUpdateEntity(action);
        }
      } else if (choice == "decline") {
        vm.resetEntityData();
        vm.$emit("hideAddForm");
      }
    },

    // processCloseFormMessage(choice) {
    //   let vm = this;
    //   console.log("Close", choice);
    //   switch (choice) {
    //     case "confirm":
    //       vm.confirmSaveOnClick();
    //       break;
    //     case "decline":
    //       vm.resetEntityData();
    //       vm.$emit("hideAddForm");
    //       break;
    //   }
    // },

    //#region format & validate function

    isaDate(dateStr) {
      dateStr = dateStr + "";
      let dateArr = dateStr.split("T");
      if (
        (dateStr.search("-") > 0 || dateStr.search("/") > 0) &&
        (dateStr.length == 19 || dateStr.length == 10) &&
        dateArr.length <= 2
      ) {
        return true;
      }

      return false;
    },

    /**
     * @param {} thisdate Xâu javascript date
     * @returns Xâu ở dạng yyyy-MM-dd
     *  Nguyễn Hùng 18/07
     * Modified:  Nguyễn Hùng 05/08 kiểm tra định dạng trước khi chuyển đổi
     */
    formatDateYMD(thisdate) {
      thisdate = thisdate + "";
      if (thisdate.length == 19 || thisdate.length == 10) {
        let dateArr = thisdate.split("T");
        if (dateArr[0].length == 10) {
          let date = new Date(dateArr[0]),
            YMDDDate =
              date.getFullYear() +
              "-" +
              ("0" + (date.getMonth() + 1)).slice(-2) +
              "-" +
              ("0" + date.getDate()).slice(-2);
          return YMDDDate;
        }
      }
      return thisdate;
    },

    //#endregion
  },
  mounted() {
    // window.addEventListener(
    //   "keydown",
    //   function(e) {
    //     console.log( "window", e.code);
    //   }.bind(this)
    // );
  },

  created() {
    let vm = this;
    eventBus.$on("AddFormResponse", (action, choice) => {
      vm.processAddFormResponse(action, choice);
    });
  },
  watch: {
    toggleForm: function() {
      let vm = this;

      //Autofocus vào ô có ref= "autofocus"
      vm.isShowed = !vm.isShowed;
      vm.updateCombobox = !vm.updateCombobox;

      //Gọi API lấy dữ liệu đối tượng có Id tương ứng
      if (vm.formMode == 1) {
        //Mode= 1: Chỉnh sửa thông tin
        vm.getEntityInfo();
      } else {
        //Mode= 0: THêm mới
        this.getNewCode
        ();
      }
    },
  },
};
</script>
<style scoped>
@import "../../css/base/modal.css";
</style>
