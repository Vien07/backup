//using Admin.ProductPolicy.Models;
using Admin.ProductManagement.Constants;
using Admin.ProductManagement.DataTransferObjects.ProductPolicy;
using Admin.ProductManagement.Models;
using Admin.ProductManagement.Models.SaveModels;
using Admin.ProductManagement.Models.SearchModels;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.Utilities.STeamHelper;
using System.Collections.Generic;
using X.PagedList;

namespace Admin.ProductManagement.Controllers
{

    public partial class ProductPolicyController
    {
        public class EditModel
        {
            public PageTitleModel PageTitle = new PageTitleModel("ProductPolicy", "Thông tin", "fas fa-layer-group", "/ProductPolicy");

            public ProductPolicyDto Detail = new();
            public List<SelectControlData> ListPolicies = new List<SelectControlData>();
            public string ProductPolicies { get; set; }

            public void InitListProductPolicyData()
            {
                try
                {
                    var listTemp = this.ProductPolicies.Trim().Split(';');
                    foreach (var item in listTemp)
                    {
                        var tempType = item.Split(":");
                        if (tempType.Count() == 2)
                        {
                            this.ListPolicies.Add(new SelectControlData() { Name = tempType[0], Value = tempType[1], Order = 0 });
                        }
                    }
                }
                catch (Exception)
                {
                    this.ListPolicies = new List<SelectControlData>();
                }
            }
        }

        EditModel _editModel = new EditModel();

        [HttpGet("ProductPolicy/Edit/{id?}")]
        public IActionResult Edit(int id)
        {
            var config = _productPolicyService.GetAllConfigs().Data.Items;
            _editModel.ProductPolicies = config.Where(p => p.Key == ProductPolicyConstants.Config.Admin.GroupPolicy).FirstOrDefault().Value;
            _editModel.InitListProductPolicyData();

            if (id != 0)
            {
                var data = _productPolicyService.GetById(id).Data;
                if (data != null)
                {
                    _editModel.Detail = data.Detail;
                }
            }
            return View(_editModel);
        }

        [HttpPost]
        public ActionResult Save(ProductPolicySaveModel input)
        {
            input.CreateUser = CreateUser;
            input.UpdateUser = CreateUser;
            _pageModel.Search = new ProductPolicySearchModel();
            var res = _productPolicyService.Save(input);
            return new JsonResult(new { response = res });
        }
    }
}
