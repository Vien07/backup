
function InitTinymce(divId) {
    try {
        tinymce.remove('#' + divId)

    } catch (e) {

    }

    tinymce.init({
        selector: `#${divId}`,
        height: 700,
        menubar: false,
        branding: false,
        plugins: [
            'advlist autolink lists link image charmap print preview hr anchor pagebreak textcolor link image ',
            'searchreplace searchreplace  visualchars code fullscreen',
            'insertdatetime emoticons directionality media table contextmenu paste code help wordcount filemanager responsivefilemanager media quickbars'
        ],

        contextmenu: "paste | link image inserttable responsivefilemanager | cell row column deletetable",
        toolbar: '| undo redo | bold italic underline strikethrough | fontselect formatselect  backcolor forecolor | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link unlink anchor | table responsivefilemanager | preview code | media | removeformat |  help',
        quickbars_selection_toolbar: ' | bold italic underline strikethrough |  formatselect  backcolor forecolor | alignleft aligncenter alignright alignjustify  |',
        image_advtab: true,
        quickbars_insert_toolbar: false,
        external_filemanager_path: "/filemanager/",
        filemanager_title: "Filemanager",
        external_plugins: {
            "filemanager": "/filemanager/plugin.min.js"
        }

    });
}

function InitTinymcePromises(divId) {
    return new Promise(function (resolve, reject) {
        try {
            tinymce.remove('#' + divId)

        } catch (e) {

        }

        tinymce.init({
            selector: `#${divId}`,
            height: 500,
            menubar: false,
            branding: false,
            plugins: [
                'advlist autolink lists link image charmap print preview hr anchor pagebreak textcolor link image ',
                'searchreplace searchreplace  visualchars code fullscreen',
                'insertdatetime emoticons directionality media table contextmenu paste code help wordcount filemanager responsivefilemanager media quickbars'
            ],

            contextmenu: "paste | link image inserttable responsivefilemanager | cell row column deletetable",
            toolbar: '| undo redo | bold italic underline strikethrough | fontselect formatselect  backcolor forecolor | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link unlink anchor | table responsivefilemanager | preview code | media | removeformat |  help',
            quickbars_selection_toolbar: ' | bold italic underline strikethrough |  formatselect  backcolor forecolor | alignleft aligncenter alignright alignjustify  |',
            image_advtab: true,
            quickbars_insert_toolbar: false,
            external_filemanager_path: "/filemanager/",
            filemanager_title: "Filemanager",
            external_plugins: {
                "filemanager": "/filemanager/plugin.min.js"
            }

        });
        resolve(true);
    })
}

function SetContentTinymce(divId, content) {
    try {
        tinymce.remove('#' + divId)

    } catch (e) {

    }
    $(`#${divId}`).val(content);
    tinymce.init({
        selector: `#${divId}`,
        height: 700,
        menubar: false,
        branding: false,
        plugins: [
            'advlist autolink lists link image charmap print preview hr anchor pagebreak textcolor link image ',
            'searchreplace searchreplace  visualchars code fullscreen',
            'insertdatetime emoticons directionality media table contextmenu paste code help wordcount filemanager responsivefilemanager media quickbars'
        ],

        contextmenu: "paste | link image inserttable responsivefilemanager | cell row column deletetable",
        toolbar: '| undo redo | bold italic underline strikethrough | fontselect formatselect  backcolor forecolor | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link unlink anchor | table responsivefilemanager | preview code | media | removeformat |  help',
        quickbars_selection_toolbar: ' | bold italic underline strikethrough |  formatselect  backcolor forecolor | alignleft aligncenter alignright alignjustify  |',
        image_advtab: true,
        quickbars_insert_toolbar: false,
        external_filemanager_path: "/filemanager/",
        filemanager_title: "Filemanager",
        external_plugins: {
            "filemanager": "/filemanager/plugin.min.js"
        }

    });
}

