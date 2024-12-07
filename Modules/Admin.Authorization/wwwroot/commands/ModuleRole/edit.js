class AdminModuleRole_Edit {
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
        this.ModalPermissionID = model.ModalPermissionID;
        this.DivModalPermissionID = model.DivModalPermissionID;
        this.TbodyPermissionId = model.TbodyPermissionId;
        this._commonAdmin = new CommonAdmin();
        this.Controller = SteamSystem.VirtualFolder +"/ModuleRole/";
    }
    ReRegister() {
        InitDragDropTable()
    }
    
    ResetForm() {
        let _this = this;

        $('#' + this.fromUploadID)[0].reset();

        if (filePond[_this.FileUploadID] != null) {
            filePond[_this.FileUploadID].removeFiles();

        }
    }

    Save(formId) {
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
                        _this._commonAdmin.ToastifyAlert("Thành công!!!", '');
                        var arr = location.href.split("/");

                        var lastValue = arr[arr.length - 1];
                        console.log(lastValue);
                        if (isNaN(lastValue)) {
                            location.href = location.href + "/" + data.response.data.pid;
                        }


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

    SavePermission(formId) {
        let _this = this;
        if (_this.Validate(formId)) {
            //var formdata = new FormData($("#" + _this.fromUploadID)[0]);
            var formdata = new FormData($("#" + formId)[0]);


            if (formdata.has('IdParent')) {

                var value = formdata.get('IdParent');
                if (value == "") {
                    // Lấy đường dẫn URL hiện tại
                    var url = window.location.href;

                    // Tách chuỗi thành mảng dựa trên dấu /
                    var segments = url.split('/');

                    // Lấy giá trị ID từ phần tử cuối cùng của mảng
                    var id = segments[segments.length - 1];
                    formdata.set('IdParent', id);
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
                    if (!data.response.isError) {

                        _this._commonAdmin.ToastifyAlert("Thành công!!!", '')
                        $("#" + _this.ModalPermissionID).modal("hide")
                        $("#" + _this.TbodyPermissionId).html(data.listData)

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
    EditPermission(id) {
        let _this = this;

        $.ajax({
            type: "GET",
            url: this.Controller + "EditPermission",
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            data: {
                idPermission: id
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {

                $("#" + _this.DivModalPermissionID).html(data.response);
                $("#" + _this.ModalPermissionID).modal("show");

            },
            failure: function (response) {
            }
        });
    }
    DeletePermissionAction(id, parrentID) {
        let _this = this;

        var listItemID = [];
        if (id == 0) {
            $(".checkItem").each(function () {
                if (this.checked) {
                    listItemID.push(parseInt(this.value));

                }
            });
        } else {
            listItemID.push(id);
        };
        console.log("DELETECHILD");
        $.ajax({
            type: "POST",
            url: this.Controller + "DeleteChild",
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            data: {
                ids: listItemID,
                parrentID: parrentID
            },
            success: function (data) {
                if (!data.response.isError) {
                    _this._commonAdmin.ToastifyAlert("Thành công!!!", '')
                    //_this.ResetForm()
                    $("#" + _this.TbodyPermissionId).html(data.listData)
                    _this.ReRegister();

                } else {
                    _this._commonAdmin.ToastifyAlert("Thất bại!!!", 'error')

                }
            },
            failure: function (response) {
            }
        });
    }
    DeletePermission(id, parrentID) {
        let _this = this;

        Swal.fire({
            title: 'Bạn có muốn xóa?',
            showDenyButton: true,
            confirmButtonText: 'Yes',
            denyButtonText: 'No',
            customClass: {
                actions: 'my-actions',
                cancelButton: 'order-1 right-gap',
                confirmButton: 'order-2',
                denyButton: 'order-3',
            }
        }).then((result) => {
            if (result.isConfirmed) {
                _this.DeletePermissionAction(id, parrentID)
            } else if (result.isDenied) {
            }
        })
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

        console.log("ComponentInThisView:", this._modelView)
    }
    ValidateConfig(formId) {
        return true;

    }
}

