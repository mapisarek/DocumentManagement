// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
let today = new Date().toISOString().substr(0, 10);
document.querySelector("#todayDate").value = today;

$(function () {
    $("#tags input").on({
        focusout: function () {
            var txt = this.value.replace(/[^a-z0-9\+\-\.\#]/ig, ''); // allowed characters
            if (txt) $("<span/>", { text: txt.toLowerCase(), insertBefore: this });
            this.value = "";
        },
        keyup: function (ev) {
            // if: comma|enter (delimit more keyCodes with | pipe)
            if (/(188|13)/.test(ev.which)) $(this).focusout();
        }
    });
    $('#tags').on('click', 'span', function () {
        if (confirm("Remove " + $(this).text() + "?")) $(this).remove();
    });

});