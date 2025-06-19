<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AttendanceListing.aspx.cs" Inherits="BigMac.Reports.AttendanceListing" %>

<%@ Register Assembly="Stimulsoft.Report.Web, Version=2008.1.206.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a"
    Namespace="Stimulsoft.Report.Web" TagPrefix="cc3" %>

<%@ Register Assembly="Stimulsoft.Report.Web" Namespace="Stimulsoft.Report.Web" TagPrefix="cc2" %>
<%@ Register Assembly="Stimulsoft.Report.Web" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"><html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Attendance Listing</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="height: 458px">
        <cc1:stireportweb id="rwModuleHC" runat="server" reportdatasources="sdsModuleHC"
   
            reportsource="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"></cc1:stireportweb>
        <asp:SqlDataSource ID="sdsModuleHC" runat="server" ConnectionString="<%$ ConnectionStrings:BigMacEntities %>"
            SelectCommand="Select @bcode as branchcode,@branchname as branchname,customername,mobile,nric,nationality,appointmentdate,starttime,endtime,description,STATUS,'' AS remarks
                 from schedule_m_appointment where transactiontype= 'attendance' and branchcode = @bcode and DATE_FORMAT(appointmentdate,'%d/%m/%Y') = @date "  
            
            ProviderName="<%$ ConnectionStrings:BigMacEntities.ProviderName %>">
           <SelectParameters>
                <asp:QueryStringParameter Name="bcode" QueryStringField="bcode" 
                    DefaultValue="CCKSPA" />
            </SelectParameters>
             <SelectParameters>
                <asp:QueryStringParameter Name="date" QueryStringField="date" 
                    DefaultValue="" />
            </SelectParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="branchname" QueryStringField="branchname" 
                    DefaultValue="CCKSPA" />
            </SelectParameters>
        </asp:SqlDataSource>

        <cc3:stiwebviewer id="wvModuleHC" runat="server"></cc3:stiwebviewer>
    
    </div>
    </form>
</body>
</html>