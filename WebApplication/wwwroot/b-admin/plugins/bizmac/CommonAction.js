//define
var editData = {};
var updateState = false;

//

//search input
$("#key").keyup(function (event) {
    if (event.keyCode === 13) {
        Search(1)
    }
});
function Search(currentPage) {

    if (currentPage > 0) {
        page = currentPage;
    }

    $.ajax({
        url: actionUrl + 'LoadData',
        method: 'GET',
        data: {
            Page: page,
            PageNumber: pageNumber,
            Key: $('#key').val(),
            Cate: $('#selectCate').val(),
            Enable: ConvertIntToBool($("#selectActive").val())
        },
        success: function (response) {
            page = page;
            LoadGrid(response.data, response.paging);
        },
        error: function (e) {
            console.error(e)
        }
    })
}
//
$("#btnSearch").click(function (event) {
    Search(0)
});
//grid action
function LoadData(langKey) {
    $.ajax({
        url: actionUrl + 'LoadData',
        method: 'GET',
        data: {
            Page: page,
            pageNumber: pageNumber,
            LangKey: lang
        },
        success: function (response) {
            LoadGrid(response.data, response.paging);
        },
        error: function (e) {
            console.error(e)
        }
    })
}
function UpdateOrder(pid) {
    var order = $(`#orderInput-${pid}`).val();
    $.ajax({
        url: actionUrl + 'UpdateOrder',
        type: 'POST',
        data: {
            "Pid": pid,
            "Order": order
        },
        success: function (response) {
            if (response.isError) {

            } else {
                AlertToast('Thông tin', "Thât bại", 'error')
            }
        },
        error: function (e) {
            console.error(e)
        }
    })
}

function MoveRow(from, to) {
    $.ajax({
        url: actionUrl + 'MoveRow',
        type: 'POST',
        data: {
            "from": from,
            "to": to,
        },
        success: function (response) {
            if (response.isError) {
                AlertToast('Thông tin', "Thành công", "success")
                LoadGrid(response.list.data, response.list.paging)
            } else {
                AlertToast('Thông tin', "Thât bại", 'error')
            }
        },
        error: function (e) {
            console.error(e)
        }
    })
}
function Coppy() {
    var listPid = GetListItemChecked("Chk");
    if (listPid.length > 0) {
        $.ajax({
            url: actionUrl + 'Coppy',
            method: 'POST',
            data: {
                Pid: listPid
            },
            success: function (response) {
                if (response.isError) {
                    AlertToast('Thông báo', "Thành công", "success")
                    Search(0);
                }
                else {
                    SweetAlert('Thông báo', "Thất bại", 'error')
                }
            },
            error: function (e) {
                console.error(e)
            }
        })
    }
}
function Enable(pid, status) {
    var listPid = pid;
    if (pid == 0) {
        listPid = GetListItemChecked("Chk");

    }
    $.ajax({
        url: actionUrl + 'Enable',
        method: 'POST',
        data: {
            Pid: listPid,
            Enabled: status
        },
        success: function (response) {
            if (response.isError) {
                AlertToast('Thông báo', "Thành công", "success")
                Search(0)
            }
            else {
                SweetAlert('Thông báo', "Thất bại", 'error')
            }
        },
        error: function (e) {
            console.error(e)
        }
    })
}


//compose action
function OpenAddModal() {
    Clear();
    $.ajax({
        method: "POST",
        url: actionUrl + 'OpenAddModal',
        data: { lang: lang },
        success: function (data) {
            state = 'add';
            $("#bodyCompose").html(data);
            $('#btnSave').prop('disabled', false);
            $('#btnSave').attr('onclick', 'Insert()')
            $("#list").attr("hidden", "true");
            $("#compose").removeAttr("hidden");
            InitInput();
            ShowRawData()
            CustomGoBack();
        },
        error: function (e) {
            console.error(e)
        }
    })
}

function Edit(Pid) {
    $.ajax({
        method: "POST",
        url: actionUrl + 'OpenAddModal',
        data: { lang: lang },
        success: function (view) {
            state = "edit";
            $("#bodyCompose").html(view);
            $("#list").attr("hidden", "true");
            $("#compose").removeAttr("hidden");
            $('#btnSave').prop('disabled', false);
            $('#btnSave').attr('onclick', `Update(${Pid})`)

            $.ajax({
                url: actionUrl + "Edit",
                method: "POST",
                data: { Pid: Pid },
                success: function (data) {
                    CustomSetDataEdit(data)
                    ShowRawData(data)
                    InitInput();
                    CustomGoBack();
                },
                error: function (e) {
                    console.error(e)
                }
            })
        },
        error: function (e) {
            console.error(e)
        }
    })
}

