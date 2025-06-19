<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewMember.aspx.cs" Inherits="BigMac.Reports.NewMember" %>

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
            
            
            
            
            
            
            
            
            
            SelectCommand="select @fdate fdate,@tdate tdate, @bcode pbcode,@uname uname, cust.createdate, cust.branchcode, inhousecode, cussupname,   evalue.value1 mobile , dateofbirth, cust.status from (select * from CusSup_m_CusSup where cussuptype like '%Customer%' and cast(createdate as date) between STR_TO_DATE(@fdate,'%d/%m/%Y') and STR_TO_DATE(@tdate,'%d/%m/%Y') and (@bcode like 'ALL%' or branchcode = @bcode)  ) cust left outer join rptencryptvalues evalue on cust.id = evalue.tblid order by trim(cussupname)"  
            
            
            
            ProviderName="<%$ ConnectionStrings:BigMacEntities.ProviderName %>">
           <SelectParameters>
                <asp:QueryStringParameter Name="uname" QueryStringField="uname" 
                    DefaultValue="Admin" />
            </SelectParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="fdate" QueryStringField="fdate" 
                    DefaultValue="29/01/2015" />
            </SelectParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="tdate" QueryStringField="tdate" 
                    DefaultValue="28/02/2015" />
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