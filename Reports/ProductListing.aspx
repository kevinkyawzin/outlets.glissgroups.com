<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductListing.aspx.cs" Inherits="BigMac.Reports.ProductListing" %>

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
            
            
            
            
            
            
            
            
            
            SelectCommand="select @uname uname, @citidesc citidesc, @bonusdesc bonusdesc, p.id,p.createdate,p.lastmodifieddate,p.cocode,p.productcode,p.category,p.categorysub,p.brand,p.RangesNSeries,p.desc,prc.uom,ifnull(p.remark,'') remark,ifnull(p.status,'') status,ifnull(redeemdollars,0) redeemdollars,ifnull(redeembonus,0) redeembonus,ifnull(sellprice,0) sellprice, ifnull(awarddollars,0) awarddollars,ifnull(awardbonus,0) awardbonus,ifnull(ServiceCommission,0) ServiceCommission from  (select * from Product_m_Product where (status = @pstatus or @pstatus like 'ALL%' )  ) p left outer join Product_m_ProductPrice prc on p.id = prc.productid"  
            
            
            
            ProviderName="<%$ ConnectionStrings:BigMacEntities.ProviderName %>">
           <SelectParameters>
                <asp:QueryStringParameter Name="uname" QueryStringField="uname" 
                    DefaultValue="Admin" />
            </SelectParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="pstatus" QueryStringField="pstatus" 
                    DefaultValue="All" />
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