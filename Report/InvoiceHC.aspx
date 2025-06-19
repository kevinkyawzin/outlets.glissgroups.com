<%@ Page Language="VB" AutoEventWireup="false" CodeFile="InvoiceHC.aspx.vb" Inherits="InvoiceHC" %>

<%@ Register Assembly="Stimulsoft.Report.Web, Version=2008.1.206.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a"
    Namespace="Stimulsoft.Report.Web" TagPrefix="cc3" %>

<%@ Register Assembly="Stimulsoft.Report.Web" Namespace="Stimulsoft.Report.Web" TagPrefix="cc2" %>
<%@ Register Assembly="Stimulsoft.Report.Web" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <cc1:stireportweb id="rwModuleHC" runat="server" reportdatasources="sdsModuleHC"
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            reportsource="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"></cc1:stireportweb>
        <asp:SqlDataSource ID="sdsModuleHC" runat="server" ConnectionString="<%$ ConnectionStrings:BigMacEntities %>"
            
            
            
            
            
            
            
            
            
            SelectCommand="SELECT id, createdate, lastmodifieddate, inhousecode, cussupname, cussuptype, dateofbirth, branchcode, cocode, photo, gender, race, nationality, nrictype, canemail, cansms, cancall FROM CusSup_m_CusSup" 
            
            
            
            ProviderName="<%$ ConnectionStrings:BigMacEntities.ProviderName %>">
        </asp:SqlDataSource>

        <cc3:stiwebviewer id="wvModuleHC" runat="server"></cc3:stiwebviewer>
    
    </div>
    </form>
</body>
</html>
