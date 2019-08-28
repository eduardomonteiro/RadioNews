$("#calcularaposentadoria").click(function (event) {
    event.preventDefault();

    var sexo = $("#formcalcular input[type='radio']:checked").val();

    var anosquejacontribuiu = $("#apon-inss").val();
    var anosdeidade = $("#apon-ida").val();

    if ($('#apon-serv :selected').val() == "") {
        $("#apon-serv").addClass("input-erro");
        return false;
    } else {
        $("#apon-serv").removeClass("input-erro");
    }
    if (anosdeidade == "") {
        $("#apon-ida").addClass("input-erro");
        return false;
    } else {
        $("#apon-ida").removeClass("input-erro");
    }
    if (anosquejacontribuiu == "") {
        $("#apon-inss").addClass("input-erro");
        return false;
    } else {
        $("#apon-inss").removeClass("input-erro");
    }

    var faltaanos = 0;

    var pontosdecontribuicao = parseInt(anosquejacontribuiu) + parseInt(anosdeidade);
    if (pontosdecontribuicao >= 85 && sexo == "f") {
        //alert("aponsentou por contribuição. Seus pontos são: " + pontosdecontribuicao + ".");
        return;
    }
    else {
        if (sexo == "f") {
            var t = 85 - pontosdecontribuicao;
            var idade = ((t / 2) + parseInt(anosdeidade));
            if (idade < 60) {
                $("#paragrafoaposentadoria").html(Math.floor(idade) + " anos");
                abrefuncy(".float-aposentado");
                return;
            } else {
                $("#paragrafoaposentadoria").html("60 anos");
                abrefuncy(".float-aposentado");
                return;
            }
        }


    }
    if (pontosdecontribuicao >= 95 && sexo == "m") {
        //alert("aponsentou por contribuição. Seus pontos são: " + pontosdecontribuicao + ".");
        return;
    } else {
        var t = 95 - pontosdecontribuicao;
        var idade = ((t / 2) + parseInt(anosdeidade));
        if (idade < 65) {
            $("#paragrafoaposentadoria").html(Math.floor(idade) + " anos");
            abrefuncy(".float-aposentado");
            return;
        } else {
            $("#paragrafoaposentadoria").html("65 anos");
            abrefuncy(".float-aposentado");
            return;
        }

    }
    //if (anosquejacontribuiu >= 30 && sexo == "f") {
    //    //alert("aponsentou por contribuição. Você já contribuiu: " + anosquejacontribuiu + " anos.");
    //    return;
    //}
    //if (anosquejacontribuiu >= 35 && sexo == "m") {
    //    //alert("aponsentou por contribuição. Você já contribuiu: " + anosquejacontribuiu + " anos.");
    //    return;
    //}
    //if (anosdeidade >= 60 && sexo == "f") {
    //    //alert("aponsentou por idade. Você tem mais de 60 anos.");
    //    return;
    //}
    //if (anosdeidade >= 65 && sexo == "m") {
    //    //alert("aponsentou por idade.  Você tem mais de 60 anos.");
    //    return;
    //}

    //if (sexo == "f") {
    //    faltaanos = 60 - parseInt(anosdeidade);
    //    var r = parseInt(anosdeidade) + faltaanos;
    //    $("#paragrafoaposentadoria").html(r + " anos");
    //    abrefuncy(".float-aposentado");
    //    //alert("Você vai se aposentar aos: " + r + " anos.");
    //    return;
    //}
    //if (sexo == "m") {
    //    faltaanos = 65 - parseInt(anosdeidade);
    //    var r1 = parseInt(anosdeidade) + faltaanos;
    //    $("#paragrafoaposentadoria").html(r1 + " anos");
    //    abrefuncy(".float-aposentado");
    //    //alert("Você vai se aposentar aos: " + r1 + " anos.");
    //    return;
    //}

    //alert(anosquejacontribuiu);
});

$("#formcalcular").submit(function (e) {
    e.preventDefault();
    return false;
});