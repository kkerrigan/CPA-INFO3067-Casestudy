$(function () {
    if ($("#register_popup") !== undefined) {
        $('#register_popup').modal('show');
    }
    if ($("#login_popup") != undefined) {
        $('#login_popup').modal('show');
    }
    // display message if modal still loaded i
    if ($('#detailsId').val() !== '') {
        console.log($('#detailsId').val());
        var data = $('#modalbtn' + $('#detailsId').val()).data('details');
        CopyToModal($('#detailsId').val(), data);
        $('#details_popup').modal('show');
    } //details
    // details anchor click - to load popup on catalogue
    $("a.btn-default").on("click", function (e) {
        var Id = $(this).data("id");
        var data = $(this).data('details');
        $("#results").text("");
        CopyToModal(Id, data);
    });
});
function CopyToModal(id, data) {
    $("#qty").val("0");
    $("#prodName").text(data.ProductName);
    $("#price").text("$" + data.MSRP);
    $("#description").text(data.Description);
    $("#detailsGraphic").attr("src", "/img/" + data.GraphicName);
    $("#detailsId").val(id);
}

$(document).on('submit', '#brandsForm', function () {
    var $theForm = $(this);
    // manually trigger validation
    $.post('/Brand/SelectProduct', $theForm.serialize())
        .done(function (response) {
            $('#results').text(response);
        })
    return false;
});

$('.nav-tabs a').on('show.bs.tab', function (e) {
    if ($(e.relatedTarget).text() === 'Demographic') { // tab 1
        $('#Firstname').valid()
        $('#Lastname').valid()
        $('#Age').valid()
        $('#CreditcardType').valid()
        if ($('#Firstname').valid() === false || $('#Lastname').valid() === false || $('#Age').valid() == false || $('#CreditcardType').valid() === false) {
            return false; // suppress click
        }
    }
    if ($(e.relatedTarget).text() === 'Address') { // tab 2
        $('#Address1').valid()
        $('#City').valid()
        $('#Region').valid()
        $('#Country').valid()
        $('#Mailcode').valid()
        if ($('#Address1').valid() === false || $('#City').valid() === false || $('#Region').valid() === false || $('#Country').valid() === false || $('#Mailcode').valid() === false) {
            return false; // suppress click
        }
    }
    if ($(e.relatedTarget).text() === 'Account') { // tab 3
        $('#Email').valid()
        $('#Password').valid()
        $('#RepeatPassword').valid()
        if ($('#Email').valid() === false || $('#Password').valid() === false || $('#RepeatPassword').valid() === false) {
            return false; // suppress click
        }
    }
}); // show bootstrap tab