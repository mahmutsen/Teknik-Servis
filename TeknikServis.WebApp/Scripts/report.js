var productid = -1;
var modalReportBodyId = "#modal_report_Body";
$(function () {

    $('#modal_report').on('show.bs.modal', function (e) {

        var btn = $(e.relatedTarget);
        productid = btn.data("product-id") //Elementin data atrrbiuteu okunur
        $(modalReportBodyId).load("/Report/ShowProductReports/" + productid);
    })
});

function doReport(btn, e, reportid, spanid) {

    var button = $(btn);
    var mode = button.data("edit-mode");

    if (e === "edit_clicked") {

        if (!mode) {

            button.data("edit-mode", true);
            button.removeClass("btn-warning");
            button.addClass("btn-success");
            var btnSpan = button.find("span");
            btnSpan.removeClass("glyphicon-edit");
            btnSpan.addClass("glyphicon-ok");

            $(spanid).addClass("editable");
            $(spanid).attr("contenteditable", true);
            $(spanid).focus();

        } else {
            button.data("edit-mode", false);
            button.addClass("btn-warning");
            button.removeClass("btn-success");
            var btnSpan = button.find("span");
            btnSpan.addClass("glyphicon-edit");
            btnSpan.removeClass("glyphicon-ok");

            $(spanid).removeClass("editable");
            $(spanid).attr("contenteditable", false);

            var txt = $(spanid).text();

            $.ajax({
                method: "POST",
                url: "/Report/Edit/" + reportid,
                data: { text: txt }
            }).done(function (data) {

                if (data.result) {
                    //Raporlar partial a tekrar yüklenir.
                    $(modalReportBodyId).load("/Report/ShowProductReports/" + productid);
                }
                else {
                    alert("Rapor Güncellenemedi.");
                }
            }).fail(function () {
                alert("Sunucu ile bağlantı kesildi.");
            });
        }
    }
    else if (e === "delete_clicked") {
        var dialog_res = confirm("Rapor silinsin mi?");
        if (!dialog_res) return false;

        $.ajax({
            method: "GET",
            url: "/Report/Delete/" + reportid
        }).done(function (data) {

            if (data.result) {
                $(modalReportBodyId).load("/Report/ShowProductReports/" + productid);
            } else {
                alert("Yorum silinemedi.")
            }
        }).fail(function () {
            alert("Sunucu ile bağlantı kurulamadı.");
        })
    } else if (e === "new_clicked") {

        var txt = $("#new_report_text").val();

        $.ajax({
            method: "POST",
            url: "/Report/Create/",
            data: { "text": txt, "productid": productid }
        }).done(function (data) {

            if (data.result) {
                //tüm raporlar tekrar yüklenir
                $(modalReportBodyId).load("/Report/ShowProductReports/" + productid);
            } else {
                alert("Rapor eklenemedi.")
            }
        }).fail(function () {
            alert("Sunucu ile bağlantı kurulamadı.");
        })
    }

}