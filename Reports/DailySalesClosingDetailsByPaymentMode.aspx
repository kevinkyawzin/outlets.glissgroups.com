<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DailySalesClosingDetailsByPaymentMode.aspx.cs" Inherits="BigMac.Reports.DailySalesClosingDetailsByPaymentMode" %>

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