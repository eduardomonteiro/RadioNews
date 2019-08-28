$('#rm-fixa').maskMoney();
$('#rm-variavel').maskMoney();
$('#vl-gasto').maskMoney();
var formato = { minimumFractionDigits: 2, style: 'currency', currency: 'BRL' }
var ctx = document.getElementById("myChart");
var labels = []; //Conterá as fatias do gráfico. Ex.: Salão, Casa, Transporte, etc.
var values = []; //Conterá os valores das fatias.
var colors_rgb = [] //Conterá as cores de cada fatia.
var colors = [] //Conterá as cores de cada fatia.
var total = 0.0;

$(document).ready(function () {
    var max_fields = 20; //maximum input boxes allowed
    var wrapper = $(".list-itens-renda"); //Fields wrapper
    var add_button = $("#add-item-renda"); //Add button ID
    var valselect = 0; // Valor select
    var valinput = 0; // Valor input
    var valcor = 0; // data-cor select
    var x = 1; //initlal text box count
    $(add_button).click(function (e) { //on add input button click
        e.preventDefault();

        if ($('#tp-gasto').val() == "0" || $('#tp-gasto').val() == "" || $('#tp-gasto').val() == "R$0,00" || $('#tp-gasto').val() == "R$ 0,00") {
            $("#tp-gasto").addClass("input-erro");
            return false;
        } else {
            $("#tp-gasto").removeClass("input-erro");
        }

        if ($('#vl-gasto').val() == "0" || $('#vl-gasto').val() == "" || $('#vl-gasto').val() == "R$0,00" || $('#vl-gasto').val() == "R$ 0,00") {
            $("#vl-gasto").addClass("input-erro");
            return false;
        } else {
            $("#vl-gasto").removeClass("input-erro");
        }

        valselect = $("#tp-gasto").val();
        valinput = $("#vl-gasto").val();
        valcor = $("#tp-gasto").find(':selected').data('cor');
        valrgb = $("#tp-gasto").find(':selected').data('rgb');
        $(".content-scroll").mCustomScrollbar();
        if (x < max_fields) { //max input box allowed
            x++; //text box increment
            addToChart(valselect, valinput, valcor, valrgb);
            $(wrapper).append('<div data-input="' + valinput + '" data-type="' + valselect + '" class="item ' + valcor + '"><strong>' + valinput + '</strong> de ' + valselect + '<div class="remove_field close fa fa-times"></div></div>');
        }
    });
    $(wrapper).on("click", ".remove_field", function (e) { //user click on remove text
        removeFromChart($(this).parent('div').data('type'), $(this).parent('div').data('input'));
        e.preventDefault(); $(this).parent('div').remove(); x--;
    })
    $("#comparareconomias").on("click", function (e) {
        if ($('#rm-fixa').val() == "0" || $('#rm-fixa').val() == "" || $('#rm-fixa').val() == "R$0,00" || $('#rm-fixa').val() == "R$ 0,00") {
            $("#rm-fixa").addClass("input-erro");
            return false;
        } else {
            $("#rm-fixa").removeClass("input-erro");
        }
        if (total <= 0) {
            $("#lista-de-valores").addClass("input-erro");
            return false;
        } else {
            $("#lista-de-valores").removeClass("input-erro");
        }

        e.preventDefault();
        $('.fs-content .stage-1').fadeOut(0);
        $('.fs-content .stage-2').fadeIn(400);
        compareWithOthers();
        callSlide();
    });
});

