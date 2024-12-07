class AdminLogin {
    constructor(model) {
        this._commonAdmin = new CommonAdmin()
        this.Controller =  "/Login/";
        this.redirectUrl = model.RedirectUrl;
    }
    ReRegister() {

    }

    Login(formId) {
        let _this = this;
        var formdata = new FormData($("#" + formId)[0]);

        $.ajax({
            type: "POST",
            url: this.Controller + "LoginUser",
            beforeSend: function (xhr) {
                showLoading();
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());

            },

            data: formdata,
            enctype: 'multipart/form-data',
            processData: false,
            contentType: false,
            success: function (data) {
                hideLoading();
                if (data.isError == false) {
                    if (!!_this.redirectUrl) {
                        window.location.href = _this.redirectUrl;
                    }
                    else {
                        window.location.href = "/dashboard";
                    }

                } else {
                    _this._commonAdmin.ToastifyAlert(data.message, 'error')

                }

            },
            failure: function (response) {
                hideLoading();
            }
        });

        //    }
    }

}

