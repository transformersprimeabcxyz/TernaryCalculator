using System;
using System.Runtime.InteropServices;
using System.Text;

namespace TernaryCalculator.Framework
{
	public unsafe struct UnbalancedTryte
    {
        public static readonly UnbalancedTryte MinValue = 0;
        public static readonly UnbalancedTryte MaxValue = 177146; // 3^11 - 1
        public static readonly UnbalancedTryte Zero = 0;
        public const int TryteSize = 11;
        private const int Limiter = 3;
        private static readonly int* _tritMultipliers;

		public readonly UnbalancedTrit[] Trits;
        // public const UnbalancedTrit SignValue = UnbalancedTrit.Double;
        
		
		static UnbalancedTryte()
		{
			_tritMultipliers = (int*)Marshal.AllocHGlobal(TryteSize * sizeof(int)).ToPointer();
			
			for (int i = 0; i < TryteSize; i++)
			{
				_tritMultipliers[i] = (int)Math.Pow(Limiter, i);
			}
		}
		
		
		private UnbalancedTryte(UnbalancedTrit[] trits)
		{
			Trits = trits;
		}

        public static UnbalancedTryte Parse(string input)
        {
            if (input.Length > TryteSize)
                throw new OverflowException();
            if (input.Length < TryteSize)
                input = new string('0', TryteSize - input.Length) + input;

            var trits = new UnbalancedTrit[TryteSize];
            for (int i = 0; i < input.Length; i++)
            {
                switch (input[i])
                {
                    case '0': trits[TryteSize - 1 - i] = UnbalancedTrit.False; break;
                    case '1': trits[TryteSize - 1 - i] = UnbalancedTrit.True; break;
                    case '2': trits[TryteSize - 1 - i] = UnbalancedTrit.Double; break;
                    default:
                        throw new FormatException();
                }
            }
            return new UnbalancedTryte(trits);
        }

        public static bool TryParse(string input, out UnbalancedTryte output)
        {
            try
            {
                output = Parse(input);
                return true;
            }
            catch
            {
                output = 0;
                return false;
            }
        }
		
		public static implicit operator UnbalancedTryte(int value)
		{
			return new UnbalancedTryte(ToTrits(value));
		}
		
		public static UnbalancedTrit[] ToTrits(int value)
		{
			unchecked
			{
                // bool signNeeded = value < 0;
                
                // if (signNeeded)
                //     value = ~value + 1;

				var trits = new UnbalancedTrit[TryteSize];
			
				for (int i = 0; i < TryteSize /* - 1 */; i++)
				{
					int whole = (int)(value / Limiter);
					int remainder = value % Limiter;
					
					trits[i] = (UnbalancedTrit)remainder;
					
					value = whole;
				}

                // if (signNeeded)
                //     trits[TryteSize - 1] = SignValue;

				return trits;
			}
		}
		
		public int ToInt32()
		{
			unchecked
			{
				int result = 0;
				
				for (int i = 0; i < TryteSize; i++)
                {
                    // if (i == TryteSize - 1 && Trits[i] == SignValue)
                    //     result = ~result + 1;
                    // else
					result += (int)Trits[i] * _tritMultipliers[i];
				}

				return result;
			}
		}
		
		public override string ToString()
		{
			var builder = new StringBuilder();
			for (int i = TryteSize - 1; i >=0 ; i--)
			{
				switch(Trits[i])
				{
					case UnbalancedTrit.False: builder.Append('0'); break;
					case UnbalancedTrit.True: builder.Append('1'); break;
					case UnbalancedTrit.Double: builder.Append('2'); break;
						
				}
			}
			
			return builder.ToString();
		}
	}
}
