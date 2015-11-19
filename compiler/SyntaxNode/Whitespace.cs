using System.Linq;


namespace Swish
{
    static class Whitespace
    {
        public static readonly char[] Characters = new[]
        {
            ' ', '\n'
        };

        public static bool Parse(ref string input)
        {
            var count = 0;
            foreach (var c in input)
                if (Characters.Contains(c))
                    count++;
                else
                    break;

            input = input.Substring(count);
            return count != 0;
        }
    }
}
