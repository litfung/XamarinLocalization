using System;
using System.Collections.Generic;
using System.Text;

namespace Localization
{
    class Utils
    {
        public static void ShiftRight<T>(T[] arr, int shifts)
        {
            Array.Copy(arr, 0, arr, shifts, arr.Length - shifts);
            Array.Clear(arr, 0, shifts);
        }
    }
}
