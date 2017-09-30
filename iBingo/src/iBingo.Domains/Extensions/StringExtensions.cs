namespace iBingo.Domains.Extensions
{
    public static class StringExtensions
    {
        public static int? ToIntOrDefault(this string self)
        {
            return int.TryParse(self, out var result) ? result : (int?)null;
        }
    }
}