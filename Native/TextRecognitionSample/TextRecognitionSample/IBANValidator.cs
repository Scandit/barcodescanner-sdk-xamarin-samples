using System;
namespace TextRecognitionSample
{
    public class IBANValidator
    {
        public static bool validate(string IBAN)
        {
            IBAN = IBAN.Replace(" ", "");
            var adjustedIBAN = IBAN.Substring(4) + IBAN.Substring(0, 4);
            var integerIBAN = "";
            foreach (char c in adjustedIBAN)
            {
                if (c >= 48 && c <= 57)
                {
                    integerIBAN += c - 48;
                }
                else if (c >= 65 &&  c <= 90) 
                {
                    integerIBAN += c - 55;
                }
                else 
                {
                    return false;
                }
            }
            int checksum = int.Parse(integerIBAN.Substring(0, 1));
            for (int i = 1; i < integerIBAN.Length; i++)
            {
                int v = int.Parse(integerIBAN.Substring(i, 1));
                checksum *= 10;
                checksum += v;
                checksum %= 97;
            }
            return checksum == 1;
        }
    }
}
