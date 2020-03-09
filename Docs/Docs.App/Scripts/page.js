$(document).ready(function () {
    ListarDocumentos();

    $("#txtValor").keyup(function () {
        var $this = $(this);
        $this.val($this.val().replace(/[^\d,]/g, ''));
    });

    $("#btncadastrar").click(CadastrarDocumento);
});

function CadastrarDocumento() {
    $("#mensagem").html("Processando seu cadastro.");

    try {
        var fileUpload = $("#txtArquivo").get(0);

        var files = fileUpload.files;

        //Pegando a extensao da imagem...
        var extensao = files[0].name.substring(files[0].name.length - 4, files[0].name.length);

        //Gerando um novo nome para a imagem...
        var filename = guid();
        filename += '.' + extensao;

        //atribuindo a imagem no formData...
        var data = new FormData();
        data.append('file', files[0], filename);

        var model = {
            CodigoDocumento: $("#txtCodigo").val(),
            TituloDocumento: $("#txtTitulo").val(),
            Revisao: $("#sltRevisao").val(),
            DataPlanejada: $("#txtData").val(),
            Valor: $("#txtValor").val(),
            ArquivoDocumento: filename,
        };

        $.ajax({
            type: "POST",
            url: "/Documento/CadastrarDocumento",
            data: model,
            success: function (obj) { //requisição bem-sucedida..
                debugger;
                if (obj instanceof Object) {
                    if (obj.hasOwnProperty("IdDocumento")) {
                        $.ajax({
                            url: "/Documento/UploadDocumento?id=" + obj.IdDocumento + "",
                            type: "POST",
                            data: data,
                            contentType: false,
                            processData: false,
                            success: function () {
                                $("#mensagem").html(obj.Mensagem);

                                ListarDocumentos();
                                LimparCampos();
                                $("#mensagem").html("");
                            },
                            error: function (er) {
                                $("#mensagemarquivo").html("Erro ao guardar o arquivo: " + er.status);
                            }
                        });
                    }
                    else {
                        var mensagem = '';
                        if (obj.hasOwnProperty("CodigoDocumento")) {
                            for (var i = 0; i < obj.CodigoDocumento.length; i++) {
                                mensagem += obj.CodigoDocumento[i] + "\r\n";
                            }
                            $("#mensagemcodigo").html(mensagem);
                        }

                        mensagem = '';
                        if (obj.hasOwnProperty("TituloDocumento")) {
                            for (var i = 0; i < obj.TituloDocumento.length; i++) {
                                mensagem += obj.TituloDocumento[i] + "\r\n";
                            }
                            $("#mensagemtitulo").html(mensagem);
                        }

                        mensagem = '';
                        if (obj.hasOwnProperty("Revisao")) {
                            for (var i = 0; i < obj.Revisao.length; i++) {
                                mensagem += obj.Revisao[i] + "\r\n";
                            }
                            $("#mensagemrevisao").html(mensagem);
                        }

                        mensagem = '';
                        if (obj.hasOwnProperty("DataPlanejada")) {
                            for (var i = 0; i < obj.DataPlanejada.length; i++) {
                                mensagem += obj.DataPlanejada[i] + "\r\n";
                            }
                            $("#mensagemdata").html(mensagem);
                        }

                        mensagem = '';
                        if (obj.hasOwnProperty("Valor")) {
                            for (var i = 0; i < obj.Valor.length; i++) {
                                mensagem += obj.Valor[i] + "\r\n";
                            }
                            $("#mensagemvalor").html(mensagem);
                        }

                        mensagem = '';
                        if (obj.hasOwnProperty("ArquivoDocumento")) {
                            for (var i = 0; i < obj.ArquivoDocumento.length; i++) {
                                mensagem += obj.ArquivoDocumento[i] + "\r\n";
                            }
                            $("#mensagemarquivo").html(ArquivoDocumento);
                        }
                    }
                }
                else {
                    $("#mensagem").html(obj);
                }
            },
            error: function (e) { //requisição falhou..
                $("#mensagem").html("Ocorreu um erro: " + e.status);
            }
        });

    }
    catch (err) {
        $("#mensagem").html("Nenhum arquivo selecionado.");
    }
};

function LimparCampos() {
    $("#txtCodigo").val("");
    $("#txtTitulo").val("");
    $("#sltRevisao").val(0);
    $("#txtData").val("");
    $("#txtValor").val("");
};

