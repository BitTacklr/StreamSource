namespace StreamSource.Naming
{
    internal static class StreamNameFormat
    {
        public const char Escape = '\\';
        public const char ComponentSeparator = ',';
        public const char ComponentNameValueSeparator = '=';

        public static bool Escapable(char value)
        {
            return value == Escape || value == ComponentNameValueSeparator || value == ComponentSeparator;
        }
    }
}
