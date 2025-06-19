Imports Stimulsoft.Report
Imports Stimulsoft.Report.Web
Imports System.Data.SqlClient
Imports System.Web.Configuration
Partial Class InvoiceHC
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Report As Stimulsoft.Report.StiReport = rwModuleHC.GetReport()
        Report.ReportName = "Invoice " + Request.QueryString("ModuleID")
        wvModuleHC.Report = Report
        wvModuleHC.PrintToPdf()
    End Sub
End Class
