<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DailySalesClosingDetails.aspx.cs" Inherits="BigMac.Reports.DailySalesClosingDetails" %>

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
            
            
            
            
            
            
            
            
            
            SelectCommand="select @citidesc citidesc, @bonusdesc bonusdesc, @uname uname,pyt.branchcode, pyt.paymentmode,pyt.customername, pyt.totalamt,pyt.createdate from (select sum(amountrecd) totalamt,invoice_m_payment.branchcode,invoice_m_payment.paymentmode,invoice_m_invoice.cussupname customername,invoice_m_payment.createdate from invoice_m_payment inner join invoice_m_invoice on invoice_m_payment.invoiceid=invoice_m_invoice.id where year(invoice_m_payment.createdate) = year(now()) and month(invoice_m_payment.createdate) = month(now()) and  day(invoice_m_payment.createdate) = day(now()) group by invoice_m_payment.branchcode,invoice_m_payment.paymentmode,invoice_m_invoice.cussupname,invoice_m_payment.createdate ) pyt"  
            
            
            
            ProviderName="<%$ ConnectionStrings:BigMacEntities.ProviderName %>">
           <SelectParameters>
                <asp:QueryStringParameter Name="uname" QueryStringField="uname" 
                    DefaultValue="Admin" />
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