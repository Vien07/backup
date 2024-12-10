using CMS.Areas.Feature.Models;
using DTO.Common;
using System.Collections.Generic;

namespace CMS.Areas.Feature
{
    public interface IFeatureCateRepository
    {
        dynamic LoadData(SearchDto search);

        bool Enable(long[] Pid, bool active);
        dynamic Delete(int Pid);
        dynamic DeleteAll(int Pid);
        dynamic Delete(long[] Pid);
        dynamic Edit(long Pid);
        dynamic Insert(FeatureCate featureCate, List<MultiLang_FeatureCate> multiLang_featureCate);
        dynamic Update(FeatureCate featureCate, List<MultiLang_FeatureCate> multiLang_featureCate);
        bool Count(int code);
        string Search(SearchDto searchData);
        bool Coppy(long[] Pid);
        dynamic Validation(dynamic data);
        bool Up(long Pid);
        bool Down(long Pid);
        bool MoveRow(long from, long to);
        bool UpdateOrder(long Pid, int order);
    }
}