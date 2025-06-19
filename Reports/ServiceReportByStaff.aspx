<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ServiceReportByStaff.aspx.cs" Inherits="BigMac.Reports.ServiceReportByStaff" %>

<%@ Register Assembly="Stimulsoft.Report.Web, Version=2008.1.206.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a"
    Namespace="Stimulsoft.Report.Web" TagPrefix="cc3" %>

<%@ Register Assembly="Stimulsoft.Report.Web" Namespace="Stimulsoft.Report.Web" TagPrefix="cc2" %>
<%@ Register Assembly="Stimulsoft.Report.Web" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Service Commission Report</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="height: 458px">
        <cc1:stireportweb id="rwModuleHC" runat="server" reportdatasources="sdsModuleHC" ReportSource="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"></cc1:stireportweb>

        
        <asp:SqlDataSource ID="sdsModuleHC" runat="server" ConnectionString="<%$ ConnectionStrings:BigMacEntities %>"
            SelectCommand="select @uname uname, STR_TO_DATE(@fdate,'%d/%m/%Y') as fdate, STR_TO_DATE(@tdate,'%d/%m/%Y') as tdate, staff.name, inv.createdate as Date , items.productcode , items.productdesc , inv.branchcode, inv.resourcecode, items.qty, items.servicecommission , (items.qty * items.servicecommission) subservicecommission, cust.cussupname as MemberName,cust.inhousecode as Membership, '' remark ,  HOUR(inv.createdate) chour , DATE(inv.createdate) cdate, DATE_ADD( DATE(inv.createdate),Interval HOUR(inv.createdate) HOUR) byHour from (select * from Invoice_m_Invoice where cast(createdate as date) between STR_TO_DATE(@fdate,'%d/%m/%Y') and STR_TO_DATE(@tdate,'%d/%m/%Y') and (@bcode like 'ALL%' or branchcode = @bcode) and ifnull(status,'') = 'CLOSE') inv inner join Invoice_m_Invoice_Items items on inv.id = items.invoiceid  and type != 'TopUP' inner join CusSup_m_CusSup cust on inv.cussupid = cust.id inner join Configuration_m_Branches branch on inv.branchcode = branch.branchcode inner join Common_m_Staff staff on staff.id= items.staffserviceid"  
            ProviderName="<%$ ConnectionStrings:BigMacEntities.ProviderName %>">
            

           <SelectParameters>
                <asp:QueryStringParameter Name="uname" QueryStringField="uname" 
                    DefaultValue="Admin" />
            </SelectParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="fdate" QueryStringField="fdate" 
                    DefaultValue="01/01/2014" />
            </SelectParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="tdate" QueryStringField="tdate" 
                    DefaultValue="31/05/2014" />
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