//modal action
function OpenAddPopup() {
    $.ajax({
        method: "POST",
        url: actionUrl + 'OpenAddModal',
        data: { lang: lang },
        success: function (data) {
            $("#bodyModal").html(data);
            $("#inputModal").modal("show");

            state = 'add';
            $('#btnSave').attr('onclick', 'Insert()')
            InitInput();
            ShowRawData()
        },
        error: function (e) {
            console.error(e)
        }
    })
}

function OpenEditModal(Pid) {
    state = "edit";
    $.ajax({
        method: "POST",
        url: actionUrl + 'OpenAddModal',
        data: { lang: lang },
        success: function (view) {
            $("#bodyModal").html(view);
            $("#inputModal").modal("show");
            InitInput();
            $('#btnSave').attr('onclick', `Update(${Pid})`)
            $.ajax({
                url: actionUrl + "Edit",
                method: "POST",
                data: { Pid: Pid },
                success: function (data) {
                    CustomSetDataEdit(data);
                    InitInput();
                    ShowRawData()
                },
                error: function (e) {
                    console.error(e)
                }
            })
        },
        error: function (e) {
            console.error(e)
        }
    })
}
$('#inputModal').on('hide.bs.modal', function () {
    ClearInputPopup();
})


//common action
function Update(data) {
    $('#btnSave').prop('disabled', true);
    if (Validate()) {
        var formData = new FormData();
        formData = GetFormData();

        $.ajax({
            url: actionUrl + 'Update',
            type: 'POST',
            contentType: false,
            processData: false,
            data: formData,
            success: function (response) {
                if (response.error.status) {
                    Clear();
                    Search();

                    AlertToast('Thông tin', "Thành công", "success")

                } else {
                    $('#btnSave').prop('disabled', false);

                    SweetAlert('Thông tin', response.error.mess, 'error')
                }
            },
            error: function (e) {
                console.error(e)
            }
        })
    } else {
        $('#btnSave').prop('disabled', false);

    }
}
function Insert() {
    $('#btnSave').prop('disabled', true);
    if (Validate()) {
        var formData = new FormData();
        formData = GetFormData();
        $.ajax({
            url: actionUrl + 'Insert',
            type: 'POST',
            contentType: false,
            processData: false,
            data: formData,
            success: function (response) {
                updateState = true;
                if (response.error.status) {
                    Clear();
                    LoadGrid(response.list.data, response.list.paging)

                    AlertToast('Thông báo', 'Thành công', 'success')

                } else {
                    $('#btnSave').prop('disabled', false);

                    SweetAlert('Thông báo', response.error.mess, 'error')
                }
            },
            error: function (e) {
                $('#btnSave').prop('disabled', false);
            }
        })
    } else {
        $('#btnSave').prop('disabled', false);

    }
}
function Preview() {
    if (Validate()) {
        $('#preview').prop('disabled', true);
        var formData = new FormData();
        formData = GetFormData();
        $.ajax({
            url: actionUrl + 'Preview',
            type: 'POST',
            cache: false,
            contentType: false,
            processData: false,
            data: formData,
            success: function (response) {
                if (response) {
                    window.open(response)
                }
                $('#preview').prop('disabled', false);
            },
            error: function (e) {
                $('#preview').prop('disabled', false);
            }
        })
    }
}
function Delete(Pid) {
    Swal.fire({
        title: 'Bạn có muốn xóa?',
        text: "",
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Đồng ý'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                url: actionUrl + "Delete",
                method: "POST",
                data: {
                    Pid: Pid
                }
            }).done(function (data) {
                if (data.isError) {
                    AlertToast('Thông báo', "Thành công", "success")
                    Search(0);
                } else {
                    if (data.messError) {
                        SweetAlert('Thông báo', data.messError, 'error')
                    }
                    else {
                        SweetAlert('Thông báo', "Thất bại", 'error')
                    }
                }
            })
        }
    })

}
function DeleteMulti() {
    Swal.fire({
        title: 'Bạn có đồng ý xóa?',
        text: "",
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Đồng ý'
    }).then((result) => {
        if (result.value) {
            var listPid = GetListItemChecked("Chk");
            $.ajax({
                url: actionUrl + 'DeleteMulti',
                method: 'POST',
                data: {
                    Pid: listPid
                }
            }).done(function (data) {
                if (data.isError) {
                    AlertToast('Thông báo', "Thành công", "success")
                    Search(0);
                }
                else {
                    if (data.messError) {
                        SweetAlert('Thông báo', data.messError, 'error')
                    }
                    else {
                        SweetAlert('Thông báo', "Thất bại", 'error')
                    }
                }
            })
        }
    })

}

