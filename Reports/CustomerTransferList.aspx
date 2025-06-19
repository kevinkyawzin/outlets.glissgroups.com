<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerTransferList.aspx.cs" Inherits="BigMac.Reports.CustomerTransferList" %>

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
            
            
            
            
            
            
            
            
            
            SelectCommand="select @citidesc citidesc, @bonusdesc bonusdesc, r.branchcode,
            CONVERT(DATE_FORMAT(r.createdate,'%d/%m/%Y'),CHAR(50)) createdate, 
            r.cussupid, cust.cussupname, r.productid,product.productcode,product.desc productdesc,l.open,
            CASE r.redemptiontype WHEN 'PQ' THEN r.credit ELSE 0 END qty,l.liability,
            l.admissionfees,CASE r.redemptiontype WHEN 'C$' THEN r.credit ELSE 0 END  citidollar,
            CASE r.redemptiontype WHEN 'B$' THEN r.credit ELSE 0 END  rewarddollar,
            STR_TO_DATE(@fdate,'%d/%m/%Y') fdate,STR_TO_DATE(@tdate,'%d/%m/%Y') tdate,@uname uname
            from
            cussup_m_cusredemption r 
            INNER JOIN cussup_m_citibellaliability l ON l.id=r.invoiceitemid
            INNER JOIN cussup_m_cussup cust ON r.cussupid=cust.id
            INNER JOIN product_m_product product ON r.productid=product.id
            inner join Configuration_m_Branches branch on r.branchcode = branch.branchcode 
            where r.resource='CustomerTransfer' and 
            cast(r.createdate as date) between STR_TO_DATE(@fdate,'%d/%m/%Y') and STR_TO_DATE(@tdate,'%d/%m/%Y') 
            order by STR_TO_DATE(r.createdate,'%d/%m/%Y'),r.branchcode"  
            
            
            
            ProviderName="<%$ ConnectionStrings:BigMacEntities.ProviderName %>">
           <SelectParameters>
                <asp:QueryStringParameter Name="uname" QueryStringField="uname" 
                    DefaultValue="Admin" />
            </SelectParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="fdate" QueryStringField="fdate" 
                    DefaultValue="01/02/2014" />
            </SelectParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="tdate" QueryStringField="tdate" 
                    DefaultValue="31/03/2014" />
            </SelectParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="bcode" QueryStringField="bcode" 
                    DefaultValue="ALL" />
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
