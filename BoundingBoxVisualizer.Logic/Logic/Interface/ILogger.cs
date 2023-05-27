using System;

namespace BoundingBoxVisualizer.Logic.Logic
{
    public interface ILogger
    {
        void Error(string message);
        void Error(string message, Exception ex);
        void Warning(string message);
        void Information(string message);
    }
}