//tab  lang action
$("ul[id=langTab] li").click(function () {
    SaveRawData();
    var langKey = $(this)[0].lang
    lang = langKey;
    //console.log(langKey);
    $.ajax({
        method: "POST",
        url: actionUrl + 'OpenAddModal',
        data: { lang: lang }

    }).done(function (data) {
        $("#bodyCompose").html(data);
        //if (state == "new") {

        //} else if (state == "edit") {

        //}
        InitInput();
        ShowRawData();

    });

});


//  table action
function InitDragDropTable() {
    let tableRow = document.querySelector("#tableData");
    tableDragger(document.querySelector("#tableData"), { mode: "row", onlyBody: true, dragHandler: ".handle" })
        .on('drag', (el) => {


        })
        .on('drop', (from, to, el, mode) => {
            if (from != to) {
                if (from < to) {

                    MoveRow(tableRow.rows[to].id, tableRow.rows[to - 1].id);

                }
                else {
                    MoveRow(tableRow.rows[to].id, tableRow.rows[to + 1].id);
                }
            }
        });
}
function InitToggle(data) {
    $(`#toggle_${data.Pid}`).toggles({
        drag: false,
        click: true,
        text: {
            on: '',
            off: ''
        },
        on: data.Enabled,
        animate: 250,
        easing: 'swing',
        type: 'compact'
    })
        .on('toggle', function (e, active) {
            Enable(data.Pid, active);
        });
}
//

