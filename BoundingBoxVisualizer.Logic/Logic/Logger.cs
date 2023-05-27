using System;
using System.Diagnostics;

namespace BoundingBoxVisualizer.Logic.Logic
{
    public class Logger : ILogger
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
            throw new System.NotImplementedException();
        }

        public void Warning(string message)
        {
            throw new System.NotImplementedException();
        }
    }
}
