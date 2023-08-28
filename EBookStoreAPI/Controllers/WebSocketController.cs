using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using Microsoft.AspNetCore.Cors;

namespace EBookStoreAPI.Controllers
{
    [EnableCors("AllowAll")]
    //[Route("api/[controller]")]
    [ApiController]
    public class WebSocketController : ControllerBase
    {
        static ConcurrentDictionary<int, WebSocket> WebSockets = new ConcurrentDictionary<int, WebSocket>();

        // GET: api/WebSocket/ws
        [HttpGet("ws")]
        public async Task Connect()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using WebSocket socket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                await ProcessWebSocket(socket);
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }

        private async Task ProcessWebSocket(WebSocket socket)
        {
            WebSockets.TryAdd(socket.GetHashCode(), socket);
            byte[] buffer = new byte[1024 * 4];
            var res = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            string? UserName = null;
            while (!res.CloseStatus.HasValue)
            {
                UserName = "客服";
                string cmd = Encoding.UTF8.GetString(buffer, 0, res.Count);
                JObject data = JObject.Parse(cmd);
                string Name = Convert.ToString(data["userName"]);
                string? Message = $"{Convert.ToString(data["message"])}";
                if (!string.IsNullOrEmpty(Name))
                {
                    UserName = Name;
                }
                Broadcast(JsonConvert.SerializeObject(new
                {
                    userName = UserName,
                    message = Message

                }));
                res = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            await socket.CloseAsync(res.CloseStatus.Value, res.CloseStatusDescription, CancellationToken.None);
            WebSockets.TryRemove(socket.GetHashCode(), out var remove);
            Broadcast(JsonConvert.SerializeObject(new
            {
                userName = UserName,
                message = "離開聊天室",
            }));
        }


        private void Broadcast(string Message)
        {
            byte[] buff = Encoding.UTF8.GetBytes(Message);
            var data = new ArraySegment<byte>(buff, 0, buff.Length);
            Parallel.ForEach(WebSockets.Values, async (socket) =>
            {
                if (socket.State == WebSocketState.Open)
                {
                    await socket.SendAsync(data, WebSocketMessageType.Text, true, CancellationToken.None);
                }
            });
        }

    }
}
