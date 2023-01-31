using ConfiApp.Modelos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ConfiApp.Utils
{
    class HttpNotificaciones
    {
        public static async Task GetAllNewsAsync(Action<IEnumerable<Notificaciones>> action)
        {

             var request = new HttpRequestMessage();
            request.RequestUri = new Uri(App.Current.Properties["DDNS"].ToString() + "/ApiNotificacionesUsuario?Usuario=" + App.Current.Properties["name"].ToString());
            request.Method = HttpMethod.Get;
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {

                string content = await response.Content.ReadAsStringAsync();
                var list  = JsonConvert.DeserializeObject<IEnumerable<Notificaciones>>(content);



                action(list);








            }
        }
    }
}
