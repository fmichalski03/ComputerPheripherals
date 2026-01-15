// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Allow comma or dot as decimal separator in jQuery validation.
$(function () {
    if ($.validator && $.validator.methods) {
        $.validator.methods.number = function (value, element) {
            return this.optional(element) || /^-?\d+(?:[.,]\d+)?$/.test(value);
        };
    }
    if ($.validator && $.validator.messages) {
        $.extend($.validator.messages, {
            required: "To pole jest wymagane.",
            remote: "Wprowad\u017A poprawn\u0105 warto\u015B\u0107.",
            email: "Wprowad\u017A poprawny adres e-mail.",
            url: "Wprowad\u017A poprawny adres URL.",
            date: "Wprowad\u017A poprawn\u0105 dat\u0119.",
            dateISO: "Wprowad\u017A poprawn\u0105 dat\u0119 (RRRR-MM-DD).",
            number: "Wprowad\u017A poprawn\u0105 liczb\u0119.",
            digits: "Wprowad\u017A same cyfry.",
            creditcard: "Wprowad\u017A poprawny numer karty.",
            equalTo: "Wprowad\u017A ponownie t\u0119 sam\u0105 warto\u015B\u0107.",
            maxlength: $.validator.format("Wprowad\u017A nie wi\u0119cej ni\u017C {0} znak\u00F3w."),
            minlength: $.validator.format("Wprowad\u017A co najmniej {0} znak\u00F3w."),
            rangelength: $.validator.format("Wprowad\u017A warto\u015B\u0107 o d\u0142ugo\u015Bci od {0} do {1} znak\u00F3w."),
            range: $.validator.format("Wprowad\u017A warto\u015B\u0107 z zakresu {0} do {1}."),
            max: $.validator.format("Wprowad\u017A warto\u015B\u0107 mniejsz\u0105 lub r\u00F3wn\u0105 {0}."),
            min: $.validator.format("Wprowad\u017A warto\u015B\u0107 wi\u0119ksz\u0105 lub r\u00F3wn\u0105 {0}.")
        });
    }
});
