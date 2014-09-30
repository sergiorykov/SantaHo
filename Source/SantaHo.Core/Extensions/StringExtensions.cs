namespace SantaHo.Core.Extensions
{
    public static class StringExtensions
    {
        public static string F(this string format, params object[] values)
        {
            return string.Format(format, values);
        }
    }
}