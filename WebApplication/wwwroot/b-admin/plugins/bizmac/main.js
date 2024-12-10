class BizmacCore {
    constructor(moduleName) {
        this.moduleName = moduleName;
        this.input = [];
        this.lang = lang;
        this.actionUrl = `/b-admin/${this.moduleName}/`;
    }

    get moduleName() {
        return this.moduleName;
    }

    get currentLanguage() {
        return this.currentLanguage;
    }

    setLanguage(langKey) {
        this.lang = langKey;
    }

    setInput(div, type) {
        this.input.push({ id: div, value: type });
    }

    initialInput() {
        for (const item of this.input) {
            switch (item.value) {
                case 'editor':
                    this.initialEditor(item.id).then(() => {

                    });
                    break;
                case 'money':
                    break;
                default:
                    break;
            }
        }
    }

    getValues() {

    }

    initialEditor(divId) {
        return new Promise((resolve, reject) => {
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
            resolve(true)
        })

    }

    insert() {

    }

    update() {

    }

    edit() {

    }

    delete() {

    }

    deleteMulti() {

    }

    enable() {

    }

    enableMulti() {

    }

    search() {

    }

    loadData() {

    }

    moveRow() {

    }

}

const core = new BizmacCore("News", "abc", "cde")

console.log(core)