// Efeito do menu reduzido
var sticky = function() {
	var calcH = $('.main-header').height();
	$(window).scroll(function(){
		var position =  $(window).scrollTop();
		if (position > calcH) {
			$('body').addClass('sticky');
		}
		else {
			$('body').removeClass('sticky');
		}
	});
	$('#nav-bottom').click(function(){
		$('.main-header .navigation-bottom .center').slideToggle();
	});
	$('.wrapper-general').css('padding-top', calcH + 30);
};

// Área lateral de colunistas
var colunistas = function() {
	$element = $('.box-columnists');
	var forPage = 2;
	var atual = 1;
	var l = $element.find('.list li').length;
	for (var i = 0; i < l / Math.ceil(forPage); i++) {
		$element.find('.dots').append('<li></li>');
		$element.find('.dots li:first-child').addClass('on');
	}
	$element.find('.list li').slice((atual * forPage) - forPage, (atual * forPage)).show();

	var change = function() {
		$element.find('.list li').hide();
		$element.find('.list li').slice((atual * forPage) - forPage, (atual * forPage)).show();
		$('li.on').removeClass('on');
		$('.dots li:nth-child('+ atual + ')').addClass('on');
	};

	$('.dots li').click(function(){
		atual = $('.dots li').index($(this)) + 1;
		change();	
	});
	setInterval(function(){
		if (atual == Math.ceil(l / 2)) {
			atual = 1;
		}
		else {
			atual++;
		}
		change();
	}, 4000);
};

var Plantao = function() {
	$('.item-scroll').slick({
		infinite: true,
  		slidesToShow: 1,
  		arrows: false,
  		slidesToScroll: 1,
  		autoplay: true,
  		autoplaySpeed: 3000
	});
};

// Próximos Jogos

var ProxJogos = function() {
	$('.card-players .equips li').click(function(){
		var valor = $(this).attr('id');
		var color = "#3c393b";

		var applycolor = function(valor) {
			$('.card-players footer').css('background', valor);
			$('.card-players').css('border-top-color', valor);
			$('.card-players .content h5').css('color', valor);
		};

		switch(valor) {
		    case "atletico":
		    	color = "#3c393b";
		        break;
		    case "brasil":
		    	color = "#008837";
		        break;
		    case "cruzeiro":
		    	color = "#003399";
		        break;
		    case "america":
		    	color = "#33835b";
				break;
			default:
				color = "#3c393b";	
		}
		applycolor(color);
		$('.card-players .tabs').hide();
		$('.card-players .tabs[data-tab="'+ valor +'"]').show();
		$('.equips li.active').removeClass('active');
		$(this).addClass('active');
	});
};

// Chamada de Vídeo

var PlayVideo = function() {
	$('.thumb-video').click(function(){
		// Passa o valor do atributo data-href para a url do vídeo
		var valor = $(this).attr('data-href');
		$.fancybox.open({
			content: '<video id="video-loading" preload="auto" controls="controls" autoplay="autoplay" class="video-request"><source src="'
			+ valor + '" type="video/mp4" /><div>Seu navegador não dá suporte a este tipo de vídeo.</div></video>',
			afterShow: function() {
			},
			beforeClose: function() {
				$('#video-loading').remove();
			}
		});
	});
};


