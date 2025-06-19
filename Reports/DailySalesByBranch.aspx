<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DailySalesByBranch.aspx.cs" Inherits="BigMac.Reports.DailySalesByBranch" %>

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
            
            
            
            
            
            
            
            
            
            SelectCommand="select @citidesc citidesc, @bonusdesc bonusdesc, @bcode pbcode,@uname uname,CONVERT(DATE_FORMAT(inv.createdate,'%d/%m/%Y'),CHAR(50)) createdate, 
            inv.resourcecode, inv.branchcode, inv.cocode, inv.createid, inv.cussupid, inv.type salestype,cust.cussupname, cust.inhousecode, 
            ifnull(payment.amountrecd,0) salesamt, items.awarddollars,items.awardbonus,STR_TO_DATE(@fdate,'%d/%m/%Y') fdate,STR_TO_DATE(@tdate,'%d/%m/%Y') tdate, 
            refno posRefNo,ifnull(payment.paymentmode,'') paymentmode from
             (select * from Invoice_m_Invoice where cast(createdate as date) between STR_TO_DATE(@fdate,'%d/%m/%Y') and STR_TO_DATE(@tdate,'%d/%m/%Y') 
            and (@bcode like 'ALL%' or branchcode = @bcode) and ifnull(status,'') = 'CLOSE' ) inv 
            inner join (SELECT Invoiceid,SUM(UnitPrice*Qty) salesamt,SUM(awarddollars) awarddollars,SUM(awardbonus) awardbonus
            FROM Invoice_m_Invoice_Items  GROUP BY Invoiceid) items on inv.id = items.invoiceid and (inv.type = 'TopUP' or inv.type='POS') 
            inner join CusSup_m_CusSup cust on inv.cussupid = cust.id 
            inner join Configuration_m_Branches branch on inv.branchcode = branch.branchcode 
            left outer join invoice_m_payment payment on inv.id=payment.invoiceid order by ifnull(payment.paymentmode,''),STR_TO_DATE(inv.createdate,'%d/%m/%Y'),inv.branchcode"  
            
            
            
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
