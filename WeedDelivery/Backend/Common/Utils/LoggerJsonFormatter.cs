using System.Collections;
using Newtonsoft.Json;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Json;

namespace WeedDelivery.Backend.Common.Utils;

public class LoggerJsonFormatter : ITextFormatter
{
    private readonly string _environmentUserNameRoot;
    private readonly string _environmentMachineNameRoot;
    private readonly string _processNameRoot;
    private readonly string _sourceContextRoot;
    private readonly JsonValueFormatter _valueFormatter = new JsonValueFormatter(typeTagName: null);

    /// <summary>
    /// Initializes a new instance of the <see cref="LoggerJsonFormatter"/> class.
    /// </summary>
    /// <param name="environmentUserNameRoot">Дополнительное поле, которое попадает в Properties, чтобы идентифицировать User</param>
    /// <param name="environmentMachineNameRoot">Дополнительное поле, которое попадает в Properties, чтобы идентифицировать Host</param>
    /// <param name="processNameRoot">Дополнительное поле, которое попадает в Properties, чтобы идентифицировать System</param>
    public LoggerJsonFormatter(string environmentUserNameRoot = null, string environmentMachineNameRoot = null, string processNameRoot = null)
    {
        _environmentUserNameRoot = environmentUserNameRoot ?? "EnvironmentUserName";
        _environmentMachineNameRoot = environmentMachineNameRoot ?? "MachineName";
        _processNameRoot = processNameRoot ?? "ProcessName";
        _sourceContextRoot = "SourceContext";
    }

    public void Format(LogEvent logEvent, TextWriter output)
    {
        output.Write("{\"date\":\"");
        output.Write(logEvent.Timestamp.LocalDateTime.ToString("O"));

        output.Write(",\"level\":\"");
        output.Write(logEvent.Level);
        output.Write('\"');

        var propCount = logEvent.Properties.Count;

        if (logEvent.Exception != null)
        {
            output.Write(",\"traceBack\":");
            output.Write($"\"{logEvent.Exception.StackTrace}\"");
            
            output.Write(",\"message\":");
            output.Write($"\"{logEvent.Exception.Message}\"");
        }
        else
        {
            output.Write("\",\"message\":");
            var message = logEvent.MessageTemplate.Render(logEvent.Properties);
            JsonValueFormatter.WriteQuotedJsonString(message, output);
        }

        if (logEvent.Properties.TryGetValue(_environmentUserNameRoot, out var environmentUser))
        {
            output.Write(",\"user\":");
            _valueFormatter.Format(environmentUser, output);
            propCount--;
        }
        
        if (logEvent.Properties.TryGetValue(_environmentMachineNameRoot, out var environmentMachineName))
        {
            output.Write(",\"host\":");
            _valueFormatter.Format(environmentMachineName, output);
            propCount--;
        }
        
        if (logEvent.Properties.TryGetValue(_processNameRoot, out var processNameProperty))
        {
            output.Write(",\"system\":");
            _valueFormatter.Format(processNameProperty, output);
            propCount--;
        }
        
        if (logEvent.Properties.TryGetValue(_sourceContextRoot, out var sourceContextProperty))
        {
            output.Write(",\"subsystem\":");
            _valueFormatter.Format(sourceContextProperty, output);
            propCount--;
        }

        output.WriteLine('}');
    }

    private string SerializeExceptionToJson(Exception exception)
    {
        var error = new Dictionary<string, string>
        {
            {"type", exception.GetType().ToString()},
            {"message", exception.Message},
            {"stackTrace", exception.StackTrace?.Trim()}
        };

        foreach (DictionaryEntry data in exception.Data)
        {
            if (data.Value != null)
                error.Add(data.Key.ToString() ?? string.Empty, data.Value.ToString());
        }

        var json = JsonConvert.SerializeObject(error, Formatting.Indented);

        return json;
    }
}