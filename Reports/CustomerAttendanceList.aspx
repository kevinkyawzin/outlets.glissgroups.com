<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerAttendanceList.aspx.cs" Inherits="BigMac.Reports.CustomerAttendanceList" %>

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
            
            
            
            
            
            
            
            
            
            SelectCommand="select @bcode pbcode,cust.resourcedate, cust.branchcode, cust.inhousecode, cust.cussupname,evalue.value1 mobile, cust.starttime, cust.endtime, cust.productdesc,cust.remark
            from (select o.resourcedate,o.branchcode,o.cocode,o.cussupid,c.inhousecode,o.cussupname,c.nric,o.starttime,o.endtime,d.productdesc,ifnull(o.remark,'') 
            remark from salesorder_m_detail d inner join salesorder_m_salesorder o  on o.id=d.salesorderid inner join cussup_m_cussup c on c.id=o.cussupid 
            where o.`type`='ORDER TAKING' and (@bcode like 'ALL%' or o.branchcode = @bcode)
            and cast(o.resourcedate as date) between STR_TO_DATE(@fdate,'%d/%m/%Y') and STR_TO_DATE(@tdate,'%d/%m/%Y') 
            ) cust left outer join rptencryptvalues evalue on cust.cussupid = evalue.tblid order by cust.resourcedate desc"  
            
            
            
            ProviderName="<%$ ConnectionStrings:BigMacEntities.ProviderName %>">
            <SelectParameters>
                <asp:QueryStringParameter Name="bcode" QueryStringField="bcode" 
                    DefaultValue="ALL" />
            </SelectParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="fdate" QueryStringField="fdate" 
                    DefaultValue="02/01/2014" />
            </SelectParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="tdate" QueryStringField="tdate" 
                    DefaultValue="31/05/2014" />
            </SelectParameters>
        </asp:SqlDataSource>

        <cc3:stiwebviewer id="wvModuleHC" runat="server"></cc3:stiwebviewer>
    
    </div>
    </form>
</body>
</html>