function ExcluirDocumento(id) {
    if (confirm('Deseja excluir este documento?')) {
        $.ajax({
            type: "GET",
            url: "/Documento/ExcluirDocumento/" + id,
            data: '',
            success: function (obj) {
                $("#mensagem").html(obj);
                ListarDocumentos();
                $("#mensagem").html("");
            },
            error: function (e) { //requisição falhou..
                $("#mensagem").html("Ocorreu um erro: " + e.status);
            }
        })
    }
};

function guid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
};

function ListarDocumentos() {
    $.ajax({
        type: "GET",
        url: "/Documento/ListarDocumentos",
        data: '',
        success: function (obj) { //requisição bem-sucedida..
            var conteudo = '';
            if (obj instanceof Array) {
                debugger;
                for (var i = 0; i < obj.length; i++) {
                    conteudo += "<tr>";
                    conteudo += "<td>" + obj[i].CodigoDocumento + "</td>";
                    conteudo += "<td>" + obj[i].TituloDocumento + "</td>";
                    conteudo += "<td>" + TrataRevisao(obj[i].Revisao) + "</td>";
                    conteudo += "<td>" + ConvertJSONtoDateOnlyDate(obj[i].DataPlanejada) + "</td>";
                    conteudo += "<td>" + obj[i].Valor + "</td>";
                    conteudo += "<td><button class='btn btn-warning btn-sm edit' data-id='" + obj[i].IdDocumento + "' data-toggle='modal'>Editar</button>&nbsp;"
                    conteudo += "<button class='btn btn-danger btn-sm delete' data-id='" + obj[i].IdDocumento + "'>Excluir</button>&nbsp;"
                    conteudo += "<a href='/Documentos/" + obj[i].IdDocumento + "/" + obj[i].ArquivoDocumento + "' class='btn btn-success btn-sm' download>Download</a>&nbsp;</td>";
                    conteudo += "</tr>";
                }
            }
            $("table tbody").html(conteudo);
            $("#quantidade").html(obj.length);

            $(".edit").click(function () {
                var id = $(this).attr("data-id");

                ListaPorId(id);
            });

            $(".delete").click(function () {
                var id = $(this).attr("data-id");

                ExcluirDocumento(id)
            });
        },
        error: function (e) { //requisição falhou..
            $("#mensagem").html("Ocorreu um erro: " + e.status);
        }
    });
}

function ConvertJSONtoDateOnlyDate(jsondate, format) {
    if (jsondate === null) {
        return "";
    }
    var dateString = jsondate.substr(6);
    var currentTime = new Date(parseInt(dateString));
    var month = currentTime.getMonth() + 1;
    var day = currentTime.getDate();
    var year = currentTime.getFullYear();
    var hours = currentTime.getHours();
    var minutes = currentTime.getMinutes();
    var seconds = currentTime.getSeconds();
    if (day.toString().length === 1) { day = "0" + day; }
    if (month.toString().length === 1) { month = "0" + month; }
    if (hours.toString().length === 1) { hours = "0" + hours; }
    if (minutes.toString().length === 1) { minutes = "0" + minutes; }
    //var date = hours + ":" + minutes;
    if (format == 1) {
        var date = year + "-" + month + "-" + day + "";
    }
    else {
        var date = day + "/" + month + "/" + year + "";
    }
    return date;
}

