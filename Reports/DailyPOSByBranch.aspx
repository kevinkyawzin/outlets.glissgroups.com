<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DailyPOSByBranch.aspx.cs" Inherits="BigMac.Reports.DailyPOSByBranch" %>

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
            
            
            
            
            
            
            
            
            
            SelectCommand="select @citidesc citidesc, @bonusdesc bonusdesc, @bcode pbcode,@uname uname,inv.createdate, inv.resourcecode, inv.branchcode, inv.cocode, inv.createid, inv.cussupid, cust.cussupname, cust.inhousecode, items.UnitPrice, items.qty , awarddollars,awardbonus,STR_TO_DATE(@fdate,'%d/%m/%Y') fdate,STR_TO_DATE(@tdate,'%d/%m/%Y') tdate, refno posRefNo from (select * from Invoice_m_Invoice where cast(createdate as date) between STR_TO_DATE(@fdate,'%d/%m/%Y') and STR_TO_DATE(@tdate,'%d/%m/%Y') and (@bcode like 'ALL%' or branchcode = @bcode) and ifnull(status,'') = 'CLOSE' ) inv inner join Invoice_m_Invoice_Items items on inv.id = items.invoiceid and type = 'POS' inner join CusSup_m_CusSup cust on inv.cussupid = cust.id inner join Configuration_m_Branches branch on inv.branchcode = branch.branchcode"  
            
            
            
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