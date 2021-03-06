using System;
using Microsoft.Extensions.Logging;
using SharpRaven;
using SharpRaven.Data;

namespace LBHAddressesAPI.Infrastructure.V1.Logging
{
    public class SentryLogger : ILogger
    {
        private readonly string _name;
        private readonly string _url;
        private readonly string _environment;
        private readonly RavenClient _ravenClient;
        

        public SentryLogger(string name, string url, string environment)
        {
            _name = name;
            _url = url;
            _environment = environment;
            _ravenClient = new RavenClient(_url);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if(exception != null)
            {
                var ev = new SentryEvent(exception);
                ev.Tags.Add("environment", _environment);
                _ravenClient.Capture(ev);
            }
        }
    }
}
