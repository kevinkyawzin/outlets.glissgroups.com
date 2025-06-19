<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MonthlyStockBalance.aspx.cs" Inherits="BigMac.Reports.MonthlyStockBalance" %>

<%@ Register Assembly="Stimulsoft.Report.Web, Version=2008.1.206.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a"
    Namespace="Stimulsoft.Report.Web" TagPrefix="cc3" %>

<%@ Register Assembly="Stimulsoft.Report.Web" Namespace="Stimulsoft.Report.Web" TagPrefix="cc2" %>
<%@ Register Assembly="Stimulsoft.Report.Web" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"><html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="height: 458px">
        <cc1:stireportweb id="rwModuleHC" runat="server" reportdatasources="sdsModuleHC"
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            reportsource="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"></cc1:stireportweb>
        <asp:SqlDataSource ID="sdsModuleHC" runat="server" ConnectionString="<%$ ConnectionStrings:BigMacEntities %>"
                       SelectCommand="select mov.branchcode, mov.productid, mov.productcode, mov.productdesc,mov.uom , mov.productbalance, mov.createdate  from (select * from stock_m_stockmovement where id in (select max(id) from stock_m_stockmovement where ((year(createdate) < @stkYr) or ((year(createdate) = @stkYr)  and month(createdate)<=@stkMth)) and (@bcode like 'ALL%' or branchcode = @bcode) group by branchcode,productcode))mov order by mov.productcode;"  
            
            ProviderName="<%$ ConnectionStrings:BigMacEntities.ProviderName %>">
           <SelectParameters>
                <asp:QueryStringParameter Name="uname" QueryStringField="uname" 
                    DefaultValue="Admin" />
            </SelectParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="stkYr" QueryStringField="stkYr" 
                    DefaultValue="0" />
            </SelectParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="stkMth" QueryStringField="stkMth" 
                    DefaultValue="0" />
            </SelectParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="bcode" QueryStringField="bcode" 
                    DefaultValue="ALL" />
            </SelectParameters>
        </asp:SqlDataSource>
        <cc3:stiwebviewer id="wvModuleHC" runat="server"></cc3:stiwebviewer>
    </div>
    </form>
</body>
</html>
