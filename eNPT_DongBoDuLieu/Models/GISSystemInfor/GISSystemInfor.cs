using System;
using System.Collections.Generic;
using System.Text;

namespace eNPT_DongBoDuLieu.Models.GISSystemInfor
{
    public class GISSystemInfor
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public ArcGISRESTServicesDirectory ArcGISRESTServicesDirectory { get; set; }
        public ArcGISPortalDirectory ArcGISPortalDirectory { get; set; }
    }

    public class ArcGISRESTServicesDirectory
    {
        public string DuongDayFeatureServer { get; set; }
        public string TramBienApFeatureServer { get; set; }
        public string CotFeatureServer { get; set; }
    }

    public class ArcGISPortalDirectory
    {
        public string RootPath { get; set; }
        public string GenerateToken { get; set; }
    }
}
