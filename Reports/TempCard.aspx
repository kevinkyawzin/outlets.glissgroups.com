<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TempCard.aspx.cs" Inherits="BigMac.Reports.TempCard" %>

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
