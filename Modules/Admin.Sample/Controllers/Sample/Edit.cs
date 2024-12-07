
using Admin.Sample.Constants;
using Admin.Sample.Database;
using Admin.Sample.Models;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Mozilla; 
using Steam.Core.Base.Constant;
using Steam.Core.Base.Models;
using Steam.Core.Common.SteamString;
using Steam.Core.LocalizeManagement.ExtensionMethod;
using Steam.Core.Utilities.SteamModels;

using Config = Admin.Sample.Constants.SampleConstants.Config.Admin;

namespace Admin.Sample.Controllers
{

    public partial class SampleController
    {
        public class EditModel
        {
            public PageTitleModel PageTitle = new PageTitleModel("Sample", "Thông tin", "fas fa-layer-group", "/Sample");

            public SampleDetailDTO Detail = new SampleDetailDTO();
            public List<Database.Sample_Files> ListFiles = new List<Database.Sample_Files>();

            public string LangCode { get; set; } = DefaultConfig.DEFAULT_LANGUAGE;


        }
        EditModel _editModel = new EditModel();

        [HttpGet("Sample/Edit/{id?}")]
        public IActionResult Edit(int id, string langCode)
        {
            _editModel.LangCode = langCode ?? DefaultConfig.DEFAULT_LANGUAGE;
            //return View();
            if (id != 0)
            {
                var defaultData = _repoSampleDetail.GetById(id);

                if (!String.IsNullOrEmpty(langCode) && langCode != DefaultConfig.DEFAULT_LANGUAGE)
                {
                    defaultData = _repoSampleDetail.GetById(id).GetLocalize<SampleDetail>(id, langCode);

                }
                var data = _mapper.Map<SampleDetailDTO>(defaultData);
                if (data != null)
                {
                    _editModel.Detail = data;
                    _editModel.ListFiles = _repoSampleFile.GetListByKey(data.Pid, "SampleId");
                }
            }

            return View(_editModel);
        }

        [HttpPost]
        public ActionResult Save(SampleModelEdit input)
        {
            Response<long> response = new Response<long>();
            input.CreateUser = CreateUser;
            input.UpdateUser = CreateUser;
            #region validate
            var validate = input.ValiDate();
            if (!validate.isValidate)
            {
                response.Message = validate.Message;
                return response.BadRequest();

            }
            #endregion
            var checkSave = _rep.Save(input);
            if (checkSave > 0)
            {
                response.Data = checkSave;



                return response.Ok();

            }
            else
            {
                return response.NotFound();

            }


        }
    }
}
