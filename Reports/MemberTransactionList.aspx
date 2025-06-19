<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MemberTransactionList.aspx.cs" Inherits="BigMac.Reports.MemberTransactionList" %>

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
            
            
            
            
            
            
            
            
            
            SelectCommand="select @uname uname, STR_TO_DATE(@fdate,'%d/%m/%Y') fdate, STR_TO_DATE(@tdate,'%d/%m/%Y') tdate,@citidesc citidesc,@bonusdesc bonusdesc, cust.inhousecode, cussupname, custR.cocode, custR.branchcode, min(custR.createdate) cdate, custR.resource, custR.RefNo, custR.productid, custR.productdesc, custR.remark,sum(case when redemptiontype  = 'C$' then 1 else 0 end) ccount,sum(case when redemptiontype  = 'B$' then 1 else 0 end) bcount,sum(case when redemptiontype = 'C$' then credit else 0 end) creditC,sum(case when redemptiontype = 'C$' then debit else 0 end) debitC,sum(case when redemptiontype = 'C$' then balance else 0 end)  balanceC, sum(case when redemptiontype = 'B$' then credit else 0 end) creditB,sum(case when redemptiontype = 'B$' then debit else 0 end) debitB,sum(case when redemptiontype = 'B$' then balance else 0 end)  balanceB from CusSup_m_CusRedemption custR inner join CusSup_m_CusSup cust on custR.cussupid = cust.id where cussupid =@custid  and cast(custR.createdate as date) between STR_TO_DATE(@fdate,'%d/%m/%Y') and STR_TO_DATE(@tdate,'%d/%m/%Y')  group by custR.cocode, custR.branchcode, custR.resource, custR.RefNo, custR.productid, custR.productdesc, custR.remark,custR.invoiceitemid "
            
            
            ProviderName="<%$ ConnectionStrings:BigMacEntities.ProviderName %>">
           <SelectParameters>
                <asp:QueryStringParameter Name="uname" QueryStringField="uname" 
                    DefaultValue="Admin" />
            </SelectParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="custid" QueryStringField="custid" 
                    DefaultValue="18" />
            </SelectParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="fdate" QueryStringField="fdate" 
                    DefaultValue="01/02/2014" />
            </SelectParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="tdate" QueryStringField="tdate" 
                    DefaultValue="31/05/2014" />
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