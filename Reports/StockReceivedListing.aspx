<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StockReceivedListing.aspx.cs" Inherits="BigMac.Reports.StockReceivedListing" %>

<%@ Register Assembly="Stimulsoft.Report.Web, Version=2008.1.206.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a"
    Namespace="Stimulsoft.Report.Web" TagPrefix="cc3" %>

<%@ Register Assembly="Stimulsoft.Report.Web" Namespace="Stimulsoft.Report.Web" TagPrefix="cc2" %>
<%@ Register Assembly="Stimulsoft.Report.Web" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"><html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="height: 458px">
        <cc1:stireportweb id="rwModuleHC" runat="server" reportdatasources="sdsModuleHC"
            reportsource="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"></cc1:stireportweb>

        <asp:SqlDataSource ID="sdsModuleHC" runat="server" ConnectionString="<%$ ConnectionStrings:BigMacEntities %>"
            SelectCommand="getStockReceivedListingByID"  
            ProviderName="<%$ ConnectionStrings:BigMacEntities.ProviderName %>" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:QueryStringParameter DefaultValue="200" Name="srid" QueryStringField="srid" />
            </SelectParameters>

        </asp:SqlDataSource>

        <cc3:stiwebviewer id="wvModuleHC" runat="server"></cc3:stiwebviewer>
    
    </div>
    </form>
</body>
</html>
