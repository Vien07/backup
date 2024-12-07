class AdminMisaProduct_Edit extends AdminMisaProduct {
    constructor(...args) {
        super(...args);
    }

    Save(formId) {
        debugger
        let _this = this;
        if (_this.Validate(formId)) {
            var formdata = new FormData($("#" + formId)[0]);

            if (_this._commonAdmin.CheckObjIsDefine("Images")) {
                try {
                    formdata.append(`files`, Images.file.file)
                    formdata.append(`FileStatus`, Images.metadata.fileStatus)
                    formdata.append(`FilePath`, Images.metadata.filePath)
                } catch (e) {

                }
            }

            if (_this._commonAdmin.CheckObjIsDefine("BackImages")) {
                try {
                    formdata.append(`BackFiles`, BackImages.file.file)
                    formdata.append(`BackFileStatus`, BackImages.metadata.fileStatus)
                    formdata.append(`BackFilePath`, BackImages.metadata.filePath)
                } catch (e) {

                }
            }

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
                    if (!data.response.isError) {
                        _this._commonAdmin.ToastifyAlert("Thành công!!!", '')
                        $("#Pid").val(data.response.data.pid);
                        $("#OgImage").val(data.response.data.filePath + data.response.data.images);

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

    Delete(id, parentId) {
        let _this = this;
        _commonAdmin.AlertDelete(function (confirmed) {
            if (confirmed) {

                $.ajax({
                    type: "POST",
                    url: _this.Controller + "DeleteProductDetail",
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader("XSRF-TOKEN",
                            $('input:hidden[name="__RequestVerificationToken"]').val());
                    },
                    data: {
                        ids: id,
                        parentId: parentId
                    },
                    success: function (data) {
                        if (!data.response.isError) {
                            Swal.fire('Xóa thành công!', '', 'success')
                            $("#" + _this.tbodyID).html(data.listData)

                        } else {
                            Swal.fire('Xóa thất bại!', '', 'error')
                        }
                    },
                    failure: function (response) {
                    }
                });
            }
            else {
            }
        });
    }

    Enable(id, isEnable) {
        //let _this = this;

        //var listItemID = [];
        //if (id == 0) {
        //    $(".checkItem").each(function () {
        //        if (this.checked) {
        //            listItemID.push(parseInt(_this.value));
        //        }
        //    });
        //} else {
        //    listItemID.push(id);
        //}
        //$.ajax({
        //    type: "POST",
        //    url: _this.Controller + "Enable",
        //    beforeSend: function (xhr) {
        //        xhr.setRequestHeader("XSRF-TOKEN",
        //            $('input:hidden[name="__RequestVerificationToken"]').val());
        //    },
        //    data: {
        //        ids: listItemID,
        //        isEnable: isEnable
        //    },
        //    success: function (data) {
        //        if (!data.response.isError) {
        //            _this._commonAdmin.ToastifyAlert("Thành công!!!", '')
        //            $("#" + _this.ModalDataID).modal("hide")
        //            _this.ResetForm()
        //            $("#" + _this.tbodyID).html(data.listData)
        //        } else {
        //            _this._commonAdmin.ToastifyAlert("Thất bại!!!", 'error')

        //        }
        //        _this.ReRegister();

        //    },
        //    failure: function (response) {
        //    }
        //});
    }

}