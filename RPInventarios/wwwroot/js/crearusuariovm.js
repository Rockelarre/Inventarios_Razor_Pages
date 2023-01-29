; (function (app) {

    const $inputUsername = $('#Usuario_Username');
    $inputUsername.on('change', validarUsuario);

    function validarUsuario() {

        var valor = $(this).val();

        app.ServicioDatos.existeUsername(valor).then(response => {
            if (response.existeUsuario) {
                $(':input[type="submit"]').prop('disabled', true);
                var notyf = new Notyf();
                notyf.error('Lo sentimos, ya existe un usuario con la cuenta sugerida.');
                $inputUsername.addClass('border-danger');
            }
            else {
                $(':input[type="submit"]').prop('disabled', false);
                $inputUsername.removeClass('border-danger');
            }
        });
    }

}(window.app));
