using eNPT_DongBoDuLieu.Models.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eNPT_DongBoDuLieu.Services.DataBases
{
    public interface IDataBaseServices
    {
        Task RemoveAndInsertFullTextSearch(ELoaiDT loaiDT, string strFeatures);
    }
}
