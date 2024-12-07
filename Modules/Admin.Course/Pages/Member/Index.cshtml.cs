using Admin.Course.Models;
using Common.Helper;
using ComponentUILibrary.Models;
using ComponentUILibrary.ViewComponents;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace Admin.Course.CMS.Pages
{
    public class MemberModel : PageModel
    {
        IViewRendererHelper _viewRender;

        private IMemberRepository _rep;
        private ILoggerHelper _logger;

        private string CreateUser = "admin";

        public IPagedList<Database.Member> List;
        public Database.Member ModelMember;
        public ParamSearch search;
        public PaginationModel _PageModel = new PaginationModel();

        public string _PartialUrl = "~/Pages/Member/_Partial/";
        public string _PartialModel = "_PartialModal.cshtml";
        public string _PartialTable = "_PartialTable.cshtml";
        public string _PartialModalConfig = "_PartialModalConfig.cshtml";
        public MemberModel(IMemberRepository rep, IViewRendererHelper viewRender, ILoggerHelper logger)
        {
            _viewRender = viewRender;
            _rep = rep;
            _logger = logger;
        }
        public void OnGet(ParamSearch search)
        {
            ModelMember = new Database.Member();
            _PageModel = new PaginationModel();

            List = _rep.GetList(search).Data;
            _PageModel.PageIndex = search.PageIndex;
            _PageModel.PageCount = List.PageCount;
        }
        public JsonResult OnGetList(ParamSearch search)
        {
            List = _rep.GetList(search).Data;
            return new JsonResult(List);
        }
        public JsonResult OnGetSearch(ParamSearch search)
        {
            List = _rep.GetList(search).Data;
            string list = _viewRender.RenderPartialViewToString(_PartialUrl, _PartialTable, List);


            _PageModel.PageIndex = search.PageIndex;
            _PageModel.PageCount = List.PageCount;
            string paging = _viewRender.RenderPartialViewToString(PaginationComponentInfo.Path, PaginationComponentInfo.Name, _PageModel);
            return new JsonResult(new { list = list, paging = paging });
        }
        public JsonResult OnPostDelete(List<int> ids)
        {
            search = new ParamSearch();
            var res = _rep.Delete(ids);

            var listData = _viewRender.RenderPartialViewToString(_PartialUrl, _PartialTable, _rep.GetList(search).Data);
            return new JsonResult(new { response = res, listData = listData });
        }
        public JsonResult OnPostEnable(List<int> ids, bool isEnable)
        {
            search = new ParamSearch();
            var res = _rep.Enable(ids, isEnable);
            var listData = _viewRender.RenderPartialViewToString(_PartialUrl, _PartialTable, _rep.GetList(search).Data);

            return new JsonResult(new { response = res, listData = listData });
        }
        public JsonResult OnGetMove(int fromId, int toId)
        {
            search = new ParamSearch();

            var res = _rep.Move(fromId, toId);
            var listData = _viewRender.RenderPartialViewToString(_PartialUrl, _PartialTable, _rep.GetList(search).Data);
            return new JsonResult(new { response = res, listData = listData });

        }
        public JsonResult OnGetEnableUpdateOrder()
        {
            search = new ParamSearch();

            var res = _rep.EnableUpdateOrder();
            var listData = _viewRender.RenderPartialViewToString(_PartialUrl, _PartialTable, _rep.GetList(search).Data);
            return new JsonResult(new { response = res, listData = listData });

        }
        public JsonResult OnGetUpdateOrder(int id, double order)
        {
            search = new ParamSearch();

            var res = _rep.UpdateOrder(id, order);
            return new JsonResult(new { response = res });

        }
        public JsonResult OnGetConfig()
        {
            var res = _rep.GetConfig();
            return new JsonResult(new { response = res });
        }
        public JsonResult OnPostConfig(MemberConfigDto config)
        {
            var res = _rep.SaveConfig(config);
            return new JsonResult(new { response = res });
        }
    }
}
