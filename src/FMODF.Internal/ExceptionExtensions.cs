using System;
using System.Reflection;

namespace FullModdedFuriesAPI.Internal
{
    /// <summary>Provides extension methods for handling exceptions.</summary>
    internal static class ExceptionExtensions
    {
        /*********
        ** Public methods
        *********/
        /// <summary>Get a string representation of an exception suitable for writing to the error log.</summary>
        /// <param name="exception">The error to summarize.</param>
        public static string GetLogSummary(this Exception exception)
        {
            try
            {
                switch (exception)
                {
                    case TypeLoadException ex:
                        return $"Failed loading type '{ex.TypeName}': {exception}";

                    case ReflectionTypeLoadException ex:
                        string summary = ex.ToString();
                        foreach (Exception childEx in ex.LoaderExceptions ?? new Exception[0])
                            summary += $"\n\n{childEx?.GetLogSummary()}";
                        return summary;

                    default:
                        return exception?.ToString() ?? $"<null exception>\n{Environment.StackTrace}";
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed handling {exception?.GetType().FullName} (original message: {exception?.Message})", ex);
            }
        }

        /// <summary>Get the lowest exception in an exception stack.</summary>
        /// <param name="exception">The exception from which to search.</param>
        public static Exception GetInnermostException(this Exception exception)
        {
            while (exception.InnerException != null)
                exception = exception.InnerException;
            return exception;
        }
    }
}
