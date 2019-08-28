namespace Site.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Data.Entity.Infrastructure.Annotations;

    public partial class ModeloDados : DbContext
    {
        public ModeloDados()
            : base("name=CompanyNameContext")
        {
            Database.SetInitializer<ModeloDados>(null);
        }


        public virtual DbSet<Apresentadores> Apresentadores { get; set; }
        public virtual DbSet<AreaBanner> AreaBanner { get; set; }
        public virtual DbSet<AssuntoMateria> AssuntoMateria { get; set; }
        public virtual DbSet<Bairros> Bairros { get; set; }
        public virtual DbSet<Banners> Banners { get; set; }
        public virtual DbSet<BannersVisualizacoesCliques> BannersVisualizacoesCliques { get; set; }
        public virtual DbSet<Bastidores> Bastidores { get; set; }
        public virtual DbSet<BastidoresMidias> BastidoresMidias { get; set; }
        public virtual DbSet<Blocos> Blocos { get; set; }
        public virtual DbSet<Categorias> Categorias { get; set; }
        public virtual DbSet<CategoriasMapa> CategoriasMapa { get; set; }
        public virtual DbSet<cidade> cidade { get; set; }
        public virtual DbSet<Colunista> Colunista { get; set; }
        public virtual DbSet<ColunistaSeguidores> ColunistaSeguidores { get; set; }
        public virtual DbSet<Comentario> Comentarios { get; set; }
        public virtual DbSet<Editoriais> Editoriais { get; set; }
        public virtual DbSet<Editoriais_Equipe> Editoriais_Equipe { get; set; }
        public virtual DbSet<Enquete> Enquete { get; set; }
        public virtual DbSet<Equipe> Equipe { get; set; }
        public virtual DbSet<Especiais> Especiais { get; set; }
        public virtual DbSet<Especiais_Modelos> Especiais_Modelos { get; set; }
        public virtual DbSet<Especiais_Secoes> Especiais_Secoes { get; set; }
        public virtual DbSet<estado> estado { get; set; }
        public virtual DbSet<FaleConosco> FaleConosco { get; set; }
        public virtual DbSet<Galeria> Galeria { get; set; }
        public virtual DbSet<Horario_programacao> Horario_programacao { get; set; }
        public virtual DbSet<Materia> Materia { get; set; }
        public virtual DbSet<MateriaNovidades> MateriaNovidades { get; set; }
        public virtual DbSet<MidiaKit> MidiaKit { get; set; }
        public virtual DbSet<Midias> Midias { get; set; }
        public virtual DbSet<Newsletter> Newsletter { get; set; }
        public virtual DbSet<Noticias> Noticias { get; set; }
        public virtual DbSet<NoticiasRegioes> NoticiasRegioes { get; set; }
        public virtual DbSet<Ouvintes> Ouvintes { get; set; }
        public virtual DbSet<OuvintesArquivos> OuvintesArquivos { get; set; }
        public virtual DbSet<Programacao> Programacao { get; set; }
        public virtual DbSet<Promocoes> Promocoes { get; set; }
        public virtual DbSet<Promocoes_Participantes> Promocoes_Participantes { get; set; }
        public virtual DbSet<Promocoes_Resultados> Promocoes_Resultados { get; set; }
        public virtual DbSet<RedesSociais> RedesSociais { get; set; }
        public virtual DbSet<Radios> Radios { get; set; }
        public virtual DbSet<Regioes> Regioes { get; set; }
        public virtual DbSet<Respostas> Respostas { get; set; }
        public virtual DbSet<RotasNoRio> RotasNoRio { get; set; }
        public virtual DbSet<Secoes_Locais> Secoes_Locais { get; set; }
        public virtual DbSet<Tags> Tags { get; set; }
        public virtual DbSet<TrabalheConosco> TrabalheConosco { get; set; }
        public virtual DbSet<Transporte> Transporte { get; set; }
        public virtual DbSet<UserProfile> UserProfile { get; set; }
        public virtual DbSet<Elencos> Elencos { get; set; }
        public virtual DbSet<Times> Times { get; set; }
        public virtual DbSet<Eventos> Eventos { get; set; }
        public virtual DbSet<Acontecimentos> Acontecimentos { get; set; }
        public virtual DbSet<FotosAcontecimento> FotosAcontecimento { get; set; }
        public virtual DbSet<Horoscopo> Horoscopo { get; set; }

        public virtual DbSet<LiveStreaming> LiveStreamings { get; set; }
        public virtual DbSet<Audio> Audios { get; set; }
        public virtual DbSet<CategoriaAudios> CategoriasAudios { get; set; }
        public virtual DbSet<ColecaoAudios> ColecoesAudios { get; set; }

        #region PAD
        public virtual DbSet<ApoioPAD> ApoioPad { get; set; }
        public virtual DbSet<TimesPAD> TimesPAD { get; set; }
        public virtual DbSet<EGolPAD> EGolPAD { get; set; }
        public virtual DbSet<NoticiasPAD> NoticiasPAD { get; set; }
        public virtual DbSet<RedesSociaisPAD> RedesSociaisPAD { get; set; }
        #endregion

        public virtual DbSet<NotificacoesPush> NotificacoesPush { get; set; }
        public virtual DbSet<GruposPush> GruposPush { get; set; }

        public virtual DbSet<IndicadoresBovespa> IndicadoresBovespa { get; set; }
        public virtual DbSet<MoedaValor> MoedaValor { get; set; }
        public virtual DbSet<IndicadorFinanceiro> IndicadoresFinanceiros { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Apresentadores>()
                .Property(e => e.nome)
                .IsUnicode(false);

            modelBuilder.Entity<Apresentadores>()
                .Property(e => e.fotoInterna)
                .IsUnicode(false);

            modelBuilder.Entity<Apresentadores>()
                .Property(e => e.fotoExterna)
                .IsUnicode(false);

            modelBuilder.Entity<Apresentadores>()
                .Property(e => e.chamada)
                .IsUnicode(false);

            modelBuilder.Entity<Apresentadores>()
                .Property(e => e.texto)
                .IsUnicode(false);

            modelBuilder.Entity<Apresentadores>()
                .Property(e => e.chave)
                .IsUnicode(false);

            modelBuilder.Entity<Apresentadores>()
                .Property(e => e.Instagram)
                .IsUnicode(false);

            modelBuilder.Entity<Apresentadores>()
                .HasMany(e => e.Programacao)
                .WithMany(e => e.Apresentadores)
                .Map(m => m.ToTable("Prog_Apresent").MapLeftKey("idApresent").MapRightKey("idProg"));

            modelBuilder.Entity<AreaBanner>()
                .Property(e => e.Tamanho)
                .IsUnicode(false);

            modelBuilder.Entity<AreaBanner>()
                .HasMany(e => e.Banners)
                .WithMany(e => e.AreaBanner)
                .Map(m => m.ToTable("AreasBanners").MapLeftKey("AreasId").MapRightKey("BannersId"));

            modelBuilder.Entity<AssuntoMateria>()
                .Property(e => e.assunto)
                .IsUnicode(false);

            modelBuilder.Entity<AssuntoMateria>()
                .HasMany(e => e.Materia)
                .WithRequired(e => e.AssuntoMateria)
                .HasForeignKey(e => e.idAssunto)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LiveStreaming>()
                .HasRequired(e => e.Noticia)
                .WithOptional(e => e.LiveStreaming)
                .Map(e => e.MapKey("IdNoticia"));

            modelBuilder.Entity<Banners>()
                .Property(e => e.Titulo)
                .IsUnicode(false);

            modelBuilder.Entity<Banners>()
                .Property(e => e.Anunciante)
                .IsUnicode(false);

            modelBuilder.Entity<Banners>()
                .HasMany(e => e.BannersVisualizacoesCliques)
                .WithRequired(e => e.Banners)
                .HasForeignKey(e => e.CodigoBanner)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Bastidores>()
                .Property(e => e.titulo)
                .IsUnicode(false);

            modelBuilder.Entity<Bastidores>()
                .Property(e => e.chamada)
                .IsUnicode(false);

            modelBuilder.Entity<Bastidores>()
                .Property(e => e.texto)
                .IsUnicode(false);

            modelBuilder.Entity<Bastidores>()
                .HasMany(e => e.BastidoresMidias)
                .WithOptional(e => e.Bastidores)
                .HasForeignKey(e => e.idGaleria);

            modelBuilder.Entity<BastidoresMidias>()
                .Property(e => e.midia)
                .IsUnicode(false);

            modelBuilder.Entity<BastidoresMidias>()
                .Property(e => e.legenda)
                .IsUnicode(false);

            modelBuilder.Entity<Blocos>()
                .Property(e => e.Descricao)
                .IsUnicode(false);

            modelBuilder.Entity<Categorias>()
                .Property(e => e.Titulo)
                .IsUnicode(false);

            modelBuilder.Entity<Categorias>()
                .HasMany(e => e.Noticias)
                .WithMany(e => e.Categorias)
                .Map(m => m.ToTable("Categorias_Noticias").MapLeftKey("idEditoriaCategoria").MapRightKey("idNoticia"));

            modelBuilder.Entity<CategoriasMapa>()
                .Property(e => e.Titulo)
                .IsUnicode(false);

            modelBuilder.Entity<CategoriasMapa>()
                .Property(e => e.Icone)
                .IsUnicode(false);

            modelBuilder.Entity<CategoriasMapa>()
                .Property(e => e.IconeMapa)
                .IsUnicode(false);

            modelBuilder.Entity<CategoriasMapa>()
                .HasMany(e => e.Noticias)
                .WithOptional(e => e.CategoriasMapa)
                .HasForeignKey(e => e.CategoriaMapaId);

            modelBuilder.Entity<cidade>()
                .Property(e => e.nome)
                .IsUnicode(false);

            modelBuilder.Entity<cidade>()
                 .HasMany(e => e.Ouvintes)
                .WithRequired(e => e.cidade)
                .HasForeignKey(e => e.cidade_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Colunista>()
                .Property(e => e.nome)
                .IsUnicode(false);

            modelBuilder.Entity<Colunista>()
                .Property(e => e.descricao)
                .IsUnicode(false);

            modelBuilder.Entity<Colunista>()
                .Property(e => e.foto)
                .IsUnicode(false);

            modelBuilder.Entity<Colunista>()
                .Property(e => e.fotoMini)
                .IsUnicode(false);

            #region PAD

            modelBuilder.Entity<TimesPAD>()
                .HasMany(e => e.Gols)
                .WithRequired(e => e.Mandante)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TimesPAD>()
                .HasMany(e => e.Gols)
                .WithRequired(e => e.Visitante)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApoioPAD>()
                .HasMany(a => a.Noticias)
                .WithOptional(a => a.Apoio)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApoioPAD>()
                .HasMany(a => a.Gols)
                .WithOptional(a => a.Apoio)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApoioPAD>()
                .HasMany(a => a.RedesSociais)
                .WithOptional(a => a.Apoio)
                .WillCascadeOnDelete(false);

            #endregion

            modelBuilder.Entity<Colunista>()
                .HasMany(e => e.ColunistaSeguidores)
                .WithRequired(e => e.Colunista)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Colunista>()
                .HasMany(e => e.Noticias)
                .WithOptional(e => e.Colunista)
                .HasForeignKey(e => e.idColunista);

            modelBuilder.Entity<ColunistaSeguidores>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Editoriais>()
                .Property(e => e.nome)
                .IsUnicode(false);

            modelBuilder.Entity<Editoriais>()
                .HasMany(e => e.Categorias)
                .WithOptional(e => e.Editoriais)
                .HasForeignKey(e => e.EditoriaId);

            modelBuilder.Entity<Editoriais>()
                .HasMany(e => e.Especiais_Secoes)
                .WithRequired(e => e.Editoriais)
                .HasForeignKey(e => e.EditoriaId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Editoriais>()
                .HasMany(e => e.Programacao)
                .WithRequired(e => e.Editoriais)
                .HasForeignKey(e => e.editoriaId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Editoriais>()
                .HasMany(e => e.Galeria)
                .WithMany(e => e.Editoriais)
                .Map(m => m.ToTable("Editoriais_Galerias").MapLeftKey("EditorialId").MapRightKey("GaleriaId"));

            modelBuilder.Entity<Editoriais>()
                .HasMany(e => e.Noticias)
                .WithMany(e => e.Editoriais)
                .Map(m => m.ToTable("Noticias_Editoriais").MapLeftKey("EditoriaId").MapRightKey("NoticiaId"));

            modelBuilder.Entity<Editoriais_Equipe>()
                .Property(e => e.nome)
                .IsUnicode(false);

            modelBuilder.Entity<Editoriais_Equipe>()
                .Property(e => e.texto)
                .IsUnicode(false);

            modelBuilder.Entity<Editoriais_Equipe>()
                .Property(e => e.funcao)
                .IsUnicode(false);

            modelBuilder.Entity<Editoriais_Equipe>()
                .Property(e => e.imagem)
                .IsUnicode(false);

            modelBuilder.Entity<Editoriais_Equipe>()
                .Property(e => e.imagemVertical)
                .IsUnicode(false);

            modelBuilder.Entity<Editoriais_Equipe>()
                .Property(e => e.instagram)
                .IsUnicode(false);

            modelBuilder.Entity<Editoriais_Equipe>()
                .Property(e => e.facebook)
                .IsUnicode(false);

            modelBuilder.Entity<Editoriais_Equipe>()
                .Property(e => e.twitter)
                .IsUnicode(false);

            modelBuilder.Entity<Enquete>()
                .Property(e => e.pergunta)
                .IsUnicode(false);

            modelBuilder.Entity<Enquete>()
                .HasMany(e => e.Respostas)
                .WithOptional(e => e.Enquete)
                .HasForeignKey(e => e.idEnquete);

            modelBuilder.Entity<Equipe>()
                .Property(e => e.nome)
                .IsUnicode(false);

            modelBuilder.Entity<Equipe>()
                .Property(e => e.texto)
                .IsUnicode(false);

            modelBuilder.Entity<Equipe>()
                .Property(e => e.funcao)
                .IsUnicode(false);

            modelBuilder.Entity<Equipe>()
                .Property(e => e.imagem)
                .IsUnicode(false);

            modelBuilder.Entity<Equipe>()
                .Property(e => e.instagram)
                .IsUnicode(false);

            modelBuilder.Entity<Equipe>()
                .Property(e => e.facebook)
                .IsUnicode(false);

            modelBuilder.Entity<Equipe>()
                .Property(e => e.twitter)
                .IsUnicode(false);

            modelBuilder.Entity<Equipe>()
                .Property(e => e.chave)
                .IsUnicode(false);

            modelBuilder.Entity<Especiais_Modelos>()
                .HasMany(e => e.Editoriais)
                .WithOptional(e => e.Especiais_Modelos)
                .HasForeignKey(e => e.modeloEspecial);

            modelBuilder.Entity<Especiais_Secoes>()
                .HasMany(e => e.Secoes_Locais)
                .WithRequired(e => e.Especiais_Secoes)
                .HasForeignKey(e => e.SecaoId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<estado>()
                .Property(e => e.nome)
                .IsUnicode(false);

            modelBuilder.Entity<estado>()
                .Property(e => e.uf)
                .IsUnicode(false);

            modelBuilder.Entity<FaleConosco>()
                .Property(e => e.assunto)
                .IsUnicode(false);

            modelBuilder.Entity<FaleConosco>()
                .Property(e => e.nome)
                .IsUnicode(false);

            modelBuilder.Entity<FaleConosco>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<FaleConosco>()
                .Property(e => e.estado)
                .IsUnicode(false);

            modelBuilder.Entity<FaleConosco>()
                .Property(e => e.cidade)
                .IsUnicode(false);

            modelBuilder.Entity<FaleConosco>()
                .Property(e => e.celular)
                .IsUnicode(false);

            modelBuilder.Entity<FaleConosco>()
                .Property(e => e.telefone)
                .IsUnicode(false);

            modelBuilder.Entity<FaleConosco>()
                .Property(e => e.mensagem)
                .IsUnicode(false);

            modelBuilder.Entity<FaleConosco>()
                .Property(e => e.resposta)
                .IsUnicode(false);

            modelBuilder.Entity<Galeria>()
                .Property(e => e.titulo)
                .IsUnicode(false);

            modelBuilder.Entity<Galeria>()
                .Property(e => e.chamada)
                .IsUnicode(false);

            modelBuilder.Entity<Galeria>()
                .Property(e => e.texto)
                .IsUnicode(false);

            modelBuilder.Entity<Galeria>()
                .HasMany(e => e.Midias)
                .WithOptional(e => e.Galeria)
                .HasForeignKey(e => e.idGaleria);

            modelBuilder.Entity<Galeria>()
                .HasMany(e => e.Noticias)
                .WithOptional(e => e.Galeria)
                .HasForeignKey(e => e.idGaleria);

            modelBuilder.Entity<Horario_programacao>()
                .Property(e => e.horario)
                .IsUnicode(false);

            modelBuilder.Entity<Materia>()
                .Property(e => e.titulo)
                .IsUnicode(false);

            modelBuilder.Entity<Materia>()
                .Property(e => e.chamada)
                .IsUnicode(false);

            modelBuilder.Entity<Materia>()
                .Property(e => e.texto)
                .IsUnicode(false);

            modelBuilder.Entity<Materia>()
                .Property(e => e.foto)
                .IsUnicode(false);

            modelBuilder.Entity<Materia>()
                .Property(e => e.chave)
                .IsUnicode(false);

            modelBuilder.Entity<Materia>()
                .HasMany(e => e.MateriaNovidades)
                .WithRequired(e => e.Materia)
                .HasForeignKey(e => e.idMateria)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MateriaNovidades>()
                .Property(e => e.status)
                .IsUnicode(false);

            modelBuilder.Entity<MateriaNovidades>()
                .Property(e => e.novidades)
                .IsUnicode(false);

            modelBuilder.Entity<MateriaNovidades>()
                .Property(e => e.audio)
                .IsUnicode(false);

            modelBuilder.Entity<Midias>()
                .Property(e => e.midia)
                .IsUnicode(false);

            modelBuilder.Entity<Midias>()
                .Property(e => e.legenda)
                .IsUnicode(false);

            modelBuilder.Entity<Noticias>()
                .Property(e => e.texto)
                .IsUnicode(false);

            modelBuilder.Entity<Noticias>()
                .Property(e=>e.dataCadastro)
                .HasColumnAnnotation(
                IndexAnnotation.AnnotationName,
                new IndexAnnotation(new IndexAttribute("IX_DataCadastro")));

            modelBuilder.Entity<Noticias>()
                .Property(e => e.dataAtualizacao)
                .HasColumnAnnotation(
                IndexAnnotation.AnnotationName,
                new IndexAnnotation(new IndexAttribute("IX_DataAtualizacao")));

            modelBuilder.Entity<Noticias>()
                .HasMany(e => e.Tags)
                .WithMany(e => e.Noticias)
                .Map(m => m.ToTable("NoticiasTags").MapLeftKey("NoticiasId").MapRightKey("TagsId"));

            modelBuilder.Entity<NoticiasRegioes>()
                .Property(e => e.Titulo)
                .IsUnicode(false);

            modelBuilder.Entity<NoticiasRegioes>()
                .HasMany(e => e.Noticias)
                .WithOptional(e => e.NoticiasRegioes)
                .HasForeignKey(e => e.RegiaoId);

            modelBuilder.Entity<Ouvintes>()
                .Property(e => e.nome)
                .IsUnicode(false);

            modelBuilder.Entity<Ouvintes>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<Ouvintes>()
                .Property(e => e.telefone)
                .IsUnicode(false);

            modelBuilder.Entity<Ouvintes>()
                .Property(e => e.endereco)
                .IsUnicode(false);

            modelBuilder.Entity<Ouvintes>()
                .Property(e => e.assunto)
                .IsUnicode(false);

            modelBuilder.Entity<Ouvintes>()
                .Property(e => e.horario)
                .IsUnicode(false);

            modelBuilder.Entity<Ouvintes>()
                .Property(e => e.comentario)
                .IsUnicode(false);

            modelBuilder.Entity<Ouvintes>()
                .HasMany(e => e.OuvintesArquivos)
                .WithRequired(e => e.Ouvintes)
                .HasForeignKey(e => e.idOuvinteDenuncia)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Programacao>()
                .Property(e => e.nome)
                .IsUnicode(false);

            modelBuilder.Entity<Programacao>()
                .Property(e => e.logo)
                .IsUnicode(false);

            modelBuilder.Entity<Programacao>()
                .Property(e => e.imagem)
                .IsUnicode(false);

            modelBuilder.Entity<Programacao>()
                .Property(e => e.texto)
                .IsUnicode(false);

            modelBuilder.Entity<Programacao>()
                .Property(e => e.chamada)
                .IsUnicode(false);

            modelBuilder.Entity<Programacao>()
                .Property(e => e.horario)
                .HasPrecision(0);

            modelBuilder.Entity<Programacao>()
                .Property(e => e.chave)
                .IsUnicode(false);

            modelBuilder.Entity<Programacao>()
                .HasMany(e => e.Horario_programacao)
                .WithOptional(e => e.Programacao)
                .HasForeignKey(e => e.idPrograma);

            modelBuilder.Entity<Programacao>()
                .HasMany(e => e.Materia)
                .WithRequired(e => e.Programacao)
                .HasForeignKey(e => e.idProgramacao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Promocoes>()
                .HasMany(e => e.Promocoes_Participantes)
                .WithRequired(e => e.Promocoes)
                .HasForeignKey(e => e.PromocaoCodigo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Promocoes>()
                .HasMany(e => e.Promocoes_Resultados)
                .WithRequired(e => e.Promocoes)
                .HasForeignKey(e => e.PromocaoCodigo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Regioes>()
                .HasMany(e => e.Ouvintes)
                .WithRequired(e => e.Regioes)
                .HasForeignKey(e => e.regiaoId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Respostas>()
                .Property(e => e.resposta)
                .IsUnicode(false);

            modelBuilder.Entity<RotasNoRio>()
                .Property(e => e.Texto)
                .IsUnicode(false);

            modelBuilder.Entity<Secoes_Locais>()
                .Property(e => e.Descricao)
                .IsUnicode(false);

            modelBuilder.Entity<Tags>()
                .Property(e => e.chave)
                .IsUnicode(false);

            modelBuilder.Entity<TrabalheConosco>()
                .Property(e => e.nome)
                .IsUnicode(false);

            modelBuilder.Entity<TrabalheConosco>()
                .Property(e => e.celular)
                .IsUnicode(false);

            modelBuilder.Entity<TrabalheConosco>()
                .Property(e => e.telefone)
                .IsUnicode(false);

            modelBuilder.Entity<TrabalheConosco>()
                .Property(e => e.estado)
                .IsUnicode(false);

            modelBuilder.Entity<TrabalheConosco>()
                .Property(e => e.cidade)
                .IsUnicode(false);

            modelBuilder.Entity<TrabalheConosco>()
                .Property(e => e.areaPretencao)
                .IsUnicode(false);

            modelBuilder.Entity<TrabalheConosco>()
                .Property(e => e.curriculo)
                .IsUnicode(false);

            modelBuilder.Entity<TrabalheConosco>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<Transporte>()
                .Property(e => e.Titulo)
                .IsUnicode(false);

            modelBuilder.Entity<Transporte>()
                .Property(e => e.CssClass)
                .IsUnicode(false);

            modelBuilder.Entity<UserProfile>()
                .Property(e => e.Sexo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<UserProfile>()
                .HasOptional(e => e.Promocoes_Participantes)
                .WithRequired(e => e.UserProfile);

            modelBuilder.Entity<UserProfile>()
                .HasMany(e => e.Promocoes_Resultados)
                .WithRequired(e => e.UserProfile)
                .HasForeignKey(e => e.ParticipanteCodigo)
                .WillCascadeOnDelete(false);

            ///

            modelBuilder.Entity<Editoriais>()
            .Property(e => e.nome)
            .IsUnicode(false);

            modelBuilder.Entity<Editoriais>()
                .HasMany(e => e.Times)
                .WithRequired(e => e.Editoriais)
                .HasForeignKey(e => e.EditoriaId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Times>()
                .Property(e => e.Titulos)
                .IsUnicode(false);

            modelBuilder.Entity<Times>()
                .Property(e => e.Informacoes)
                .IsUnicode(false);

            modelBuilder.Entity<Times>()
                .HasMany(e => e.Elencos)
                .WithRequired(e => e.Times)
                .HasForeignKey(e => e.TimeId)
                .WillCascadeOnDelete(false);

            #region EVENTOS E ACONTECIMENTOS
            modelBuilder.Entity<Eventos>()
                .HasMany(e => e.Acontecimentos)
                .WithRequired(e => e.Evento)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Acontecimentos>()
                .HasRequired(e => e.Evento)
                .WithMany(e => e.Acontecimentos)
                .HasForeignKey(e => e.EventoId)
                .WillCascadeOnDelete(false);
            #endregion
        }
    }
}
