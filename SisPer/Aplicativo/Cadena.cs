using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Globalization;

namespace SisPer.Aplicativo
{
    public static class Cadena
    {
        public static string Normalizar(this String s)
        {
            String normalizedString = s.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < normalizedString.Length; i++)
            {
                Char c = normalizedString[i];
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    stringBuilder.Append(c);
            }

            return stringBuilder.ToString();
        }

        public static string CompletarConCeros(int tamañoFinal, int numero)
        {
            string ret = numero.ToString();

            while (ret.Length < tamañoFinal)
            {
                ret = "0" + ret;
            }

            return ret;
        }
    }
}