class AdminFooterPage_Edit {
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
        this.DivTableChildID = model.DivTableChildID;
        this.TableChidID = model.TableChidID;
        this.DivModalChildID = model.DivModalChildID;
        this.ModalChildID = model.ModalChildID;
        this._commonAdmin = new CommonAdmin();
        this.Controller = SteamSystem.VirtualFolder+ "/FooterPage/";

    }
    ReRegister() {
        InitDragDropTable()
    }

    Save(formId) {
        let _this = this;
        if (_this.Validate(formId)) {
            //var formdata = new FormData($("#" + _this.fromUploadID)[0]);
            var formdata = new FormData($("#" + formId)[0]);
         

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
                        $("#FooterPagePid").val(data.response.data.pid)
                        $("#divTableChild").removeAttr("hidden")
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

        console.log("ComponentInThisView:", this._modelView)
    }
    ValidateConfig(formId) {
        return true;

    }
    SaveChild(formId) {
        let _this = this;
        if (_this.Validate(formId)) {
            //var formdata = new FormData($("#" + _this.fromUploadID)[0]);
            var formdata = new FormData($("#" + formId)[0]);
            formdata.append("FooterPagePid", $("#FooterPagePid").val());
            $.ajax({
                type: "POST",
                url: this.Controller + "SaveChild",
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
                    debugger;
                    if (!data.response.isError) {
                        _this._commonAdmin.ToastifyAlert("Thành công!!!", '')
                        //$("#Pid").val(data.response.Data.Pid);
                        $("#" + _this.ModalChildID).modal("hide")
                        $("#tdBody_" + _this.TableChidID).html(data.listData)

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
    EditChild(id) {
        let _this = this;

        $.ajax({
            type: "GET",
            url: _this.Controller + "EditChild",
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            data: {
                idChild: id
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                //console.log(data.response)
                $("#" + _this.DivModalChildID).html(data.response);
                $("#" + _this.ModalChildID).modal("show");
            },
            failure: function (response) {
            }
        });
    }
    DeleteChild(id) {
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
                    url: _this.Controller + "DeleteChild",
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader("XSRF-TOKEN",
                            $('input:hidden[name="__RequestVerificationToken"]').val());
                    },
                    data: {
                        ids: listItemID,
                        FooterPagePid: $("#FooterPagePid").val()
                    },
                    success: function (data) {
                        if (!data.response.isError) {
                            _this._commonAdmin.ToastifyAlert("Thành công!!!", '')
                            $("#tdBody_" + _this.TableChidID).html(data.listData)
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
   
}

