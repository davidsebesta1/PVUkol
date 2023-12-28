namespace PVUkol.Extensions
{
    /// <summary>
    /// Static class containing methods to write a colorful text in a console (yipee)
    /// </summary>
    public static class ConsoleExtension
    {
        /// <summary>
        /// Writes a text in console with specified color
        /// </summary>
        /// <param name="message"></param>
        /// <param name="color"></param>
        public static void Write(string? message, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.Write(message);
            Console.ResetColor();
        }

        /// <summary>
        /// Writes a text and a line break in console with specified color
        /// </summary>
        /// <param name="message"></param>
        /// <param name="color"></param>
        public static void WriteLine(string? message, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