function MontaModalEdicao() {
    var modal = '';
    modal += "<div class='modal-dialog'>";
    modal += "<div class='modal-content'>";
    modal += "<div class='modal-header'>";
    modal += "<h4 class='modal-title'><i class='fa fa-hourglass'></i>&nbsp;<span>Editar Documento</span></h4>";
    modal += "</div>";
    modal += "<div class='modal-body'>";
    modal += "<div class='form-horizontal'>";
    modal += "<input type='hidden' id='txtiddocumento'/>";
    modal += "<span id='mensagemidedit' class='text-danger'></span>";

    modal += "<div class='form-group'>";
    modal += "<div class='col-md-10'>";
    modal += "<label>Código Documento</label>";
    modal += "<input type='text' class='form-control' id='txtcodigoedit'/>";
    modal += "<span id='mensagemcodigoedit' class='text-danger'></span>";
    modal += "</div>";
    modal += "</div>";

    modal += "<div class='form-group'>";
    modal += "<div class='col-md-10'>";
    modal += "<label>Titulo Documento</label>";
    modal += "<input type='text' class='form-control' id='txttituloedit'/>";
    modal += "<span id='mensagemtituloedit' class='text-danger'></span>";
    modal += "</div>";
    modal += "</div>";

    modal += "<div class='form-group'>";
    modal += "<div class='col-md-10'>";
    modal += "<label>Revisão Documento</label>";
    modal += "<select id='sltrevisaoedit' class='form-control'>";
    modal += "<option value='0'>0</option>";
    modal += "<option value='1'>A</option>";
    modal += "<option value='2'>B</option>";
    modal += "<option value='3'>C</option>";
    modal += "<option value='4'>D</option>";
    modal += "<option value='5'>E</option>";
    modal += "<option value='6'>F</option>";
    modal += "<option value='7'>G</option>";
    modal += "</select>";
    modal += "<span id='mensagemrevisaoedit' class='text-danger'></span>";
    modal += "</div>";
    modal += "</div>";

    modal += "<div class='form-group'>";
    modal += "<div class='col-md-10'>";
    modal += "<label>Data Documento</label>";
    modal += "<input type='date' class='form-control' id='txtdataedit'/>";
    modal += "<span id='mensagemdataedit' class='text-danger'></span>";
    modal += "</div>";
    modal += "</div>";

    modal += "<div class='form-group'>";
    modal += "<div class='col-md-10'>";
    modal += "<label>Valor do Documento</label>";
    modal += "<input type='text' class='form-control' id='txtvaloredit'/>";
    modal += "<span id='mensagemvaloredit' class='text-danger'></span>";
    modal += "</div>";
    modal += "</div>";

    modal += "<div class='form-group'>";
    modal += "<div class='col-md-10'>";
    modal += "<label>Arquivo</label>";
    modal += "<input type='file' id='txtarquivoedit'/>";
    modal += "<span id='mensagemarquivoedit' class='text-danger'></span>";
    modal += "</div>";
    modal += "</div>";

    modal += "</div>";
    modal += "<div class='modal-footer'>";
    modal += "<button class='btn btn-warning' id='btneditardados'>Editar</button>";
    modal += "<input type='button' value='Cancelar' class='btn btn-info' data-dismiss='modal' onclick='window.location.reload()' />";
    modal += "<span id='mensagemedit' class='text-danger'></span>";
    modal += "</div>";
    modal += "</div>";
    modal += "</div>";

    return modal;
}

function ListaPorId(id) {

    var modal = "";
    modal = MontaModalEdicao();

    $.ajax({
        type: "GET",
        url: "/Documento/ListarDocumentosPorId/" + id,
        data: '',
        success: function (obj) { //requisição bem-sucedida..
            debugger;
            if (obj instanceof Object) {
                $("#modal").append("");
                $("#modal").append(modal);
                $('#modal').modal({ show: true });

                $("#txtiddocumento").val(obj.IdDocumento);
                $("#txtcodigoedit").val(obj.CodigoDocumento);
                $("#txttituloedit").val(obj.TituloDocumento);
                $("#sltrevisaoedit").val(obj.Revisao);
                $("#txtdataedit").val(ConvertJSONtoDateOnlyDate(obj.DataPlanejada, 1));
                $("#txtvaloredit").val(obj.Valor.replace(/./,','));

                $("#txtvaloredit").keyup(function () {
                    var $this = $(this);
                    $this.val($this.val().replace(/[^\d,]/g, ''));
                });

                $("#btneditardados").click(EditarDocumento);
            }
            else {
                $("#mensagem").html("Ocorreu um erro: " + obj);
            }
        },
        error: function (e) { //requisição falhou..
            $("#mensagem").html("Ocorreu um erro: " + e.status);
        }
    });
}

