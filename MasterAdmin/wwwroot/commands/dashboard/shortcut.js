class AdminShortcut {
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
        this.Controller = SteamSystem.VirtualFolder + "/Dashboard/"
    }

    //Enable(id, isEnable) {
    //    let _this = this;

    //    var listItemID = [];
    //    if (id == 0) {
    //        nav - link - lang
    //        $(".checkItem").each(function () {
    //            if (this.checked) {
    //                listItemID.push(parseInt(_this.value));
    //            }
    //        });
    //    } else {
    //        listItemID.push(id);
    //    }
    //    $.ajax({
    //        type: "POST",
    //        url: _this.Controller+"Enable",
    //        beforeSend: function (xhr) {
    //            xhr.setRequestHeader("XSRF-TOKEN",
    //                $('input:hidden[name="__RequestVerificationToken"]').val());
    //        },
    //        data: {
    //            ids: listItemID,
    //            isEnable: isEnable
    //        },
    //        success: function (data) {
    //            if (!data.response.isError) {
    //                _this._commonAdmin.ToastifyAlert("Thành công!!!", '')
    //                $("#" + _this.ModalDataID).modal("hide")
    //                _this.ResetForm()
    //                $("#" + _this.tbodyID).html(data.listData)
    //            } else {
    //                _this._commonAdmin.ToastifyAlert("Thất bại!!!", 'error')

    //            }
    //            _this.ReRegister();

    //        },
    //        failure: function (response) {
    //        }
    //    });
    //}

    Delete(id) {
        let _this = this;
        _commonAdmin.AlertDelete(function (confirmed) {
            if (confirmed) {              
                $.ajax({
                    type: "POST",
                    url: _this.Controller + "Delete",
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader("XSRF-TOKEN",
                            $('input:hidden[name="__RequestVerificationToken"]').val());
                    },
                    data: {
                        id: id
                    },
                    success: function (data) {
                        if (data.response) {
                            debugger;
                            $("#shortcut_" + id).remove()
                            _this._commonAdmin.ToastifyAlert("Thành công!!!", '')
                            $("#" + _this.ModalDataID).modal("hide")
                            //_this.ResetForm()
                            $("#" + _this.tbodyID).html(data.listData)
                            _this.ReRegister();
                            

                        } else {
                            _this._commonAdmin.ToastifyAlert("Thất bại!!!", 'error')

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
    Edit(id) {
        let _this = this;

        $.ajax({
            type: "GET",
            url: "/Collection/ById",
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            data: {
                id: id
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {

                $("#" + _this.divModalDataID).html(response);
                $("#" + _this.ModalDataID).modal("show");

            },
            failure: function (response) {
            }
        });
    }
    Save(formId) {
        let _this = this;
        if (_this.Validate(formId)) {
            //var formdata = new FormData($("#" + _this.fromUploadID)[0]);
            var formdata = new FormData($("#" + formId)[0]);
          
            



            $.ajax({
                type: "POST",
                url: this.Controller + "SaveShortCut",
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
                        $("#shortcut_list").html(data.listData)
                        _this._commonAdmin.ToastifyAlert("Thành công!!!", '')


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

            return false;if (_this._commonAdmin.CheckObjIsDefine("filePond")) {
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
    EditModal(id) {

        let _this = this;

        $.ajax({
            type: "GET",
            url: _this.Controller + "EditModal",
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            data: {
                id: id
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                $("#divModalAddShortcut").html(response.modal);
                $("#addNewShorcut").modal("show");
            },
            failure: function (response) {
            }
        });


    }

}

