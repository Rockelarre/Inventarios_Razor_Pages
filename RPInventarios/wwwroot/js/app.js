; (function (window) {
    window.app = window.app || {};

    window.app.rootDir = window.location.protocol + "//" + window.location.host + "/";

    var $loading = $('#ajax-loader').hide();

    $(document)
        .ajaxStart(function () {
            $loading.show();
        })
        .ajaxStop(function () {
            $loading.hide();
        });


}(window));
