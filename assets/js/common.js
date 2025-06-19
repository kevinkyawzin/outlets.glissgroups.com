
    function getDateFromAspString(aspString) {
        var epochMilliseconds = aspString.replace(
            /^\/Date\(([0-9]+)([+-][0-9]{4})?\)\/$/,
            '$1');
        if (epochMilliseconds != aspString) {
            return new Date(parseInt(epochMilliseconds));
        }
    }

    Number.prototype.round = function (p) {
        p = p || 10;
        return parseFloat(this.toFixed(p));
    };

    function IsNumeric(vlu) {
        if (isNaN(vlu) || !(/^-?\d+$/.test(vlu)) || parseInt(vlu) < 0) return false;
        return true;
    }

    function getQueryStrings() {
        var assoc = {};
        var decode = function (s) { return decodeURIComponent(s.replace(/\+/g, " ")); };
        var queryString = location.search.substring(1);
        var keyValues = queryString.split('&');

        for (var i in keyValues) {
            var key = keyValues[i].split('=');
            if (key.length > 1) {
                assoc[decode(key[0])] = decode(key[1]);
            }
        }

        return assoc;
    }


    function getReportHeader(branchcode) {
        var content = "";
        content += '	<div style="float:left;"><img style="position:absolute; max-width: 120px; max-height: 120px; width: auto;  height: auto;" src="../images/gliss_logo.png"></div>'
        content += '	<div style="text-align:center;"> Gliss Wellness Group Pte. Ltd.</div>	'
       
        switch (branchcode) {
            case 'GWG0410':
                content += '	<div style="text-align:center;"> 6 Eu Tong Sen St,( The Central ) #04-10/22</div>	'
                content += '	<div style="text-align:center;"> Tel   : 6937 8127 / 6937 8127</div>	'
                content += '	<div style="text-align:center;"> Email : info@glisswellness.com</div>	'
                content += '	<div style="text-align:center;"> Uen   : 202331809E </div>	'
                break;
            case 'GWG0375':
                content += '	<div style="text-align:center;"> 6 Eu Tong Sen St,( The Central ) #03-70/71/72/75 & #04-11/12/53'
                content += '	<div style="text-align:center;"> Tel   : 6373 0127 / 6358 4127</div>	'
                content += '	<div style="text-align:center;"> Email : info@glisswellness.com</div>	'
                content += '	<div style="text-align:center;"> Uen   : 202349313H </div>	'
                break;

            default:

               
        }

        content += '	<br>	'
       

        return content;
    }


    function printMemberCardFunction(btn) {
        //var desc = $('#txtMemberDesc').val();
        var rcode = $(btn).attr("data-rcode");
        window.open("../CusSup/PrintCardList?rcode=" + rcode, "_self");
    }

    function memberCardCollectionFunction(btn) {
        //var desc = $('#txtMemberDesc').val();
        var rcode = $(btn).attr("data-rcode");
        window.open("../CusSup/CollectionCardList?rcode=" + rcode, "_self");
    }

    function rptProductListFunction(btn) {
        var rcode = $(btn).attr("data-rcode");
        window.open("../Report/ProductListingReport?rcode=" + rcode, "_self");
    }

    function rptVoidTransactionListFunction(btn) {
        var rcode = $(btn).attr("data-rcode");
        window.open("../Report/VoidTransactionReport?rcode=" + rcode, "_self");
    }

    function rptMemberTransactionListFunction(btn) {
        var rcode = $(btn).attr("data-rcode");
        window.open("../Report/MemberTransactionReport?rcode=" + rcode, "_self");
    }

    function rptServiceFunction(btn) {
        var rcode = $(btn).attr("data-rcode");
        window.open("../Report/ServiceReportByBranch?rcode=" + rcode, "_self");
    }

    function rptServiceCommissionFunction(btn) {
        var rcode = $(btn).attr("data-rcode");
        window.open("../Report/SalesCommissionByBranch?rcode=" + rcode, "_self");
    }

    function rptDailySalesFunction(btn) {
        //var desc = $('#txtProductDesc').val();
        var rcode = $(btn).attr("data-rcode");
        window.open("../Report/DailySalesByBranch?rcode=" + rcode, "_self");
    }
    //added by alexis on 26 Feb 2015
    function rptNewMember(btn) {
        var rcode = $(btn).attr("data-rcode");
        window.open("../Report/NewMember?rcode=" + rcode, "_self");
    }
    //added by zawzo on 26 Feb 2015
    function rptMembersListByDOB(btn) {
        var rcode = $(btn).attr("data-rcode");
        window.open("../Report/MembersListByDOB?rcode=" + rcode, "_self");
    }
    //added by zawzo on 10 Feb 2016
    function rptCustomerAttendanceList(btn) {
        var rcode = $(btn).attr("data-rcode");
        window.open("../Report/CustomerAttendanceList?rcode=" + rcode, "_self");
    }
    //added by zawzo on 10 Feb 2016
    function rptCustomerTransferList(btn) {
        var rcode = $(btn).attr("data-rcode");
        window.open("../Report/CustomerTransferList?rcode=" + rcode, "_self");
    }
    function rptDailyRedeemFunction(btn) {
        //var desc = $('#txtProductDesc').val();
        var rcode = $(btn).attr("data-rcode");
        window.open("../Report/DailyRedemptionByBranch?rcode=" + rcode, "_self");
    }

    function CreateMemberFunction(btn) {
        var rcode = $(btn).attr("data-rcode");
        window.open("../CusSup/CreateMember?id=0&rcode=" + rcode, "_self");
    }

    function MemberSearchFunction(btn) {
        var desc = $('#txtMemberDesc').val();
        var rcode = $(btn).attr("data-rcode");
        if (desc == undefined)
            desc = "";
        //alert(desc);
        window.open("../CusSup/MemberListing?rcode=" + rcode + "&filter=" + desc, "_self");
    }

    function CustomerTransferFunction(btn) {
        //var desc = $('#txtMemberDesc').val();
        var rcode = $(btn).attr("data-rcode");
        window.open("../CusSup/CitiMemberListing?rcode=" + rcode, "_self");
    }

    function TopUpFunction(btn, midFlag, mcode) {

        var rcode = $(btn).attr("data-rcode");
        if (midFlag == "1")
            window.open("../sales/TopUp?rcode=" + rcode + "&mid=" + $('#id').val(), "_self");
        else if (midFlag == "2")
            window.open("../sales/TopUp?rcode=" + rcode + "&mcode=" + mcode, "_self");
        else if (midFlag == "3")
            window.open("../sales/TopUp?rcode=" + rcode + "&mid=" + mcode, "_self");
        else {
            var cno = $('#txtTopUpMember').val();
            window.open("../sales/TopUp?rcode=" + rcode + "&cardno=" + cno, "_self");
        }
    }

    function RedeemFunction(btn, midFlag, mcode) {

        var rcode = $(btn).attr("data-rcode");
        if (midFlag == "1")
            window.open("../sales/Redeem?rcode=" + rcode + "&mid=" + $('#id').val(), "_self");
        else if (midFlag == "2")
            window.open("../sales/Redeem?rcode=" + rcode + "&mcode=" + mcode, "_self");
        else if (midFlag == "3")
            window.open("../sales/Redeem?rcode=" + rcode + "&mid=" + mcode, "_self");
        else {
            var cno = $('#txtRedeemMember').val();
            window.open("../sales/Redeem?rcode=" + rcode + "&cardno=" + cno, "_self");
        }
    }

    function PackageRedeemFunction(btn) {

        var rcode = $(btn).attr("data-rcode");
        //if (midFlag == "1")
        //    window.open("../sales/RedeemPackage?rcode=" + rcode + "&mid=" + $('#id').val(), "_self");
        //else if (midFlag == "2")
        //    window.open("../sales/RedeemPackage?rcode=" + rcode + "&mcode=" + mcode, "_self");
        //else if (midFlag == "3")
        //    window.open("../sales/RedeemPackage?rcode=" + rcode + "&mid=" + mcode, "_self");
        //else {
        var cno = $('#txtPkgRedeemMember').val();
        if (cno.trim() == "")
            window.open("../sales/RedeemPackage?rcode=" + rcode + "&id=0" , "_self");
            else
            window.open("../sales/RedeemPackage?rcode=" + rcode + "&cardno=" + cno, "_self");
        //}
    }

    function TransferProductFunction(btn) {
        //var desc = $('#txtMemberDesc').val();
        var rcode = $(btn).attr("data-rcode");
        window.open("../Product/ProductCitiBellaList?rcode=" + rcode, "_self");
    }

    function CompleteTransferFunction(btn) {
        //var desc = $('#txtMemberDesc').val();
        var rcode = $(btn).attr("data-rcode");
        window.open("../Product/PostedProductCitiBellaList?rcode=" + rcode, "_self");
    }

    function PromotionListingFunction(btn) {
        //var desc = $('#txtMemberDesc').val();
        var rcode = $(btn).attr("data-rcode");
        window.open("../Product/PromotionListing?rcode=" + rcode, "_self");
    }

    function PromotionSaveFunction(btn) {
        //var desc = $('#txtMemberDesc').val();
        var rcode = $(btn).attr("data-rcode");
        window.open("../Product/PromotionSave?id=0&rcode=" + rcode, "_self");
    }

    function MemberCheckInOut(inFlag) {
        var cNo = "";
        //alert(inFlag);
        if (inFlag == '1') {
            if ($('#txtCheckInMember').val().trim() == "") { alert("Please key in cardno!"); return false; }
            cNo = $('#txtCheckInMember').val();
        }

        else {
            if ($('#txtCheckOutMember').val().trim() == "") { alert("Please key in cardno!"); return false; }
            cNo = $('#txtCheckOutMember').val();
        }


        $.ajax({
            url: '@Url.Action("MemberCheckInOut", "CusSup")',
            data: { cno: cNo, inflag: inFlag },
            type: 'POST',
            success: function (data) {
                alert(data);
            },
            error: function (ts) { alert(ts.responseText) }
        });
    }

    function BranchStaffFunction(btn) {
        //var desc = $('#txtMemberDesc').val();
        var rcode = $(btn).attr("data-rcode");
        window.open("../Common/BranchStaffListing?rcode=" + rcode, "_self");
    }
    function GroupReportFunction(btn) {
        //var desc = $('#txtMemberDesc').val();
        var rcode = $(btn).attr("data-rcode");
        //alert($('#branchsale').val().trim());
        //if ($('#branchsale').val().trim() == "1")
            window.open("../Report/BranchSalesReport?rcode=" + rcode, "_self");
        //else
        //    window.open("../Report/ReportGroupSummary?rcode=" + rcode, "_self");
    }
    
    //function BranchSalesReportFunction(btn) {        
    //    var rcode = $(btn).attr("data-rcode");
    //    window.open("../Report/BranchSalesReport", "_self");
    //}

    function CampaignDetailFunction(btn) {
        //var desc = $('#txtMemberDesc').val();
        //alert('dtl');
        var rcode = $(btn).attr("data-rcode");
        ////window.open("../Campaign/CampaignDetail?id=1&rcode=" +  rcode, "_self");
        //if ($('#possale').val().trim() == "1")
            window.open("../Campaign/CampaignDetail?id=1&rcode=" + rcode, "_self");
        //else
        //    window.open("../Campaign/CampaignDetailManual?id=1&rcode=" + rcode, "_self");
    }

    function POSFunction(btn) {
        //var desc = $('#txtMemberDesc').val();
        //alert('dtl');
        var rcode = $(btn).attr("data-rcode");
        window.open("../POS/POS", "_self");
    }


    function PackageFunction(btn) {
        //var desc = $('#txtMemberDesc').val();
        //alert('dtl');
        var rcode = $(btn).attr("data-rcode");
        window.open("../Product/ProductSave?id=0&amp;ptype=Package", "_self");
    }


    function StaffScheduleFunction(btn) {
        var rcode = $(btn).attr("data-rcode");
        window.open("../StaffSchedule/Listing", "_self");
    }
