

class CommonAdmin {
    constructor() {
        /* this.intial = intial;*/
    }
    ShowLoadingInBlock(divID,status) {
        var g = $("#" + divID)
        if (status) {
            g.block({
                message: '<div class="sk-wave mx-auto"><div class="sk-rect sk-wave-rect"></div> <div class="sk-rect sk-wave-rect"></div> <div class="sk-rect sk-wave-rect"></div> <div class="sk-rect sk-wave-rect"></div> <div class="sk-rect sk-wave-rect"></div></div>',
                css: {
                    backgroundColor: "transparent",
                    color: "#fff",
                    border: "0"
                },
                overlayCSS: {
                    opacity: .5
                }
            })
        } else {
            g.unblock();
        }
    }
    ToastifyAlert(mess, type) {
        var backgroundColor = "linear-gradient(to right, #00b09b, #96c93d)";
        if (type == "error") {
            backgroundColor = "linear-gradient(#e66465, #9198e5)";
        }
        Toastify({
            text: mess,
            duration: 3000,
            backgroundColor: backgroundColor,
        }).showToast();

    }
    SetDeviceMode(iframeId, view) {
        debugger;
        var iframe = document.getElementById(iframeId);

        if (view == "mobile") {
            iframe.width = 420;
            iframe.height = 720;

        }
        else if (view == "desktop") {
            //var parentDiv = iframe.offsetParent;
            //var parentDivWidth = parentDiv.clientWidth;
            iframe.width = '100%';
            iframe.height = 400;


        } else if (view == "tablet") {
            iframe.width = 720;

        }
    }
    CheckUnExistingObject(myUnexistingObject) {
        if (typeof myUnexistingObject !== 'undefined' && myUnexistingObject !== null) {
            return true;
        }
        else {
            return false;
        }
    }
    CopyText(text) {
        var _this = this
        var input = document.createElement('input');
        input.setAttribute('value', text);
        document.body.appendChild(input);
        input.select();
        var result = document.execCommand('copy');
        document.body.removeChild(input);
        console.log(text + "coppied")
        if (result) {
            _this.ToastifyAlert("Đã sao chép")
        }
    //    return result;;
    }
    CheckObjIsDefine(name) {
        var v = window, path = name.split('.');
        while (path.length > 0 && v) v = v[path.shift()];
        return this.CheckUnExistingObject(v)
    }
    async AlertDelete(callback) {
        return Swal.fire({
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
                callback(true);

            } else if (result.isDenied) {
                callback(false);
            }
        })
    }
    async AlertDelete(callback,mess) {
        return Swal.fire({
            title: mess,
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
                callback(true);

            } else if (result.isDenied) {
                callback(false);
            }
        })
    }
    async AlertWarning(callback,warningText) {
        return Swal.fire({
            title: warningText,
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
                callback(true);

            } else if (result.isDenied) {
                callback(false);
            }
        })
    }
    ConvertToSlug(str) {
        try {
            const normalizedStr = str.normalize("NFD").replace(/[\u0300-\u036f]/g, "");


            const slug = normalizedStr.replace(/[\s\W]+/g, "-");


            return slug.replace(/^-+|-+$/g, "").toLowerCase();
        } catch (e) {
            return "";
        }

    }
    GetListCheckedInGrid(className) {
        var rs = [];

        try {
            $("." + className).each(function () {
                if (this.checked) {
                    rs.push(parseInt(this.value));

                }
            });
        } catch (e) {

        }

        return rs;
    }
     DecodeHtmlEntities(input) {
    var entities = {
        '&lt;': '<',
        '&gt;': '>',
        '&amp;': '&',
    };

    return input.replace(/&lt;|&gt;|&amp;/g, function (match) {
        return entities[match];
    });
}
}

