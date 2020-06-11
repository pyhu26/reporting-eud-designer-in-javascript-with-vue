<template>
<!-- designer mode -->
<div ref="designer" style="position: absolute; left: 0; right: 0; top: 0; bottom: 0" data-bind="dxReportDesigner: $data"></div>
<!-- viewer mode -->
<!-- <div ref="designer" style="position: absolute; left: 0; right: 0; top: 0; bottom: 0" data-bind="dxReportViewer: $data"></div> -->
</template>

<script>
import ko from "knockout";
// designer mode
import "devexpress-reporting/dx-reportdesigner";
// viewer mode
import "devexpress-reporting/dx-webdocumentviewer";

export default {
// name: "ReportDesignerComponent",
name: "WebDocumentViewer",

mounted() {
    var designerOptions = {
        reportUrl: ko.observable("Products"), // The URL of a report that is opened in the Report Designer when the application starts.
        requestOptions: {
        host: "http://192.168.61.239:54115/",        
        // Use this line for the ASP.NET MVC backend.
         getDesignerModelAction: "ReportDesigner/GetReportDesignerModel"
        
        // Uncomment this line if you use an ASP.NET Core backend.
        // test git        
        // invokeAction: "WebDocumentViewer/Invoke"
            }
    };
    ko.applyBindings(designerOptions, this.$refs.designer);
},
beforeDestroy() {
    ko.cleanNode(this.$refs.designer);
}
};
</script>

