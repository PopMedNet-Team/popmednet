using PopMedNet.DMCS.Data.Enums;

namespace PopMedNet.DMCS.Utils
{
    public static class EnumExtensions
    {
        public static string GetDisplayValue(this LogEventLevel level)
        {
            switch (level)
            {
                case LogEventLevel.Verbose:
                    return "Verbose";
                case LogEventLevel.Debug:
                    return "Debug";
                case LogEventLevel.Information:
                    return "Information";
                case LogEventLevel.Warning:
                    return "Warning";
                case LogEventLevel.Error:
                    return "Error";
                case LogEventLevel.Fatal:
                    return "Fatal";
                default:
                    return "Unkown Level";
            }
        }
    }
}