var PodCast = function(){
	$podcast=  $('.podcast-item');
	$podcast.each(function(){
		var item = $(this);
		var audio = $(this).find('audio');
		item.find('.playbar').append('<input type="range" max="100" min="0" value="0" />');

		
		audio[0].addEventListener('loadedmetadata', function() {
		    item.find('.full-time').text(("0" + Math.floor(audio[0].duration / 60)).slice(-2) + ':' + ("0" + Math.floor(audio[0].duration % 60)).slice(-2));
		});

		item.find('.button').click(function(){
			if ($(this).is('.fa-play-circle')) {
				$('audio').each(function(){
					$(this)[0].pause();
					$podcast.find('.button').removeClass('fa-pause-circle').addClass('fa-play-circle');
					item.find('.button').removeClass('fa-play-circle').addClass('fa-pause-circle');
				});
				audio[0].play();
			} else {
				$(this).removeClass('fa-pause-circle').addClass('fa-play-circle');
				audio[0].pause();
			}
		});
		audio[0].addEventListener("timeupdate", function(e){
			var current = audio[0].currentTime;
			var minutescurrent = Math.floor(current / 60);
			var secondscurrent = current % 60;
			item.find('.rolling').text(("0" + minutescurrent).slice(-2) + ':' + ("0" + Math.floor(secondscurrent)).slice(-2));
			item.find('.progress').css('width', ((current * 100) / audio[0].duration) + '%');
		});

		item.find('input').on('change', function(){
			var valor = $(this).val();
			var counter = (valor / 100) * audio[0].duration;
			audio[0].currentTime = counter;
		});

		audio[0].addEventListener("ended", function(e){
			item.find('.button').removeClass('fa-pause-circle').addClass('fa-play-circle');
			item.find('.progress').css('width', '0');
		});
	});
};

var Album = function() {
	$('.thumbs-galery').slick({
		infinite: true,
  		slidesToShow: 4,
  		arrows: false,
  		slidesToScroll: 4,
  		arrows: true,
  		nextArrow: '<div class="fa fa-angle-right arrows"></div>',
  		prevArrow: '<div class="fa fa-angle-left arrows"></div>',
  		responsive: [
		    {
		      breakpoint: 600,
		      settings: {
		        slidesToShow: 2,
		        slidesToScroll: 2
		      }
		    }
		]
	});
	$('.thumbs-galery img').click(function(){
		$('img.selected').removeClass('selected');
		$(this).addClass('selected');
		var valor = $(this).attr('src');
		$('.big-pic img').attr('src', valor);
	});
};

var acordion = function() {
	$('.acordion li header').click(function(){
		if ($(this).parent().hasClass('enable')) {
			$('.acordion li').removeClass('enable');
			$('.acordion li .content').slideUp();
		}else {
			$('.acordion li .content').slideUp();
			$(this).parent().find('.content').slideDown();
			$('.acordion li').removeClass('enable');
			$(this).parent().addClass('enable');
		}
	});
};


var elenco = function() {
	$element = $('#elenco');
	$element.find('.nav-elenco span').click(function(){
		var valor = $(this).attr('id');
		$('span.enable').removeClass('enable');
		$(this).addClass('enable');
		$('.list-elenco').hide();
		$('.list-elenco[data-nav="'+ valor +'"]').show();
	});
};

var Carnaval = function() {
	$element = $('#banner-carnaval');
	$element.slick({
		infinite: true,
  		slidesToShow: 1,
  		slidesToScroll: 1,
  		arrows: false,
  		dots: true,
  		autoplay: true,
  		autoplaySpeed: 4000
  	});	
};

var Banner = function() {
	$element = $('.featured-banner');
	$element.slick({
		infinite: true,
  		slidesToShow: 1,
  		slidesToScroll: 1,
  		arrows: true,
  		dots: true,
  		autoplay: true,
  		autoplaySpeed: 4000,
  		nextArrow: '<div class="fa fa-angle-right arrows"></div>',
  		prevArrow: '<div class="fa fa-angle-left arrows"></div>',
  		responsive: [
		    {
		      breakpoint: 740,
		      settings: {
		        dots: false,
		        arrows: false
		      }
		    }
		]
  	});	

  	var w = $(window).width();
  	if (w < 670) {
  		$('.featured-noticies.mini').slick({
  			slidesToShow: 1,
  			slidesToScroll: 1,
  			arrows: false,
  			dots: true
  		});
  	}
};

var enquete = function() {
	$form = $('#enquete');
	$form.find('button').click(function(e){
		e.preventDefault();
		var valor = $(this).attr('id');
		$('[data-step]').hide();
		$('[data-step="'+ valor +'"]').show();
	});
};

