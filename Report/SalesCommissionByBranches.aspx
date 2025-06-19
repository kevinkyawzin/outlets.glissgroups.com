<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SalesCommissionByBranches.aspx.cs" Inherits="BigMac.Reports.SalesCommissionByBranches" %>

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
            
            
            
            
            
            
            
            
            
            SelectCommand="select a.citidesc, a.bonusdesc, a.pbcode,a.branchcode,a.salestaff,a.uname,a.invoicedate,a.customer,a.productcode,a.productdesc,a.category,a.categorysub,a.qty,a.saleamt,a.servicecommission, a.fdate,a.tdate from (select @citidesc citidesc, @bonusdesc bonusdesc, @bcode pbcode,@invtype invtype,sbr.branchcode branchcode,@uname uname,staff.name salestaff,STR_TO_DATE(inv.createdate,'%d/%m/%Y') invoicedate,inv.cussupname as customer,items.productcode,items.productdesc,product.category,product.categorysub,sum(items.qty) qty,sum(items.lineamount) saleamt,sum(items.servicecommission) servicecommission,STR_TO_DATE(@fdate,'%d/%m/%Y') fdate,STR_TO_DATE(@tdate,'%d/%m/%Y') tdate from invoice_m_invoice inv inner join common_m_staff staff on inv.staffid=staff.id inner join common_m_staffbranch sbr on sbr.staffid=staff.id inner join invoice_m_invoice_Items items on inv.id = items.invoiceid inner join product_m_product product on items.productcode=product.productcode where cast(inv.createdate as date) between STR_TO_DATE(@fdate,'%d/%m/%Y') and STR_TO_DATE(@tdate,'%d/%m/%Y') and (@bcode like 'ALL%' or sbr.branchcode = @bcode) and inv.status<>'VOID' group by staff.name,inv.createdate,inv.cussupname,items.productcode,items.productdesc,STR_TO_DATE(@fdate,'%d/%m/%Y'),STR_TO_DATE(@tdate,'%d/%m/%Y')) a order by a.saleamt desc;"  
            
            
            
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
                    DefaultValue="31/05/2014" />
            </SelectParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="bcode" QueryStringField="bcode" 
                    DefaultValue="ALL" />
            </SelectParameters>            
            <SelectParameters>
                <asp:QueryStringParameter Name="invtype" QueryStringField="invtype" 
                    DefaultValue="ALL%" />
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
