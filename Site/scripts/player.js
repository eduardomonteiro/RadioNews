    $(document).ready(function(){
    var audio = $('#player');
    var playButton = $('#play-button');
    var sliderAudio = $('#audio-range');
    playButton.click(function(){
        if(!audio[0].paused){
            audio[0].pause();
            playButton.css("background-image", "url('/images/player/icon-play.png')");
        }else{
            audio[0].play();
            playButton.css("background-image", "url('/images/player/icon-pause.png')");
        }

    }); 
    sliderAudio.on('input', function () {
        //$(this).trigger('change');
        audio[0].volume = this.value;
        console.log(audio[0].volume);
        });
    });
    function secToStr( sec_num ) {
        sec_num = Math.floor( sec_num );
        var horas   = Math.floor(sec_num / 3600);
        var minutos = Math.floor((sec_num - (horas * 3600)) / 60);
        var segundos = sec_num - (horas * 3600) - (minutos * 60);
         
        if (horas   < 10)  horas    = "0"+horas;
        if (minutos < 10)  minutos  = "0"+minutos;
        if (segundos < 10) segundos = "0"+segundos;
         
        var tempo    = horas+':'+minutos+':'+segundos;
         
        return tempo;
    }