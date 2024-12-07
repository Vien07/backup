class AdminSample_Edit {
    constructor(model) {
        this._validateModel(model);

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
        this.RouteEdit = model.RouteEdit;
        this.SearchFormID = model.SearchFormID;
        this.SearchFormID = model.SearchFormID;
        this.DivSectionID = model.DivSectionID;
        this._commonAdmin = new CommonAdmin();
        this.Controller = SteamSystem.VirtualFolder + "/Sample/";
        console.log("AdminSample_Edit init succeeded.");

    }
    _validateModel(model) {
        if (!model) {
            throw new Error("Model is required.");
        }
        const requiredProperties = [
            'DivModalDataID', 'ModalDataID', 'DivModalConfigID', 'ModalConfigID',
              'FromUploadID', 'NavPagingID',
            'FileUploadID', 'RouteIndex', 'RouteEdit',  'DivSectionID'
        ];

        requiredProperties.forEach(prop => {
            if (!model[prop]) {
                throw new Error(`${prop} is required.`);
            }
        });
        if (typeof model.DivModalDataID !== 'string' || model.DivModalDataID.trim() === '') {
            throw new Error("DivModalDataID must be a non-empty string.");
        }

        if (typeof model.ModalDataID !== 'string' || model.ModalDataID.trim() === '') {
            throw new Error("ModalDataID must be a non-empty string.");
        }
        console.log("AdminSample_Edit Model validation succeeded.");

    }

    ReRegister() {
        InitDragDropTable()
    }

    Save(formId) {
        let _this = this;

        _this._commonAdmin.ShowLoadingInBlock(_this.DivSectionID, true)

        if (_this.Validate(formId)) {
            //var formdata = new FormData($("#" + _this.fromUploadID)[0]);
            var formdata = new FormData($("#" + formId)[0]);
          //save start file pond
            if (_this._commonAdmin.CheckObjIsDefine("Images")) {
                try {
                    formdata.append(`files`, Images.file.file)
                    formdata.append(`FileStatus`, Images.metadata.fileStatus)
                    formdata.append(`FilePath`, Images.metadata.filePath)
                } catch (e) {

                }

            }

            ///end save file pond

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
                success: function (response) {
                    _this._commonAdmin.ShowLoadingInBlock(_this.DivSectionID, false)

                    console.log(response)
                    if (!response.isError) {
                        _this._commonAdmin.ToastifyAlert("Thành công!!!", '')                      
                        location.href = `${_this.RouteEdit}/${response.data}`;

                    } else {
                        _this._commonAdmin.ToastifyAlert("Thất bại!!!", 'error')

                    }


                },
                failure: function (response) {
                    _this._commonAdmin.ShowLoadingInBlock(_this.DivSectionID, false)

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

    ChangeLang(lang) {

    }
    Validate(formId) {
        let _this = this;

        var validate = $("#" + formId).parsley().validate();
        if (!validate) {
            _this._commonAdmin.ToastifyAlert("Vui lòng kiểm tra lại thông tin nhập!!!", 'error')

            return false;
        }
        try {
            //if (_this._commonAdmin.CheckObjIsDefine("filePond")) {
            //    filePond.forEach((element, index) => {
            //        var pondFiles = filePond[index].getFiles();
            //        if (typeof pondFiles == "undefined" || pondFiles == null || pondFiles.length == 0) {
            //            _this._commonAdmin.ToastifyAlert("Vui lòng đính kèm ảnh!!!", 'error')

            //            return false
            //        }
            //    })

            //}


        } catch (e) {
            _this._commonAdmin.ToastifyAlert("Có lỗi khi Validate!!!", 'error')

            return false
        }

        return true;
    }
    CheckFunction() {

        console.log("ComponentInThisView:", this._modelView)
    }
    ValidateConfig(formId) {
        return true;

    }

}

