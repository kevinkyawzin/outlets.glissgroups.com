<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DailyStockCollectionByBranch.aspx.cs" Inherits="BigMac.Reports.DailyStockCollectionByBranch" %>

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
                       SelectCommand="select @citidesc citidesc, @bonusdesc bonusdesc,mov.branchcode, mov.productid, mov.productcode, mov.productdesc, mov.qty, mov.uom ,mov.unitprice,mov.currency, mov.lineamount,STR_TO_DATE(@fdate,'%d/%m/%Y') fdate,STR_TO_DATE(@tdate,'%d/%m/%Y') tdate  from (select * from stock_m_stockmovement where Qty>0 and cast(createdate as date) between STR_TO_DATE(@fdate,'%d/%m/%Y') and STR_TO_DATE(@tdate,'%d/%m/%Y') and (@bcode like 'ALL%' or branchcode = @bcode)) mov order by mov.createdate "  
            
            ProviderName="<%$ ConnectionStrings:BigMacEntities.ProviderName %>">
           <SelectParameters>
                <asp:QueryStringParameter Name="uname" QueryStringField="uname" 
                    DefaultValue="Admin" />
            </SelectParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="fdate" QueryStringField="fdate" 
                    DefaultValue="01/02/2014" />
            </SelectParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="tdate" QueryStringField="tdate" 
                    DefaultValue="31/03/2014" />
            </SelectParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="bcode" QueryStringField="bcode" 
                    DefaultValue="ALL" />
            </SelectParameters>
           <SelectParameters>
                <asp:QueryStringParameter Name="citidesc" QueryStringField="citidesc" 
                    DefaultValue="Citip" />
            </SelectParameters>
           <SelectParameters>
                <asp:QueryStringParameter Name="bonusdesc" QueryStringField="bonusdesc" 
                    DefaultValue="Bonusp" />
            </SelectParameters>
        </asp:SqlDataSource>
        <cc3:stiwebviewer id="wvModuleHC" runat="server"></cc3:stiwebviewer>
    </div>
    </form>
</body>
</html>