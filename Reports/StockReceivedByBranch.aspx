<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StockReceivedByBranch.aspx.cs" Inherits="BigMac.Reports.StockReceivedByBranch" %>

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
            SelectCommand="StockReceivedByBranch"  
            ProviderName="<%$ ConnectionStrings:BigMacEntities.ProviderName %>" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:QueryStringParameter DefaultValue="11/11/2000" Name="fdate" QueryStringField="fdate" />
                <asp:QueryStringParameter DefaultValue="11/11/2014" Name="tdate" QueryStringField="tdate" />
                <asp:QueryStringParameter DefaultValue="CCKSPA" Name="bcode" QueryStringField="bcode" />
            </SelectParameters>
        </asp:SqlDataSource>

        <cc3:stiwebviewer id="wvModuleHC" runat="server"></cc3:stiwebviewer>    
    </div>
    </form>
</body>
</html>