using System;
using System.Text;

namespace Explorer.Service.Contract.DataModel
{
    public class Name
    {
        private const string Charmap = ".12345abcdefghijklmnopqrstuvwxyz";
        private const ulong Mask = 0xF800000000000000;

        public ulong Value { get; }

        public Name(ulong value)
        {
            Value = value;
        }

        public Name(string name)
        {
            if (name.Length > 13)
            {
                throw new Exception("string is too long to be a valid name");
            }

            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            ulong value = 0;
            int n = Math.Min(name.Length, 12);
            for (int i = 0; i < n; ++i)
            {
                value <<= 5;
                value |= (ulong) CharToValue(name[i]);
            }

            value <<= (4 + 5 * (12 - n));
            if (name.Length == 13)
            {
                int v = CharToValue(name[12]);
                if (v > CharToValue('j'))
                {
                    throw new Exception("thirteenth character in name cannot be a letter that comes after j");
                }

                value |= (ulong) v;
            }

            Value = value;
        }

        public override string ToString()
        {
            var v = Value;
            StringBuilder strBuilder = new StringBuilder();
            for (var i = 0; i < 13; ++i, v <<= 5)
            {
                if (v == 0) return strBuilder.ToString();

                int indx = (int) ((v & Mask) >> (i == 12 ? 60 : 59));
                strBuilder.Append(Charmap[indx]);
            }

            return strBuilder.ToString();
        }


        private int CharToValue(char c)
        {
            if (c == '.')
                return 0;
            else if (c >= '1' && c <= '5')
                return (c - '1') + 1;
            else if (c >= 'a' && c <= 'z')
                return (c - 'a') + 6;
            else
                throw new Exception("char not support");
        }
    }
}