var callSlide = function() {
    $('.content-pontuacao-grafico .list-grafico').slick({
        infinite: true,
        slidesToShow: 9,
        slidesToScroll: 1,
        prevArrow: '<div class="fa fa-angle-left arrows"></div>',
        nextArrow: '<div class="fa fa-angle-right arrows"></div>',
        dots: false,
        responsive: [
        {
            breakpoint: 800,
            settings: {
            slidesToShow: 6,
            slidesToScroll: 1,
            infinite: true,
            dots: false
            }
        },
        {
            breakpoint: 600,
            settings: {
            slidesToShow: 3,
            slidesToScroll: 3,
            arrows: false,
            }
        },
        {
            breakpoint: 480,
            settings: {
            slidesToShow: 2,
            slidesToScroll: 2,
            arrows: false,
            }
        }
        ]
    });
};
function compareWithOthers() {
    var rendaFixa = parseFloat($('#rm-fixa').val().replace('R$ ', '').replace('.', '').replace(',', '.'));
    var rendaVariavel = parseFloat($('#rm-variavel').val().replace('R$ ', '').replace('.', '').replace(',', '.'));
    if (isNaN(rendaVariavel) || rendaVariavel == "" || rendaVariavel == null) {
        rendaVariavel = 0;
    }
    var rendaTotal = rendaFixa + rendaVariavel;
    var saldo = rendaTotal - total;
    var scorePercent = total / rendaTotal;
    if (scorePercent >= 1) $('#seubolso-pontuacao').html("Ruim");
    else if (scorePercent >= 0.70) $('#seubolso-pontuacao').html("Regular");
    else if (scorePercent >= 0.35) $('#seubolso-pontuacao').html("Boa");
    else if (scorePercent >= 0) $('#seubolso-pontuacao').html("Excelente");
    
    $('#seubolso-renda-mensal').html(rendaTotal.toLocaleString('pt-br', formato));
    $('#seubolso-soma-gastos').html(total.toLocaleString('pt-br', formato));
    $('#seubolso-saldo').html(saldo.toLocaleString('pt-br', formato));

    labels.forEach(function (item, index) {
        var percent = (values[index] / total);
        var new_element = '<div class="item barra-'
        var new_element = new_element + colors[index];
        var new_element = new_element + '"><div class="bar-progress"><div class="bar" style="height: ';
        var new_element = new_element + Math.ceil(percent * 200);
        var new_element = new_element + 'px;" title="';
        var new_element = new_element + Math.ceil(percent * 100);
        var new_element = new_element + '%"></div></div><div class="item-progress"><i class="fa fa-home" aria-hidden="true"></i><p>';
        var new_element = new_element + item;
        var new_element = new_element + '</p></div></div>';
        $('.list-grafico').append(new_element);
    });
}
function calculateTotal() {
    $('#seubolso-soma-gastos').html(total.toLocaleString('pt-br', formato));
    $('#seubolso-total-gastos').html(total.toLocaleString('pt-br', formato));
}
function createChart() {
    var myChart = new Chart(ctx, {
        type: 'doughnut',
        cutoutPercentage: '0',
        data: {
            labels: labels,
            datasets: [{
                data: values,
                backgroundColor: colors_rgb,
                borderWidth: 0
            }]
        },
        options: {
            legend: {
                display: false,
            },
            responsive: true,
            cutoutPercentage: 50,
        },
    });
}
function addToChart(type, value, color, rgb) {
    var parsedValue = parseFloat(value.replace('R$ ', '').replace('.', '').replace(',', '.'));
    var index = labels.indexOf(type);
    if (index < 0) {
        labels.push(type);
        values.push(parsedValue);
        colors_rgb.push(rgb);
        colors.push(color);
    } else {
        values[index] = parsedValue + values[index];
    }
    total = parsedValue + total;
    calculateTotal();
    createChart();
}
function removeFromChart(type, value) {
    var parsedValue = parseFloat(value.replace('R$ ', '').replace('.', '').replace(',', '.'));
    var index = labels.indexOf(type);
    if (index < 0) return;
    values[index] = values[index] - parsedValue;
    if (parsedValue <= 0) {
        values.splice(index, 1);
        colors_rgb.splice(index, 1);
        colors.splice(index, 1);
        values.splice(index, 1);
    }
    total = total - parsedValue;
    calculateTotal();
    createChart();
}