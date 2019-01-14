using System.Collections.Generic;

namespace BaseFramework
{
    // comparer string by length
    public class StringLengthComparer : IComparer<string>
    {

        public int Compare(string x, string y)
        {
            if (x == null)
                return -1;
            if (y == null)
                return 1;

            if (x.Length - y.Length == 0)
            {
                return string.Compare(x, y);
            }
            return x.Length - y.Length;
        }
    }
}
