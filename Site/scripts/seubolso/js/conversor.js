$(document).ready(function () {

    $.ajax({
        url: 'http://api.promasters.net.br/cotacao/v1/valores',
        type: "GET",
        success: function (data) {
            rates = { USD: data.valores.USD.valor, EUR: data.valores.EUR.valor, GBP: data.valores.GBP.valor };
            // checa se money.js foi carregado:
            if (typeof fx !== "undefined" && fx.rates) {
                fx.rates = rates;
                fx.base = "BRL";
            } else {
                // se, aplica o fxSetup global:
                var fxSetup = {
                    rates: rates,
                    base: "BRL"
                }
            }
        }
    });

    $("#conversor-valor").maskMoney({ showSymbol: false, decimal: ",", thousands: "." });

    //conversão de moeda para o resultado
    var valorConvert = 0;
    var de = "";
    var para = "";
    $("#conversor-valor").keyup(function () {
        converter();
    });
    $("#conversor-de").change(function () {
        converter();
    });
    $("#conversor-para").change(function () {
        converter();
    });
});

function converter() {
    var valorConvert = 0;
    var de = "";
    var para = "";

    de = $("#conversor-de :selected").val().split("-");
    para = $("#conversor-para :selected").val().split("-");
    valorConvert = fx.convert($("#conversor-valor").val().replace(/\./g, '').replace(/\,/g, '') / 100, { from: para[0].trim(), to: de[0].trim() });
    $("#conversor-resultado").html(valorConvert.toFixed(2));
}