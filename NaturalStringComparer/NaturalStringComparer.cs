
using System;

namespace Gihan.Helpers
{
    class NaturalStringComparer
    {
        public static int StrCmpLogicalW(string lpszStr, string lpszComp)
        {
            if (lpszStr != null && lpszComp != null)
            {
                int Ptr_lpszStr = 0;
                int Ptr_lpszComp = 0;

                while (Ptr_lpszStr < lpszStr.Length)
                {
                    if (Ptr_lpszComp == lpszComp.Length)
                        return 1;
                    else if (char.IsDigit(lpszStr[Ptr_lpszStr]))
                    {
                        int iStr = 0, iComp = 0;
                        if (char.IsDigit(lpszComp[Ptr_lpszComp]))
                            return -1;

                        StrToIntExW(lpszStr, Ptr_lpszStr, ref iStr);
                        StrToIntExW(lpszComp, Ptr_lpszComp, ref iComp);

                        if (iStr < iComp)
                            return -1;
                        else if (iStr > iComp)
                            return 1;

                        while (char.IsDigit(lpszStr[Ptr_lpszStr]))
                            Ptr_lpszStr++;
                        while (char.IsDigit(lpszComp[Ptr_lpszComp]))
                            Ptr_lpszComp++;

                    }
                    else if (char.IsDigit(lpszComp[Ptr_lpszComp]))
                        return 1;
                    else
                    {
                        if (lpszStr[Ptr_lpszStr] > lpszComp[Ptr_lpszComp])
                            return 1;
                        else if (lpszStr[Ptr_lpszStr] < lpszComp[Ptr_lpszComp])
                            return -1;

                        Ptr_lpszStr++;
                        Ptr_lpszComp++;
                    }
                }
                if (Ptr_lpszComp < lpszComp.Length)
                    return -1;
            }
            return 0;
        }

        private static void StrToIntExW(string lpszStr, int ptr_lpszStr, ref int iStr)
        {
            var Str = new string(lpszStr.ToCharArray());
            Str = Str.Substring(ptr_lpszStr);
            var DigitsEnd = 0;
            while (char.IsDigit(Str[DigitsEnd]))
                DigitsEnd++;
            Str = Str.Substring(0, DigitsEnd);
            iStr = int.Parse(Str);
        }
    }
}
