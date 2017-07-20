using System;
using Microsoft.Extensions.Logging;

namespace Music.Infrastructure.Logging
{
    public class Logger
    {
        public static ILoggerFactory LoggerFactory { get; set; }
        public static ILogger CreateLogger<T>() => LoggerFactory.CreateLogger<T>();
    }
}