﻿using DevExpress.DataAccess.Sql;
using DevExpress.Web.Mvc.Controllers;
using DevExpress.XtraReports.Web.ClientControls;
using DevExpress.XtraReports.Web.ReportDesigner;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ServerSide.Controllers
{
    public class ReportDesignerController : ReportDesignerApiController
    {
        public ClientSideModelSettings ClientSideModeSetting { get; private set; }

        public override ActionResult Invoke() {
            var result = base.Invoke();
            // Allow cross-domain requests.
            Response.AppendHeader("Access-Control-Allow-Origin", "*");
            return result;
        }

        public ActionResult GetReportDesignerModel(string reportUrl)
        {
            Response.AppendHeader("Access-Control-Allow-Origin", "*");

            string modelJsonScript =
                new ReportDesignerClientSideModelGenerator()
                .GetJsonModelScript(
                    reportUrl,                 // The URL of a report that is opened in the Report Designer when the application starts.
                    GetAvailableDataSources(), // Available data sources in the Report Designer that can be added to reports.
                    "ReportDesigner/Invoke",   // The URI path of the controller action that processes requests from the Report Designer.
                    "WebDocumentViewer/Invoke",// The URI path of the controller action that processes requests from the Web Document Viewer.
                    "QueryBuilder/Invoke"      // The URI path of the controller action that processes requests from the Query Builder.
                );
            return Content(modelJsonScript, "application/json");
        }

        Dictionary<string, object> GetAvailableDataSources()
        {
            var dataSources = new Dictionary<string, object>();
            SqlDataSource ds = new SqlDataSource("BS_PMS");
            var query = SelectQueryFluentBuilder.AddTable("V_WEIGHT_DATA_11_H").SelectAllColumns().Build("V_WEIGHT_DATA_11_H");
            ds.Queries.Add(query);
            ds.RebuildResultSchema();
            dataSources.Add("SqlDataSource", ds);
            return dataSources;
        }
    }
}