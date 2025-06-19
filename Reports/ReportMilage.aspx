<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportMileage.aspx.cs" Inherits="BigMac.Reports.ReportMilage" %>

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
            
            
            
            
            
            
            
            
            
            SelectCommand="SELECT  mr.cussupid,mr.resourcecode AS CODE,mr.name AS customername,mr.mobile,mr.receiptno, DATE_FORMAT(mr.receiptdate, '%d/%m/%Y %r') AS receiptdate,  
        mr.amount,mr.rewardbonus, SUM(cr.credit) - SUM(cr.debit) AS balance, cp.name AS partner, @fdate as fdate, @tdate as tdate 
        FROM mileage_m_rewards mr INNER JOIN cussup_m_cusredemption cr ON cr.cussupid = mr.cussupid
        INNER JOIN common_m_partnership cp ON cp.id = mr.partnerid
        WHERE CAST(mr.receiptdate AS DATE) BETWEEN STR_TO_DATE(@fdate,'%d/%m/%Y') AND STR_TO_DATE(@tdate,'%d/%m/%Y') 
        GROUP BY mr.cussupid,mr.resourcecode,mr.name,mr.mobile,mr.receiptno,mr.receiptdate,mr.amount,mr.rewardbonus,
        cp.name ORDER BY mr.receiptdate"  
            
            
            
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
