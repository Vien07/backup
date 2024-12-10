// Write your JavaScript code.
function initParsley(frm, lang) {
    $(frm).parsley();
    Parsley.addMessages('vi', {
        defaultMessage: "Giá trị này có vẻ không hợp lệ.",
        type: {
            email: "E-mail không hợp lệ",
        },
        required: "Vui lòng không để trống.",
        equalto: "Không trùng khớp"
    });

    Parsley.addMessages('en', {
        defaultMessage: "This value has not a valid.",
        type: {
            email: "E-mail has not a valid.",
        },
        required: "Please do not leave it blank.",
        equalto: "No match"
    });

    Parsley.setLocale(lang);
}
function AlertToast(title, content, type) {
    $.toast({
        heading: title + "!",
        text: `<p>${content}</p>`,
        position: 'top-right',
        loaderBg: '#7a5449',
        class: `jq-toast-${type}`,
        hideAfter: 3500,
        stack: 6,
        showHideTransition: 'fade'
    });
}

$(document).on("change", "input[type='checkbox']", function () {
    var value = $(this).prop('checked');
    $(this).val(value);
});

function setCookie(name, value, days) {
    var expires = "";
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toUTCString();
    }
    document.cookie = name + "=" + (value || "") + expires + "; path=/";
}
function getCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}

function convertEmbedCode(value) {
    let embedCode = value;
    embedCode = embedCode.replaceAll("&lt;", "<");
    embedCode = embedCode.replaceAll("&gt;", ">");
    embedCode = embedCode.replaceAll("&quot;", "\"");
    embedCode = embedCode.replaceAll("&#xA;", "\n");
    return embedCode;
}

function ShowPopupBizMaC(delayTime) {
    delayTime = Number(delayTime);
    if (delayTime < 0) {
        delayTime = 2;
    }
    var popup = getCookie('PopupBizMaC');
    if (!popup) {
        $(window).on('load', function () {
            if (document.getElementById('popupModalBody') !== null) {
                setTimeout(function () {
                    $('#popupModal').modal('show');
                    setCookie('PopupBizMaC', 'f84d9082f07a1c4ac95fc8055d3d466d2d912e6b8917f7daba7d4f8d93c7d269');
                }, delayTime * 1000);
            }
        });
    }
}
