namespace Site.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Acontecimentos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Local = c.String(nullable: false, maxLength: 500),
                        Data = c.DateTime(nullable: false),
                        Liberado = c.Boolean(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                        HoraInicio = c.String(nullable: false, maxLength: 5),
                        HoraFim = c.String(nullable: false, maxLength: 5),
                        EventoId = c.Int(nullable: false),
                        Excluido = c.Boolean(nullable: false),
                        FlickrId = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Eventos", t => t.EventoId)
                .Index(t => t.EventoId);
            
            CreateTable(
                "dbo.Eventos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Titulo = c.String(nullable: false, maxLength: 250),
                        DataCadastro = c.DateTime(nullable: false),
                        Liberado = c.Boolean(nullable: false),
                        Texto = c.String(nullable: false),
                        Imagem = c.String(nullable: false, maxLength: 250),
                        Excluido = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ApoioPADs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 350),
                        Logo = c.String(),
                        Excluido = c.Boolean(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EGolPADs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Placar = c.String(nullable: false, maxLength: 5),
                        Jogador = c.String(nullable: false, maxLength: 20),
                        HashTag = c.String(nullable: false, maxLength: 15),
                        HoraDoGol = c.String(nullable: false, maxLength: 15),
                        BgColor = c.String(nullable: false, maxLength: 100),
                        GolMandante = c.Boolean(nullable: false),
                        Excluido = c.Boolean(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                        ApoioId = c.Int(),
                        MandanteId = c.Int(nullable: false),
                        VisitanteId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TimesPADs", t => t.VisitanteId)
                .ForeignKey("dbo.TimesPADs", t => t.MandanteId, cascadeDelete: true)
                .ForeignKey("dbo.ApoioPADs", t => t.ApoioId)
                .Index(t => t.ApoioId)
                .Index(t => t.MandanteId)
                .Index(t => t.VisitanteId);
            
            CreateTable(
                "dbo.TimesPADs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 150),
                        Logo = c.String(nullable: false),
                        Excluido = c.Boolean(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NoticiasPADs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Titulo = c.String(nullable: false, maxLength: 85),
                        Chamada = c.String(maxLength: 350),
                        Categoria = c.String(maxLength: 15),
                        Foto = c.String(),
                        Excluido = c.Boolean(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                        ApoioId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApoioPADs", t => t.ApoioId)
                .Index(t => t.ApoioId);
            
            CreateTable(
                "dbo.RedesSociaisPADs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Chamada = c.String(nullable: false, maxLength: 70),
                        Hashtag = c.String(nullable: false, maxLength: 40),
                        TipoRede = c.Int(nullable: false),
                        Foto = c.String(),
                        Excluido = c.Boolean(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                        ApoioId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApoioPADs", t => t.ApoioId)
                .Index(t => t.ApoioId);
            
            CreateTable(
                "dbo.Apresentadores",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        nome = c.String(maxLength: 100, unicode: false),
                        fotoInterna = c.String(maxLength: 100, unicode: false),
                        fotoExterna = c.String(maxLength: 100, unicode: false),
                        chamada = c.String(unicode: false),
                        texto = c.String(unicode: false),
                        chave = c.String(nullable: false, maxLength: 200, unicode: false),
                        excluido = c.Boolean(nullable: false),
                        Instagram = c.String(maxLength: 100, unicode: false),
                        twitter = c.String(maxLength: 50),
                        participanteConvidado = c.Boolean(),
                        facebook = c.String(maxLength: 50),
                        DataCadastro = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Programacao",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        editoriaId = c.Int(nullable: false),
                        nome = c.String(nullable: false, maxLength: 150, unicode: false),
                        logo = c.String(maxLength: 100, unicode: false),
                        imagem = c.String(maxLength: 100, unicode: false),
                        periodo = c.Int(nullable: false),
                        texto = c.String(unicode: false),
                        chamada = c.String(unicode: false),
                        diaSemana = c.Int(),
                        horario = c.Time(precision: 0),
                        dataCadastro = c.DateTime(),
                        excluido = c.Boolean(nullable: false),
                        chave = c.String(nullable: false, maxLength: 150, unicode: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Editoriais", t => t.editoriaId)
                .Index(t => t.editoriaId);
            
            CreateTable(
                "dbo.Editoriais",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        nome = c.String(nullable: false, maxLength: 200, unicode: false),
                        Descricao = c.String(maxLength: 512),
                        FotoCapa = c.String(maxLength: 512),
                        Action = c.String(maxLength: 200),
                        especial = c.Boolean(nullable: false),
                        modeloEspecial = c.Int(),
                        ativo = c.Boolean(nullable: false),
                        esportes = c.Boolean(nullable: false),
                        excluido = c.Boolean(nullable: false),
                        canal = c.Boolean(nullable: false),
                        chave = c.String(nullable: false, maxLength: 300),
                        DataCadastro = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Especiais_Modelos", t => t.modeloEspecial)
                .Index(t => t.modeloEspecial);
            
            CreateTable(
                "dbo.Categorias",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EditoriaId = c.Int(),
                        destaque = c.Boolean(nullable: false),
                        Titulo = c.String(nullable: false, maxLength: 50, unicode: false),
                        Excluido = c.Boolean(nullable: false),
                        Ordem = c.Int(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Editoriais", t => t.EditoriaId)
                .Index(t => t.EditoriaId);
            
            CreateTable(
                "dbo.Noticias",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        TituloCapa = c.String(maxLength: 90),
                        idColunista = c.Int(),
                        audio = c.String(maxLength: 250),
                        CategoriaMapaId = c.Int(),
                        titulo = c.String(nullable: false, maxLength: 300),
                        subtitulo = c.String(maxLength: 350),
                        local = c.String(maxLength: 350),
                        texto = c.String(nullable: false, unicode: false, storeType: "text"),
                        foto = c.String(maxLength: 500),
                        fotoDescricao = c.String(maxLength: 100),
                        fotoMini = c.String(maxLength: 500),
                        link = c.String(maxLength: 250),
                        destaque = c.Boolean(nullable: false),
                        destaqueEditoria = c.Boolean(nullable: false),
                        destaqueHome = c.Boolean(nullable: false),
                        DestaqueHomeMenor = c.Boolean(nullable: false),
                        TipoDestaque = c.Int(),
                        assuntoSemana = c.Boolean(nullable: false),
                        chamada = c.String(maxLength: 500),
                        data = c.DateTime(),
                        hora = c.Time(precision: 7),
                        idGaleria = c.Int(),
                        latitude = c.String(maxLength: 200),
                        longitude = c.String(maxLength: 200),
                        videoYoutube = c.String(maxLength: 100),
                        videoDestaqueHome = c.Boolean(nullable: false),
                        transito = c.Boolean(nullable: false),
                        RegiaoId = c.Int(),
                        dataAtualizacao = c.DateTime(nullable: false),
                        url = c.String(maxLength: 600),
                        plantao = c.Boolean(nullable: false),
                        visualizacao = c.Int(nullable: false),
                        projeto = c.Boolean(nullable: false),
                        liberado = c.Boolean(nullable: false),
                        fotoCredito = c.String(maxLength: 200),
                        porAutor = c.String(maxLength: 100),
                        excluido = c.Boolean(nullable: false),
                        ExibirImagemInterna = c.Boolean(nullable: false),
                        CreditoFotoNoTexto = c.Boolean(nullable: false),
                        migrado = c.Boolean(),
                        dataCadastro = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.CategoriasMapa", t => t.CategoriaMapaId)
                .ForeignKey("dbo.Colunista", t => t.idColunista)
                .ForeignKey("dbo.Galeria", t => t.idGaleria)
                .ForeignKey("dbo.NoticiasRegioes", t => t.RegiaoId)
                .Index(t => t.idColunista)
                .Index(t => t.CategoriaMapaId)
                .Index(t => t.idGaleria)
                .Index(t => t.RegiaoId)
                .Index(t => t.dataAtualizacao, name: "IX_DataAtualizacao")
                .Index(t => t.dataCadastro, name: "IX_DataCadastro");
            
            CreateTable(
                "dbo.CategoriasMapa",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Titulo = c.String(nullable: false, maxLength: 100, unicode: false),
                        Icone = c.String(nullable: false, maxLength: 50, unicode: false),
                        IconeMapa = c.String(maxLength: 50, unicode: false),
                        Excluido = c.Boolean(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Colunista",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        nome = c.String(nullable: false, maxLength: 200, unicode: false),
                        Ordem = c.Int(nullable: false),
                        email = c.String(maxLength: 150),
                        sexo = c.String(maxLength: 1),
                        descricao = c.String(unicode: false),
                        foto = c.String(maxLength: 200, unicode: false),
                        fotoMini = c.String(maxLength: 200, unicode: false),
                        chave = c.String(nullable: false, maxLength: 250),
                        liberado = c.Boolean(nullable: false),
                        excluido = c.Boolean(nullable: false),
                        dataCadastro = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.ColunistaSeguidores",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ColunistaId = c.Int(nullable: false),
                        Email = c.String(nullable: false, maxLength: 100, unicode: false),
                        DataCadastro = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Colunista", t => t.ColunistaId)
                .Index(t => t.ColunistaId);
            
            CreateTable(
                "dbo.Comentarios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Texto = c.String(),
                        CPF = c.String(),
                        Nome = c.String(),
                        IPAcesso = c.String(),
                        UrlFacebook = c.String(),
                        Gostei = c.Int(nullable: false),
                        NaoGostei = c.Int(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                        IdComentarioRaiz = c.Int(),
                        IdNoticia = c.Int(nullable: false),
                        Bloqueado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Comentarios", t => t.IdComentarioRaiz)
                .ForeignKey("dbo.Noticias", t => t.IdNoticia, cascadeDelete: true)
                .Index(t => t.IdComentarioRaiz)
                .Index(t => t.IdNoticia);
            
            CreateTable(
                "dbo.Galeria",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        titulo = c.String(nullable: false, maxLength: 300, unicode: false),
                        chamada = c.String(unicode: false),
                        texto = c.String(unicode: false),
                        Fixa = c.Boolean(nullable: false),
                        chave = c.String(nullable: false, maxLength: 370),
                        excluido = c.Boolean(nullable: false),
                        dataCadastro = c.DateTime(nullable: false),
                        ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Midias",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        idGaleria = c.Int(),
                        midia = c.String(maxLength: 500, unicode: false),
                        legenda = c.String(unicode: false),
                        video = c.Boolean(nullable: false),
                        excluido = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Galeria", t => t.idGaleria)
                .Index(t => t.idGaleria);
            
            CreateTable(
                "dbo.LiveStreamings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Legenda = c.String(nullable: false),
                        CodigoTransmissao = c.String(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                        DataFinalizacao = c.DateTime(),
                        Excluido = c.Boolean(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                        IdNoticia = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Noticias", t => t.IdNoticia)
                .Index(t => t.IdNoticia);
            
            CreateTable(
                "dbo.NoticiasRegioes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Titulo = c.String(nullable: false, maxLength: 100, unicode: false),
                        Excluido = c.Boolean(nullable: false),
                        Ordem = c.Int(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Titulo = c.String(nullable: false, maxLength: 100),
                        Excluido = c.Boolean(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                        chave = c.String(maxLength: 200, unicode: false),
                        GruposPush_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GruposPush", t => t.GruposPush_Id)
                .Index(t => t.GruposPush_Id);
            
            CreateTable(
                "dbo.Editoriais_Equipe",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        EditoriaisId = c.Int(),
                        nome = c.String(nullable: false, maxLength: 200, unicode: false),
                        texto = c.String(unicode: false),
                        funcao = c.String(maxLength: 100, unicode: false),
                        imagem = c.String(maxLength: 200, unicode: false),
                        imagemVertical = c.String(maxLength: 200, unicode: false),
                        instagram = c.String(maxLength: 100, unicode: false),
                        facebook = c.String(maxLength: 100, unicode: false),
                        twitter = c.String(maxLength: 100, unicode: false),
                        excluido = c.Boolean(nullable: false),
                        chave = c.String(nullable: false, maxLength: 260),
                        DataCadastro = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Editoriais", t => t.EditoriaisId)
                .Index(t => t.EditoriaisId);
            
            CreateTable(
                "dbo.Especiais_Modelos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Tipo = c.String(nullable: false, maxLength: 150),
                        Action = c.String(maxLength: 150),
                        TemSecao = c.Boolean(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                        especial = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Especiais_Secoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EditoriaId = c.Int(nullable: false),
                        Titulo = c.String(nullable: false, maxLength: 100),
                        Ativo = c.Boolean(nullable: false),
                        Excluido = c.Boolean(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Editoriais", t => t.EditoriaId)
                .Index(t => t.EditoriaId);
            
            CreateTable(
                "dbo.Secoes_Locais",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SecaoId = c.Int(nullable: false),
                        Tipo = c.String(maxLength: 100),
                        Titulo = c.String(nullable: false, maxLength: 150),
                        Chave = c.String(maxLength: 255),
                        Imagem = c.String(maxLength: 250),
                        Descricao = c.String(unicode: false, storeType: "text"),
                        Endereco = c.String(maxLength: 350),
                        Latitude = c.String(maxLength: 50),
                        Longitude = c.String(maxLength: 50),
                        Ativo = c.Boolean(nullable: false),
                        Excluido = c.Boolean(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Especiais_Secoes", t => t.SecaoId)
                .Index(t => t.SecaoId);
            
            CreateTable(
                "dbo.Times",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EditoriaId = c.Int(nullable: false),
                        Nome = c.String(nullable: false, maxLength: 150),
                        Escudo = c.String(maxLength: 150),
                        Chave = c.String(nullable: false, maxLength: 300),
                        Cor = c.String(maxLength: 10),
                        Titulos = c.String(unicode: false, storeType: "text"),
                        Informacoes = c.String(unicode: false, storeType: "text"),
                        Ativo = c.Boolean(nullable: false),
                        Excluido = c.Boolean(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Editoriais", t => t.EditoriaId)
                .Index(t => t.EditoriaId);
            
            CreateTable(
                "dbo.Elencos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TimeId = c.Int(nullable: false),
                        Foto = c.String(maxLength: 300),
                        Nome = c.String(nullable: false, maxLength: 150),
                        Posicao = c.String(nullable: false, maxLength: 50),
                        Ativo = c.Boolean(nullable: false),
                        Excluido = c.Boolean(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Times", t => t.TimeId)
                .Index(t => t.TimeId);
            
            CreateTable(
                "dbo.Horario_programacao",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        idPrograma = c.Int(),
                        diaSemana = c.Int(),
                        horario = c.String(maxLength: 10, unicode: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Programacao", t => t.idPrograma)
                .Index(t => t.idPrograma);
            
            CreateTable(
                "dbo.Materia",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        idProgramacao = c.Int(nullable: false),
                        idAssunto = c.Int(nullable: false),
                        titulo = c.String(nullable: false, maxLength: 200, unicode: false),
                        chamada = c.String(nullable: false, maxLength: 255, unicode: false),
                        texto = c.String(nullable: false, unicode: false),
                        foto = c.String(nullable: false, maxLength: 200, unicode: false),
                        status = c.Int(nullable: false),
                        chave = c.String(nullable: false, maxLength: 250, unicode: false),
                        excluido = c.Boolean(nullable: false),
                        dataCadastro = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.AssuntoMateria", t => t.idAssunto)
                .ForeignKey("dbo.Programacao", t => t.idProgramacao)
                .Index(t => t.idProgramacao)
                .Index(t => t.idAssunto);
            
            CreateTable(
                "dbo.AssuntoMateria",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        assunto = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.MateriaNovidades",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        idMateria = c.Int(nullable: false),
                        status = c.String(nullable: false, maxLength: 100, unicode: false),
                        dataPrograma = c.DateTime(nullable: false, storeType: "date"),
                        novidades = c.String(nullable: false, maxLength: 200, unicode: false),
                        audio = c.String(maxLength: 50, unicode: false),
                        dataCadastro = c.DateTime(nullable: false),
                        excluido = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Materia", t => t.idMateria)
                .Index(t => t.idMateria);
            
            CreateTable(
                "dbo.AreaBanner",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Titulo = c.String(nullable: false, maxLength: 200),
                        Descricao = c.String(nullable: false, maxLength: 200),
                        chave = c.String(nullable: false, maxLength: 50),
                        Tamanho = c.String(maxLength: 50, unicode: false),
                        Tamanho2 = c.String(maxLength: 50),
                        DataCadastro = c.DateTime(nullable: false),
                        Ativo = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Banners",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Titulo = c.String(nullable: false, maxLength: 200, unicode: false),
                        Arquivo = c.String(maxLength: 200),
                        Arquivo2 = c.String(maxLength: 200),
                        Anunciante = c.String(maxLength: 200, unicode: false),
                        Link = c.String(maxLength: 300),
                        TipoArquivo = c.Boolean(nullable: false),
                        Exibicoes = c.Int(),
                        Cliques = c.Int(),
                        Excluido = c.Boolean(nullable: false),
                        Liberado = c.Boolean(nullable: false),
                        DataInicio = c.DateTime(nullable: false),
                        DataFim = c.DateTime(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                        Local = c.Int(),
                        Html = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BannersVisualizacoesCliques",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CodigoBanner = c.Int(nullable: false),
                        Visualizacao = c.Boolean(nullable: false),
                        Clique = c.Boolean(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Banners", t => t.CodigoBanner)
                .Index(t => t.CodigoBanner);
            
            CreateTable(
                "dbo.Audios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Legenda = c.String(),
                        Url = c.String(),
                        DataCadastro = c.DateTime(nullable: false),
                        Excluido = c.Boolean(nullable: false),
                        Liberado = c.Boolean(nullable: false),
                        ColecaoId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ColecoesAudios", t => t.ColecaoId)
                .Index(t => t.ColecaoId);
            
            CreateTable(
                "dbo.ColecoesAudios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Titulo = c.String(),
                        Excluido = c.Boolean(nullable: false),
                        Liberado = c.Boolean(nullable: false),
                        CategoriaId = c.Int(),
                        DataCadastro = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CategoriasAudios", t => t.CategoriaId)
                .Index(t => t.CategoriaId);
            
            CreateTable(
                "dbo.CategoriasAudios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Descricao = c.String(),
                        Excluido = c.Boolean(nullable: false),
                        Liberado = c.Boolean(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Bairros",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 50),
                        Ativo = c.Boolean(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Bastidores",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        titulo = c.String(nullable: false, maxLength: 300, unicode: false),
                        chamada = c.String(unicode: false),
                        texto = c.String(unicode: false),
                        chave = c.String(nullable: false, maxLength: 370),
                        excluido = c.Boolean(nullable: false),
                        dataCadastro = c.DateTime(nullable: false),
                        ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.BastidoresMidias",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        idGaleria = c.Int(),
                        midia = c.String(maxLength: 500, unicode: false),
                        legenda = c.String(unicode: false),
                        video = c.Boolean(nullable: false),
                        ativo = c.Boolean(nullable: false),
                        excluido = c.Boolean(nullable: false),
                        dataCadastro = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Bastidores", t => t.idGaleria)
                .Index(t => t.idGaleria);
            
            CreateTable(
                "dbo.Blocos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 50),
                        Chave = c.String(maxLength: 200),
                        Imagem = c.String(maxLength: 250),
                        Local = c.String(maxLength: 50),
                        Data = c.DateTime(nullable: false),
                        Descricao = c.String(nullable: false, unicode: false, storeType: "text"),
                        Ativo = c.Boolean(nullable: false),
                        Excluido = c.Boolean(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.cidade",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        nome = c.String(maxLength: 120, unicode: false),
                        estado = c.Int(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Ouvintes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        nome = c.String(nullable: false, unicode: false),
                        email = c.String(nullable: false, maxLength: 200, unicode: false),
                        telefone = c.String(nullable: false, maxLength: 100, unicode: false),
                        regiaoId = c.Int(nullable: false),
                        cidade_id = c.Int(nullable: false),
                        bairro = c.String(maxLength: 50),
                        endereco = c.String(nullable: false, maxLength: 200, unicode: false),
                        assunto = c.String(nullable: false, maxLength: 200, unicode: false),
                        horario = c.String(nullable: false, maxLength: 100, unicode: false),
                        data = c.DateTime(nullable: false, storeType: "date"),
                        comentario = c.String(nullable: false, unicode: false),
                        DataCadastro = c.DateTime(nullable: false),
                        liberado = c.Boolean(nullable: false),
                        excluido = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Regioes", t => t.regiaoId)
                .ForeignKey("dbo.cidade", t => t.cidade_id)
                .Index(t => t.regiaoId)
                .Index(t => t.cidade_id);
            
            CreateTable(
                "dbo.OuvintesArquivos",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        idOuvinteDenuncia = c.Int(nullable: false),
                        Arquivo = c.String(nullable: false, maxLength: 150),
                        DataCadastro = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Ouvintes", t => t.idOuvinteDenuncia)
                .Index(t => t.idOuvinteDenuncia);
            
            CreateTable(
                "dbo.Regioes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        titulo = c.String(nullable: false, maxLength: 150),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Radios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Titulo = c.String(nullable: false, maxLength: 50),
                        Stream1 = c.String(maxLength: 400),
                        Stream2 = c.String(maxLength: 400),
                        TipoStream1 = c.Int(nullable: false),
                        TipoStream2 = c.Int(nullable: false),
                        Imagem = c.String(),
                        Chave = c.String(maxLength: 150),
                        Ativo = c.Boolean(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Enquete",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        pergunta = c.String(maxLength: 500, unicode: false),
                        ativa = c.Boolean(nullable: false),
                        excluido = c.Boolean(nullable: false),
                        destaque = c.Boolean(nullable: false),
                        chave = c.String(nullable: false, maxLength: 100),
                        dataCadastro = c.DateTime(nullable: false),
                        dataInicioVotacao = c.DateTime(),
                        dataFimVotacao = c.DateTime(),
                        dataFimResultado = c.DateTime(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Respostas",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        idEnquete = c.Int(),
                        resposta = c.String(unicode: false),
                        excluido = c.Boolean(nullable: false),
                        certa = c.Boolean(),
                        votos = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Enquete", t => t.idEnquete)
                .Index(t => t.idEnquete);
            
            CreateTable(
                "dbo.Equipe",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        nome = c.String(nullable: false, maxLength: 200, unicode: false),
                        texto = c.String(unicode: false, storeType: "text"),
                        funcao = c.String(maxLength: 100, unicode: false),
                        imagem = c.String(maxLength: 200, unicode: false),
                        instagram = c.String(maxLength: 100, unicode: false),
                        facebook = c.String(maxLength: 100, unicode: false),
                        email = c.String(maxLength: 100),
                        telefone = c.String(maxLength: 50),
                        twitter = c.String(maxLength: 100, unicode: false),
                        tipo = c.Short(nullable: false),
                        excluido = c.Boolean(nullable: false),
                        chave = c.String(maxLength: 260, unicode: false),
                        ordem = c.Int(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Especiais",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Titulo = c.String(nullable: false, maxLength: 100),
                        Chave = c.String(nullable: false, maxLength: 100),
                        Ativo = c.Boolean(nullable: false),
                        Excluido = c.Boolean(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.estado",
                c => new
                    {
                        id = c.Int(nullable: false),
                        nome = c.String(maxLength: 75, unicode: false),
                        uf = c.String(maxLength: 5, unicode: false),
                        pais = c.Int(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.FaleConosco",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        assunto = c.String(maxLength: 200, unicode: false),
                        nome = c.String(nullable: false, maxLength: 300, unicode: false),
                        email = c.String(nullable: false, maxLength: 300, unicode: false),
                        estado = c.String(nullable: false, maxLength: 100, unicode: false),
                        cidade = c.String(nullable: false, maxLength: 150, unicode: false),
                        celular = c.String(nullable: false, maxLength: 100, unicode: false),
                        telefone = c.String(nullable: false, maxLength: 100, unicode: false),
                        mensagem = c.String(nullable: false, unicode: false),
                        dataCadastro = c.DateTime(nullable: false),
                        resposta = c.String(unicode: false),
                        dataResposta = c.DateTime(),
                        excluido = c.Boolean(nullable: false),
                        lida = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.FotosAcontecimentoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DataCadastro = c.DateTime(nullable: false),
                        Legenda = c.String(),
                        Imagem = c.String(nullable: false),
                        AcontecimentoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Acontecimentos", t => t.AcontecimentoId, cascadeDelete: true)
                .Index(t => t.AcontecimentoId);
            
            CreateTable(
                "dbo.GruposPush",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Excluido = c.Boolean(nullable: false),
                        Liberado = c.Boolean(nullable: false),
                        Title = c.String(nullable: false, maxLength: 30),
                        DataCadastro = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NotificacoesPush",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 30),
                        Message = c.String(nullable: false, maxLength: 250),
                        DataCadastro = c.DateTime(nullable: false),
                        Enviado = c.Boolean(nullable: false),
                        RetornoAPI = c.String(),
                        idGrupo = c.Int(),
                        idNoticia = c.Int(),
                        GrupoPush_Id = c.Int(),
                        Noticia_id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GruposPush", t => t.GrupoPush_Id)
                .ForeignKey("dbo.Noticias", t => t.Noticia_id)
                .Index(t => t.GrupoPush_Id)
                .Index(t => t.Noticia_id);
            
            CreateTable(
                "dbo.Horoscopoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Signo = c.String(nullable: false),
                        Audio = c.String(nullable: false),
                        Descricao = c.String(),
                        DataAtualizacao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IndicadoresBovespas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PercentualVariacao = c.String(),
                        Pontos = c.String(),
                        ValorVariacao = c.String(),
                        Leitura = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IndicadorFinanceiroes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(),
                        Valor = c.String(),
                        Liberado = c.Boolean(nullable: false),
                        Atualizado = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MidiaKit",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Titulo = c.String(nullable: false, maxLength: 100),
                        Arquivo = c.String(nullable: false, maxLength: 255),
                        Miniatura = c.String(maxLength: 255),
                        Ativo = c.Boolean(nullable: false),
                        Excluido = c.Boolean(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MoedaValors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(),
                        Valor = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Newsletter",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false, maxLength: 250),
                        DataCadastro = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Promocoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Titulo = c.String(nullable: false, maxLength: 150),
                        Descricao = c.String(),
                        Premio = c.String(nullable: false, maxLength: 200),
                        Quantidade = c.Int(nullable: false),
                        DataInicio = c.DateTime(nullable: false),
                        DataFim = c.DateTime(nullable: false),
                        Imagem = c.String(maxLength: 255),
                        Regulamento = c.String(),
                        EmailTexto = c.String(),
                        ResultadoAutomatico = c.Boolean(nullable: false),
                        TipoPromocao = c.Int(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                        Excluido = c.Boolean(nullable: false),
                        DataCadastro = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Promocoes_Participantes",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        PromocaoCodigo = c.Int(nullable: false),
                        ParticipanteCodigo = c.Int(nullable: false),
                        CampoExtra = c.String(),
                        DataCadastro = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfile", t => t.Id)
                .ForeignKey("dbo.Promocoes", t => t.PromocaoCodigo)
                .Index(t => t.Id)
                .Index(t => t.PromocaoCodigo);
            
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Nome = c.String(maxLength: 150),
                        Email = c.String(maxLength: 50),
                        UserName = c.String(nullable: false, maxLength: 56),
                        DataNascimento = c.DateTime(),
                        Sexo = c.String(maxLength: 1, fixedLength: true, unicode: false),
                        Identidade = c.String(maxLength: 50),
                        CPF = c.String(maxLength: 20),
                        Telefone = c.String(maxLength: 50),
                        Celular = c.String(maxLength: 50),
                        CEP = c.String(maxLength: 50),
                        Estado = c.Int(),
                        Cidade = c.Int(),
                        Bairro = c.String(maxLength: 150),
                        Número = c.String(maxLength: 50),
                        Complemento = c.String(maxLength: 500),
                        ColunistaId = c.Int(),
                        Ativo = c.Boolean(),
                        DataCadastro = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Promocoes_Resultados",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PromocaoCodigo = c.Int(nullable: false),
                        ParticipanteCodigo = c.Int(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                        Excluido = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfile", t => t.ParticipanteCodigo)
                .ForeignKey("dbo.Promocoes", t => t.PromocaoCodigo)
                .Index(t => t.PromocaoCodigo)
                .Index(t => t.ParticipanteCodigo);
            
            CreateTable(
                "dbo.RedesSociais",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TipoRede = c.Int(nullable: false),
                        Imagem = c.String(maxLength: 400),
                        Texto = c.String(),
                        Link = c.String(maxLength: 800),
                        Data = c.DateTime(),
                        DataCadastro = c.DateTime(nullable: false),
                        Video = c.String(),
                        PostId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RotasNoRio",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Titulo = c.String(nullable: false, maxLength: 200),
                        Status = c.String(nullable: false, maxLength: 50),
                        Texto = c.String(unicode: false, storeType: "text"),
                        Excluido = c.Boolean(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                        Atualizacao = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TrabalheConosco",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        nome = c.String(nullable: false, maxLength: 300, unicode: false),
                        celular = c.String(nullable: false, maxLength: 50, unicode: false),
                        telefone = c.String(nullable: false, maxLength: 50, unicode: false),
                        sexo = c.Int(nullable: false),
                        estado = c.String(nullable: false, maxLength: 100, unicode: false),
                        cidade = c.String(nullable: false, maxLength: 200, unicode: false),
                        areaPretencao = c.String(nullable: false, maxLength: 300, unicode: false),
                        dataNascimento = c.DateTime(nullable: false),
                        curriculo = c.String(nullable: false, maxLength: 200, unicode: false),
                        email = c.String(nullable: false, maxLength: 200, unicode: false),
                        dataCadastro = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Transporte",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Titulo = c.String(nullable: false, maxLength: 50, unicode: false),
                        CssClass = c.String(nullable: false, maxLength: 200, unicode: false),
                        Status = c.String(nullable: false, maxLength: 10),
                        Texto = c.String(maxLength: 500),
                        Excluido = c.Boolean(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                        Atualizacao = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NoticiasTags",
                c => new
                    {
                        NoticiasId = c.Int(nullable: false),
                        TagsId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.NoticiasId, t.TagsId })
                .ForeignKey("dbo.Noticias", t => t.NoticiasId, cascadeDelete: true)
                .ForeignKey("dbo.Tags", t => t.TagsId, cascadeDelete: true)
                .Index(t => t.NoticiasId)
                .Index(t => t.TagsId);
            
            CreateTable(
                "dbo.Categorias_Noticias",
                c => new
                    {
                        idEditoriaCategoria = c.Int(nullable: false),
                        idNoticia = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.idEditoriaCategoria, t.idNoticia })
                .ForeignKey("dbo.Categorias", t => t.idEditoriaCategoria, cascadeDelete: true)
                .ForeignKey("dbo.Noticias", t => t.idNoticia, cascadeDelete: true)
                .Index(t => t.idEditoriaCategoria)
                .Index(t => t.idNoticia);
            
            CreateTable(
                "dbo.Editoriais_Galerias",
                c => new
                    {
                        EditorialId = c.Int(nullable: false),
                        GaleriaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.EditorialId, t.GaleriaId })
                .ForeignKey("dbo.Editoriais", t => t.EditorialId, cascadeDelete: true)
                .ForeignKey("dbo.Galeria", t => t.GaleriaId, cascadeDelete: true)
                .Index(t => t.EditorialId)
                .Index(t => t.GaleriaId);
            
            CreateTable(
                "dbo.Noticias_Editoriais",
                c => new
                    {
                        EditoriaId = c.Int(nullable: false),
                        NoticiaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.EditoriaId, t.NoticiaId })
                .ForeignKey("dbo.Editoriais", t => t.EditoriaId, cascadeDelete: true)
                .ForeignKey("dbo.Noticias", t => t.NoticiaId, cascadeDelete: true)
                .Index(t => t.EditoriaId)
                .Index(t => t.NoticiaId);
            
            CreateTable(
                "dbo.Prog_Apresent",
                c => new
                    {
                        idApresent = c.Int(nullable: false),
                        idProg = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.idApresent, t.idProg })
                .ForeignKey("dbo.Apresentadores", t => t.idApresent, cascadeDelete: true)
                .ForeignKey("dbo.Programacao", t => t.idProg, cascadeDelete: true)
                .Index(t => t.idApresent)
                .Index(t => t.idProg);
            
            CreateTable(
                "dbo.AreasBanners",
                c => new
                    {
                        AreasId = c.Int(nullable: false),
                        BannersId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AreasId, t.BannersId })
                .ForeignKey("dbo.AreaBanner", t => t.AreasId, cascadeDelete: true)
                .ForeignKey("dbo.Banners", t => t.BannersId, cascadeDelete: true)
                .Index(t => t.AreasId)
                .Index(t => t.BannersId);
            
            CreateTable(
                "dbo.Radioscidades",
                c => new
                    {
                        Radios_Id = c.Int(nullable: false),
                        cidade_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Radios_Id, t.cidade_id })
                .ForeignKey("dbo.Radios", t => t.Radios_Id, cascadeDelete: true)
                .ForeignKey("dbo.cidade", t => t.cidade_id, cascadeDelete: true)
                .Index(t => t.Radios_Id)
                .Index(t => t.cidade_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Promocoes_Resultados", "PromocaoCodigo", "dbo.Promocoes");
            DropForeignKey("dbo.Promocoes_Participantes", "PromocaoCodigo", "dbo.Promocoes");
            DropForeignKey("dbo.Promocoes_Resultados", "ParticipanteCodigo", "dbo.UserProfile");
            DropForeignKey("dbo.Promocoes_Participantes", "Id", "dbo.UserProfile");
            DropForeignKey("dbo.Tags", "GruposPush_Id", "dbo.GruposPush");
            DropForeignKey("dbo.NotificacoesPush", "Noticia_id", "dbo.Noticias");
            DropForeignKey("dbo.NotificacoesPush", "GrupoPush_Id", "dbo.GruposPush");
            DropForeignKey("dbo.FotosAcontecimentoes", "AcontecimentoId", "dbo.Acontecimentos");
            DropForeignKey("dbo.Respostas", "idEnquete", "dbo.Enquete");
            DropForeignKey("dbo.Radioscidades", "cidade_id", "dbo.cidade");
            DropForeignKey("dbo.Radioscidades", "Radios_Id", "dbo.Radios");
            DropForeignKey("dbo.Ouvintes", "cidade_id", "dbo.cidade");
            DropForeignKey("dbo.Ouvintes", "regiaoId", "dbo.Regioes");
            DropForeignKey("dbo.OuvintesArquivos", "idOuvinteDenuncia", "dbo.Ouvintes");
            DropForeignKey("dbo.BastidoresMidias", "idGaleria", "dbo.Bastidores");
            DropForeignKey("dbo.Audios", "ColecaoId", "dbo.ColecoesAudios");
            DropForeignKey("dbo.ColecoesAudios", "CategoriaId", "dbo.CategoriasAudios");
            DropForeignKey("dbo.AreasBanners", "BannersId", "dbo.Banners");
            DropForeignKey("dbo.AreasBanners", "AreasId", "dbo.AreaBanner");
            DropForeignKey("dbo.BannersVisualizacoesCliques", "CodigoBanner", "dbo.Banners");
            DropForeignKey("dbo.Prog_Apresent", "idProg", "dbo.Programacao");
            DropForeignKey("dbo.Prog_Apresent", "idApresent", "dbo.Apresentadores");
            DropForeignKey("dbo.Materia", "idProgramacao", "dbo.Programacao");
            DropForeignKey("dbo.MateriaNovidades", "idMateria", "dbo.Materia");
            DropForeignKey("dbo.Materia", "idAssunto", "dbo.AssuntoMateria");
            DropForeignKey("dbo.Horario_programacao", "idPrograma", "dbo.Programacao");
            DropForeignKey("dbo.Times", "EditoriaId", "dbo.Editoriais");
            DropForeignKey("dbo.Elencos", "TimeId", "dbo.Times");
            DropForeignKey("dbo.Programacao", "editoriaId", "dbo.Editoriais");
            DropForeignKey("dbo.Noticias_Editoriais", "NoticiaId", "dbo.Noticias");
            DropForeignKey("dbo.Noticias_Editoriais", "EditoriaId", "dbo.Editoriais");
            DropForeignKey("dbo.Editoriais_Galerias", "GaleriaId", "dbo.Galeria");
            DropForeignKey("dbo.Editoriais_Galerias", "EditorialId", "dbo.Editoriais");
            DropForeignKey("dbo.Especiais_Secoes", "EditoriaId", "dbo.Editoriais");
            DropForeignKey("dbo.Secoes_Locais", "SecaoId", "dbo.Especiais_Secoes");
            DropForeignKey("dbo.Editoriais", "modeloEspecial", "dbo.Especiais_Modelos");
            DropForeignKey("dbo.Editoriais_Equipe", "EditoriaisId", "dbo.Editoriais");
            DropForeignKey("dbo.Categorias", "EditoriaId", "dbo.Editoriais");
            DropForeignKey("dbo.Categorias_Noticias", "idNoticia", "dbo.Noticias");
            DropForeignKey("dbo.Categorias_Noticias", "idEditoriaCategoria", "dbo.Categorias");
            DropForeignKey("dbo.NoticiasTags", "TagsId", "dbo.Tags");
            DropForeignKey("dbo.NoticiasTags", "NoticiasId", "dbo.Noticias");
            DropForeignKey("dbo.Noticias", "RegiaoId", "dbo.NoticiasRegioes");
            DropForeignKey("dbo.LiveStreamings", "IdNoticia", "dbo.Noticias");
            DropForeignKey("dbo.Noticias", "idGaleria", "dbo.Galeria");
            DropForeignKey("dbo.Midias", "idGaleria", "dbo.Galeria");
            DropForeignKey("dbo.Comentarios", "IdNoticia", "dbo.Noticias");
            DropForeignKey("dbo.Comentarios", "IdComentarioRaiz", "dbo.Comentarios");
            DropForeignKey("dbo.Noticias", "idColunista", "dbo.Colunista");
            DropForeignKey("dbo.ColunistaSeguidores", "ColunistaId", "dbo.Colunista");
            DropForeignKey("dbo.Noticias", "CategoriaMapaId", "dbo.CategoriasMapa");
            DropForeignKey("dbo.RedesSociaisPADs", "ApoioId", "dbo.ApoioPADs");
            DropForeignKey("dbo.NoticiasPADs", "ApoioId", "dbo.ApoioPADs");
            DropForeignKey("dbo.EGolPADs", "ApoioId", "dbo.ApoioPADs");
            DropForeignKey("dbo.EGolPADs", "MandanteId", "dbo.TimesPADs");
            DropForeignKey("dbo.EGolPADs", "VisitanteId", "dbo.TimesPADs");
            DropForeignKey("dbo.Acontecimentos", "EventoId", "dbo.Eventos");
            DropIndex("dbo.Radioscidades", new[] { "cidade_id" });
            DropIndex("dbo.Radioscidades", new[] { "Radios_Id" });
            DropIndex("dbo.AreasBanners", new[] { "BannersId" });
            DropIndex("dbo.AreasBanners", new[] { "AreasId" });
            DropIndex("dbo.Prog_Apresent", new[] { "idProg" });
            DropIndex("dbo.Prog_Apresent", new[] { "idApresent" });
            DropIndex("dbo.Noticias_Editoriais", new[] { "NoticiaId" });
            DropIndex("dbo.Noticias_Editoriais", new[] { "EditoriaId" });
            DropIndex("dbo.Editoriais_Galerias", new[] { "GaleriaId" });
            DropIndex("dbo.Editoriais_Galerias", new[] { "EditorialId" });
            DropIndex("dbo.Categorias_Noticias", new[] { "idNoticia" });
            DropIndex("dbo.Categorias_Noticias", new[] { "idEditoriaCategoria" });
            DropIndex("dbo.NoticiasTags", new[] { "TagsId" });
            DropIndex("dbo.NoticiasTags", new[] { "NoticiasId" });
            DropIndex("dbo.Promocoes_Resultados", new[] { "ParticipanteCodigo" });
            DropIndex("dbo.Promocoes_Resultados", new[] { "PromocaoCodigo" });
            DropIndex("dbo.Promocoes_Participantes", new[] { "PromocaoCodigo" });
            DropIndex("dbo.Promocoes_Participantes", new[] { "Id" });
            DropIndex("dbo.NotificacoesPush", new[] { "Noticia_id" });
            DropIndex("dbo.NotificacoesPush", new[] { "GrupoPush_Id" });
            DropIndex("dbo.FotosAcontecimentoes", new[] { "AcontecimentoId" });
            DropIndex("dbo.Respostas", new[] { "idEnquete" });
            DropIndex("dbo.OuvintesArquivos", new[] { "idOuvinteDenuncia" });
            DropIndex("dbo.Ouvintes", new[] { "cidade_id" });
            DropIndex("dbo.Ouvintes", new[] { "regiaoId" });
            DropIndex("dbo.BastidoresMidias", new[] { "idGaleria" });
            DropIndex("dbo.ColecoesAudios", new[] { "CategoriaId" });
            DropIndex("dbo.Audios", new[] { "ColecaoId" });
            DropIndex("dbo.BannersVisualizacoesCliques", new[] { "CodigoBanner" });
            DropIndex("dbo.MateriaNovidades", new[] { "idMateria" });
            DropIndex("dbo.Materia", new[] { "idAssunto" });
            DropIndex("dbo.Materia", new[] { "idProgramacao" });
            DropIndex("dbo.Horario_programacao", new[] { "idPrograma" });
            DropIndex("dbo.Elencos", new[] { "TimeId" });
            DropIndex("dbo.Times", new[] { "EditoriaId" });
            DropIndex("dbo.Secoes_Locais", new[] { "SecaoId" });
            DropIndex("dbo.Especiais_Secoes", new[] { "EditoriaId" });
            DropIndex("dbo.Editoriais_Equipe", new[] { "EditoriaisId" });
            DropIndex("dbo.Tags", new[] { "GruposPush_Id" });
            DropIndex("dbo.LiveStreamings", new[] { "IdNoticia" });
            DropIndex("dbo.Midias", new[] { "idGaleria" });
            DropIndex("dbo.Comentarios", new[] { "IdNoticia" });
            DropIndex("dbo.Comentarios", new[] { "IdComentarioRaiz" });
            DropIndex("dbo.ColunistaSeguidores", new[] { "ColunistaId" });
            DropIndex("dbo.Noticias", "IX_DataCadastro");
            DropIndex("dbo.Noticias", "IX_DataAtualizacao");
            DropIndex("dbo.Noticias", new[] { "RegiaoId" });
            DropIndex("dbo.Noticias", new[] { "idGaleria" });
            DropIndex("dbo.Noticias", new[] { "CategoriaMapaId" });
            DropIndex("dbo.Noticias", new[] { "idColunista" });
            DropIndex("dbo.Categorias", new[] { "EditoriaId" });
            DropIndex("dbo.Editoriais", new[] { "modeloEspecial" });
            DropIndex("dbo.Programacao", new[] { "editoriaId" });
            DropIndex("dbo.RedesSociaisPADs", new[] { "ApoioId" });
            DropIndex("dbo.NoticiasPADs", new[] { "ApoioId" });
            DropIndex("dbo.EGolPADs", new[] { "VisitanteId" });
            DropIndex("dbo.EGolPADs", new[] { "MandanteId" });
            DropIndex("dbo.EGolPADs", new[] { "ApoioId" });
            DropIndex("dbo.Acontecimentos", new[] { "EventoId" });
            DropTable("dbo.Radioscidades");
            DropTable("dbo.AreasBanners");
            DropTable("dbo.Prog_Apresent");
            DropTable("dbo.Noticias_Editoriais");
            DropTable("dbo.Editoriais_Galerias");
            DropTable("dbo.Categorias_Noticias");
            DropTable("dbo.NoticiasTags");
            DropTable("dbo.Transporte");
            DropTable("dbo.TrabalheConosco");
            DropTable("dbo.RotasNoRio");
            DropTable("dbo.RedesSociais");
            DropTable("dbo.Promocoes_Resultados");
            DropTable("dbo.UserProfile");
            DropTable("dbo.Promocoes_Participantes");
            DropTable("dbo.Promocoes");
            DropTable("dbo.Newsletter");
            DropTable("dbo.MoedaValors");
            DropTable("dbo.MidiaKit");
            DropTable("dbo.IndicadorFinanceiroes");
            DropTable("dbo.IndicadoresBovespas");
            DropTable("dbo.Horoscopoes");
            DropTable("dbo.NotificacoesPush");
            DropTable("dbo.GruposPush");
            DropTable("dbo.FotosAcontecimentoes");
            DropTable("dbo.FaleConosco");
            DropTable("dbo.estado");
            DropTable("dbo.Especiais");
            DropTable("dbo.Equipe");
            DropTable("dbo.Respostas");
            DropTable("dbo.Enquete");
            DropTable("dbo.Radios");
            DropTable("dbo.Regioes");
            DropTable("dbo.OuvintesArquivos");
            DropTable("dbo.Ouvintes");
            DropTable("dbo.cidade");
            DropTable("dbo.Blocos");
            DropTable("dbo.BastidoresMidias");
            DropTable("dbo.Bastidores");
            DropTable("dbo.Bairros");
            DropTable("dbo.CategoriasAudios");
            DropTable("dbo.ColecoesAudios");
            DropTable("dbo.Audios");
            DropTable("dbo.BannersVisualizacoesCliques");
            DropTable("dbo.Banners");
            DropTable("dbo.AreaBanner");
            DropTable("dbo.MateriaNovidades");
            DropTable("dbo.AssuntoMateria");
            DropTable("dbo.Materia");
            DropTable("dbo.Horario_programacao");
            DropTable("dbo.Elencos");
            DropTable("dbo.Times");
            DropTable("dbo.Secoes_Locais");
            DropTable("dbo.Especiais_Secoes");
            DropTable("dbo.Especiais_Modelos");
            DropTable("dbo.Editoriais_Equipe");
            DropTable("dbo.Tags");
            DropTable("dbo.NoticiasRegioes");
            DropTable("dbo.LiveStreamings");
            DropTable("dbo.Midias");
            DropTable("dbo.Galeria");
            DropTable("dbo.Comentarios");
            DropTable("dbo.ColunistaSeguidores");
            DropTable("dbo.Colunista");
            DropTable("dbo.CategoriasMapa");
            DropTable("dbo.Noticias");
            DropTable("dbo.Categorias");
            DropTable("dbo.Editoriais");
            DropTable("dbo.Programacao");
            DropTable("dbo.Apresentadores");
            DropTable("dbo.RedesSociaisPADs");
            DropTable("dbo.NoticiasPADs");
            DropTable("dbo.TimesPADs");
            DropTable("dbo.EGolPADs");
            DropTable("dbo.ApoioPADs");
            DropTable("dbo.Eventos");
            DropTable("dbo.Acontecimentos");
        }
    }
}
  