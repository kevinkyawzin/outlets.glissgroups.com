<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ServiceReportByHour.aspx.cs" Inherits="BigMac.Reports.ServiceReportByHour" %>

<%@ Register Assembly="Stimulsoft.Report.Web, Version=2008.1.206.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a"
    Namespace="Stimulsoft.Report.Web" TagPrefix="cc3" %>

<%@ Register Assembly="Stimulsoft.Report.Web" Namespace="Stimulsoft.Report.Web" TagPrefix="cc2" %>
<%@ Register Assembly="Stimulsoft.Report.Web" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Service Commission Report</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="height: 458px">
        <cc1:stireportweb id="rwModuleHC" runat="server" reportdatasources="sdsModuleHC" ReportSource="H4sIAChlj1MA/+1de2/bOBL/v0C/g5D9ZxdwZZHUs3W8SJPrA9cX4uz2gMPhIEt0IlSWfJLc1lvsdz9SpGTJpl+NKdmJt2jXJoc0H/Ob4QyHZO/37+NQ+YqTNIij8zOgamcKjrzYD6Lb87NpNnpmnylp5ka+G8YRPj+b4fTs9/7TJ71BFgxwErhh8BdOFmpwJ5Mw8NwsTyGE13gSJ9kZKaYovavAoxluMlOu8YiUOFOy2YRUPc84U4L0n3h2fpYlU8yK0YJu5g7dFKck912QZjxb8eJpRL6Q3+1WSQfxNPFWEYOiVkKc+un72J+G+M0laxEsW0SquXGHIWZ1CZuVV3ERBm7ar1TU67KkCs1lHE7H0Yr2QFitj1B/dcMp7g/JTODOYJZmeKwOsoTMSq/LskTUcTRNfZx625dI3Mi72+1HZm/IYBTkZITwTTDGawp4PqHZhf6uUv/bKDP1dcRBFuzU46vdGjPasfHv8XiIkw/uePsBZUXSu2CydZFol/onCeFJL9tpknmZnUb2f9msHCjsBWM3XEOcYA8HX7Ef+FvXn+Cxm3zZgTzNQbtTv1OcfA1omfE4SKlA275H6XR4j9LZjnw23YYFel0uc6ppFfEbpIUA7lYpKPfWZVmeskDxNmJCcZmyzCklbLdCU4jobkVGF2nXOMyVxhYSnqmUogvVnD9dopWIzN5UCWlBORQs4XUYD6k+y9vABnVjJZ/c2406JicCTLegQrfQtBUKpfcyTnyc9D8QjfviZeh6X17AF4M4DPwXr9wwxS90ltrrcsJ5wWSa3vVviFRPJ26Co4yQ5EklxWU8npBqo2xFo52aXqONfINd8hu8+Xq1+SznJVkcrNSM27SJtSsMSNY19sha4zbEfa2jqbBjqbZFPhiEkxfy64U3dMqsN4mUuMHfM94lo+gSTVvTke07s7JDoANVk3ZsfX94nyI/2A4MlVKv4ijrU9mRKh/wN+U6HrtRx+m8jEO/180zl4q8CyKcfhz9EZG5DMnnnO163aXkpYLv3eQ2IMKF9Iz+6XWLBAHl9wFZJvY5FfuyTBVEFSr+ZYkql0X59C3JJU5BWbMQDEg4Sp/yqStodCEN/Yn+gEl05bIU6cpwptAFUK+bEwiLMY7gEJ0nCGlfxcnYzRgnmgUnvsYRTtyQ5dVZUthUUqj/j+8TovJoE8lv0oQFjs8bAkQ40NmvW83gwMgRAMhf+9CQ8CZOyKr9NhqT9vSvg9s70qta2oPBjt4AdriGHiXxWAZYbOlg0UVgsdivO82AxVR1DpaT2tgT61sNsP6PynpXzU3Iv2VAAGgFF15O0yweiyCwVAuphy1tGXXf95+9f/9sRv7rdWsZSz/fnf/+fWBliWCFDN4l0AywLA4reILVfmCFjAZwdRNLgRGUrkqQIWR6kzcANcT0ObuftMk+2d5sWp1k8tSJfozqBJmLyWTJHvxFuMwNKftwKx+UZv4gC8bTMI1HmcqWqOrcfaCSzHrxT0kwJgj4ijfBUmTv6x1Ldcj/NbAV1sI4KaaNfZHPwwtjJZeZ83YgwhfL7ckNrWpL6g6e7nyKFj0/OwooOoRvMLXt+mwI+Zc6TRBVaMovNZp8oCr+McHQrR+2hSFbyO91K3UvuuRexXFWuuSAWfXJsSxZPjmgqY5TeOXQPb1yutArB23eLas5v5zmtKQTH5o6hPa+JAjl6lX6kMjkKMO+8jF6LkUPynctQFvI+g5vQEPeBcbzhPkhs4VOzL9AtSPzO00w/4+b2HdnUhaA8ND8CWx/s9zQWwcoR2hgabxjoCl3nVVoE/sEqPsbV1ojgKKVfYhH7+Vg6l7Ohe0BgLT1ZhBXLRC1YAZp4MjsINmCfAdDqC7XDtoQ4laJFEOI111Nr8UlwNJ3ITcoAaigMH/0e5o/jjgooVBZu4UlsDiQizDk8SJgq3iR3YZjxZCwQdEOeCv3kvyDk4e6lwv2piXhGh8k/QUp+rGBoAdNCLRCcljHATREl5QEavAEtfagBpqA2jzoyJcDOfn+DSAMNAKQN8A5DsjlYDvE3bLHBDnYBOSuaTj6JFN+kYE3pMnHGxTiDfEGgGPBm0NV3Alt7aENNYE2duJFkbWkRPLjOQAS4o1H0iJ0HHhDuXazToBrEXB6c4CjR8zkKDhdPuCEwbiARw2iI3GWmKphsxWlvnn/nNVwwtz+MWc0aMU9fTI/OyIFew34T4TBi4AHL6Ij8Z9YKjoZc20jz2zGmKMHhVMpaGvAdWKK0IYKU+5IXCcG3RLoAPWk51rcPkdNeiufPrnCqZcEk0ySptPlu1HQkllX34zl/ksdtLCJjqwj20SX7bgbZDPS86t4OgwxDTWahcuTutCiWv7B7qXLCigWBBO/TuLphKUXvD13nszz5O2ml8HE8L5n/JFwO52rTR01GFHZQe0tMjuf4iDKOrl+p4B9YAFhezufALc8bTO/IElKdJj+U54TrqxE/M4dInpzl1rQ6zlO3C6D240muP1lzuCKlEB63fx57l7yOSycDyoEu9XCUgzueyX2ehr4fR9Ay4MO9nTk6oZvuQa2/dFo6FuWj8mHXjcna3oZB2QL3WIdl223iFvYetliFbdGos9pDmkBWF2V7X0VWK18xVKwuOvJbnApCFXD2NfBMkO4FOTnyvSGDtcAlbn69c7pqPVe1OHejpZRvl6lDunVTnKUoSHffSE8VIb4jTUGaMoEQofJ97n2tEe+Yw/xaGRqQEdE0/q6bWLDGSLPtIdDa5WSPVbQIKsJ1NALKyWhpoG7OcQX0nB1YTTmOWDq4nRubA9M34iqqK0q80uO/yvFR2AcyrUcHC5CLcOPlxkN+R1gi5u5Dw0tTuNgYVeIywGLuQew3N0pWXZfnCwdSK6bsTx6wngQ/gvJLghTMofu6IGob9Dv7oAouP/gnQ9ApvOhtg9F7wIvrmMuvQ40UZa7AamwdDdAKOUkJ+QnOQ1ZDgc3IsP5jdBOiZopvm3hl8iD7FWtBa+9/dB0J9zb2UtjW+U5f2FCigI1f9pNQSr6mAd/pER8Z1/w7FOCR8H3czqVHYVO5Tuqs87Zbo2S3698E7/DozLphii1MX14hhX5HCf+58SdnFMO7ygXlD3PtY7yKkjS7MYdfhyNUpyd6yTpKqBv1Hj4Jc6+YRyRzPQcah3W86JV97ARofAIKeQiywQHhXAe1n/C9x7wDVrCN43Vl4NveMJ3jYjhW3heFfKYGBMdFL7NPMikFePXPl2dD2HjIkH0tJAc4VC6mj5MqRTa1nrOl/UDTNjdzYh92OEL/XmKoMwHfOtSA/aTm2U4ifpU0i4kCUr9keJ3seeGA5xlRJL0c8nS6y4m72CtP3jRJjwaDHn8hGkclGhr8ZDGw1u7oMYFFXuiTY5oMk/rlhoRA7cwCLBYtlhnh+V44LdptGGYdK7x7TR0k4cdMQhbQPz8lUU5uLcfCe45oEV4BjyEwzwsTyK/+u3kZ9jDwUirceRGsjyIlvaoEAuE4SOAh49Yh+UaLI9lnnSwNCjbjUO58lqzHESffIaLZH/iJFs+R1xPFUoLcYwm30iwDsvRWF45eZIWsqQFAm1JC2krdkt/PNLiZ8UAWtpvqIeYFKsH4xQdVKcSRAfJ1rc7RgfVBfw+D5nTeJhrHOav09Mei/IHuTWej0cF873uQl6t3KsgJGzbTrRRHviz5zAjOmf05eEtH5366oZT3L8YkFFiH4X5UX5Z4RqCTRVUFmkiOsJcpM2VJDZny6f1a29/WWbtiJbcx78QDZeS+/ZX8RT5kdwpZeSnXDSjc0jPJz2iG27g3m5PtNY8EDOYDpWbmIh3KaulBh4bE96dCPlVAVZjT5kbDCuaatqdQ76D7WGHLxhNQObHYDr+tRrs29kUz/CbFFvE1h5nQAMVBbnIWvtm2oY7FniIgN30bVcVSWEdkR2CJAMrbwjc6tGoDdeY8e0kG7YysdA6som1Dmdi63sNB3uDWdVQkXOGRPAeWCWdB0DYqEHrSFeN0jqiz+BKMI/4mTFbPyrrCB2UdZQf7wfW0PS8ERj5o6GuWZY7xL5l2BAixzctzV51C8AjMq32dubORi1ZVrYh3bISXtUL+Flo22zUsEJcqx7wRWyPx8oCThPwWbSyYFtWlnWyslbf573hIHjxDqFtt2NmoX2vxpmG1dEQYH1oYh/qnj20LdP0gDYCmu2MbM1r6zI7IBmXO6zlwYY9wOK1PNtpx0yTwxi+aY5M3fAdzXJ0sgZzXOQh5Fo6sg0DuVpbjAEPiDGO5J7qqsEl08qrXVNY2/5ySveeXNvOqNwWcN9nn8WXE0Ju2zkN3dJm0IM2Kjg9ttemK35vNpajrV4kSrOvHPlXs0GhgQW5/9JBTRlYeo6Vdi5nO+1bMbBYTYAlt6jyuIuWTClHP5lSq8XBUqh9feHEo0ecNqLmNOPIwuZ0yXjaYcGrr59WHqjgmC1MKzq2aTUOZ1o3XeHP78dy2rgCT7Ol3OHvupYGoTsE5lDXTWfoQB8PkY9MiHzf8fy2rFvtcLgC1K9SOljrVtb25eLOpXAAdus84z001IGFoW1amqY7uuO6hmUgG9lDxyIMCRd4b76go+ey53+XF3ZsPGinF0dj9UjkOXxsAVBNp9etpNSoPgd+dte3VbrCm3+vkExwkvPVhU4Jim8lARMVRSvqA/PZJWNN7yTgomZrH2tZcKVwYcv1iyRwww7Qlvc8KsvAfxva8/zPf0RrwV63/DGexsaBsQj7zCajR0QeNYj5GivlnVrjH8wL0P4slBQaaWQkR5jwrof9izTF4yERiyvYzy5Gggc3D2ZphsfqVRjWY5sXshP3G/n1TWSfg8iPv6UqXSymG+ski/VNNP8ah6tJ5mNGMJclcZhuQ/vSTfE2dHweFih7XdFQF5NASxALzk377DOlnqdViC7viL7Cft/s6l2oAV0xn0PrObKUT++LMgVJtVSCCb8RedGFBisGtOdQew6gcjEvx4kq5XLpYSAMrJGNgAagbprY9iwXIcc0PcO2HdcrypeihhfOxUa9O6Uk4SR/REHWfxt5dzgtSPKkCsmfOKGmVx9qmq0CFWpmQVnk5MSD/BnK/jQlzKYwHnjx9En1a8GKi8k11lsqQ1htnrbMN6I8xieLP7bIH6tz1CsiX+LbdA3FXHARoqdP6EGFdOJ6WGH56dMnPxh3sn8nU8JsnuKFbppyEuW5oFqSwj6xYj/mkm3+idfF6H79bZ5Roab/ZXdBqr4lk0k6Q2R32eJff3sxJ/x7/vGXBN+S2VTKNihXOA1uI5wot7mvibCmQg+wKc8UP1aiOFPGsR+MZr/gyP/ZsuznSSuIYc85qMJN7wiOpkQS9y8Hd24yKUjKZEZKzwet19j0EFEwwAkbiaT/f2+B+82BuQAAb4H7zYG5AAA="></cc1:stireportweb>

        
        <asp:SqlDataSource ID="sdsModuleHC" runat="server" ConnectionString="<%$ ConnectionStrings:BigMacEntities %>"
            SelectCommand="select @citidesc citidesc, @bonusdesc bonusdesc, @uname uname, STR_TO_DATE(@fdate,'%d/%m/%Y') as fdate, STR_TO_DATE(@tdate,'%d/%m/%Y') as tdate, staff.name, inv.createdate as Date , items.productcode , items.productdesc , inv.branchcode, inv.resourcecode, items.qty, items.servicecommission , (items.qty * items.servicecommission) subservicecommission, cust.cussupname as MemberName,cust.inhousecode as Membership, '' remark ,  HOUR(inv.createdate) chour , DATE(inv.createdate) cdate, DATE_ADD( DATE(inv.createdate),Interval HOUR(inv.createdate) HOUR) byHour from (select * from Invoice_m_Invoice where cast(createdate as date) between STR_TO_DATE(@fdate,'%d/%m/%Y') and STR_TO_DATE(@tdate,'%d/%m/%Y') and (@bcode like 'ALL%' or branchcode = @bcode) and ifnull(status,'') = 'CLOSE') inv inner join Invoice_m_Invoice_Items items on inv.id = items.invoiceid  and type != 'TopUP' inner join CusSup_m_CusSup cust on inv.cussupid = cust.id inner join Configuration_m_Branches branch on inv.branchcode = branch.branchcode inner join Common_m_Staff staff on staff.id= items.staffserviceid"  
            ProviderName="<%$ ConnectionStrings:BigMacEntities.ProviderName %>">
            

           <SelectParameters>
                <asp:QueryStringParameter Name="uname" QueryStringField="uname" 
                    DefaultValue="Admin" />
            </SelectParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="fdate" QueryStringField="fdate" 
                    DefaultValue="01/01/2014" />
            </SelectParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="tdate" QueryStringField="tdate" 
                    DefaultValue="31/05/2014" />
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
