﻿using Newtonsoft.Json;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace PostgresSink
{
    public static class LogEventExtensions
    {
        internal static string Json(this LogEvent logEvent, bool storeTimestampInUtc = false)
        {
            return JsonConvert.SerializeObject(ConvertToDictionary(logEvent, storeTimestampInUtc));
        }

        internal static IDictionary<string, object> Dictionary(this LogEvent logEvent,
            bool storeTimestampInUtc = false,
            IFormatProvider formatProvider = null)
        {
            return ConvertToDictionary(logEvent, storeTimestampInUtc, formatProvider);
        }

        internal static string Json(this IReadOnlyDictionary<string, LogEventPropertyValue> properties)
        {
            return JsonConvert.SerializeObject(ConvertToDictionary(properties));
        }

        internal static IDictionary<string, object> Dictionary(
            this IReadOnlyDictionary<string, LogEventPropertyValue> properties)
        {
            return ConvertToDictionary(properties);
        }

        #region Private implementation

        private static dynamic ConvertToDictionary(IReadOnlyDictionary<string, LogEventPropertyValue> properties)
        {
            var expObject = new ExpandoObject() as IDictionary<string, object>;
            foreach (var property in properties)
                expObject.Add(property.Key, Simplify(property.Value));
            return expObject;
        }

        private static dynamic ConvertToDictionary(LogEvent logEvent,
            bool storeTimestampInUtc,
            IFormatProvider formatProvider = null)
        {
            var eventObject = new ExpandoObject() as IDictionary<string, object>;
            eventObject.Add("Timestamp", storeTimestampInUtc
                ? logEvent.Timestamp.ToUniversalTime().ToString("o")
                : logEvent.Timestamp.ToString("o"));

            eventObject.Add("LogLevel", logEvent.Level.ToString());
            eventObject.Add("LogMessage", logEvent.RenderMessage(formatProvider));
            eventObject.Add("LogException", logEvent.Exception);
            eventObject.Add("LogProperties", logEvent.Properties.Dictionary());

            return eventObject;
        }

        private static object Simplify(LogEventPropertyValue data)
        {
            var value = data as ScalarValue;
            if (value != null)
                return value.Value;

            // ReSharper disable once SuspiciousTypeConversion.Global
            var dictValue = data as IReadOnlyDictionary<string, LogEventPropertyValue>;
            if (dictValue != null)
            {
                var expObject = new ExpandoObject() as IDictionary<string, object>;
                foreach (var item in dictValue.Keys)
                    expObject.Add(item, Simplify(dictValue[item]));
                return expObject;
            }

            var seq = data as SequenceValue;
            if (seq != null)
                return seq.Elements.Select(Simplify).ToArray();

            var str = data as StructureValue;
            if (str == null) return null;
            {
                try
                {
                    if (str.TypeTag == null)
                        return str.Properties.ToDictionary(p => p.Name, p => Simplify(p.Value));

                    if (!str.TypeTag.StartsWith("DictionaryEntry") && !str.TypeTag.StartsWith("KeyValuePair"))
                        return str.Properties.ToDictionary(p => p.Name, p => Simplify(p.Value));

                    var key = Simplify(str.Properties[0].Value);
                    if (key == null)
                        return null;

                    var expObject = new ExpandoObject() as IDictionary<string, object>;
                    expObject.Add(key.ToString(), Simplify(str.Properties[1].Value));
                    return expObject;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return null;
        }

        #endregion
    }
}
