class WebSetting {
    constructor(model) {
        this.fromUploadID = model.FromUploadID;
        this.FileUploadID = model.FileUploadID;
        this._commonAdmin = new CommonAdmin()
        this.Controller = SteamSystem.VirtualFolder + "/WebSetting/"
    }

    Save(typeSave) {
        let _this = this;

        var formdata = new FormData($("#" + _this.fromUploadID)[0]);
        formdata.append(`TypeSave`, typeSave)

        if (_this._commonAdmin.CheckObjIsDefine("Logo")) {
            try {
                formdata.append(`Logo`, Logo.file.file)
                formdata.append(`FileStatus_Logo`, Logo.metadata.fileStatus)
                formdata.append(`FilePath_Logo`, Logo.metadata.filePath)
            } catch (e) {

            }
        }
        if (_this._commonAdmin.CheckObjIsDefine("ogImage")) {
            try {
                formdata.append(`ogImage`, ogImage.file.file)
                formdata.append(`FileStatus_ogImage`, ogImage.metadata.fileStatus)
                formdata.append(`FilePath_ogImage`, ogImage.metadata.filePath)
            } catch (e) {

            }
        } if (_this._commonAdmin.CheckObjIsDefine("Favicon")) {
            try {
                formdata.append(`Favicon`, Favicon.file.file)
                formdata.append(`FileStatus_Favicon`, Favicon.metadata.fileStatus)
                formdata.append(`FilePath_Favicon`, Favicon.metadata.filePath)
            } catch (e) {

            }
        }


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
                if (!data.response.isError) {
                    _this._commonAdmin.ToastifyAlert("Thành công!!!", '')
                    location.reload();

                } else {
                    _this._commonAdmin.ToastifyAlert("Thất bại!!!", 'error')
                }
            },
            failure: function (response) {
                _this._commonAdmin.ToastifyAlert("Thất bại!!!", 'error')

            }
        });
    }
    GenerateMeta() {
        let _this = this;

        var formdata = new FormData($("#" + _this.fromUploadID)[0]);
        //filePond.forEach((element, index) => {
        //    try {
        //        formdata.append(`${element.name}`, element.getFiles()[0].file)
        //    } catch (e) {
        //        console.error(e)
        //    }
        //})

        $.ajax({
            type: "POST",
            url: _this.Controller + "GenerateMetaTag",
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

                codemirror_WebsiteMeta.setValue(data.response)

                if (!data.response.isError) {


                } else {
                    _this._commonAdmin.ToastifyAlert("Thất bại!!!", 'error')
                }
            },
            failure: function (response) {
                _this._commonAdmin.ToastifyAlert("Thất bại!!!", 'error')

            }
        });
    }
    CheckFunction() {
        console.log("Init WebSettingCommand.js.....")
    }
}

