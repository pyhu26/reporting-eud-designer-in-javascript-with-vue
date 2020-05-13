using System.Collections.Generic;
using System.IO;
using System.Linq;
using DevExpress.XtraReports.Configuration;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Web.Extensions;

namespace ServerSide {
    public class MyReportStorage : ReportStorageWebExtension {
        public Dictionary<string, XtraReport> Reports = new Dictionary<string, XtraReport>();

        public MyReportStorage()
        {
            Reports.Add("Products", new XtraReport1());
            Reports.Add("Categories", new XtraReport2());

            //Set path to save
            string path = @"C:\UserReports\";
            if (Directory.Exists(path) == false)
                Directory.CreateDirectory(path); //directory is being created ok    

            Settings.Default.StorageOptions.RootDirectory = path;

            LoadLocalFileToReport(path);

        }

        private void LoadLocalFileToReport(string path)
        {
            //해당 폴더가 존재하는지 확인
            if (System.IO.Directory.Exists(path))
            {
                //DirectoryInfo 객체 생성
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(path);

                //해당 폴더에 있는 파일이름을 출력
                foreach (var item in di.GetFiles())
                {
                    Reports.Add(item.Name, XtraReport.FromXmlFile(item.FullName));
                }
            }
        }

        public override bool CanSetData(string url) {
            return true;
        }
        public override byte[] GetData(string url) {
            var report = Reports[url];
            using(MemoryStream stream = new MemoryStream()) {
                report.SaveLayoutToXml(stream);
                return stream.ToArray();
            }
        }
        public override Dictionary<string, string> GetUrls() {
            return Reports.ToDictionary(x => x.Key, y => y.Key);
        }
        public override void SetData(XtraReport report, string url)
        {
            if (Reports.ContainsKey(url))
            {
                Reports[url] = report;
            }
            else
            {
                Reports.Add(url, report);
            }

            SaveReportToLocalFile(report);
        }

        private static void SaveReportToLocalFile(XtraReport report)
        {
            report.SaveLayoutToXml(
                            Settings.Default.StorageOptions.RootDirectory +
                            report.Name + ".repx"); ;
        }

        public override string SetNewData(XtraReport report, string defaultUrl) {
            SetData(report, defaultUrl);
            return defaultUrl;
        }
        public override bool IsValidUrl(string url) {
            return true;
        }
    }
}