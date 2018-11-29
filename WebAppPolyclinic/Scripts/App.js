
jQuery(document).ready(function ($) {
    $("#doctor-toggle .form-check-input").click(function () {
        $("#doctor-fields").toggleClass("collapse");
        });

    $("#patient-toggle .form-check-input").click(function () {
        $("#patient-fields").toggleClass("collapse");
    });
    isChecked = function (selector, target) {
        if ($(selector).is(":checked")) {
            console.log($(selector));
            $(target).removeClass("collapse");
        }
    }

    isChecked("#doctor-toggle .form-check-input", "#doctor-fields");
    
    isChecked("#patient-toggle .form-check-input", "#patient-fields");
})