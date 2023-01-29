; (function (app) {

    var ServicioDatos = {
        existeUsername: function (username) {
            return app.serviciosAjax.get('Usuarios/Create?handler=ExisteUsername', { username });
        },
    };

    app.ServicioDatos = ServicioDatos;

}(window.app));
