export default class ResourceVI {
    static Gender = {
        Male: "Nam",
        Female: "Nữ",
        Other: "Khác"
    }





    static pageSizeText = "bản ghi trên trang";



    //Các tùy chọn chức năng với đối tượng trong bảng.
    static optionTexts = {
        employee: [
            { optionId: 0, optionText: "Sửa", optionAction: "ReqEdit" },
            { optionId: 1, optionText: "Xóa", optionAction: "ReqDelete" },
            { optionId: 2, optionText: "Nhân bản", optionAction: "ReqDuplicate" },
            { optionId: 3, optionText: "Ngừng sử dụng", optionAction: "ReqStopUsing" },
        ],
        cusomter: [
            { optionId: 0, optionText: "Xóa" },
            { optionId: 1, optionText: "Nhân bản" },
        ],
    }

    //Kết quả thực hiện
    static UserMsg = {
        AddSuccess: "Thao tác thêm mới thành công",
        AddFailed: "Thao tác thêm mới thất bại",
        UpdateSuccess: "Thao tác cập nhật thành công",
        UpdateFailed: "Thao tác cập nhật thất bại",
        ValidateFailed: "Vui lòng kiểm tra lại các trường",
        SaveError: "Có lỗi xảy ra !",
        GetNewCodeFailed: "Không thể lấy được mã nhân viên mới, lỗi server",
    }

    static UserMsg = {
        AddSuccess: "Thao tác thêm mới thành công",
        AddFailed: "Thao tác thêm mới thất bại",
        UpdateSuccess: "Thao tác cập nhật thành công",
        UpdateFailed: "Thao tác cập nhật thất bại",
        ValidateFailed: "Vui lòng kiểm tra lại các trường",
        SaveError: "Có lỗi xảy ra !",
        GetNewCodeFailed: "Không thể lấy được mã nhân viên mới, lỗi server",
    }

    //Popup message template

    static PopupMessage = {
        SaveThenClose: 'Bạn có muốn lưu dữ liệu nhân viên {0} và đóng form  không?',
        SaveThenAdd: 'Bạn có muốn lưu dữ liệu nhân viên {0} không?',
        UpdateThenClose: 'Bạn có muốn sửa dữ liệu nhân viên {0} và đóng form  không?',
        UpdateThenAdd: 'Bạn có muốn sửa dữ liệu nhân viên {0} không?',
        CloseModifedForm: 'Dữ liệu đã bị thay đổi. Bạn có muốn cất không?',
    }


    static ValidateMessage = {
        NOT_NULL: '{0} không được để trống.',
        INVALID_FORMAT: '{0} sai định dạng.',
        CONTAIN_ALPHABETS_ONLY: '{0} chỉ chứa các chữ cái.',
        DUPLICATED: '{0} đã tồn tại!',
    }

    //static enum
    static Enums = {
        "GenderName": [{
                Gender: "0",
                GenderName: "Nữ",
            },
            {
                Gender: "1",
                GenderName: "Nam",
            },
            {
                Gender: "2",
                GenderName: "Khác",
            },
        ],

        "WorkStatusName": [{
                WorkStatus: "0",
                WorkStatusName: "Chờ phỏng vấn",
            },
            {
                WorkStatus: "1",
                WorkStatusName: "Thử việc",
            },
            {
                WorkStatus: "2",
                WorkStatusName: "Đang làm việc",
            },
            {
                WorkStatus: "3",
                WorkStatusName: "Đã nghỉ việc",
            },
        ]
    }

}