function InitTinymceMinControlWithHeight(divId, height) {
    try {
        tinymce.remove('#' + divId)

    } catch (e) {

    }

    tinymce.init({
        selector: `#${divId}`,
        height: height,
        menubar: false,
        branding: false,
        plugins: [
            'lists code'
        ],
        contextmenu: "paste",
        //toolbar: '| bold italic underline strikethrough | bullist numlist outdent indent | code',
        toolbar: '| bold italic underline strikethrough | bullist numlist outdent indent ',
        quickbars_selection_toolbar: ' | bold italic underline strikethrough |  formatselect  backcolor forecolor | alignleft aligncenter alignright alignjustify  |',
        image_advtab: true,
        quickbars_insert_toolbar: false,
        external_filemanager_path: "/filemanager/",
        filemanager_title: "Filemanager",
        external_plugins: {
            "filemanager": "/filemanager/plugin.min.js"
        }
    });
}
function SetContentTinymce(divId, content) {
    try {
        tinymce.remove('#' + divId)

    } catch (e) {

    }
    $(`#${divId}`).val(content);
    tinymce.init({
        selector: `#${divId}`,
        height: 700,
        menubar: false,
        branding: false,
        plugins: [
            'advlist autolink lists link image charmap print preview hr anchor pagebreak textcolor link image ',
            'searchreplace searchreplace  visualchars code fullscreen',
            'insertdatetime emoticons directionality media table contextmenu paste code help wordcount filemanager responsivefilemanager media quickbars'
        ],

        contextmenu: "paste | link image inserttable responsivefilemanager | cell row column deletetable",
        toolbar: '| undo redo | bold italic underline strikethrough | fontselect formatselect  backcolor forecolor | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link unlink anchor | table responsivefilemanager | preview code | media | removeformat |  help',
        quickbars_selection_toolbar: ' | bold italic underline strikethrough |  formatselect  backcolor forecolor | alignleft aligncenter alignright alignjustify  |',
        image_advtab: true,
        quickbars_insert_toolbar: false,
        external_filemanager_path: "/filemanager/",
        filemanager_title: "Filemanager",
        external_plugins: {
            "filemanager": "/filemanager/plugin.min.js"
        }

    });
}

function SetContentTinymceMinControl(divId, content, height) {
    try {
        tinymce.remove('#' + divId)

    } catch (e) { console.warn(e) }

    $(`#${divId}`).val(content);
    tinymce.init({
        selector: `#${divId}`,
        height: height,
        menubar: false,
        branding: false,
        plugins: [
            'lists code'
        ],
        contextmenu: "paste",
        toolbar: '| bold italic underline strikethrough | bullist numlist outdent indent ',
        quickbars_selection_toolbar: ' | bold italic underline strikethrough |  formatselect  backcolor forecolor | alignleft aligncenter alignright alignjustify  |',
        image_advtab: true,
        quickbars_insert_toolbar: false,
        external_filemanager_path: "/filemanager/",
        filemanager_title: "Filemanager",
        external_plugins: {
            "filemanager": "/filemanager/plugin.min.js"
        }
    });
}


function SetContentAsync(divId, content) {
    var setContentAsyncId = setInterval(function () {
        if (Object.keys($(`#${divId}_ifr`)).length > 0) {
            tinymce.get(divId).setContent(content);
            clearInterval(setContentAsyncId);
        }
    }, 200)
}

function SetContentTinymceMinControl2(divId, content, height) {
    try {
        tinymce.remove('#' + divId)

    } catch (e) { console.warn(e) }

    $(`#${divId}`).val(content);
    tinymce.init({
        selector: `#${divId}`,
        height: height,
        menubar: false,
        branding: false,
        plugins: [
            'lists code'
        ],
        contextmenu: "paste",
        toolbar: '| bold italic underline strikethrough | bullist numlist outdent indent | code ',
        quickbars_selection_toolbar: ' | bold italic underline strikethrough |  formatselect  backcolor forecolor | alignleft aligncenter alignright alignjustify  |',
        image_advtab: true,
        quickbars_insert_toolbar: false,
        external_filemanager_path: "/filemanager/",
        filemanager_title: "Filemanager",
        external_plugins: {
            "filemanager": "/filemanager/plugin.min.js"
        }
    });
}

function InitTinymceMinControlWithHeight2(divId, height) {
    try {
        tinymce.remove('#' + divId)

    } catch (e) {

    }

    tinymce.init({
        selector: `#${divId}`,
        height: height,
        menubar: false,
        branding: false,
        plugins: [
            'lists code'
        ],
        contextmenu: "paste",
        //toolbar: '| bold italic underline strikethrough | bullist numlist outdent indent | code',
        toolbar: '| bold italic underline strikethrough | bullist numlist outdent indent | code',
        quickbars_selection_toolbar: ' | bold italic underline strikethrough |  formatselect  backcolor forecolor | alignleft aligncenter alignright alignjustify  |',
        image_advtab: true,
        quickbars_insert_toolbar: false,
        external_filemanager_path: "/filemanager/",
        filemanager_title: "Filemanager",
        external_plugins: {
            "filemanager": "/filemanager/plugin.min.js"
        }
    });
}
