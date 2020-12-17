$(document).ready(function () {
    // GET GENDER
    var gender = -1;
    $("#male").click(function () {
        gender = 0;
    });
    $("#female").click(function () {
        gender = 1;
    });
    $("#another").click(function () {
        gender = 2;
    });

    $("#signIn").click(function (event) {
        // Prevent From from submit
        event.preventDefault();
        // Get DOB
        var dayDoB = $("#dayDoB").val();
        var monthDoB = $("#monthDoB").val();
        var yearDoB = $("#yearDoB").val();
        $("#DoB").val(yearDoB + "-" + monthDoB + "-" + dayDoB);
        $("#signUpForm").submit();
    });
});