function EditarDocumento() {
    $("#mensagemedit").html("Processando sua alteração.");

    try {
        var fileUpload = $("#txtarquivoedit").get(0);

        var files = fileUpload.files;

        //Pegando a extensao da imagem...
        var extensao = files[0].name.substring(files[0].name.length - 4, files[0].name.length);

        //Gerando um novo nome para a imagem...
        var filename = guid();
        filename += '.' + extensao;

        //atribuindo a imagem no formData...
        var data = new FormData();
        data.append('file', files[0], filename);




        var model = {
            IdDocumento: $("#txtiddocumento").val(),
            CodigoDocumento: $("#txtcodigoedit").val(),
            TituloDocumento: $("#txttituloedit").val(),
            Revisao: $("#sltrevisaoedit").val(),
            DataPlanejada: $("#txtdataedit").val(),
            Valor: $("#txtvaloredit").val(),
            ArquivoDocumento: filename,
        };

        $.ajax({
            type: "POST",
            url: "/Documento/EditarDocumento",
            data: model,
            success: function (obj) { //requisição bem-sucedida..
                debugger;
                if (obj instanceof Object) {
                    if (obj.hasOwnProperty("Mensagem")) {
                        $.ajax({
                            url: "/Documento/UploadDocumento?id=" + obj.IdDocumento + "",
                            type: "POST",
                            data: data,
                            contentType: false,
                            processData: false,
                            success: function () {
                                $("#mensagemedit").html(obj.Mensagem);

                                window.location.reload();
                            },
                            error: function (er) {
                                $("#mensagemedit").html("Erro ao guardar o arquivo: " + er.status);
                            }
                        });
                    }
                    else {
                        var mensagem = '';
                        if (obj.hasOwnProperty("IdDocumento")) {
                            for (var i = 0; i < obj.IdDocumento.length; i++) {
                                mensagem += obj.IdDocumento[i] + "\r\n";
                            }
                            $("#mensagemidedit").html(mensagem);
                        }

                        mensagem = '';
                        if (obj.hasOwnProperty("CodigoDocumento")) {
                            for (var i = 0; i < obj.CodigoDocumento.length; i++) {
                                mensagem += obj.CodigoDocumento[i] + "\r\n";
                            }
                            $("#mensagemcodigoedit").html(mensagem);
                        }

                        mensagem = '';
                        if (obj.hasOwnProperty("TituloDocumento")) {
                            for (var i = 0; i < obj.TituloDocumento.length; i++) {
                                mensagem += obj.TituloDocumento[i] + "\r\n";
                            }
                            $("#mensagemtituloedit").html(mensagem);
                        }

                        mensagem = '';
                        if (obj.hasOwnProperty("Revisao")) {
                            for (var i = 0; i < obj.Revisao.length; i++) {
                                mensagem += obj.Revisao[i] + "\r\n";
                            }
                            $("#mensagemrevisaoedit").html(mensagem);
                        }

                        mensagem = '';
                        if (obj.hasOwnProperty("DataPlanejada")) {
                            for (var i = 0; i < obj.DataPlanejada.length; i++) {
                                mensagem += obj.DataPlanejada[i] + "\r\n";
                            }
                            $("#mensagemdataedit").html(mensagem);
                        }

                        mensagem = '';
                        if (obj.hasOwnProperty("Valor")) {
                            for (var i = 0; i < obj.Valor.length; i++) {
                                mensagem += obj.Valor[i] + "\r\n";
                            }
                            $("#mensagemvaloredit").html(mensagem);
                        }

                        mensagem = '';
                        if (obj.hasOwnProperty("ArquivoDocumento")) {
                            for (var i = 0; i < obj.ArquivoDocumento.length; i++) {
                                mensagem += obj.ArquivoDocumento[i] + "\r\n";
                            }
                            $("#mensagemarquivoedit").html(ArquivoDocumento);
                        }
                    }
                }
                else {
                    $("#mensagemedit").html(obj);
                }
            },
            error: function (e) { //requisição falhou..
                $("#mensagemedit").html("Ocorreu um erro: " + e.status);
            }
        });

    }
    catch (err) {
        $("#mensagemedit").html("Nenhum arquivo selecionado.");
    }
}

function TrataRevisao(revisao) {
    switch (revisao) {
        case 0:
            revisao = 0;
            break;
        case 1:
            revisao = "A";
            break;
        case 2:
            revisao = "B";
            break;
        case 3:
            revisao = "C";
            break;
        case 4:
            revisao = "D";
            break;
        case 5:
            revisao = "E";
            break;
        case 6:
            revisao = "F";
            break;
        case 7:
            revisao = "G";
            break;
        default:
            revisao = 0;
            break;
    }
    return revisao;
}