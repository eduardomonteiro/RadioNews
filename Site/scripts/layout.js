//function showBannerTopo() {
//    var cont = 0;
//    $('.banner-exansivo').each(function (index) {
//        $(this).addClass('banner-topo-' + (index + 1));
//        cont++;
//    });

//    var x = 0 + Math.floor(Math.random() * cont);
//    $('.banner-topo-' + x + ' .image').addClass('promotional-banner');
//    $('.banner-topo-' + x).show();
//}

function clickBanner(banner_id) {
    $.ajax({
        url: "http://www.eduardoDev.com.br/CompanyNamebanner/upload.php",
        type: "POST",
        data: { 'banner_id': '' + banner_id + '', 'acao': '1' },
        success: function (data) {
        }
    });
}

function streaming(url) {
    $.get(url).done(function (data) {
        if (data && data.CodigoTransmissao) {
            var fechou = localStorage.getItem("live-" + data.CodigoTransmissao);

            if (fechou) {
                $($(".wrapper-stream-hero")[0]).remove();
            } else {
                var wrapper =
                  "<section class='wrapper-stream-hero'>                                                                                               "
                + "    <div class='center'>                                                                                                            "
                + "        <nav class='link-social'>                                                                                                   "
                + "            <a href='https://www.youtube.com/channel/UC9T0ZAarQHL4cJGFzORVsFg' target='_blank' class='fa fa-youtube'></a>           "
                + "        </nav>                                                                                                                      "
                + "        <a href='/live' class='title'>                                                                                              "
                + "            Assista ao vivo <i class='fa fa-play-circle'></i> " + data.Legenda + "                                                  "
                + "        </a>                                                                                                                        "
                + "        <button class='close' id='close-tab'>Fechar <i class='fa fa-times'></i></button>                                            "
                + "    </div>                                                                                                                          "
                + "</section>                                                                                                                          ";

                $($(".main-footer")[0]).prepend(wrapper);

                $("#close-tab").click(function () {

                    $($(".wrapper-stream-hero")[0]).remove();
                    localStorage.clear();
                    localStorage.setItem("live-" + data.CodigoTransmissao, true);
                });
            }
        }
        $(window).trigger('live-notified');
    });
}

function bannerView() {
    ids = [];

    $(".promotional-banner").each(function () {
        var idAtual = $(this).attr("data-banner-id");
        ids.push(idAtual);
    });

    $(".promotion-side").each(function () {
        var idAtual = $(this).attr("data-banner-id");
        ids.push(idAtual);
    });


    $(".banner-act").each(function () {
        var idAtual = $(this).attr("data-banner-id");
        ids.push(idAtual);
    });

    $(".random-banners").each(function () {
        var postition = Math.floor((Math.random() * $(this).find(".random-banner").length) + 1);
        var banner = $(this).find(".random-banner")[postition - 1];
        $($(banner).closest(".random-content")).show();
        var homeId = $(banner).attr("data-banner-id");
        ids.push(homeId);
    });

    if (ids.length > 0) {
        var obj = {
            ids: ids
        }
        for (var i = 0; i < ids.length; i++) {
            $.ajax({
                url: "http://www.eduardoDev.com.br/CompanyNamebanner/upload.php",
                type: "POST",
                data: { 'banner_id': ids[i], 'acao': '0' },
                success: function (data) {
                }
            });
        }
    }

};

function popStream(chave) {
    var url = "/Home/Player?chave=" + chave;
    console.log(url);
    window.open(url,
        "mywindow", "location=no,status=no,scrollbars=no,width=425,height=200");
}

function previsaoTempo(url) {
    $.ajax({
        url: url,
        type: "GET",
        success: function (data) {

            var tempo = data.previsao[0].tempo;
            var minima = data.previsao[0].minima;
            var maxima = data.previsao[0].maxima;
            var iuv = data.previsao[0].iuv;
            var previsaoDescricao = data.altPrevisao;

            var iconeClimaB = "/images/clima_icones/branco/" + tempo + ".png";
            var iconeClimaP = "/images/clima_icones/preto/" + tempo + ".png";

            $(".icone-clima-branco").attr("src", iconeClimaB);
            $(".icone-clima-branco").attr("alt", previsaoDescricao);
            $(".icone-clima-branco").attr("title", previsaoDescricao);

            $(".icone-clima-preto").attr("src", iconeClimaP);
            $(".icone-clima-preto").attr("alt", previsaoDescricao);
            $(".icone-clima-preto").attr("title", previsaoDescricao);

            $("#texto-previsao").html(minima + "ºc - " + maxima + "ºc");
            $("#tempoMin").html(minima + "º");
            $("#tempoMax").html(maxima + "º");


        }
    });
}
function busca(queryString) {
    $("#btn-busca-all").click(function (e) {
        e.preventDefault();
        var termo = $("#buscar").val();
        if (termo !== '' && termo !== queryString) {
            url = "/busca/" + termo.toLowerCase();
            location.href = url;
        }
    });
}
function floaterApePreVoce() {
    $('.mask-floater-2016').fadeIn(800);
    $('.mask-floater-2016 .wrap').css('margin-left', -($('.mask-floater-2016 .wrap').outerWidth() / 2));
    $('.mask-floater-2016 .close').on('click', function () {
        $('.mask-floater-2016').fadeOut(800);
    });
}
function layout() {
    //showBannerTopo();
    $("#buscar").keypress(function (event) {
        if (event.which == 13 && $(this).val() != "") {
            event.preventDefault();
            $("#btn-busca-all").click();

        }
    });
    $(".thumb-video").fancybox({
        maxWidth: 800,
        maxHeight: 600,
        fitToView: false,
        width: '70%',
        height: '70%',
        autoSize: false,
        closeClick: false,
        openEffect: 'none',
        closeEffect: 'none'
    });
    floaterApePreVoce()
    bannerView();
}