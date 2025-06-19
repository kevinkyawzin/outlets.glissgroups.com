<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VoidTransactionList.aspx.cs" Inherits="BigMac.Reports.VoidTransactionList" %>

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
            
            
            
            
            
            
            
            
            
            SelectCommand="select  @bcode pbcode, @uname uname, @citidesc citidesc, @bonusdesc bonusdesc,inv.type invtype, inv.createdate, inv.resourcecode, inv.branchcode, inv.cocode, inv.createid, inv.cussupid, cust.cussupname,cust.inhousecode, items.UnitPrice, items.qty , (items.qty * redeemdollars) redeemdollars, (items.qty * redeembonus) redeembonus, (items.qty * awarddollars) awarddollars, (items.qty * awardbonus) awardbonus,  STR_TO_DATE(@fdate,'%d/%m/%Y') fdate,STR_TO_DATE(@tdate,'%d/%m/%Y') tdate,items.productcode,items.productdesc,staffserviceid,staff.name,ifnull(redemc.balance,-999) citibal,ifnull(redemb.balance,-999) bonusbal from (select * from Invoice_m_Invoice where status = 'VOID' and cast(createdate as date) between STR_TO_DATE(@fdate,'%d/%m/%Y') and STR_TO_DATE(@tdate,'%d/%m/%Y') and (@bcode like 'ALL%' or branchcode = @bcode) and (@invtype like 'ALL%' or Invoice_m_Invoice.type = @invtype) )inv  inner join Invoice_m_Invoice_Items items on inv.id = items.invoiceid inner join CusSup_m_CusSup cust on inv.cussupid = cust.id left outer join CusSup_m_CusRedemption redemc on inv.resourcecode = redemc.RefNo and redemc.productid = items.productid and redemc.redemptiontype = 'C$'and (items.id = redemc.invoiceitemid or redemc.invoiceitemid =0) left outer join CusSup_m_CusRedemption redemb on inv.resourcecode = redemb.RefNo and redemb.productid = items.productid and redemb.redemptiontype = 'B$' and (items.id = redemb.invoiceitemid or  redemb.invoiceitemid =0) left outer join Common_m_Staff staff on items.staffserviceid = staff.id order by inv.createdate;"  
            
            
            
            ProviderName="<%$ ConnectionStrings:BigMacEntities.ProviderName %>">
           <SelectParameters>
                <asp:QueryStringParameter Name="uname" QueryStringField="uname" 
                    DefaultValue="Admin" />
            </SelectParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="fdate" QueryStringField="fdate" 
                    DefaultValue="02/01/2014" />
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