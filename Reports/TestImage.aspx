<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestImage.aspx.cs" Inherits="BigMac.Reports.TestImage" %>

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
            
            
            
            
            
            
            
            
            
            SelectCommand="SELECT @name AS name,@cno AS cno,@cexpdate AS cexpdate,@staffid "  
            
            
            
            ProviderName="<%$ ConnectionStrings:BigMacEntities.ProviderName %>">
           <SelectParameters>
                <asp:QueryStringParameter Name="name" QueryStringField="name" 
                    DefaultValue="Member Name" />
            </SelectParameters>

            <SelectParameters>
                <asp:QueryStringParameter Name="cno" QueryStringField="cno" 
                    DefaultValue="111111" />
            </SelectParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="cexpdate" QueryStringField="cexpdate" 
                    DefaultValue="02/01/2014" />
            </SelectParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="staffid" QueryStringField="staffid" 
                    DefaultValue="" />
            </SelectParameters>
        </asp:SqlDataSource>

        <cc3:stiwebviewer id="wvModuleHC" runat="server"></cc3:stiwebviewer>
    
    </div>
    </form>
</body>
</html>
