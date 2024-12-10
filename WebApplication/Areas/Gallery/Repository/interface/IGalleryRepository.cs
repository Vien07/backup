using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using CMS.Areas.Gallery.Models;
using System.Collections.Generic;
using DTO.Common;

namespace CMS.Areas.Gallery
{
    public interface IGalleryRepository
    {
        dynamic LoadData(SearchDto search);

        bool Enable(long[] Pid, bool active);
        bool Delete(int Pid);
        dynamic Delete(long[] Pid);
        dynamic Edit(int Pid);
        dynamic Insert(GalleryDetail galleryDetail, List<MultiLang_GalleryDetail> multiLangGalleryDetail,
                           IFormFile Images, List<Temp_Images> listImagesGallery,
                           List<Temp_MultiLang_Images> listLangImagesGallery, string listCates);
        dynamic Update(GalleryDetail galleryDetail,
            List<MultiLang_GalleryDetail> multiLangGalleryDetail,
            IFormFile Images, List<Temp_Images> listDeleteImages, List<Temp_Images> listImagesGallery, List<Temp_MultiLang_Images> listLangImagesGallery, string listCates);
        bool Count(int code);
        string Search(SearchDto searchData);
        bool Coppy(long[] Pid);
        dynamic Validation(dynamic data);
        bool Preview(string obj, string objDetail, IFormFile PicThumb);
        bool MoveRow(long from, long to);
        bool UpdateOrder(long Pid, int order);
        bool SaveStatus(long pid, bool value, string type);

    }
}