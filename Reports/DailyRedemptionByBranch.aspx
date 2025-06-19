<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DailyRedemptionByBranch.aspx.cs" Inherits="BigMac.Reports.DailyRedemptionByBranch" %>

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
            
            
            
            
            
            
            
            
            
            SelectCommand="SELECT  @citidesc citidesc,@bonusdesc bonusdesc, @bcode pbcode,
    @uname uname,inv.createdate, items.id itemid,
     items.createdate itemcreatedate,
      inv.resourcecode, inv.branchcode,
       inv.cocode, inv.createid, inv.cussupid,
        cust.cussupname, cust.inhousecode,
         items.UnitPrice, items.qty ,
          (items.qty * redeemdollars) redeemdollars,(items.qty* redeembonus) redeembonus,
           (items.qty * awarddollars) awarddollars, (items.qty * awardbonus) awardbonus,
             STR_TO_DATE(@fdate,'%d/%m/%Y') fdate,STR_TO_DATE(@tdate,'%d/%m/%Y')
              tdate,items.productcode,items.productdesc, (redemc.balance - redemc.credit + redemc.debit) obc,(redemb.balance - redemb.credit + redemb.debit) obb, redemc.balance cbc,redemb.balance cbb,staffserviceid,staff.name 
              FROM (SELECT * FROM Invoice_m_Invoice WHERE CAST(createdate AS DATE) BETWEEN STR_TO_DATE(@fdate,'%d/%m/%Y') AND STR_TO_DATE(@tdate,'%d/%m/%Y')
              AND (@bcode LIKE 'ALL%' OR branchcode = @bcode)
              AND (@cocode LIKE 'ALL%' OR cocode = @cocode)
              AND IFNULL(STATUS,'') = 'CLOSE' )inv
              INNER JOIN Invoice_m_Invoice_Items items
              ON inv.id = items.invoiceid AND TYPE = 'Redeem' 
              INNER JOIN CusSup_m_CusSup cust ON inv.cussupid = cust.id 
              INNER JOIN Configuration_m_Branches branch ON inv.branchcode = branch.branchcode 
              LEFT OUTER JOIN CusSup_m_CusRedemption redemc ON inv.resourcecode = redemc.RefNo AND redemc.productid = items.productid AND redemc.redemptiontype = 'C$'  AND (items.id = redemc.invoiceitemid OR  redemc.invoiceitemid =0) LEFT OUTER JOIN CusSup_m_CusRedemption redemb ON inv.resourcecode = redemb.RefNo AND redemb.productid = items.productid AND redemb.redemptiontype = 'B$' AND (items.id = redemb.invoiceitemid OR  redemb.invoiceitemid =0) LEFT OUTER JOIN Common_m_Staff staff ON items.staffserviceid = staff.id"  
            
            
            
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
                <asp:QueryStringParameter Name="citidesc" QueryStringField="citidesc" 
                    DefaultValue="Citip" />
            </SelectParameters>
           <SelectParameters>
                <asp:QueryStringParameter Name="bonusdesc" QueryStringField="bonusdesc" 
                    DefaultValue="Bonusp" />
            </SelectParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="cocode" QueryStringField="cocode" 
                    DefaultValue="ALL" />
            </SelectParameters>
        </asp:SqlDataSource>

        <cc3:stiwebviewer id="wvModuleHC" runat="server"></cc3:stiwebviewer>
    
    </div>
    </form>
</body>
</html>
