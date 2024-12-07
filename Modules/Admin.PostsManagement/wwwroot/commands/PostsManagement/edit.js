class AdminPostsManagement_Edit {
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
        this.RouteEdit = model.RouteEdit;
        this.DivSection = model.DivSection;

        this.SearchFormID = model.SearchFormID;
        this.SearchFormID = model.SearchFormID;
        this._commonAdmin = new CommonAdmin();
        this.Controller = SteamSystem.VirtualFolder + "/PostsManagement/"
    }
    ReRegister() {
    }
    GetSubCate(id) {

        let _this = this;

        $.ajax({
            type: "GET",
            url: this.Controller + "GetListSubCate",
            data: {
                parentId: id
            },
            success: function (response) {
                console.log(JSON.parse(response.jsonSubCate))
                treeselect_SubCate.options = JSON.parse(response.jsonSubCate);
                treeselect_SubCate.mount();
                //    $("#div_SubCate").html(response)
            },
            failure: function (response) {
            }
        });


    }
    showLoading() {
        var button = document.getElementById("btnSave"); // Lấy nút Log in bằng id
        var originalText = button.innerHTML; // Lưu lại nội dung ban đầu của nút
        button.innerHTML = '<span class="spinner-border spinner-border-sm" role="status"><span class="sr-only"></span></span>';
    }
    Save(formId, type) {
        let _this = this;
        _this._commonAdmin.ShowLoadingInBlock(_this.DivSection,true)
        if (_this.Validate(formId)) {
            //var formdata = new FormData($("#" + _this.fromUploadID)[0]);
            var formdata = new FormData($("#" + formId)[0]);

            if (_this._commonAdmin.CheckObjIsDefine("Images")) {
                try {
                    formdata.append(`files`, Images.file.file)
                    formdata.append(`FileStatus`, Images.metadata.fileStatus)
                    formdata.append(`FilePath`, Images.metadata.filePath)
                } catch (e) {

                }

            }

            //for (var i = 0; i < pondFiles.length; i++) {
            //    // append the blob file
            //    formdata.append('photos', pondFiles[i].file);
            //}

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
                    console.log(data)
                    _this._commonAdmin.ShowLoadingInBlock(_this.DivSection, false)
                    if (!data.response.isError) {
                        _this._commonAdmin.ToastifyAlert("Thành công!!!", '')
                        $("#Pid").val(data.response.data.pid);
                        $("#OgImage").val(data.response.data.filePath + data.response.data.images);


                        if (type == 0) {
                            debugger;
                            var currentLocation = window.location;
                            var path = (currentLocation.pathname + "/" + data.response.data.pid)
                            setTimeout(location.href = path, 1000);

                        }
                        else {
                            setTimeout(location.reload(), 1000);

                        }

                    } else {
                        _this._commonAdmin.ToastifyAlert("Thất bại!!!", 'error')

                    }


                },
                failure: function (response) {
                    _this._commonAdmin.ToastifyAlert("Thất bại!!!", 'error')
                    _this._commonAdmin.ShowLoadingInBlock(_this.DivSection, false)

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

            return false; if (_this._commonAdmin.CheckObjIsDefine("filePond")) {
                filePond.forEach((element, index) => {
                    var pondFiles = filePond[index].getFiles();
                    if (typeof pondFiles == "undefined" || pondFiles == null || pondFiles.length == 0) {
                        _this._commonAdmin.ToastifyAlert("Vui lòng đính kèm ảnh!!!", 'error')

                        return false
                    }
                })

            }
        }
        try {



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

