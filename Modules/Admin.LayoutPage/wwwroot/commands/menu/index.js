﻿class AdminMenu_Index {
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
        this.RouteIndex = SteamSystem.VirtualFolder + model.RouteIndex
        this.SearchFormID = model.SearchFormID
        this.SearchFormID = model.SearchFormID
        this._commonAdmin = new CommonAdmin()
        this.Controller = SteamSystem.VirtualFolder + "/Menu/"
    }
    ReRegister() {
        InitDragDropTable()
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
                console.log(response)
                $("#divModalEditData").html(response.modal);
                $("#ModalEditData").modal("show");
            },
            failure: function (response) {
            }
        });


    }
    Enable(id, isEnable) {
        let _this = this;

        var listItemID = [];
        if (id == 0) {
            listItemID = _this._commonAdmin.GetListCheckedInGrid("checkItem")

        } else {
            listItemID.push(id);
        }
        $.ajax({
            type: "POST",
            url: _this.Controller + "Enable",
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            data: {
                ids: listItemID,
                isEnable: isEnable
            },
            success: function (data) {
                if (!data.response.isError) {
                    _this._commonAdmin.ToastifyAlert("Thành công!!!", '')
                    //$("#" + _this.ModalDataID).modal("hide")
                    //_this.ResetForm()
                    //$("#" + _this.tbodyID).html(data.listData)
                    $("#divTableTree").html(data.listData);
                } else {
                    _this._commonAdmin.ToastifyAlert("Thất bại!!!", 'error')

                }
                _this.ReRegister();

            },
            failure: function (response) {
            }
        });
    }
    UpdateParent(id, parentId) {
        let _this = this;

        $.ajax({
            type: "POST",
            url: _this.Controller + "UpdateParent",
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            data: {
                id: id,
                parentId: parentId
            },
            success: function (data) {
                if (!data.response.isError) {
                    _this._commonAdmin.ToastifyAlert("Thành công!!!", '')
   
        
                    $("#divtest").html(data.listData)
                } else {
                    _this._commonAdmin.ToastifyAlert("Thất bại!!!", 'error')

                }
                _this.ReRegister();

            },
            failure: function (response) {
            }
        });
    }
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
                        if (!data.response.isError) {
                            //_this._commonAdmin.ToastifyAlert("Thành công!!!", '')
                            Swal.fire('Xóa thành công!', '', 'success')

                            //$("#" + _this.ModalDataID).modal("hide")
                            //_this.ResetForm()
                           // $("#" + _this.tbodyID).html(data.listData)

                            $("#divTableTree").html(data.listData);

                            _this.ReRegister();

                        } else {
                            Swal.fire('Xóa thất bại!', '', 'error')

                            //_this._commonAdmin.ToastifyAlert("Thất bại!!!", 'error')

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
            url: "?handler=ById",
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
    Search(page) {
        let _this = this;
        var formdata = new FormData($("#" + _this.SearchFormID)[0]);
        formdata.append("PageIndex", page);
        $.ajax({
            type: "POST",
            url: _this.Controller + "Search",
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            data: formdata,
            enctype: 'multipart/form-data',
            processData: false,
            contentType: false,
            success: function (data) {
                $("#" + _this.tbodyID).html(data.list)
                $("#" + _this.NavPagingID).html(data.paging)
                _this.ReRegister();

            },
            failure: function (response) {
                _this._commonAdmin.ToastifyAlert("Thất bại!!!", 'error')

            }
        });
    }

    Move(from, to) {
        let _this = this;

        $.ajax({
            type: "GET",
            url: _this.Controller + "Move",
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            data: {
                fromId: from,
                toId: to,
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (!data.response.isError) {
                    //_this._commonAdmin.ToastifyAlert("Thành công!!!", '')
                    $("#" + _this.tbodyID).html(data.listData)

                } else {
                    _this._commonAdmin.ToastifyAlert("Thất bại!!!", 'error')
                    $("#" + _this.tbodyID).html(data.listData)

                }
                _this.ReRegister();
            },
            failure: function (response) {
                _this._commonAdmin.ToastifyAlert("Thất bại!!!", 'error')

            }
        });
    }
    EnableUpdateOrder(state) {
        if (state == "edit") {
            $("#btnOrder").html(`<i class="fa-solid fa-arrow-down-up-lock fa-beat"></i>`);
            $("#btnOrder").attr("onclick", `_action.EnableUpdateOrder('update')`)
            $("#btnOrder").addClass("btn-order-active")
        }
        if (state == "update") {
            $("#btnOrder").html(`<i class="fa-solid fa-arrow-down-up-across-line "></i>`);
            $("#btnOrder").attr("onclick", `_action.EnableUpdateOrder('edit')`)
            $("#btnOrder").removeClass("btn-order-active")

        }
        let _this = this;

        $.ajax({
            type: "GET",
            url: _this.Controller + "EnableUpdateOrder",
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            data: {
                PageIndex: PageIndex,
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (!data.response.isError) {
                    //_this._commonAdmin.ToastifyAlert("Thành công!!!", '')
                    $("#" + _this.tbodyID).html(data.listData)

                } else {
                    _this._commonAdmin.ToastifyAlert("Thất bại!!!", 'error')
                    $("#" + _this.tbodyID).html(data.listData)

                }
                if (state == "edit") {
                    $(".order-input").each(function () {
                        this.disabled = false;
                        this.className = "form-control-sm order-input";
                    });
                }

                if (state == "update") {
                    _this.ReRegister();
                }

            },
            failure: function (response) {
                _this._commonAdmin.ToastifyAlert("Thất bại!!!", 'error')

            }
        });
    }
    UpdateOrder(id, order) {

        let _this = this;
        $.ajax({
            type: "GET",
            url: _this.Controller + "UpdateOrder",
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            data: {
                id: id,
                order: order
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data.response.isError) {
                    _this._commonAdmin.ToastifyAlert("Thất bại!!!", 'error')

                } else {
                    //location.href = "/Menu/Index";

                }

            },
            failure: function (response) {
                _this._commonAdmin.ToastifyAlert("Thất bại!!!", 'error')

            }
        });
    }

    ChangeLang(lang) {

        //let _this = this;
        //$("#Lang").val(lang)
        //if (_this.Validate()) {
        //    var formdata = new FormData($("#" + _this.fromUploadID)[0]);
        //    var pondFiles = filePond[_this.FileUploadID].getFiles();
        //    formdata.append('image', pondFiles[0].file);

        //} else {
        //    $(".nav-link-lang").each(function () {
        //        this.className = "nav-link nav-link-lang"
        //    });
        //    $("#lang-tab-" + DefaultLang).addClass("active")
        //    _this._commonAdmin.ToastifyAlert("Vui lòng nhập thông tin ngôn ngữ mặc định!!!", 'error')

        //}

        //$.ajax({
        //    type: "POST",
        //    url: "?handler=ChangeLang",
        //    beforeSend: function (xhr) {
        //        xhr.setRequestHeader("XSRF-TOKEN",
        //            $('input:hidden[name="__RequestVerificationToken"]').val());

        //    },

        //    data: formdata,
        //    enctype: 'multipart/form-data',
        //    processData: false,
        //    contentType: false,
        //    success: function (data) {
        //        if (!data.response.isError) {
        //            _this._commonAdmin.ToastifyAlert("Thành công!!!", '')
        //            $("#" + _this.ModalDataID).modal("hide")
        //            _this.ResetForm()
        //            $("#" + _this.tbodyID).html(data.listData)
        //            _this.ReRegister();

        //        } else {
        //            _this._commonAdmin.ToastifyAlert("Thất bại!!!", 'error')

        //        }


        //    },
        //    failure: function (response) {
        //        _this._commonAdmin.ToastifyAlert("Thất bại!!!", 'error')

        //    }
        //});

    }
    CheckFunction() {

        console.log("ComponentInThisView:", this._modelView)
    }
    ValidateConfig(formId) {
        return true;

    }
    SaveConfig(formId, tab) {
        let _this = this;
        if (_this.ValidateConfig(formId)) {
            //var formdata = new FormData($("#" + _this.fromUploadID)[0]);
            var formdata = new FormData($("#" + formId)[0]);
            formdata.append(`tab`, tab)

            $.ajax({
                type: "POST",
                url: _this.Controller + "SaveConfig",
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
                        _this._commonAdmin.ToastifyAlert("Lưu thành công!!!", '')

                    } else {
                        _this._commonAdmin.ToastifyAlert("Lưu thất bại!!!", 'error')

                    }
                },
                failure: function (response) {
                    _this._commonAdmin.ToastifyAlert("Lưu thất bại!!!", 'error')

                }
            });
        }
    }
    SaveModal(formId) {
        let _this = this;
        if (_this.Validate(formId)) {
            //var formdata = new FormData($("#" + _this.fromUploadID)[0]);
            var formdata = new FormData($("#" + formId)[0]);




            //for (var i = 0; i < pondFiles.length; i++) {
            //    // append the blob file
            //    formdata.append('photos', pondFiles[i].file);
            //}

            $.ajax({
                type: "POST",
                url: _this.Controller + "SaveModal",
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
                        $("#ModalEditData").modal("hide");
                        $("#divTableTree").html(data.listData);

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

}

