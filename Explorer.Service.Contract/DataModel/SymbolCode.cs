using System;
using System.Text;

namespace Explorer.Service.Contract.DataModel
{
    public class SymbolCode
    {
        private const ulong Mask = 0xFF;
        public ulong Value { get; set; }

        public SymbolCode(ulong value)
        {
            Value = value;
        }

        public SymbolCode(string symbol)
        {
            if (symbol.Length > 7)
            {
                throw new Exception("string is too long to be a valid symbol_code");
            }

            ulong value = 0;
            for (int i = symbol.Length - 1; i >= 0; i--)
            {
                char c = symbol[i];
                if (c < 'A' || c > 'Z')
                {
                    throw new Exception("only uppercase letters allowed in symbol_code string");
                }

                value <<= 8;
                value |= c;
            }

            Value = value;
        }

        public override string ToString()
        {
            var v = Value;
            StringBuilder strBuilder = new StringBuilder();
            for (var i = 0; i < 7; ++i, v >>= 8)
            {
                if (v == 0) return strBuilder.ToString();

                strBuilder.Append((char) (v & Mask));
            }

            return strBuilder.ToString();
        }
    }
}