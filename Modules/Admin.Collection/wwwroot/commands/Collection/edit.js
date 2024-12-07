class AdminCollection_Edit {
    constructor(model) {
        this._modelView = model;
        this.CollectionPid = model.CollectionPid;
        this.divModalDataID = model.DivModalDataID;
        this.ModalDataID = model.ModalDataID;
        this.divModalConfigID = model.DivModalConfigID;
        this.ModalConfigID = model.ModalConfigID;
        this.tableDataID = model.TableDataID;
        this.tbodyID = model.TbodyId;
        this.tbodyChooseProductID = model.tbodyChooseProductID;
        this.tbodyCollectionProductID = model.tbodyCollectionProductID;
        this.fromUploadID = model.FromUploadID;
        this.NavPagingID = model.NavPagingID;
        this.NavPagingChooseProductID = model.NavPagingChooseProductID;
        this.FileUploadID = model.FileUploadID;
        this.RouteIndex = model.RouteIndex;
        this.SearchFormID = model.SearchFormID;
        this.SearchModalChooseProductFormID = model.SearchModalChooseProductFormID;
        this._commonAdmin = new CommonAdmin();
        this.Controller = SteamSystem.VirtualFolder + "/Collection/"
    }
    ReRegister() {
    }
    SearchProductForCollection(page) {
        let _this = this;
        var formdata = new FormData($("#" + _this.SearchModalChooseProductFormID)[0]);
        formdata.append("PageIndex", page);
        formdata.append("ChoosenProducts", $("#ProductIDs").val());
        $.ajax({
            type: "POST",
            url: this.Controller + "SearchProductForCollection",
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
                $("#" + _this.tbodyChooseProductID).html(data.list)
                $("#" + _this.NavPagingChooseProductID).html(data.paging)
                _this.ReRegister();

            },
            failure: function (response) {
                _this._commonAdmin.ToastifyAlert("Thất bại!!!", 'error')

            }
        });
    }
    AddProductToCollection(sku) {
        let _this = this;
        debugger;
        //var formdata = new FormData($("#" + _this.SearchModalChooseProductFormID)[0]);
        //formdata.append("PageIndex", page);
        $.ajax({
            type: "POST",
            url: this.Controller + "AddProductToCollection",
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            data: {
                sku: sku
            },
            success: function (data) {

                console.log(data)
                $("#" + _this.tbodyCollectionProductID).append(data.list)
                _this.ReRegister();

            },
            failure: function (response) {
                _this._commonAdmin.ToastifyAlert("Thất bại!!!", 'error')

            }
        });
    }
    ChooseProduct(pid,sku) {
        let _this = this;
  
        try {
            var listSku = $('#ProductIDs').val();
            if (listSku == "") {
                listSku = ","+sku + ",";

            } else {
                listSku += sku + ",";

            }
            $('#ProductIDs').val(listSku)
            $('#tr_tableChooseProduct_' + pid).remove();
            _this.AddProductToCollection(sku);


        } catch (e) {

        }
    }
    RemoveChooseProduct(pid, sku) {
        let _this = this;
        var listSku = $('#ProductIDs').val();
        listSku = listSku.replace("," + sku + ",", ",");
        $('#ProductIDs').val(listSku)
        $('#tr_tableCollection_' + pid).remove();

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
    Save(formId) {
        let _this = this;
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
              
                //var pondFiles = filePond[0].getFiles();

                //debugger;
                //filePond.forEach((element, index) => {
                //    debugger;
                //    var pondFiles = filePond[index].getFiles();
                //    formdata.append(`files`, pondFiles[0].file)
                //    formdata.append(`FileStatus`, filePond[index].fileStatus)
                //    formdata.append(`FilePath`, filePond[index].filePath)

                //    //filePond.forEach((e, i) => {
                //    //    try {
                //    //        formdata.append(`files`, pondFiles[i].file)

                //    //    } catch (e) {

                //    //    }

                //    //})
                //})
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
                    if (!data.response.isError) {
                        _this._commonAdmin.ToastifyAlert("Thành công!!!", '')
                        $("#Pid").val(data.response.data.pid);
                        $("#OgImage").val(data.response.data.filePath + data.response.data.images);

                        //location.href = _this.RouteIndex;

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
    CheckFunction() {

        console.log("ComponentInThisView:", this._modelView)
    }
    ValidateConfig(formId) {
        return true;

    }
 
}