// mullti images
var imagesList = [];
var multiLangImagesList = [];
var imagesDeleteList = [];
function UploadImages() {
    var order = 0;
    if (imagesList != null) {
        order = imagesList.length;
    } else {
        imagesList = [];
        multiLangImagesList = []
    }
    for (var i = 0; i < myDropzone.files.length; i++) {
        if (actionUrl == "/b-admin/Product/" && order >= 10) break;
        var item = {};
        var itemLang = {};
        order = order + 1;
        var id = new Date().getTime()
        item.Pid = id;

        item.Order = order;
        item.Images = myDropzone.files[i].dataURL;
        item.Status = "new";

        imagesList.push(item);
        itemLang.Caption = "";
        itemLang.ImagesPid = item.Pid;
        itemLang.LangKey = lang;
        multiLangImagesList.push(itemLang);
        $("#listImagesUpload")
            .append(` <div class="mr-3 mb-4 d-inline-block text-center"><div id="${id}" onclick="EditImages(this.id)" style="background-image: url('${myDropzone.files[i].dataURL}');cursor:pointer" class="avatar avatar-lg border rounded d-block mb-1 p-1"></div>
                        <input id="order-${imagesList[i].Pid}" onfocusout="OrderImages(${imagesList[i].Pid})" value="${item.Order}"  type="text" tabindex="${500 + i}" class="form-control form-control-sm d-inline  text-center border" placeholder=""  style="width:45px;height:25px "> </div>`);
    }
    myDropzone.removeAllFiles(true);
}
function LoadImages() {
    if (imagesList) {
        imagesList.sort(compareValues('Order'))
        $("#listImagesUpload").html('');

        for (var i = 0; i < imagesList.length; i++) {
            $("#listImagesUpload")
                .append(`
                  <div class="mr-3 mb-4 d-inline-block text-center">  <div id="${imagesList[i].Pid}" onclick="EditImages(this.id)" style="background-image: url('${imagesList[i].Images}');cursor:pointer" class="avatar avatar-lg border rounded d-block mb-1 p-1"></div>
                      <input id="order-${imagesList[i].Pid}" onfocusout="OrderImages(${imagesList[i].Pid})" value="${imagesList[i].Order}"  type="text" tabindex="${500 + i}" class="form-control form-control-sm d-inline  text-center border" placeholder=""  style="width:45px;height:25px "> </div>`);

        }
    }
}
function EditImages(id) {
    ResetImages();
    $("#imageModel").modal("show");
    var rs = imagesList.find(x => x.Pid == id);
    var multiLang = multiLangImagesList.find(x => x.ImagesPid == id && x.LangKey == lang);
    if (typeof multiLang != "undefined") {
        $("#caption").val(multiLang.Caption);

    }

    document.getElementById("modalImages").src = rs.Images;


    $("#order").val(rs.order);
    $("#btnSaveImages").attr("onclick", `SaveImages(${id})`);
    $("#btnDeleteImages").attr("onclick", `DeleteImages(${id})`);
}
function DeleteImages(id) {
    for (var i = 0; i < imagesList.length; i++) {
        if (imagesList[i].Pid === id) {
            //imagesList[i].Status = "delete";
            imagesDeleteList.push(imagesList[i]);
            imagesList.splice(i, 1);
            break;
        }
    }
    multiLangImagesList = multiLangImagesList.filter(x => x.ImagesPid != id);

    //for (var i = 0; i < multiLangImagesList.length; i++) {
    //    if (multiLangImagesList[i].ImagesPid === id) {
    //        //multiLangImagesList.splice(i, 1);

    //    }
    //}
    imagesList.sort(compareValues('Order'))
    $("#listImagesUpload").html('');
    for (var i = 0; i < imagesList.length; i++) {
        imagesList[i].Order = i + 1;
        $("#listImagesUpload")
            .append(`
                  <div class="mr-3 mb-4 d-inline-block text-center">  <div id="${imagesList[i].Pid}" onclick="EditImages(this.id)" style="background-image: url('${imagesList[i].Images}');cursor:pointer" class="avatar avatar-lg border rounded d-block mb-1 p-1"></div>
                      <input id="order-${imagesList[i].Pid}" onfocusout="OrderImages(${imagesList[i].Pid})" value="${imagesList[i].Order}" type="text" tabindex="${500 + i}" class="form-control form-control-sm d-inline  text-center border" placeholder=""  style="width:45px;height:25px "> </div>`);

    }
    $("#imageModel").modal("hide");

}
function OrderImages(id) {
    var rs = imagesList.find(x => x.Pid == id);
    rs.order = parseInt($("#order-" + id).val());
    imagesList.sort(compareValues('order'))
    LoadImages()

}
function ResetImages() {
    $("#caption").val('');
}
function ClearImagesMulti() {
    multiLangImagesList = [];
    imagesList = []; imagesDeleteList = []
    ResetImages();
}
function SaveImages(id) {
    var rs = imagesList.find(x => x.Pid == id);
    var multiLang = multiLangImagesList.find(x => x.ImagesPid == id && x.LangKey == lang);
    if (typeof multiLang == "undefined") {
        var itemMultiLang = {}
        itemMultiLang.ImagesPid = id;
        itemMultiLang.LangKey = lang;
        itemMultiLang.Caption = $("#caption").val();
        itemMultiLang.Pid = "0";
        multiLangImagesList.push(itemMultiLang)
    } else {
        multiLang.Caption = $("#caption").val();
    }
    LoadImages();
    $("#imageModel").modal("hide");

}
//

//images  action
var defaultUrlImages = "/b-admin/dist/img/avatar3.jpg";

function ClearImages(divId) {
    imagesObj = {
        base64: '',
        type: ''
    };
    $(divId).css("background-image", "url('" + defaultUrlImages + "'");
    $(divId + 'FileName').addClass("d-none");
}
function readURL(input, divImages, id) {
    try {
        if (FileReader) {
            var reader = new FileReader();
            //console.log(FileReader)
            reader.readAsDataURL(input.files[0]);
            reader.onload = function (e) {
                var image = new Image();
                image.src = e.target.result;
                image.onload = function () {
                    imagesObj.base64 = image.src;
                    imagesObj.type = "new";
                    //if (state == "add") {

                    //    localStorage.setItem('rawGalleryObjsCommon', JSON.stringify(currentDataCommon));
                    //}

                    $(divImages).css("background-image", "url('" + image.src + "'");
                    $(divImages + 'FileName').removeClass("d-none");
                };
            }
        }
        else {
            // Not supported
        }
    } catch (e) {

    }
}


