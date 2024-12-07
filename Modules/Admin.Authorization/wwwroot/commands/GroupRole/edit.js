class AdminGroupRole_Edit {
    constructor(model) {
        this._modelView = model;
        this.divModalDataID = model.DivModalDataID;
        this.ModalDataID = model.ModalDataID;
        this.divModalConfigID = model.DivModalConfigID;
        this.ModalConfigID = model.ModalConfigID;
        this.tableDataID = model.TableDataID;
        this.tbodyID = model.TbodyId;
        this.fromUploadID = model.FromUploadID;
        this.NavPagingID = model.NavPagingID;
        this.FileUploadID = model.FileUploadID;
        this.RouteIndex = model.RouteIndex;
        this.SearchFormID = model.SearchFormID;
        this.SearchFormID = model.SearchFormID;
        this._commonAdmin = new CommonAdmin();
        this.Controller = SteamSystem.VirtualFolder + "/GroupRole/";
    }
    ReRegister() {
        InitDragDropTable()
    }

    SetCheckedAll() {
        var isCheckedAll = true;
        // Lấy tất cả các checkbox
        var checkboxes = document.querySelectorAll('input[type="checkbox"]');
        // Kiểm tra checked của từng checkbox
        for (let index = 0; index < checkboxes.length; index++) {
            if (checkboxes[index].checked == false) {
                isCheckedAll = false;
                break;
            }
        }
        checkboxes.forEach(function (checkbox) {
            checkbox.checked = isCheckedAll == true ? false : true;
        });
    }

    IsCheckedAllCheckbox() {
        var checkboxes = document.querySelectorAll('input[type="checkbox"]');
        if (checkboxes.length == 0) {
            return false;
        }
        for (let i = 0; i < checkboxes.length; i++) {
            if (checkboxes[i].checked == false) {
                return false;
                break;
            }
        }
        return true;
    }

    Save(formId) {
        let _this = this;
        if (_this.Validate(formId)) {
            //var formdata = new FormData($("#" + _this.fromUploadID)[0]);
            var formdata = new FormData($("#" + formId)[0]);

            // Lấy tất cả các ô đánh dấu
            var checkboxes = document.querySelectorAll('input[type="checkbox"]');

            // Tạo một mảng để lưu trữ giá trị của các ô đánh dấu được chọn
            var selectedValues = [];

            // Lặp qua từng ô đánh dấu và kiểm tra xem nó có được chọn hay không
            checkboxes.forEach(function (checkbox) {
                if (checkbox.checked) {
                    formdata.append('pidRoleSelected[]', checkbox.value);
                }
            });

            $.ajax({
                type: "POST",
                url: this.Controller + "Save",
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("XSRF-TOKEN",
                        $('input:hidden[name="__RequestVerificationToken"]').val());

                },

                data: formdata,
                enctype: 'multipart/form-data',
                processData: false,
                contentType: false,
                success: function (data) {
                    if (!data.response.isError) {
                        _this._commonAdmin.ToastifyAlert("Thành công!!!", '')
                        location.href = _this.RouteIndex;

                    } else {
                        _this._commonAdmin.ToastifyAlert("Thất bại!!!", 'error')

                    }
                },
                failure: function (response) {
                    _this._commonAdmin.ToastifyAlert("Thất bại!!!", 'error')

                }
            });
        }
    }
    ResetForm() {
        let _this = this;

        $('#' + this.fromUploadID)[0].reset();

        if (filePond[_this.FileUploadID] != null) {
            filePond[_this.FileUploadID].removeFiles();

        }
    }

    Validate(formId) {
        let _this = this;

        var validate = $("#" + formId).parsley().validate();
        if (!validate) {
            _this._commonAdmin.ToastifyAlert("Vui lòng kiểm tra lại thông tin nhập!!!", 'error')

            return false;
        }
        try {
            if (_this._commonAdmin.CheckObjIsDefine("filePond")) {
                filePond.forEach((element, index) => {
                    var pondFiles = filePond[index].getFiles();
                    if (typeof pondFiles == "undefined" || pondFiles == null || pondFiles.length == 0) {
                        _this._commonAdmin.ToastifyAlert("Vui lòng đính kèm ảnh!!!", 'error')

                        return false
                    }
                })

            }


        } catch (e) {
            _this._commonAdmin.ToastifyAlert("Có lỗi khi Validate!!!", 'error')

            return false
        }

        return true;
    }
    CheckFunction() {

    }
    ValidateConfig(formId) {
        return true;

    }
}

