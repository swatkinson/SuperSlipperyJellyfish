using BepInEx.Logging;

namespace SuperSlipperyJellyfish
{
    /// <summary>
    /// Lightweight wrapper around BepInEx logging to avoid repeating boilerplate.
    /// </summary>
    public static class Log
    {
        private static readonly object SourceLock = new object();
        private static readonly ManualLogSource Source = new ManualLogSource(ModInfo.guid);
        private static bool _initialized;

        public static ManualLogSource Write => Source;

        public static void Init(string initMessage)
        {
            lock (SourceLock)
            {
                if (_initialized)
                    return;

                BepInEx.Logging.Logger.Sources.Add(Source);
                Source.LogMessage(initMessage);
                Source.LogMessage("Logger component initialised.");
                _initialized = true;
            }
        }

        public static void Delete()
        {
            lock (SourceLock)
            {
                if (!_initialized)
                    return;

                Source.LogMessage("Logger component deleted.");
                BepInEx.Logging.Logger.Sources.Remove(Source);
                _initialized = false;
            }
        }
    }
}
