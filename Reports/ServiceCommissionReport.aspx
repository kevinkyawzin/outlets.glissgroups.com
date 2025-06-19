<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ServiceCommissionReport.aspx.cs" Inherits="BigMac.Reports.ServiceCommissionReport" %>

<%@ Register Assembly="Stimulsoft.Report.Web, Version=2008.1.206.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a"
    Namespace="Stimulsoft.Report.Web" TagPrefix="cc3" %>

<%@ Register Assembly="Stimulsoft.Report.Web" Namespace="Stimulsoft.Report.Web" TagPrefix="cc2" %>
<%@ Register Assembly="Stimulsoft.Report.Web" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Service Commission Report</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="height: 458px">
        <cc1:stireportweb id="rwModuleHC" runat="server" reportdatasources="sdsModuleHC" ReportSource="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"></cc1:stireportweb>

        
        <asp:SqlDataSource ID="sdsModuleHC" runat="server" ConnectionString="<%$ ConnectionStrings:BigMacEntities %>"
            SelectCommand="pro_service_commission"  
            ProviderName="<%$ ConnectionStrings:BigMacEntities.ProviderName %>" SelectCommandType="StoredProcedure">
            
            <FilterParameters>
                <asp:QueryStringParameter DefaultValue="Admin" Name="uname" QueryStringField="uname" />
                <asp:QueryStringParameter DefaultValue="ALL" Name="bcode" QueryStringField="bcode" />
                <asp:QueryStringParameter DefaultValue="01/01/2014" Name="fdate" QueryStringField="fdate" />
                <asp:QueryStringParameter DefaultValue="04/04/2014" Name="tdate" QueryStringField="tdate" />
            </FilterParameters>
            


            <SelectParameters>
                <asp:QueryStringParameter DefaultValue="Admin" Name="uname" QueryStringField="uname" Type="String" />
                <asp:QueryStringParameter DefaultValue="ALL" Name="bcode" QueryStringField="bcode" Type="String" />
                <asp:QueryStringParameter DefaultValue="01/01/2012" Name="fdate" QueryStringField="fdate" Type="String" />
                <asp:QueryStringParameter DefaultValue="01/01/2015" Name="tdate" QueryStringField="tdate" Type="String" />
            </SelectParameters>
            


        </asp:SqlDataSource>

        <cc3:stiwebviewer id="wvModuleHC" runat="server"></cc3:stiwebviewer>    
    </div>
    </form>
</body>
</html>
