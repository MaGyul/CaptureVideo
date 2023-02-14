using System;

namespace CaptureVideo
{
    internal class SimpleConsole
    {
        public void log(object msg) => Console.WriteLine(msg);

        public void info(object msg) => ColorWriteLine(ConsoleColor.Blue, msg);

        public void warn(object msg) => ColorWriteLine(ConsoleColor.Yellow, msg);

        public void error(object msg) => ColorWriteLine(ConsoleColor.Red, msg);

        private void ColorWriteLine(ConsoleColor color, object msg)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(msg);
            Console.ResetColor();
        }
    }
}
