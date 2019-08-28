    $(function () {
      $('.banner-bi .list-ban').slick({
        infinite: true,
        slidesToShow: 1,
        slidesToScroll: 1,
        prevArrow: '<div class="fa fa-angle-left arrows"></div>',
        nextArrow: '<div class="fa fa-angle-right arrows"></div>',
        dots: true,
      });
    
        $( ".apon-sexo label" ).click(function() {
			    $(this).parent( ".apon-sexo label").removeClass( "ativo" );
			     if ($(this).find('input').is(':checked')){
			  	    $(this).addClass( "ativo" );
			      } else {
			  	    $('.apon-sexo label').removeClass( "ativo" );
			      }
		    });	
    });
    function openwindow(url) {
        window.open(url,
            "mywindow", "location=no,status=no,scrollbars=no,width=400,height=300");
    }
    (function ($) {
        $(window).on("load", function () {
            $(".content-scroll").mCustomScrollbar();
        });
    })(jQuery);

    $(document).ready(function () {
        $.getJSON('http://api.promasters.net.br/cotacao/v1/valores', function (json) {
            $("#dolarvalor").text((json.valores.USD.valor).toFixed(4));
            $("#eurovalor").text((json.valores.EUR.valor).toFixed(4));
            $("#libravalor").text((json.valores.GBP.valor).toFixed(4));
        });
    });

    $("#cpa-form").submit(function (e) {
        return false;
    });

    function abrefuncy(classe) {
        $(classe).fancybox().trigger('click');
        $('.float-seubolso').unbind('click');
        $('.float-aposentado').unbind('click');
    }