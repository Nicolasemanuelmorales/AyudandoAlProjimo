$(document).ready(function () {

    $("#formmon").show(1000);

    function Cambio() {
        if ($('#TipoDonacion').val() == 1) {

            $("#formmon").show(1000);
            $("#formins").hide(1000);
            $("#formhor").hide(1000);
        }
        if ($('#TipoDonacion').val() == 2) {

            $("#formmon").hide(1000);
            $("#formins").show(1000);
            $("#formhor").hide(1000);
        }
        if ($('#TipoDonacion').val() == 3) {

            $("#formmon").hide(1000);
            $("#formins").hide(1000);
            $("#formhor").show(1000);
        }
    }
    $('#TipoDonacion').on("change", Cambio);
});
