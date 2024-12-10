function ValidateRecaptcha() {
    if (recapchaStatus == "on") {
        var recaptcha = grecaptcha.getResponse();
        if (recaptcha === "") {
            $("#parsley-id-recCatcha").removeAttr("hidden")
            return false;

        } else {
            $("#parsley-id-recCatcha").attr("hidden", "true")
            return true;
        }
    } else {
        return true;
    }
}
function SendContact() {
    var $selector = $('#formContact'),
        form = $selector.parsley();
    var validate = form.validate();
    if (validate == true) {
        if (ValidateRecaptcha()) {
            var formData = new FormData($("#formContact").get(0));
            $.ajax({
                url: "/Contact/SendContact",
                method: "POST",
                data: formData,
                processData: false,
                contentType: false,
                success: function (data) {
                    if (!data.isError) {
                        $("#formContact").attr('hidden', true);
                        $('#Content').val("");
                        $('#Subject').val("formContact");
                        $('#FullName').val("");
                        $('#Email').val("");
                        $('#Phone').val("");
                        setTimeout(function () { window.location.replace(homeUrl) }, 3000);
                        Swal.fire({ position: 'center', type: 'success', title: content, showConfirmButton: false, timer: 1500 });
                    }
                },
                error: function (err) { }
            })
        }
        else {
            Swal.fire({ position: 'center', type: 'error', title: errorValidateRecaptcha, showConfirmButton: false, timer: 1500 });
        }
    }
}

function SendEnquire() {
    var $selector = $('#formEnquire'),
        form = $selector.parsley();
    var validate = form.validate();
    if (validate == true) {
        if (ValidateRecaptcha()) {
            var formData = new FormData($("#formEnquire").get(0));
            formData.append("ServiceName", $('#ServiceDetailPidEnquire').find("option:selected").text());
            formData.append("DateEnquire", $('#calendar-layout').val());
            $.ajax({
                url: "/Contact/SendEnquire",
                method: "POST",
                data: formData,
                processData: false,
                contentType: false,
                success: function (data) {
                    if (!data.isError) {
                        $("#formEnquire").attr('hidden', true);
                        $('#ContentEnquire').val("");
                        $('#FullNameEnquire').val("");
                        $('#EmailEnquire').val("");
                        $('#PhoneEnquire').val("");
                        $('#ServicePidEnquire').val(0);
                        $('#calendar-service').datepicker("setDate", new Date());
                        setTimeout(function () { window.location.replace(homeUrl) }, 3000);
                        Swal.fire({ position: 'center', type: 'success', title: enquireContent, showConfirmButton: false, timer: 1500 });
                    }
                },
                error: function (err) { }
            })
        }
        else {
            Swal.fire({ position: 'center', type: 'error', title: errorValidateRecaptcha, showConfirmButton: false, timer: 1500 });
        }
    }
}

function SendService() {
    var $selector = $('#formService'),
        form = $selector.parsley();
    var validate = form.validate();
    if (validate == true) {
        if (ValidateRecaptcha()) {
            var formData = new FormData($("#formService").get(0));
            formData.append("ServiceName", $('#ServiceDetailPid').find("option:selected").text());
            formData.append("DateEnquire", $('#calendar-service').val());
            $.ajax({
                url: "/Contact/SendEnquire",
                method: "POST",
                data: formData,
                processData: false,
                contentType: false,
                success: function (data) {
                    if (!data.isError) {
                        $("#formService").attr('hidden', true);
                        $('#Content').val("");
                        $('#FullName').val("");
                        $('#Email').val("");
                        $('#Phone').val("");
                        $('#ServiceDetailPid').val(0);
                        $('#calendar-service').datepicker("setDate", new Date());
                        setTimeout(function () { window.location.replace(homeUrl) }, 3000);
                        Swal.fire({ position: 'center', type: 'success', title: enquireContent, showConfirmButton: false, timer: 1500 });
                    }
                },
                error: function (err) { }
            })
        }
        else {
            Swal.fire({ position: 'center', type: 'error', title: errorValidateRecaptcha, showConfirmButton: false, timer: 1500 });
        }

    }
}