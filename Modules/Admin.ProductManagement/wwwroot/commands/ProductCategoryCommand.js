class AdminMisaProductCategory {
    constructor(model) {
        this._modelView = model;
        this.divModalDataID = model.DivModalDataID;
        this.ModalDataID = model.ModalDataID;
        this.divModalConfigID = model.DivModalConfigID;
        this.ModalConfigID = model.ModalConfigID;
        this.tableDataID = model.TableDataID;
        this.tbodyID = model.TbodyId;
        this.fromUploadID = model.FromUploadID;
        this.NavPagingID = model.NavPagingID;
        this.FileUploadID = model.FileUploadID;
        this.RouteIndex =  model.RouteIndex
        this.SearchFormID = model.SearchFormID
        this.SearchFormID = model.SearchFormID
        this._commonAdmin = new CommonAdmin()
        this.Controller = SteamSystem.VirtualFolder + "/ProductCategory/"
    }
}