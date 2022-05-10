$(document).ready(function () {
    debugger;
    $.ajax({
        url: "ReturnUsers",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        datatype: JSON,
        success: function (result) {
            $(result).each(function () {
                $("#AllUsers").append($("<option></option>").val(this.Id).html(this.FirstName + " " + this.LastName));
            });
        },
        error: function (data) { }
    });
    $.ajax({
        url: "ReturnOffers",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        datatype: JSON,
        success: function (result) {
            $(result).each(function () {
                $("#AllOffers").append($("<option></option>").val(this.Id).html(this.Title));
            });
        },
        error: function (data) { }
    });
    $.ajax({
        url: "ReturnRequsts",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        datatype: JSON,
        success: function (result) {
            $(result).each(function () {
                $("#AllRequests").append($("<option></option>").val(this.Id).html(this.Title));
            });
        },
        error: function (data) { }
    });
});
