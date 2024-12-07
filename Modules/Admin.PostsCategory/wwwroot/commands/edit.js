class AdminPostsCategory_Edit {
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
        this.RouteIndex = model.RouteIndex
        this.RouteEdit = model.RouteEdit;
        this.SearchFormID = model.SearchFormID
        this.SearchFormID = model.SearchFormID
        this._commonAdmin = new CommonAdmin()
        this.Controller = SteamSystem.VirtualFolder + "/PostsCategory/"
    }
    ReRegister() {
        InitDragDropTable()
    }

    Save(formId) {
        let _this = this;
        if (_this.Validate(formId)) {
            //var formdata = new FormData($("#" + _this.fromUploadID)[0]);
            var formdata = new FormData($("#" + formId)[0]);
            if (_this._commonAdmin.CheckObjIsDefine("Thumbnail")) {
                try {
                    formdata.append(`file_Thumbnail`, Thumbnail.file.file)
                    formdata.append(`FileStatus_Thumbnail`, Thumbnail.metadata.fileStatus)
                    formdata.append(`FilePath_Thumbnail`, Thumbnail.metadata.filePath)
                } catch (e) {

                }

            }
            if (_this._commonAdmin.CheckObjIsDefine("Banner")) {
                try {
                    formdata.append(`file_Banner`, Banner.file.file)
                    formdata.append(`FileStatus_Banner`, Banner.metadata.fileStatus)
                    formdata.append(`FilePath_Banner`, Banner.metadata.filePath)
                } catch (e) {

                }

            }
            //for (var i = 0; i < pondFiles.length; i++) {
            //    // append the blob file
            //    formdata.append('photos', pondFiles[i].file);
            //}

            $.ajax({
                type: "POST",
                url: _this.Controller + "Save",
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("XSRF-TOKEN",
                        $('input:hidden[name="__RequestVerificationToken"]').val());

                },

                data: formdata,
                enctype: 'multipart/form-data',
                processData: false,
                contentType: false,
                success: function (data) {
                    console.log(data)
                    if (!data.response.isError) {
                        _this._commonAdmin.ToastifyAlert("Thành công!!!", '')
                        //location.href = _this.RouteEdit + `/` + data.response.data.pid;
                        $("#Pid").val(data.response.data.pid)
                        $("#OgImage").val(data.response.data.filePath + data.response.data.images);

                        //if (type == 0) {
                        //    _this.ResetForm();
                        //}
                        //else if (type == 1) {

                        //}

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
        //let _this = this;

        //var validate = $("#" + formId).parsley().validate();
        //if (!validate) {
        //    _this._commonAdmin.ToastifyAlert("Vui lòng kiểm tra lại thông tin nhập!!!", 'error')

        //    return false;
        //}
        //try {
        //    if (_this._commonAdmin.CheckObjIsDefine("filePond")) {
        //        filePond.forEach((element, index) => {
        //            var pondFiles = filePond[index].getFiles();
        //            if (typeof pondFiles == "undefined" || pondFiles == null || pondFiles.length == 0) {
        //                _this._commonAdmin.ToastifyAlert("Vui lòng đính kèm ảnh!!!", 'error')

        //                return false
        //            }
        //        })

        //    }


        //} catch (e) {
        //    _this._commonAdmin.ToastifyAlert("Có lỗi khi Validate!!!", 'error')

        //    return false
        //}

        return true;
    }
    CheckFunction() {

        console.log("ComponentInThisView:", this._modelView)
    }

}

