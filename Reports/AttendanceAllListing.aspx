<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AttendanceAllListing.aspx.cs" Inherits="BigMac.Reports.AttendanceAllListing" %>

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
            SelectCommand="Select @bcode as branchcode, @branchname as branchname,customername,mobile,nric,appointmentdate,nationality,starttime,endtime,description,STATUS,'' AS remarks
                 from schedule_m_appointment where transactiontype='attendance' and branchcode = @bcode"  

            ProviderName="<%$ ConnectionStrings:BigMacEntities.ProviderName %>">
           <SelectParameters>
                <asp:QueryStringParameter Name="bcode" QueryStringField="bcode" 
                    DefaultValue="CCKSPA" />
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