<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MembersListByDOB.aspx.cs" Inherits="BigMac.Reports.MembersListByDOB" %>

<%@ Register Assembly="Stimulsoft.Report.Web, Version=2008.1.206.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a"
    Namespace="Stimulsoft.Report.Web" TagPrefix="cc3" %>

<%@ Register Assembly="Stimulsoft.Report.Web" Namespace="Stimulsoft.Report.Web" TagPrefix="cc2" %>
<%@ Register Assembly="Stimulsoft.Report.Web" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"><html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Members List by DOB</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="height: 458px">
        <cc1:stireportweb id="rwModuleHC" runat="server" reportdatasources="sdsModuleHC"
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            reportsource="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"></cc1:stireportweb>
        <asp:SqlDataSource ID="sdsModuleHC" runat="server" ConnectionString="<%$ ConnectionStrings:BigMacEntities %>"
            
            
            
            
            
            
            
            
            
            SelectCommand="select @bcode pbcode,@mth as mth,@selmth as selmth,cust.createdate, cust.branchcode, inhousecode, cussupname, evalue.value1 mobile , dateofbirth, cust.status from (select * from CusSup_m_CusSup where cussuptype like '%Customer%' and (@mth='0' or month(dateofbirth)=@mth)) cust left outer join rptencryptvalues evalue on cust.id = evalue.tblid order by trim(cussupname)"  
            
            
            
            ProviderName="<%$ ConnectionStrings:BigMacEntities.ProviderName %>">
            <SelectParameters>
                <asp:QueryStringParameter Name="mth" QueryStringField="mth" 
                    DefaultValue="0" />
            </SelectParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="selmth" QueryStringField="selmth" 
                    DefaultValue="ALL" />
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