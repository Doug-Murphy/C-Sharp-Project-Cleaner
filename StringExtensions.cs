using System;

namespace CsprojCleaner
{
    public static class StringExtensions
    {
        public static bool EndsWithIgnoreCase(this string str, string endsWithString)
        {
            return str.EndsWith(endsWithString, StringComparison.OrdinalIgnoreCase);
        }
        
        public static bool EqualsIgnoreCase(this string str, string endsWithString)
        {
            return str.Equals(endsWithString, StringComparison.OrdinalIgnoreCase);
        }
        
        public static bool ContainsIgnoreCase(this string str, string endsWithString)
        {
            return str.Contains(endsWithString, StringComparison.OrdinalIgnoreCase);
        }
    }
}