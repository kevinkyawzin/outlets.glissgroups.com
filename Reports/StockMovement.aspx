<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StockMovement.aspx.cs" Inherits="BigMac.Reports.StockMovement" %>

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
            
            
            
            
            
            
            
            
            
            SelectCommand="select mov1.id mov1id,  mov1.createdate mov1createdate, mov1.stockmoduletype mov1stockmoduletype, mov1.branchcode, mov1.productid, p.productcode, p.desc, mov1.resourcecode mov1resourcecode, mov1.qty mov1qty, mov1.uom mov1uom, mov1.productbalance mov1lastbal, mov1.productbalance mov1prodbal, mov2.id mov2id, mov2.stockmoduletype mov2stockmoduletype,mov2.resourcecode mov2resourcecode, mov2.createdate mov2createdate, mov2.qty mov2qty, mov2.uom mov2uom, mov2.productbalance mov2lastbal, mov2.productbalance mov2prodbal,(select productbalance from stock_m_stockmovement where productid = mov1.productid and branchcode = mov1.branchcode  order by id desc limit 1) branchbal, getProductBalance(mov1.productid,mov1.uom) prdbal from (select * from stock_m_stockmovement where stockrefid =0 and (productid = @prdid or @prdid=0) and (@zflag=0 or lastbalance != 0)) mov1 left outer join (select * from stock_m_stockmovement where stockrefid != 0 and (productid = @prdid or @prdid=0)) mov2 on mov1.id = mov2.stockrefid and mov1.productid = mov2.productid and mov1.branchcode = mov2.branchcode and mov1.uom = mov2.uom inner join product_m_product p on mov1.productid = p.id;"  
            
            
            
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
                <asp:QueryStringParameter Name="prdid" QueryStringField="prdid" 
                    DefaultValue="0" />
            </SelectParameters>
           <SelectParameters>
                <asp:QueryStringParameter Name="zflag" QueryStringField="zflag" 
                    DefaultValue="0" />
            </SelectParameters>
<%--           <SelectParameters>
                <asp:QueryStringParameter Name="citidesc" QueryStringField="citidesc" 
                    DefaultValue="Citip" />
            </SelectParameters>
           <SelectParameters>
                <asp:QueryStringParameter Name="bonusdesc" QueryStringField="bonusdesc" 
                    DefaultValue="Bonusp" />
            </SelectParameters>--%>
        </asp:SqlDataSource>

        <cc3:stiwebviewer id="wvModuleHC" runat="server"></cc3:stiwebviewer>
    
    </div>
    </form>
</body>
</html>