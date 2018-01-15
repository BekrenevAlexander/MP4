using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.WebSockets;
using System.Web.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MP4_Durak
{
    class client
    {
        public WebSocket socket;
        public string room;

        public client(WebSocket ws, string room)
        {
            this.room = room;
            this.socket = ws;
        }
    }
    /// <summary>
    /// Сводное описание для ChatHandler
    /// </summary>
    public class ChatHandler : IHttpHandler
    {
        // Список всех клиентов
        private static readonly List<client> Clients = new List<client>();

        // Блокировка для обеспечения потокабезопасности
        private static readonly ReaderWriterLockSlim Locker = new ReaderWriterLockSlim();

        public void ProcessRequest(HttpContext context)
        {
            //Если запрос является запросом веб сокета
            if (context.IsWebSocketRequest)
                context.AcceptWebSocketRequest(WebSocketRequest);
        }
        private async Task WebSocketRequest(AspNetWebSocketContext context)
        {
            // Получаем сокет клиента из контекста запроса
            var socket = context.WebSocket;
            var req = HttpContext.Current.Request;
            
            // Добавляем его в список клиентов
            Locker.EnterWriteLock();
            try
            {
                string roomId = Convert.ToString(req.QueryString["room"]);
                Clients.Add(new client(socket, roomId));
            }
            finally
            {
                Locker.ExitWriteLock();
            }

            // Слушаем его
            while (true)
            {
                var buffer = new ArraySegment<byte>(new byte[1024]);

                // Ожидаем данные от него
                var result = await socket.ReceiveAsync(buffer, CancellationToken.None);


                //Передаём сообщение всем клиентам
                for (int i = 0; i < Clients.Count; i++)
                {

                    client client = Clients[i];

                    try
                    {
                        var requestFront = System.Text.Encoding.Default.GetString(buffer.Array);
                        dynamic dataFromFront = JsonConvert.DeserializeObject(requestFront);

                        string message = dataFromFront.message;
                        string room = dataFromFront.room;

                        var message_byte = new ArraySegment<byte>(System.Text.Encoding.Default.GetBytes(message));
                        if (client.socket.State == WebSocketState.Open && client.room == room)
                        {
                            await client.socket.SendAsync(message_byte, WebSocketMessageType.Text, true, CancellationToken.None);
                        }
                    }

                    catch (ObjectDisposedException)
                    {
                        Locker.EnterWriteLock();
                        try
                        {
                            Clients.Remove(client);
                            i--;
                        }
                        finally
                        {
                            Locker.ExitWriteLock();
                        }
                    }
                }

            }
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}