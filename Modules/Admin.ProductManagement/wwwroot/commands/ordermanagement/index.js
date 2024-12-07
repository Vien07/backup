﻿class AdminOrderManagement_Index extends AdminOrderManagement {
    constructor(...args) {
        super(...args);
    }
    ReRegister() {
        InitDragDropTable()
    }
    NewModal() {

        let _this = this;

        $.ajax({
            type: "GET",
            url: "?handler=New",
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
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
    Enable(id, isEnable) {
        let _this = this;

        var listItemID = [];
        if (id == 0) {
            $(".checkItem").each(function () {
                if (this.checked) {
                    listItemID.push(parseInt(_this.value));
                }
            });
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
                    $("#" + _this.ModalDataID).modal("hide")
                    _this.ResetForm()
                    $("#" + _this.tbodyID).html(data.listData)
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
                var listItemID = [];
                if (id == 0) {
                    $(".checkItem").each(function () {
                        if (this.checked) {
                            listItemID.push(parseInt(this.value));

                        }
                    });
                } else {
                    listItemID.push(id);
                }

                console.log(listItemID)
                $.ajax({
                    type: "POST",
                    url: _this.Controller + "Delete",
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader("XSRF-TOKEN",
                            $('input:hidden[name="__RequestVerificationToken"]').val());
                    },
                    data: {
                        ids: listItemID
                    },
                    success: function (data) {
                        if (!data.response.isError) {
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
        },"Bạn có muốn hủy hóa đơn?");

    }
    Edit(id) {
        let _this = this;

        $.ajax({
            type: "GET",
            url: "/OrderManagement/ById",
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
    ResetForm() {
        let _this = this;

        $('#' + this.fromUploadID)[0].reset();

        if (filePond[_this.FileUploadID] != null) {
            filePond[_this.FileUploadID].removeFiles();

        }
    }
    Search(page) {
        let _this = this;
        var formdata = new FormData($("#" + _this.SearchFormID)[0]);
        formdata.append("PageIndex", page);
        $.ajax({
            type: "POST",
            url: this.Controller + "Search",
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
            url: this.Controller + "Move",
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
            url: "?handler=EnableUpdateOrder",
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
            url: "?handler=UpdateOrder",
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
                    //location.href = "/OrderManagement/Index";

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
                url: this.Controller + "SaveConfig",
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

    ExcuteSyncMisa(formId) {
        let _this = this;
        let frm = $("#" + formId);
        var formdata = new FormData(frm[0]);
        formdata.append("SyncNewProduct", $(frm.find('#chkSyncNewProduct')).is(":checked"));
        formdata.append("SyncOldProduct", $(frm.find('#chkSyncOldProduct')).is(":checked"));
        $.ajax({
            type: "POST",
            url: this.Controller + "ExcuteSyncMisa",
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
                $(frm.find('#btnExcute')).attr('hidden', 'hidden');
                $(frm.find('#btnExcuting')).removeAttr('hidden');
            },
            data: formdata,
            enctype: 'multipart/form-data',
            processData: false,
            contentType: false,
            success: function (data) {
                $(frm.find('#btnExcute')).removeAttr('hidden');
                $(frm.find('#btnExcuting')).attr('hidden', 'hidden');
                if (!data.response.isError) {
                    _this._commonAdmin.ToastifyAlert("Lưu thành công!!!", '')

                } else {
                    _this._commonAdmin.ToastifyAlert("Lưu thất bại!!!", 'error')
                }
            },
            failure: function (response) {
                $(frm.find('#btnExcute')).removeAttr('hidden');
                $(frm.find('#btnExcuting')).attr('hidden', 'hidden');
                _this._commonAdmin.ToastifyAlert("Lưu thất bại!!!", 'error')
            }
        });
    }
}