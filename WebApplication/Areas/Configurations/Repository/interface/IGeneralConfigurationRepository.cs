using CMS.Areas.Configurations.Models;
using DTO.Common;
using System.Collections.Generic;

namespace CMS.Areas.Configurations
{
    public interface IGeneralConfigurationRepository
    {
        dynamic Update(Configuration data, int type);
        dynamic Update(Configuration data);
        string GetList();

        #region
        dynamic LoadData(SearchDto search);
        string LoadContactData(string lang);
        bool Enable(long[] Pid, bool active);
        bool Delete(int Pid);
        dynamic Delete(long[] Pid);
        dynamic Edit(int Pid);
        dynamic Insert(Models.EmailTemplate newsDetail, List<MultiLang_EmailTemplate> multiLangEmailTemplate);
        dynamic Update(Models.EmailTemplate newsDetail,
            List<MultiLang_EmailTemplate> multiLangEmailTemplateDetail);
        bool Count(int code);
        string Search(SearchDto searchData);
        dynamic Validation(dynamic data);
        #endregion
    }
}