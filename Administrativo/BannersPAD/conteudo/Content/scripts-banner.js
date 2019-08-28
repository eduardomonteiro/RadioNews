function update() {
    $.ajax({
        url: 'http://localhost/Admin/BannersPAD/Update896x288',
        type: 'GET',
        success: function (data) {
            $('#conteudo').html(data);
            window.localStorage.setItem('conteudo', $('#conteudo').html());

            setImageToStorage('.brand-apoio', '#img-apoio', 'img-apoio');

            setImageToStorage('#img-goleador', '#img-goleador', 'img-goleador');
            setImageToStorage('#img-mandante', '#img-mandante', 'img-mandante');
            setImageToStorage('#img-visitante', '#img-visitante', 'img-visitante');
            setImageToStorage('#img-mandante-2', '#img-mandante-2', 'img-mandante-2');
            setImageToStorage('#img-visitante-2', '#img-visitante-2', 'img-visitante-2');

            if ($('.center.featured-img')[0].style.backgroundImage.indexOf('Content/images/bg') < 0) {
                var url_bg_img = $('.center.featured-img')[0].style.backgroundImage.replace('url("', '').replace('")', '');
                window.localStorage.setItem('img-background', getBase64FromImageUrl(url_bg_img));
            }

        }
    });


    var bg_img = window.localStorage.getItem('img-background');
    if (bg_img == 'undefined') {
        $('.center.featured-img').css("background", "url(Content/images/bg-default.jpg) no-repeat scroll 50% 50% / cover padding-box border-box");
    }

    var data = window.localStorage.getItem('conteudo');
    if (data != '' && data != null) {
        $('#conteudo').html(data);
    }

    getImageFromStorage('.brand-apoio', '#img-apoio', 'img-apoio');
    getImageFromStorage('#img-goleador', '#img-goleador', 'img-goleador');
    getImageFromStorage('#img-mandante', '#img-mandante', 'img-mandante');
    getImageFromStorage('#img-visitante', '#img-visitante', 'img-visitante');
    getImageFromStorage('#img-mandante-2', '#img-mandante-2', 'img-mandante-2');
    getImageFromStorage('#img-visitante-2', '#img-visitante-2', 'img-visitante-2');
}
function setImageToStorage(id_item, id_img, key) {
    if ($(id_item).length > 0) {
        var url_apoio = $(id_img)[0].src;
        getBase64FromImageUrl(url_apoio, key);
    }
}
function getImageFromStorage(id_item, id_img, key) {
    if ($(id_item).length > 0) {
        var data = window.localStorage.getItem(key);
        if (data != '' && data != null) {
            $(id_img)[0].src = data;
        }
    }
}
function getBase64FromImageUrl(url, key) {
    var img = new Image();
    img.setAttribute('crossOrigin', 'anonymous');
    img.onload = function () {
        var canvas = document.createElement("canvas");
        canvas.width = this.width;
        canvas.height = this.height;
        var ctx = canvas.getContext("2d");
        ctx.drawImage(this, 0, 0);
        var dataURL = canvas.toDataURL("image/png");
        window.localStorage.setItem(key,dataURL);
    };
    img.src = url;
}
$(document).ready(function () {
    update();
    //setInterval(function () {
    //    update()
    //}, 11000);
})