//Dùng cho thẻ img và change attribute src
function ClearImagesImage(divId) {
    imagesObj = {
        base64: '',
        type: ''
    };
    $(divId).attr("src", defaultUrlImages);
}
//Dùng cho thẻ img và change attribute src
function readURLImage(input, divImages, id) {
    try {
        if (FileReader) {
            var reader = new FileReader();
            //console.log(FileReader)
            reader.readAsDataURL(input.files[0]);
            reader.onload = function (e) {
                var image = new Image();
                image.src = e.target.result;
                image.onload = function () {
                    imagesObj.base64 = image.src;
                    imagesObj.type = "new";
                    //if (state == "add") {

                    //    localStorage.setItem('rawGalleryObjsCommon', JSON.stringify(currentDataCommon));
                    //}

                    $(divImages).attr("src", image.src);
                };
            }
        }
        else {
            // Not supported
        }
    } catch (e) {

    }
}
//Card
function SearchCard(currentPage, orderPid) {

    if (currentPage > 0) {
        page = currentPage;
    }

    $.ajax({
        url: actionUrl + 'LoadDataCard',
        method: 'GET',
        data: {
            Page: page,
            PageNumber: pageNumber,
            LangKey: lang,
            Key: $('#key').val(),
            OrderPid: orderPid
        },
        success: function (response) {
            page = page;
            LoadGridCard(response.data, response.paging);
        },
        error: function (e) {
            console.error(e)
        }
    })
}

function LoadDataCard(langKey, orderPid) {
    $.ajax({
        url: actionUrl + 'LoadDataCard',
        method: 'GET',
        data: {
            Page: page,
            pageNumber: pageNumber,
            LangKey: lang,
            OrderPid: orderPid
        },
        success: function (response) {
            LoadGridCard(response.data, response.paging);
        },
        error: function (e) {
            console.error(e)
        }
    })
}
function OpenAddPopupCard() {
    $.ajax({
        method: "POST",
        url: actionUrl + 'OpenAddModal',
        data: { lang: lang },
        success: function (data) {
            $("#bodyModal").html(data);
            $("#inputModalCard").modal("show");

            state = 'add';
            $('#btnSaveCard').attr('onclick', 'SaveCard()')
            InitInput();
            ShowRawData()
        },
        error: function (e) {
            console.error(e)
        }
    })
}
function DeleteCard(Pid) {
    Swal.fire({
        title: 'Bạn có muốn xóa?',
        text: "",
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Đồng ý'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                url: actionUrl + "DeleteCard",
                method: "POST",
                data: {
                    Pid: Pid,
                    OrderPid: orderPid,
                }
            }).done(function (data) {
                if (data.isError) {
                    AlertToast('Thông báo', "Thành công", "success")
                    SearchCard(null, orderPid);
                } else {
                    SweetAlert('Thông báo', data.messError, 'error')
                }
            })
        }
    })

}
function DeleteMultiCard() {
    Swal.fire({
        title: 'Bạn có đồng ý xóa?',
        text: "",
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Đồng ý'
    }).then((result) => {
        if (result.value) {
            var listPid = GetListItemChecked("Chk");
            $.ajax({
                url: actionUrl + 'DeleteMultiCard',
                method: 'POST',
                data: {
                    Pid: listPid,
                    OrderPid: orderPid,
                }
            }).done(function (data) {
                if (data.isError) {
                    AlertToast('Thông báo', "Thành công", "success")
                    SearchCard(null, orderPid);
                }
                else {
                    SweetAlert('Thông báo', "Thất bại", 'error')
                }
            })
        }
    })

}
function OpenEditModalCard(Pid) {
    state = "edit";
    $.ajax({
        method: "POST",
        url: actionUrl + 'OpenAddModal',
        data: { lang: lang },
        success: function (view) {
            $("#bodyModal").html(view);
            $("#inputModalCard").modal("show");
            //InitInput();
            $('#btnSaveCard').attr('onclick', `SaveCard(${Pid})`)
            $.ajax({
                url: actionUrl + "EditCard",
                method: "POST",
                data: { Pid: Pid },
                success: function (data) {
                    CustomSetDataEditCard(data);
                    //InitInput();
                    ShowRawDataCard()
                },
                error: function (e) {
                    console.error(e)
                }
            })
        },
        error: function (e) {
            console.error(e)
        }
    })
}

function UpdateCard(data) {
    $('#btnSaveCard').prop('disabled', true);
    if (Validate()) {
        var formData = new FormData();
        formData = GetFormDataCard();

        $.ajax({
            url: actionUrl + 'UpdateCard',
            type: 'POST',
            contentType: false,
            processData: false,
            data: formData,
            success: function (response) {
                if (response.error.status) {
                    ClearCard();
                    SearchCard(null, orderPid);

                    AlertToast('Thông tin', "Thành công", "success")

                } else {
                    $('#btnSaveCard').prop('disabled', false);

                    SweetAlert('Thông tin', response.error.mess, 'error')
                }
            },
            error: function (e) {
                console.error(e)
            }
        })
    } else {
        $('#btnSave').prop('disabled', false);

    }
}