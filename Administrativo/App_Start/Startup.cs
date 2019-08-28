using System;
using Hangfire;
using Owin;
using Microsoft.Owin;
using Hangfire.SqlServer;
using Administrativo.Controllers;
using Administrativo.Services;

[assembly: OwinStartup(typeof(Administrativo.App_Start.Startup))]
namespace Administrativo.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //JobStorage.Current = new SqlServerStorage("Radio");
            //app.UseHangfireDashboard();
            //app.UseHangfireServer();
            //var notificacoesController = new NotificacoesPushController();
            //var redesSociaisController = new RedesSociaisController();
            ////RecurringJob.AddOrUpdate(() => notificacoesController.VerificaNotificacao(), Cron.Minutely, TimeZoneInfo.Local);
            //RecurringJob.AddOrUpdate(() => redesSociaisController.AtualizaRedesSociais(), Cron.Minutely, TimeZoneInfo.Local);
            //RecurringJob.AddOrUpdate(() => UpdateCache.Sync(), Cron.MinuteInterval(5), TimeZoneInfo.Local);

            ////RecurringJob.AddOrUpdate(() => UpdateIbovespa.Sync(), Cron.MinuteInterval(15), TimeZoneInfo.Local);
            ////RecurringJob.AddOrUpdate(() => Site.Services.RedisService.FlushAll(""), Cron.MinuteInterval(20), TimeZoneInfo.Local);
            ////RecurringJob.AddOrUpdate(() => BannersFIFO.BannerFromFIFOToDB(), Cron.Minutely, TimeZoneInfo.Local);
        }
    }
}