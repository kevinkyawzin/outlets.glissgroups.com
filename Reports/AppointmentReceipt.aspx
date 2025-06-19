<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AppointmentReceipt.aspx.cs" Inherits="BigMac.Reports.AppointmentReceipt" %>

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
   
            reportsource="H4sIABmtXGYA/+1d3W7bOBa+L9B3EDIXuwt4bPFPohrHg6adTottZ7pJZrrAYi8okkqFylYgyTPNLvbdl5QoR7aZxEksps4YbdqIPKRI8fvO4aEOqfEPX6eZ97ssyjSfHR2AoX/gyRnPRTo7PzqYV8n39MArKzYTLMtn8ujgUpYHP0yePxufVumpLFKWpf+RxUoN7OIiSzmr6hQleCIv8qI6UMU8b/w65TqDFZfeiUxUiQOvurxQVV9lHHhp+Xd5eXRQFXPZFNMFWcViVspS5b5Py8pkezyfz9SFuu+oK3qazwt+nTBoa1XCpSg/5GKeybevmhbBRYtUNWcszmRTl7VZdRUvs5SVk05F41GT1JF5lWfz6ey69kTd+pT07yybywmfl1U+lcWMTeXg9LKs5HR4WhVqcMajRuKGQmoUNy90pztM8zjN7lB3kfKNhRXYqnm5sbicsjS7S+VJsrF0nOdfpIgvNy6gcJ+ns2oqZ5Vg1eIBKRjJs3Qqb25YUVXpHcZAzsSd5IUseZFeaIbdpwzYHH53AysTopDl5gPOK7n5ePOEfd1ceCMsjUeGyN20jk5Ly1arjboSP6uHsqwg6pQViXezRtOsSy5yFmpr1JFp9d6oo/jatBOZ1Zp4A7XZ6Om2C92c35hS9UoR3laJasHiUTQJP2V5rI1E3Ybmod5ayUd2fqviroVAo7BRq7B12jVaenycF0IWk5+VGTs8zhj/cggPT/MsFYdvWFbKQ9ykjkdG8KpgMS8/T84KNisvWKG4rUTqpIXEq3x6oaqdVdc0OlgyFrqRbyVT9zDNx93mNznHyuJea242aVPTrixVWSeSKwN+nsmJP/CHcECGGA2CIVFIXslfLnxLpzBebpMqcia/VrDpE2n7pNNu6MnmvbH3aEgGoP7Bum83d8l0aybSzfjQKfUmn1UTrcNL72f5h3eST9lsAMDgRJ7PM1YMPmq9P6iBNPDHo1p8rZL36UyWvyS/ztQAZ+r3Govj0VryWsEPrDhPlcZR46f/jEdtgkXy66makE2MVHOxLpXOOlLmYk2qVlD1kK4pKyOh8dpqC2R9bh/r4WxlsFVG32Ly8sp0etpejkd1slW4wYZh61WCVfZNXkxZ1WAyaDH5k5zJgmVN3jI4rQ1UhSY/fr3QdkoBR91TJ6xgv24ItDECN3cP3TECDsGeEQvJHhiBHTDilZnGe/pOfdCB9k4HbKMDQM3tI3d8QMNgz4eFZA98AMgBIU7yfNoHD4DfOxEAsjLBWAYAXFIh2g0q/DRPxURgjEOQUCykjzkPGQoxwHHIOBGJ5Hw8qsWeDo9cGJZTvQjSC5Fg/0SymhRonA6AXBIJDfY2pZXsw+tw4Xb8qJd8euEC7t/ZsHob0DcNcOWBKwZEek1B/Y8cMwEPjvNMbEaDt3nxMkvPZ9rDnLxS/8hiPFpKfDrM8V077LJSNCp74ZEDp923eu3ENMCR3w7rdSxfee7OLQq9m0H5EzEJEwdMetELbxx498TKm8A0IHLHG7rnzUrBx+ZNsKu8gf0vBuDAxhsUmQYAV7yBNWcegzd7hqjh3lmG9O/lo8hqWczrUohcMUSvG+8Z8lg2BOwsQ/r3/TGwMYQYnwU6e/uuvP4Brv0W175/UPv+e56oQXfAk/92go2GdYTX/3phzr28/eYWxOqOEOOOwNAhJYYQGla4XhveOx96yJ0TwgQx9sMJ+gBOWF0NEpqaI5ecwI9jKPZTKT3g/TPivvBG/TvcJLSxwBgGBFzGZeHB7rx8RyGSPMFhCDnCgCCGCA1wxENBiS/94Km9fA9cBnW9ykUvQV2of+/calOMSUHIJZvCHWKTIBhK6EcRjyAOBYulhKFMuACI+SFBT41NDkzO5EO9D6kXGvXvwluNEjW3dxg+j3aIRIBgjiiGHDKKEWcxByRSdopGPsIRi54aiagDEv1cpLwXCvX/5p7aKGTeo6DQ5Q4UukMk4hIDHscYMMQxp4wGFPGEB4BxX9IIPjUSuXjT0mzv7IFE/b/Gt75raTfWOQzSx0O4QyRKwoiSMAFBhBgGgYhwIqGkCHKOYECeHIlcvIw5rvdbe8eXfTAJO4jytzHJ7HbBwCWTyA4xKaIgToBAjBGIhUQ0pjCWWIjAB0kQh0+NSchNiH817yUeEzt4+281SCasGSN34TH4G3v539AligRhAPkRVY6QH7KY+okIIqj+MobkdXT5Ey13A39XIwdw/8sOwBrtDNqjCIg7eoXfIr1oCH3CCQSEMBxGKA65SkIBZigCwA/39FJY2Vl69b8kAayTQGA2qGFnmwnQt0guxOIoDokkSE3wEBVxKJIoCEKs5n4BpsmeXAopO0uu/pcqgHXHGzBRbzhyFxf6bdJL2ayIMxAolwojzCM/Vpc4ZDiQYQCCPb0UVnaVXsTB+oU1gg6YQAnibGMCfrSNcDe/wWWYEz9GIYPKWMkkphISQaM4AIj78Z5eGis7Sy8HZxdYAyeAiZwgyB294DdJL8p9CEIeET/AKIYsJgEJAoFjHiMG9p6XxsrO0svBwoY1oAKYiApC3E0Oo2+RXkDIMETAp4QF2Bdx7CcwlkEMaIR8n4M9vRRWdpZeDhY2rMEWwERbkNDl661dirYIQIIQIIhC5GMUQIZjX4cxKTtHfIifXNwfcBFu8frqYOpe6ORgKcMadgHNMjyJ3E0G6bdorRLJeAAJBpHPMKEwCiI/lHEgRYJZEIq9tVJY2VVrFfS/lAGty/DQrBQGrpYyhs0pC+gxCKZf0zyxE+Hc73TtfH+jl819wb2WHQzGrct10CzXBcglxvEe41vCePBYGNefi+kH4/gBGLeumUGzZhYQlxgP9xjfEsZD1xhvPljUD7ofcGQBtC5ZQbNkFYTu0I322N4StqlrbOuva/WD7AccPACtq0XQrBYFkUNkD/fY3ha2I9fYrr/I1Qu4Q/8B4LYfT2ZivEKHzqVevWnAHe3BvSp1x+PGoGtwd76u1w/EH+BbYvvZ4mYzS+jQt2xOdw32+nsL+hu5hvjis5b9APwhjqV1lwkyu0xCZ44l3Znji+sFdyxDxCBjfhDrL7GIGIpY+InkkGCfIrhfl1cYcsCy73vhU/9vkZF19wlqp07OPN7I7Nnf+wXbQLzzqZP5+HE/VuUBbi+yT5vaL0A6dXujvdu7pWkTfoRpU5L0Am76ELfX+u0tZEIWqFO3F+7BvSXdDVyDO65PeYgv+8H3A3xeZD+/wfi8FLnEN9nje0v4fgyft5r3cxQwfYDDi+wOr5maUOJyRWeP7m2hGz+5iBi6cEL1Z+B78EDtsxgTGUZdvnMdBnsebIkHziPD+sP/Q9xPa0QYMhFh1Olx74g+zicQbo4rJgIw6EsU0lhiyClLABaSUcS4IJL4+/VLBRcX5yXKzHuxbFgqmf3P896wr6sZCfuqMuov9a5mNe+Hnz/rg4hR/1HKyP4lOBPdFjk7Mk6fZhA+1hdL/hxfs0bhk5upRQ6OglsOkFPQzKcXasBn1fK43R2RepTfyvT8czVpRtlcLMuks47M4mJJph5UPYZvJVPgsy2n3DzEK8O7kj8ederupuvHYyat0aZLE5spiTUFAVAd+6q3tW7wAZd7jMUDX0+ag/FEGHIuggRDiXHCojDgIo4Y45QIymxbhu5o1u+lfDZTPLcrndsVTud7L1sGYatf/lHZjmbdVImsK5B7LGfUlfxSB82Uk7d59UVefixkkn490mMx8PRYvE+naXXU4MU70ZQ9y9/LZJF0VqTTaTo7N0U+5YX4VLCLozN1v4H3UiP6yB94b9KirM5Y/EuSlLI6wirptUIzm3F5LKs/pJypzPII+oOmx22rlpr7myyqdYgtp3bZXg/fKs+xCUuISL9Ep/rg2M2CEbbG8mv5jCKKAJY+9CHBsZqkY58nmEuIWQR4AP9EfMZ+T4R+sVU6B/eh8y1zAROO1g0bqDmVcpbpcfpYaLKnv0uzkBktFnBOq3Q6z8o8qYYn8iIvquHV5GGoMq213MSpVToE9c4+7ev6G0RgKsJkedE+0ubCAnoqiA8ZR5QCTOMowhSjkGNBIUCBsPmn24Oa/blufUJjv01X5FSHjdVWf5HfTnXopsNrqeNeg6sf2Y3jurXHb+v19nmvkqWS4PIs3xBxqyW6Y2lrc1fgx5mwj2O06Tiu1XDHUQTKr0Uuh3G9y9/6IK63eJFt9bjuZvsbxRaimFEYIYJFgEMKqDLqxEcJD4KQKxO/otiurGx9WNXiZ93aXjlgq67X9c943PhUtTNHh3o7WidhSehTKqrPEzKkqJFprjsiF7KocfCSaIH2aiHQQLptxPJz+cTU9GTKii8NJYDvb8qJRclrudBMtl4WKcsGquK1SVbHyP+L+C/qP/+2WfrxaHEzk9Y8iMYXbX5vBmOs4KMnXKeyqtTkumx7Ba7vVV1Cd2ilqHXKcAV68bIs5TRWbL4Gf7R9FOPfWTZXmvWyrOR0+DrLxqMmxZpdsD/U3W8T+5TORP5HOdSzm/LWOlnFbpP55zS7XuTqmSnSVUWelZvIHrNSbiJnxmFFsqMtOo+6HQRdQs2nWTlpftfSV2kdoVeflZqVYhKM4EgxHHvkBUIvMPU+fmjLtCLdUoVUgBMTMoKqoO+HHngB6As/6hYzMp1itfaQYaSPkItCDgRGGEVxQlEcSZrgBEMRt+UXqsYUrtXGcm8WmsSI/DpLq8m7Gf8sy1akTuqIqHmNnsJOVKPpEAyh/kLmck4tfFpv+pi8m+qc0mtA8PzZ8nULxrX0JfStl1Jw6ySug8ea2aBl7YarMLkhy/x3413WNNnzZ8+f6cesPFQuvSa7bJDa/PtxroDHvVcZK0uvbUOrwd7NPssitd5CpaxKX/1mKj2dx9oL/evflpcdP1zWD0PlrKTL4Ts13EqdKvW+6MKVjDKhusqrhO9O5Lkacu9g0RjvtSyVFyoL77x2jxSAPb3n3vveE7k3yytvmos0uTz4Tldmiv/lzsWbJugq6uf2/Jn+dfGY1YytgV8Hiu8VB+dKjU9+O26zF0mNWHWZXadq4ULVvk2FkLNW68PrtX49OVc1XmO8bl+76Bq31fWDZjrQtGV1PnBl3ZYqXzV641FTfLlj+i4Lm4Ye0Lu8ELKYvMyyw9oXPASHp3mWisN6ceoQHxoP0cht/6nUHXngo6nrMLaiQcdEv4TTfZdFQ5Ji8n/jBXNHXKoAAOMFc0dcqgAA"></cc1:stireportweb>
        <asp:SqlDataSource ID="sdsModuleHC" runat="server" ConnectionString="<%$ ConnectionStrings:BigMacEntities %>"

           SelectCommand="SELECT * FROM schedule_m_appointment"
                         
            
            ProviderName="MySql.Data.MySqlClient">
         
              <SelectParameters>
                <asp:QueryStringParameter Name="id" QueryStringField="id" 
                    DefaultValue="0" />
               <asp:QueryStringParameter Name="cname" QueryStringField="cname"
                    DefaultValue="Kyaw" />
               <asp:QueryStringParameter Name="caddress" QueryStringField="caddress"
                    DefaultValue="" />
               <asp:QueryStringParameter Name="ctel" QueryStringField="ctel"
                    DefaultValue="" />
               <asp:QueryStringParameter Name="cfax" QueryStringField="cfax"
                    DefaultValue="" />
               <asp:QueryStringParameter Name="cemail" QueryStringField="cemail"
                    DefaultValue="" />
            </SelectParameters>
        </asp:SqlDataSource>

        <cc3:stiwebviewer id="wvModuleHC" runat="server"></cc3:stiwebviewer>
    
    </div>
    </form>
</body>
</html>

<%--  WHERE app.id = @id;--%>
