using StackExchange.Redis;
using System;
using System.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;

namespace Site.Services
{
    public class RedisService
    {
        #region Singleton
        private static RedisService instance;
        //Construtor que define os atributos básicos da instância.
        public RedisService()
        {
            //Lista com todas as chaves e a hora da gravação delas no cache.
            list = new List<RedisListValue>();
            //Conexão buscada direto no WebConfig - A padrão é "localhost:6379"
            CacheConnection = ConfigurationManager.AppSettings["CacheConnection"];
            //Chave que será adiciona como prefixo em todas as chaves no registro.
            GlobalKey = ConfigurationManager.AppSettings["GlobalKey"];
        }
        //Instância do Singleton.
        public static RedisService Instance
        {
            get
            {
                //Caso não exista uma instância ela será criada.
                if (instance == null)
                {
                    instance = new RedisService();
                    //Na criação de uma nova instância é bom que o cache seja limpo para evitar inconsistências.
                    
                }
                return instance;
            }
        }
        #endregion

        #region InstanceUsage
        private List<RedisListValue> list;
        private string CacheConnection = "";
        private string GlobalKey = "";
        #endregion

        #region PrivateUsage
        //Método usado para a criação da conexão com o Redis.
        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            if (string.IsNullOrEmpty(Instance.CacheConnection))
                throw new Exception("Não há uma conexão definida! Leia as instruções no Git para criá-la."); 
            return ConnectionMultiplexer.Connect(Instance.CacheConnection + ", allowAdmin=true");
        });
        //Retorna a conexão do Redis para ser usada nos métodos.
        private static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }
        //Método criado para gerenciar e manter a lista de chaves atualizada.
        private static void UpdateList<T>(string key, Func<object, T> funcao, int time = 0, params object[] parameter)
        {
            //Pega o item da lista com a chave passada.
            try
            {
                var item = Instance.list.Where(l => l.Key == key).FirstOrDefault();
                //Verifica se o item existe.
                if (item != null)
                {
                    //Se o valor de expiração for zero é porque a chave é persistente e não deverá ser atualizada.
                    if (time == 0) return;
                    //Pega o valor da ultima atualização dessa chave.
                    var callAgain = item.LastCall;
                    //Adiciona o tempo de expiração.
                    callAgain = callAgain.AddSeconds(time);
                    //Caso a chave já tenha expirado.
                    if (callAgain <= DateTime.Now)
                    {
                        //Atualiza o valor da última atualização.
                        item.LastCall = DateTime.Now;
                        //Atualiza o registro do Redis.
                        SetToRedis(key, funcao(parameter));
                    }
                }
                else
                {
                    //Se o item não existe ele passará a existir.
                    Instance.list.Add(new RedisListValue(key));
                }
            } catch { }
        }
        //Adiciona valor no registro.
        private static void SetToRedis(string key, object retorno)
        {
            try { 
                //Abre a conexão.
                IDatabase cache = Connection.GetDatabase();
                //Transforma o objeto em string para ser armazenado.
                var serializedObj = JsonConvert.SerializeObject(retorno);
                //Armazena no Redis.
                cache.StringSet(key, serializedObj);
            }
            catch (Exception e) {
                new Task(() => { SendEmailError("Erro no Set do Redis", key, e); }).Start();
            }
        }
        //Pega valores do registro.
        private static object GetFromRedis<T>(string key)
        {
            try
            {
                //Abre a conexão.
                IDatabase cache = Connection.GetDatabase();
                //Tenta pegar o valor do registro do Redis.
                string serializedObj = cache.StringGet(key);
                //Caso o valor exista ele será transformado de string para objeto e retornado.
                if (!string.IsNullOrEmpty(serializedObj))
                    return JsonConvert.DeserializeObject<T>(serializedObj);
                else
                    return null;
            } catch (Exception e)
            {
                new Task(() => { SendEmailError("Erro no Get do Redis", key, e); }).Start();
                return null;
            }
        }
        private static void SendEmailError(string subject, string key, Exception e)
        {
            var smtp = new SmtpClient
            {
                Host = "smtp.agencia1.com.br",
                Port = 587,
                Credentials = new NetworkCredential("mailer@agencia1.com.br", "ay!A1jr7")
            };
            try
            {
                var mail = new MailMessage();
                mail.To.Add("renato.coutinho@eduardodev.com.br");
                mail.From = new MailAddress("mailer@agencia1.com.br");
                mail.Subject = subject;
                var body = "<html><head></head><body>";
                body += "Houve um erro no Redis.<br>";
                body += "key: " + key + " <br>";
                if (e != null)
                {
                    body += "e.Message: " + e.Message + " <br>";
                    if (e.InnerException != null)
                    {
                        body += "e.InnerMessage: " + e.InnerException.Message + " <br>";
                    }
                }
                body += "</html>";
                mail.Body = body;
                mail.IsBodyHtml = true;
                smtp.Send(mail);
            }
            catch (Exception){
                
            }
        }
        #endregion

        #region PublicUsage
        //Função principal para pegar um valor do registro e caso ele não exista a função de callback
        //será chamada, o valor armazenado no registro e na lista.
        public static T GetOrSetToRedis<T>(string key, Func<object, T> callback, int time, params object[] parameter)
        {
            //Adiciona a chave global na chave passada.
            key = Instance.GlobalKey + key;
            //Atualiza a lista assincronamente.
            new Task(() => { UpdateList(key, callback, time, parameter); }).Start();
            //Pega o valor do registro.
            object retorno = GetFromRedis<T>(key);
            //Caso o valor não exista ele será inserido.
            if (retorno == null) {
                //Usa o callback para pegar o valor do banco de dados.
                retorno = callback(parameter);
                //Adiciona no Registro.
                if (retorno != null)
                    SetToRedis(key, retorno);
            }
            //Retorna o valor do registro ou do banco de dados.
            return (T)retorno;
        }
        //Limpa os registro do Redis que contenham um determinado padrão.
        public static void FlushAll(string pattern = "")
        {
            new Task(() => { FlushAllASync(pattern); }).Start();
        }
        public static void FlushAllASync(string pattern = "")
        {
            //Instancia o servidor do Redis.
            var server = Connection.GetServer(Instance.CacheConnection);
            //Se o padrão for vazio todos os registro serão apagados.
            if (string.IsNullOrEmpty(pattern))
            {
                server.FlushDatabase();
            }
            else
            {
                //Senão eu pego as chaves direto do servidor do Redis.
                var keys = server.Keys();
                //Abro a conexão.
                IDatabase cache = Connection.GetDatabase();
                //Para cada valor encontrado.
                foreach (var key in keys)
                {
                    //Se o valor contêm o padrão determinado ele será apagado.
                    if (key.ToString().Contains(pattern))
                    {
                        cache.KeyDelete(key);
                    }
                }
            }
        }
        public static void LeftPush(object obj, string key)
        {
            if (string.IsNullOrEmpty(key) || obj == null) return;
            //Instancia o Redis caso ele não exista.
            var globalKey = Instance.GlobalKey;
            IDatabase cache = Connection.GetDatabase();
            var serializedObj = JsonConvert.SerializeObject(obj);
            cache.ListLeftPush(key, serializedObj);
        }
        public static object RightPop<T>(string key)
        {
            //Instancia o Redis caso ele não exista.
            var globalKey = Instance.GlobalKey;
            //Abre a conexão.
            IDatabase cache = Connection.GetDatabase();
            //Tenta pegar o valor do registro do Redis.
            string serializedObj = cache.ListRightPop(key);
            //Caso o valor exista ele será transformado de string para objeto e retornado.
            if (!string.IsNullOrEmpty(serializedObj))
                return JsonConvert.DeserializeObject<T>(serializedObj);
            else
                return null;
        }
        #endregion
    }
    //Classe usada apenas para criar a lista de registros.
    public class RedisListValue
    {
        public RedisListValue(string key) {
            Key = key;
            LastCall = DateTime.Now;
        }
        public string Key { get; set; }
        public DateTime LastCall { get; set; }
    }
}