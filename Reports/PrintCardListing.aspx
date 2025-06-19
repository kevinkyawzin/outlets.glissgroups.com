<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintCardListing.aspx.cs" Inherits="BigMac.Reports.PrintCardListing" %>

<%@ Register Assembly="Stimulsoft.Report.Web, Version=2008.1.206.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a"
    Namespace="Stimulsoft.Report.Web" TagPrefix="cc3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"><html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="height: 458px">
        <cc3:stireportweb id="rwModuleHC" runat="server" reportdatasources="sdsModuleHC"
  
            reportsource="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"></cc3:stireportweb>
        <asp:SqlDataSource ID="sdsModuleHC" runat="server" ConnectionString="<%$ ConnectionStrings:BigMacEntities %>"
            
            
            
            
            
            
            
            
            
            SelectCommand="select e.value1 nric,c.inhousecode,c.cussupname,card.cardno from (select * from CusSup_m_Cards where ifnull(printedbyid,0) = 0 or printeddate is null) card inner join CusSup_m_CusSup c on card.cussupid = c.id inner join (select * from rptEncryptValues where rptname = 'printcard' and uname = @uname) e on c.id = e.tblid"  
            
            
            
            ProviderName="<%$ ConnectionStrings:BigMacEntities.ProviderName %>">

            <SelectParameters>
                <asp:QueryStringParameter Name="uname" QueryStringField="uname" 
                    DefaultValue="Admin" />
            </SelectParameters>

        </asp:SqlDataSource>

        <cc3:stiwebviewer id="wvModuleHC" runat="server"></cc3:stiwebviewer>
    
    </div>
    </form>
</body>
</html>
