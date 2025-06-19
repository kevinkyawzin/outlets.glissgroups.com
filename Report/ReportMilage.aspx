<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportMileage.aspx.cs" Inherits="BigMac.Reports.ReportMilage" %>

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
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            reportsource="H4sIADVnj1cA/+1dW2/bOBZ+L9D/IGQetgOktniTqNb1IEmnF2zTDeK2s8BiHyiJSoWRpUCSp80M9r8vKVKyJMuJMwmVSSZN4lrkObzpfOeQh7fZT9+XifUbz4s4S1/tgYm9Z/E0yMI4PXu1tyqj53TPKkqWhizJUv5q74IXez/Nnz6ZLcp4wfOYJfHvPO+lwM7PkzhgZRUiCE/5eZaXe4LNsmav40BGsPzCOuWR4NizyotzkfQ6Ys+Ki3/yi1d7Zb7iik0yspL5rOCFiP0QF6WOtoJslYoHke+0TbrIVnmwjRjUqQriIiyOs3CV8HdHqkSwKZFI5hPzE67SGixWlcRBErNi3kpoNlVBLZqjLFkt023lge30BPVvLFnx+TnLy5Tn+4uLouTLyaLMxXuZTVXkAH2wKorVeRzWDO/TEsHL6ctsyfOULfnumWTh7sTLzI+T3clzHvD4vEyznTnYUrZgTf6aB/GSJZfm8I3loZ+lq2J3Jp8lLA347gxRyMrda11ei1q30S48s6mWunZYC4BxUUNw2qb4KMShK81VSI/ifapgsUnZxDQYm7ZoapBOWyitw055UqmNHTCulEpdhXbMFyb0kkDtVYmIEjRNoQLeJpkvNVpVBtWoVyZyws6u1DIVEVDaBdXaRYZtUSmzwywPeT7/KHTuy8OEBb++hC8XWRKHL9+wpOAvsQqdTTXhmjFfFV/nn3KWFkJ58LQUJFVQQ3GULc9Fsmm5pdCko9lkId9xJvLQxcft4quYQ2EeturGXcqkypXEIuqUB8LanCV8bu/bE7jvTqgrvhAhyb34LvMVlXK6RRIcn/j3EtmqTqSukwy8pCa712awRnhCRVWA+PPEH7q8SrpaaRjvhocW15ssLeef4qUQy4/8m3WaLVm6D+D+YZaE+ydZLNRlJUX79mxa0W6k8HYVh/OQkQjZOOI4INimDqMu9QBhjCLMOQ9n04psg/ldlgvjd5YuRd3mp/HZV9FCnbANjg9xyot/RZ9TIUyJ+F7J/Wy6EbzBeMzys1hoNyEr8mc2rQMGKL8vRE9lrqnUwyZVnLao9MMGVaUMlfxsaEZNIsFRqyY0+JJOKtmpafAgjcxjHuXZcjatvg4SKOHT6mAdMEj7JsuXrFRC79RC/5aLLgZLVFxX+gcLJZjmP38/z3khu3wiTxnQQ9dUNc8Q6ICr8nfHAZ0zIRp06D6ALvAiP/Bdm7AAY+w4HmEewJx50A5hhMBOoDsSHzx/qKgD7gioKzMTmKPGMQfcQcxRlb83DuYqvO0jYb7HRxy9LuIIITYXv6HHMY4CKqwbQQhwEjBCGbW3Ie7ewoeOAJ9jMeiTqalOugkoAds8luggloguABgHTGTiagMme49wdAN2ys9WCct3Q9S9BQUZARR/tAbBk8pD8D8jyIB/Bhla5MmgyDs6ZTRWn40+inyH0oTIO2OLfGlO5PENRN7pB4tuc/y7eNcskS9R+z1A4yRYlPFylRRZVE6UeZusvQ8TEdllP8njpZDM3/hVcBnyfyBn362cBTbYCQRJltetpx7Mi1KvsczKVFUO0aMcKE812mmXpOsgmq7fUd9zdE3NIZvwHZdujblqQv3QpYnTFk3z0KGpGqrlXxtousubrddkvfjZtJV236X3JsvKxqUHnLZPT0WZ8ukBe+J5tVcP3dCrhwe9elA7GMBIHgYJVFs6F1yjxupnwSO92qKMK2FfmsdHq9a2avDWnAMSFdvMmlDqaclDK0stI+bMvJ8ADvoJoPYTAG/Mjp4AD6h6e489PTOYoGNg4g+Z2McsOjbSw4Pmh/twcLgPPV0AMJbvzK3tCX1A9kS52gICuUsx8iHF0I8oDjyEIi8IASKhGz40Vxv0RgHepyxkF2ZQ17gSjqqVIkOg20hF9tOrqXNFPQ/D58fHzy/EP9Fxb0dsZD9d538jIHtXjOh0vxeiOxjS2eC+DemAYSG+zqDuHo3q9BDLyKhOp90O7yzSgI0/xOwKDdltu6WxHB1eoaHrQ8acuLqDrqh33VmriAfACRigKPQxDFzKEXSRA0hEoshx6EMzpei2lBC8xFtZiQyrloRZR1nIjQzu4AgLL4aghHX2I3lFgFx3cS+gFNgM+CH1XJtH2HN8GoKQEIcgiS/fBQ8NSngMKJ2qRarWRyPrKKB5BwkewpCe+oUjuUegHAveCwxR7Lgu8jFnLsERxnK1oOcxIkwToW704EZ2ZEwMvWYlN4EiZN6nMjydrLMHYy27RfcERaJH5weuxzzCBXIc4mPX8YEHIhxyTOzooaEIjIGiI73HxZJZGYERNL8SaQhGelEGGmtRxj0BEfIdYYeIh0LEsUew7/AIYZ8FAGAaUfTQQOSMAaKDapeVEfRg4+hxhtCjJ4kRGXGm617gx4u4dCSIIZFtYy+KfCLQZDsUAjfgBPkPDT/uOF05uevQOpTbDo2gyLxTYWPCuOua1e4F5I7vU79nDnWj4+++xxxf/tbqPgS9g5kQeNvvTXtzQswID4jDfOz7rs+IK2w/YaHrucxztqkvwy/dGfOldw3eX3aWxNS6t4E1b3KrcT3sbJw3MtDczAitZ0agkb2rQO9dxfYDnxoB9t9jlQ64tc2kUsJ3WpAtT3Iwsm4Agxusxx7eNVpvP4djTV/gR3FvKE2IOxhd3JvDRczIPLqBzA86eICe/cZ4HJmvdprd0aaba0r93+eMAYBGx4k6UscMSMh6QVkuChdc7Lqk7G2erc4XXIg0K0Xnf1/047shAzwf+RmTo5MTVpY8T2V/tB80wHWSFXGPqx80wLW4WPpZMtDAIu5zwT9kAUsWvCzj9GxeCfds2g/eaL3t6+G02hic6Ad6KI6dsVxbj2rjr6c28B2Y1+ZkLTO6w33UHbeoOwYXOEDd1HTUqdnqiBb42M82sgZ9/FFl61BBM4rA+/M9bThoMbXYE3usdT3VZKr8fBR7Q/YP3tXw0tgmd3ITp0pX7m/TJSy9qPWxjbLFh+LVKY+bx0r24jp8b+JE2Mu78UxX7uJb9ktLO68OrNxpJ7M6yvNgcTR0EmgTH+RciFtYLRC7hOyqZHJeVG9BOgQHTx6dyqK3gtSr67jYO1vKSeOmM7udHE7wbR0RObyZXG+IJWjEDtEE3ouFApAjCm2HuohSDG1CA8QCCiLEQgIDP3hoCwVubRetBMfWLQhZKZr+hRH7YX6lzeAOWr2BlpARvZjV0XN345DYHUV/H2+ENwZ0/lisls82/Zg/mumMOY/OiNtwRtTqRem9S1SLN6RaQK1b3BFdnY+6pct41yO90ZTL+2db3J2GVAx9VDGjqhhwxbkBtZfIu4PFcsDIYjnPpw6CJHQjj+IQRpT4boBI5Ng2JoE8vuJOFstBk4juL5Yz5xm5VZeEqeME+kcJDDbA9SqvZMtFPqPQQwSHDnYpoJBgYqMocBw3gDbsydbajFT2tfnbNCfrIxb6rbG9JfQ9FlXbAjBxvPp0vE5rV1S/xGH5dU4n8iSx9XOL5JznlSQfYElQPzUEW+8lEXG/MNHWS5b/qo/gt3fVJA3j9juIqn7AgbyRSfQUNvsFLVP0H2K/qH7+O2SPZtMmMx2m2kGJiPquXsZMHYOWa51c6EqB7ZWqGGR9epyDY1DRkhGXdo+HB0XBl75Qe1cdHqH9WfU9PUnvjp5edM6+idyvIvslTsPsWzGRxqW4Mk1Wsqto/r1MtpOs20xgrsyzpNiF9pAVfBc6/R56lLPpUFPXL0FyqNus6tO822EtoqOvwh5xgfwptMUvcCwAXhD3BaLWwXHNVRO1+ZQTc06m0BF8tmuBF4C+sD3rZM2maVpslfpwAIAudm1kI4gDwhjySORGxKGB4zuY1/yNrtHMld7o1qdRJZrkcxqX8/dp8JUXNUkV1CL5oq49m4tC0wmYQNupKeuYingR5PG5SGspYwpLicHTJ93nWhw3wjvyt8klBK4VuCk+g5FKXjYy7AvKJVH6v0tz2VBlT588fSKbWYy7gvpw+ELJqvo8WQnRC6yjhBWFVZehVmHv0688jwezaC6ZW1Ovv+lEFytfjqae/djtDxxfVI0hYnrhfPJevG51w11ThTXNz2kok1wH/HDKz+TJIesb76zXvBDjLZ5bZ5XHTR4ZKV3s1nMrzKw0K61lFsbRxd4PMjHN/o9rs6siyCSqdnv6RH5tmnk21eLXEsUPAoMrocfnXw7r6CZIkZUXW+/Paq6qm72Lw5CnWu3D7Wpf3hUoE9x629WVl1a1jBvtmTbdgayK0u8PrK1bJ/G+0RNdwoq9Wy+ZS23T0A0qp27oOkgSfZMXuK2bvHZslKoeN2yZKg1tK5RsiAf5tXUJ5Pz/Ue/PQU1yAABR789BTXIAAA=="></cc1:stireportweb>
        <asp:SqlDataSource ID="sdsModuleHC" runat="server" ConnectionString="<%$ ConnectionStrings:BigMacEntities %>"
            
            
            
            
            
            
            
            
            
            SelectCommand="SELECT  mr.cussupid,mr.resourcecode AS CODE,mr.name AS customername,mr.mobile,mr.receiptno, DATE_FORMAT(mr.receiptdate, '%d/%m/%Y %r') AS receiptdate,  
        mr.amount,mr.rewardbonus, SUM(cr.credit) - SUM(cr.debit) AS balance, cp.name AS partner, @fdate as fdate, @tdate as tdate 
        FROM mileage_m_rewards mr INNER JOIN cussup_m_cusredemption cr ON cr.cussupid = mr.cussupid
        INNER JOIN common_m_partnership cp ON cp.id = mr.partnerid
        WHERE CAST(mr.receiptdate AS DATE) BETWEEN STR_TO_DATE(@fdate,'%d/%m/%Y') AND STR_TO_DATE(@tdate,'%d/%m/%Y') 
        GROUP BY mr.cussupid,mr.resourcecode,mr.name,mr.mobile,mr.receiptno,mr.receiptdate,mr.amount,mr.rewardbonus,
        cp.name ORDER BY mr.receiptdate"  
            
            
            
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
