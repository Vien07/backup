$('#btn_edit_password').click(function (e) {
    e.preventDefault();

    var $btn = $(this);  // Store the button selector
    $btn.prop('disabled', true);  // Disable the button

    var $selector = $('.form-validate-password'),
        form = $selector.parsley();
    var validate = form.validate();
    if (validate == true) {
        let formData = new FormData($('.form-validate-password').get(0));
        $.ajax({
            url: "/Customer/EditPassword",
            method: "POST",
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                responseEditPassword(response);
                $btn.prop('disabled', false);  // Re-enable the button
            },
            error: function (err) {
                console.error(err);
                $btn.prop('disabled', false);  // Re-enable the button
            }
        })
    } else {
        $btn.prop('disabled', false);  // Re-enable the button if validation fails
    }
});

function responseEditPassword(response) {
    var data = JSON.parse(response);
    if (data.statusError == "OK") {
        Swal.fire({ position: 'center', type: 'success', title: data.message, showConfirmButton: false, timer: 1500 });
        window.location.href = signInUrl;
    }
    else {
        Swal.fire({ position: 'center', type: 'error', title: data.message, showConfirmButton: false, timer: 1500 });
    }
}

$('#btn_forgot_password').click(function (e) {
    e.preventDefault();
    var $selector = $('.form-validate-password'),
        form = $selector.parsley();
    var validate = form.validate();
    if (validate == true) {
        let formData = new FormData($('.form-validate-password').get(0));
        $.ajax({
            url: "/Customer/SendMailForgotPassword",
            beforeSend: function () {
                Swal({
                    title: "Checking...",
                    text: "Please wait",
                    imageUrl: "/img/configuration/Ajax-loader.gif",
                    showConfirmButton: false,
                    allowOutsideClick: false
                });
            },
            method: "POST",
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                Swal.close();

                responseForgotPassword(response);
            },
            error: function (err) {
                console.error(err);
            }
        })
    }
})
function responseForgotPassword(response) {
    var data = JSON.parse(response);
    if (data.statusError == "OK") {
        Swal.fire({ position: 'center', type: 'success', title: data.message, showConfirmButton: false, timer: 1500 });
    }
    else {
        if (data.errorCode == "email") {
            var passwordField = $(".form-validate-password").find('input[name="Email"]');
            if (passwordField.length > 0 && passwordField.parsley()) {

                passwordField.parsley().reset();
                passwordField
                    .parsley()
                    .addError('passwordError', { message: "Không tìm thấy Email" });
            } else {
                console.error('Password field or Parsley instance not found');
            }
        }
        else {
            Swal.fire({ position: 'center', type: 'error', title: data.message, showConfirmButton: false, timer: 1500 });

        }
    }
}

$('#btn_login').click(function (e) {
    e.preventDefault();
    var $selector = $('#formInput'),
        form = $selector.parsley();
    var validate = form.validate();
    if (validate == true) {
        let formData = new FormData($('#formInput').get(0));
        $.ajax({
            url: "/Customer/Login",
            method: "POST",
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                responseLogin(response);
            },
            error: function (err) {
                console.error(err);
            }
        })
    }
})
function responseLogin(response) {
    var data = JSON.parse(response);
    if (data.statusError == "OK") {
        window.location.href = settingAccountUrl;
    }
    else {
        if (data.errorCode == "password") {
            var passwordField = $(".form-sign-up-validate").find('input[name="Password"]');
            if (passwordField.length > 0 && passwordField.parsley()) {

                passwordField.parsley().reset();
                passwordField
                    .parsley()
                    .addError('passwordError', { message: "Mật khẩu không trùng khớp" });
            } else {
                console.error('Password field or Parsley instance not found');
            }
        }
        if (data.errorCode == "email") {
            var passwordField = $(".form-sign-up-validate").find('input[name="Email"]');
            if (passwordField.length > 0 && passwordField.parsley()) {

                passwordField.parsley().reset();
                passwordField
                    .parsley()
                    .addError('passwordError', { message: "Không tìm thấy tài khoản với Email" });
            } else {
                console.error('Password field or Parsley instance not found');
            }
        }

        //Swal.fire({ position: 'center', type: 'error', title: data.message, showConfirmButton: false, timer: 1500 });
    }
}


$('#btn_resgister').click(function (e) {
    e.preventDefault();
    var $selector = $('#formInput'),
        form = $selector.parsley();
    var validate = form.validate();
    if (validate == true) {
        let formData = new FormData($('#formInput').get(0));
        $.ajax({
            url: "/Customer/Register",
            method: "POST",
            beforeSend: function () {
                Swal({
                    title: "Checking...",
                    text: "Please wait",
                    imageUrl: "/img/configuration/Ajax-loader.gif",
                    showConfirmButton: false,
                    allowOutsideClick: false
                });
            },
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                Swal.close();

                responseRegister(response);
            },
            error: function (err) {
                console.error(err);
            }
        })
    }
})
function responseRegister(response) {
    var data = JSON.parse(response);
    if (data.statusError == "OK") {
        document.getElementById('formInput').reset();
        setTimeout(function () {
            window.location.href = signInUrl;
        }, 5000)
        Swal.fire({ position: 'center', type: 'success', title: data.message, showConfirmButton: false, timer: 1500 });
    }
    else {
        if (data.statusError == "DuplicateEmail") {
            var passwordField = $(".form-sign-up-validate").find('input[name="Email"]');
            if (passwordField.length > 0 && passwordField.parsley()) {

                passwordField.parsley().reset();
                passwordField
                    .parsley()
                    .addError('passwordError', { message: "Đã có tài khoản với Email này" });
            } else {
                console.error('Password field or Parsley instance not found');
            }
        }
        else {
            Swal.fire({ position: 'center', type: 'error', title: data.message, showConfirmButton: false, timer: 1500 });

        }
    }
}

