; (function (app) {

    var serviciosAjax = {
        servicioUrl: '',
        verbosHttp: {
            POST: 'POST',
            PUT: 'PUT',
            GET: 'GET',
            DELETE: 'DELETE'
        },
        asignarUrl: function (_servicioUrl) {
            serviciosAjax.servicioUrl = app.rootDir + _servicioUrl;
        },
        ejecutar: function (type, url, data) {

            var promise = $.Deferred();

            $.ajax({
                type: type,
                url: url,
                data: data,
                contentType: 'application/json; charset=utf-8',
                //dataType: 'json',                
                beforeSend: function (xhr) {
                    if ($('input:hidden[name="__RequestVerificationToken"]').length) {
                        xhr.setRequestHeader("RequestVerificationToken",
                            $('input:hidden[name="__RequestVerificationToken"]').val());
                    }
                },
                success: function (response) {
                    promise.resolve(response);
                },
                error: function (xhr, status, error) {
                    var error = 'Error: ' + error;
                    promise.reject(error);
                }
            });
            return promise;
        },
        get: function (_url, data) {
            var type = serviciosAjax.verbosHttp.GET,
                url = _url ? app.rootDir + _url : serviciosAjax.servicioUrl;
            if (data.id > 0) {
                url += data.id;
            }
            return this.ejecutar(type, url, data);
        },
        post: function (_url, data) {
            var type = serviciosAjax.verbosHttp.POST,
                url = _url ? app.rootDir + _url : serviciosAjax.servicioUrl;
            return this.ejecutar(type, url, data);
        },
    };

    app.serviciosAjax = serviciosAjax;

}(window.app));

