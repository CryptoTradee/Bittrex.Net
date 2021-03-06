﻿using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using Bittrex.Net.Interfaces;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using Newtonsoft.Json;
using WebSocket4Net;

namespace Bittrex.Net.UnitTests.TestImplementations
{
    public class TestSocket: ISignalRSocket
    {

        private object proxyData;
        public bool CanConnect { get; set; }
        public bool Connected { get; set; }

        public event Action OnClose;
        public event Action<string> OnMessage;
        public event Action<Exception> OnError;
        public event Action OnOpen;

        public int Id { get; }
        public bool ShouldReconnect { get; set; }
        public Func<byte[], string> DataInterpreter { get; set; }
        public DateTime? DisconnectTime { get; set; }
        public string Url { get; }
        public WebSocketState SocketState { get; }
        public bool IsClosed => !Connected;
        public bool IsOpen => Connected;
        public bool PingConnection { get; set; }
        public TimeSpan PingInterval { get; set; }
        public SslProtocols SSLProtocols { get; set; }
        public TimeSpan Timeout { get; set; }

        public Task<bool> Connect()
        {
            Connected = CanConnect;
            return Task.FromResult(CanConnect);
        }

        public void Send(string data)
        {
            if(!Connected)
                throw new Exception("Socket not connected");
        }

        public void Reset() { 
        }

        public Task Close()
        {
            Connected = false;
            return Task.FromResult(0);
        }

        public void SetProxy(string host, int port)
        {
            throw new NotImplementedException();
        }
        public void Dispose()
        {
        }

        public void InvokeClose()
        {
            Connected = false;
            OnClose?.Invoke();
        }

        public void InvokeOpen()
        {
            OnOpen?.Invoke();
        }

        public void InvokeMessage(string data)
        {
            OnMessage?.Invoke(data);
        }

        public void InvokeMessage<T>(T data)
        {
            OnMessage?.Invoke(JsonConvert.SerializeObject(data));
        }

        public void SetHub(string name)
        {
        }

        public void SetProxyResponse<T>(T data)
        {
            proxyData = data;
        }

        public Task<CallResult<T>> InvokeProxy<T>(string call, params string[] pars)
        {
            return Task.FromResult<CallResult<T>>(new CallResult<T>((T)proxyData, null));
        }
    }
}
