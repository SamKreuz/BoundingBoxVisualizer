using System;
using System.Diagnostics;

namespace BoundingBoxVisualizer.Logic.Logic.Logger
{
    public class ConsoleLogger : ILogger
    {
        public void Error(string message)
        {
            Debug.WriteLine("Error: " + message);
        }

        public void Error(string message, Exception ex)
        {
            Debug.WriteLine($"Error: {message} {ex.StackTrace} {ex.Message}");
        }

        public void Information(string message)
        {
            Debug.WriteLine($"Information: {message}");
        }

        public void Warning(string message)
        {
            Debug.WriteLine($"Warning: {message}");
        }
    }
}