var structure =  {
	init: function() {
		$('#telefone, #celular').mask('(99) 99999-9999');
		$('#datafinal, #datainicio, .data').mask('99/99/9999');
		$('.cpf').mask('999-999-999-99');
		$('#cep').mask('99-999-999');
		$('#hora').mask('99:99');
		this.resizeFont();
		this.share();
		this.InputsFile();
		this.menu();
		$('.button-expandmap').click(function(){
			var animation = function() {
				var position = $('.site-map').offset().top;
				$('body').animate({
					scrollTop: position
				});
			};
			if ($('.button-expandmap .fa').hasClass('fa-angle-down')) {
				$('.site-map').slideDown(800, animation);
				$('.button-expandmap .fa').removeClass('fa-angle-down').addClass('fa-angle-up');
			}else {
				$('.site-map').slideUp(800);
				$('.button-expandmap .fa').removeClass('fa-angle-up').addClass('fa-angle-down');
			}
		});

		$('#button-esqueci').click(function(){
			$('.box-post').slideToggle(400);
		});

		$('.side-info').click(function(){
			$.fancybox.open({
				'href': '#lightbox',
				afterShow: function() {
				},
				beforeClose: function() {
					
				}
			});
		});
	},
	resizeFont: function() {
		var normal = 10;
		$('.bt-resize').click(function(){
			if($(this).is('.more')){
				if (normal == 13) {
					normal = 13;
				}
				else {
					normal++;
				}
			}else {
				if (normal == 8) {
					normal = 8;
				}
				else {
					normal--;
				}
			}
			$('.wrapper-general, .block-text').css('font-size', normal);
		});
	},
	share: function() {
		$('.box-share').click(function(){
			if ($(this).hasClass('ok')) {
				$('.box-share.ok').removeClass('ok');
			}
			else {
				$('.box-share.ok').removeClass('ok');
				$(this).toggleClass('ok');
			}
			
		});
	},
	InputsFile: function() {
		var init = function() {
			$('input[type="file"]').change(function(){
				var valor = $(this).val().split('/').pop().split('\\').pop();
				$(this).parent().find('span').text(valor);
			});
		};	
		var content = '<div class="boxs-upload"><div class="file-row"><span></span><input type="file" class="file input-form"></div><div class="bt-add">+</div></div>';

		var add = function() {
			$('.bt-add').on('click', function(){
				if ($(this).hasClass('remove')) {
					$(this).parent().remove();
				}
				else {
					$(this).html('-').addClass('remove');
					$('#section-upload').append(content);
					add();
				}
				
			});
			init();
		};
		add();
		init();
	},
	menu: function() {

		$('.bt-menu').click(function(){
			if ($(this).hasClass('enable')) {
				$(this).removeClass('enable').text('Menu');
				$('.submenu').removeClass('selected');
			}else {
				$(this).addClass('enable').text('Fechar');
				$('.submenu').addClass('selected');
			}
			
		});

		$('.list-categories li').bind('click mouseover', function(){

			var valor = $(this).attr('data-submenu');
			if (valor == undefined) {
				$('li.selected').removeClass('selected');
				$('.second-column').removeClass('active');
			}else {
				$(this).addClass('selected');
				$('.second-column').addClass('active');
				$('.list-side').hide();
				$('.list-side[data-link="'+ valor + '"]').show();
			}
		});

		$('.list-side, .bt-return').bind('click mouseleave', function(){
			$('.second-column').removeClass('active');
			$('.list-categories li.selected').removeClass('selected');
		});

		$('.submenu').bind('mouseleave', function(){
			setTimeout(function(){
				$('.bt-menu').removeClass('enable').text('Menu');
				$('.submenu').removeClass('selected');
			}, 1000);
		});
	}
};

// Chamada das funções
enquete();
elenco();
Banner();
Carnaval();
acordion();
Album();
Plantao();
PodCast();
PlayVideo();
colunistas();
ProxJogos();
sticky();
structure.init();