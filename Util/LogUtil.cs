using StardewModdingAPI;
using System.Runtime.CompilerServices;

namespace FenceTuner.Util;

internal class LogUtil(IMonitor logger)
{
    private readonly IMonitor Monitor = logger;

    public void Trace(string message, bool once = false)
    {
        if (once)
            Monitor.LogOnce(message, LogLevel.Trace);
        else
            Monitor.Log(message, LogLevel.Trace);
    }

    public void Debug(string message, bool once = false)
    {
        if (once)
            Monitor.LogOnce(message, LogLevel.Debug);
        else
            Monitor.Log(message, LogLevel.Debug);
    }

    public void Info(string message, bool once = false)
    {
        if (once)
            Monitor.LogOnce(message, LogLevel.Info);
        else
            Monitor.Log(message, LogLevel.Info);
    }

    public void Warn(string message, bool once = false)
    {
        if (once)
            Monitor.LogOnce(message, LogLevel.Warn);
        else
            Monitor.Log(message, LogLevel.Warn);
    }

    public void Error(string message, bool once = false)
    {
        if (once)
            Monitor.LogOnce(message, LogLevel.Error);
        else
            Monitor.Log(message, LogLevel.Error);
    }

    public void AlertHere([CallerMemberName] string name = "", [CallerLineNumber] int num = -1, bool once = false)
    {
        if (once)
            Monitor.LogOnce($"Warn from {name} at line {num}", LogLevel.Alert);
        else
            Monitor.Log($"Warn from {name} at line {num}", LogLevel.Alert);
    }
}