function saveProfile() {
    var $selector = $('.form-validate-profile'),
        form = $selector.parsley();
    var validate = form.validate();
    if (validate) {
        $('#btnSaveProfile').attr('disabled', 'disabled')
        let formData = new FormData($('.form-validate-profile').get(0));
        formData.append('Province', $('#Province option:selected').val());
        formData.append('District', $('#District option:selected').val());
        formData.append('Ward', $('#Ward option:selected').val());
        formData.append('avatar', $('#Image').prop('files')[0])
        $.ajax({
            url: '/Customer/UpdateProfile',
            method: 'POST',
            contentType: false,
            processData: false,
            data: formData,
            success: function (rs) {
                $('#btnSaveProfile').removeAttr('disabled')
                var res = JSON.parse(rs);
                res.statusError == "OK" ?
                    AlertToast(notificationString, res.message, "success") :
                    AlertToast(notificationString, res.message, "error");
            },
            error: function (err) {
                $('#btnSaveProfile').removeAttr('disabled')
                console.error(err)
            }
        })
    }
}
function changePassword() {
    var $selector = $('.form-validate-password'),
        form = $selector.parsley();
    var validate = form.validate();
    if (validate) {
        let formData = new FormData($('.form-validate-password').get(0));
        $.ajax({
            url: '/Customer/UpdatePassword',
            method: 'POST',
            contentType: false,
            processData: false,
            data: formData,
            success: function (rs) {
                var res = JSON.parse(rs);
                if (res.statusError == "OK") {
                    AlertToast(notificationString, res.message, "success")
                }
                else {
                    if (res.errorCode == "old-password") {
                        var passwordField = $(".form-validate-password").find('input[name="CurrentPassword"]');
                        if (passwordField.length > 0 && passwordField.parsley()) {

                            passwordField.parsley().reset();
                            passwordField
                                .parsley()
                                .addError('passwordError', { message: res.message });
                        } else {
                            console.error('Password field or Parsley instance not found');
                        }
                    }
                    else {
                        AlertToast(notificationString, res.message, "error")
                    }
                }

            },
            error: function (err) {
                console.error(err)
            }
        })
    }
}

function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $('#avatar').attr('src', e.target.result);
        };
        reader.readAsDataURL(input.files[0]);
    }
}

function getSelectProvince(divProvinceId, divDistrictId, divWardId, provinceValue, districtValue, wardValue) {
    if (!provinceValue) {
        provinceValue = 79;
    }
    if (!districtValue) {
        districtValue = 760;
    }
    if (!wardValue) {
        wardValue = 26740;
    }
    $.ajax({
        url: '/data/VietNam/province.json',
        method: 'GET',
        success: function (data) {
            let rs = [];

            Object.keys(data).forEach((i) => {
                rs.push(data[i])
            });

            rs = rs.sort(function (a, b) {
                return ('' + a.name_with_type).localeCompare(b.name_with_type);
            });

            data = {};

            for (let i in rs) {
                data[i] = rs[i]
            }


            let html = ``;
            Object.keys(data).forEach((i) => {
                html += `<option ${provinceValue == data[i].code ? 'selected' : ''} value="${data[i].code}">${data[i].name_with_type}</option>`
            });

            $('#' + divProvinceId).html(html);

            $('#' + divProvinceId).change((e) => {
                let code = e.target.value;
                $.ajax({
                    url: `/data/VietNam/district/${code}.json`,
                    method: 'GET',
                    success: function (data) {
                        let html = ``;
                        Object.keys(data).forEach((i) => {
                            html += `<option ${districtValue == data[i].code ? 'selected' : ''} value="${data[i].code}">${data[i].name_with_type}</option>`
                        });

                        $('#' + divDistrictId).html(html);

                        $('#' + divDistrictId).change((e) => {
                            let code = e.target.value;
                            $.ajax({
                                url: `/data/VietNam/ward/${code}.json`,
                                method: 'GET',
                                success: function (data) {
                                    let html = ``;
                                    Object.keys(data).forEach((i) => {
                                        html += `<option ${wardValue == data[i].code ? 'selected' : ''} value="${data[i].code}">${data[i].name_with_type}</option>`
                                    });

                                    $('#' + divWardId).html(html);
                                    $('#' + divWardId).trigger('change');
                                },
                                error: function (err) {
                                    console.log(err)
                                }
                            })

                        })

                        $('#' + divDistrictId).trigger('change');

                    },
                    error: function (err) {
                        console.log(err)
                    }
                })
            })

            $('#' + divProvinceId).trigger('change');

        },
        error: function (err) {
            console.log(err)
        }
    })
}