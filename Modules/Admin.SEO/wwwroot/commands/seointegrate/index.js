class SEOIntegrate_Index {
    constructor(model) {
        this._modelView = model;
        this.divModalDataID = model.DivModalDataID;
        this.ModalDataID = model.ModalDataID;
        this.divModalConfigID = model.DivModalConfigID;
        this.ModalConfigID = model.ModalConfigID;
        this.tableDataID = model.TableDataID;
        this.fromUploadID = model.FromUploadID;
        this.NavPagingID = model.NavPagingID;
        this.FileUploadID = model.FileUploadID;
        this.RouteIndex =model.RouteIndex;
        this.SearchFormID = model.SearchFormID;
        this.SlugGenerateFrom = model.SlugGenerateFrom;
        this.PostTitleGenerateFrom = model.PostTitleGenerateFrom;
        this.MetaDescriptionGenerateFrom = model.MetaDescriptionGenerateFrom;
        this.GetCatePidFrom = model.GetCatePidFrom;
        this.GetPostPidFrom = model.GetPostPidFrom;
        this.GetOgImageGenerateFrom = model.GetOgImageGenerateFrom;
        this._commonAdmin = new CommonAdmin();
        this.Controller = SteamSystem.VirtualFolder + "/SEOIntegrate/"
    }
    ReRegister() {
    }

    Edit(id) {
        let _this = this;

        $.ajax({
            type: "GET",
            url: "/SEO/ById",
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
            var postId = $('#' + _this.GetPostPidFrom).val();
            var ogImage = $('#' + _this.GetOgImageGenerateFrom).val();

            formdata.append(`PostPid`, postId)
            //formdata.append(`OgImage`, ogImage)

            //if (_this._commonAdmin.CheckObjIsDefine("filePond")) {
            //    filePond.forEach((element, index) => {

            //        var pondFiles = filePond[index].getFiles();
            //        filePond.forEach((e, i) => {
            //            formdata.append(`files`, pondFiles[i].file)

            //        })
            //    })
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
                        codemirror_Meta.setValue(data.response.data.meta)

                        //location.href = _this.RouteIndex;

                        //if (type == 0) {
                        //    _this.ResetForm();
                        //}
                        //else if (type == 1) {

                        //}

                    } else {
                        _this._commonAdmin.ToastifyAlert(data.response.message, 'error')

                    }


                },
                failure: function (response) {
                    _this._commonAdmin.ToastifyAlert("Thất bại!!!", 'error')

                }
            });
        }
    }
    SetData() {
        debugger;
        var _this = this;
        //generate Slug
        var checkText = $("#PostSlug").val()
        if (checkText == "") {
            var inputString = $('#' + _this.SlugGenerateFrom).val()
            const slug = _this._commonAdmin.ConvertToSlug(inputString);
            $("#PostSlug").val(slug)
        }
        //generate description
        var checkDescriptionText = $("#MetaDescription").val()
        if (checkDescriptionText == "") {
            if (_this.MetaDescriptionGenerateFrom != "") {
                var inputDesString = $('#' + _this.MetaDescriptionGenerateFrom).val()
                let strippedString = inputDesString.replace(/(<([^>]+)>)/gi, "");

                $("#MetaDescription").val(strippedString)
            }

        }
        //set pageTitle
        const pageTitle = $('#' + _this.PostTitleGenerateFrom).val();
        $("#PostTitle").val(pageTitle)

        //// set pid post
        //var postId = $('#' + _this.GetPostPidFrom).val();
        //$("#PostPid").val(postId).trigger('change');
        ////
        //// set pid cate
        //var cateId = $('#' + _this.GetCatePidFrom).val();
        //$("#CatePid").val(cateId).trigger('change');
        ////

    }
    GenerateMetaTag(formId) {
        let _this = this;
        if (_this.Validate(formId)) {
            //var formdata = new FormData($("#" + _this.fromUploadID)[0]);
            var formdata = new FormData($("#" + formId)[0]);
            $.ajax({
                type: "POST",
                url: this.Controller + "GenerateMetaTag",
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
                    codemirror_Meta.setValue(data.response)

                    //if (!data.response.isError) {

                    //} else {
                    //    _this._commonAdmin.ToastifyAlert(data.response.message, 'error')

                    //}


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
    Validate(formId) {
        let _this = this;

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
    ValidateConfig(formId) {
        return true;

    }

}

