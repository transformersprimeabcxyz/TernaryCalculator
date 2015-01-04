using System;
using System.Runtime.InteropServices;
using System.Text;

namespace TernaryCalculator.Framework
{
    public unsafe struct BalancedTryte : IComparable<BalancedTryte>, IEquatable<BalancedTryte>
    {
        public static readonly BalancedTryte Zero = 0;
        public static readonly BalancedTryte MinValue = -88573;
        public static readonly BalancedTryte MaxValue = 88573;

		private static readonly int* _tritMultipliers;
				
		public const int TryteSize = 11;
        public readonly BalancedTrit[] Trits;

        static BalancedTryte()
		{
			_tritMultipliers = (int*)Marshal.AllocHGlobal(TryteSize * sizeof(int)).ToPointer();
			
			for (int i = 0; i < TryteSize; i++)
			{
				_tritMultipliers[i] = (int)Math.Pow(3, i);
			}
		}

        private BalancedTryte(BalancedTrit[] trits)
		{
			Trits = trits;
		}

        #region Input to tryte conversions

        public static BalancedTryte FromInt32(int input)
        {
            return new BalancedTryte(ToTrits(input));
        }

        public static BalancedTryte Parse(string input)
        {
            if (input.Length > TryteSize)
                throw new OverflowException();
            if (input.Length < TryteSize)
                input = new string('0', TryteSize - input.Length) + input;

            var trits = new BalancedTrit[TryteSize];
            for (int i = 0; i < input.Length; i++)
            {
                switch (input[i])
                {
                    case 't':
                    case 'T': 
                        trits[TryteSize - 1 - i] = BalancedTrit.Anti; 
                        break;
                    case '0':
                        trits[TryteSize - 1 - i] = BalancedTrit.False; 
                        break;
                    case '1': 
                        trits[TryteSize - 1 - i] = BalancedTrit.True; 
                        break;
                    default:
                        throw new FormatException();
                }
            }
            return new BalancedTryte(trits);
        }

        public static bool TryParse(string input, out BalancedTryte output)
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

        public static BalancedTrit[] ToTrits(int value)
        {
            unchecked
            {
                var trits = new BalancedTrit[TryteSize];

                for (int i = 0; i < TryteSize; i++)
                {
                    var remainder = value % 3;

                    if (remainder == 2)
                    {
                        trits[i] = BalancedTrit.Anti;
                        value++;
                    }
                    else if (remainder == -2)
                    {
                        trits[i] = BalancedTrit.True;
                        value--;
                    }
                    else
                    {
                        trits[i] = (BalancedTrit)remainder;
                    }

                    value /= 3;
                }

                if (value != 0)
                    throw new OverflowException();

                return trits;
            }
        }

        #endregion

        #region C# operators

        public static implicit operator BalancedTryte(int value)
        {
            return BalancedTryte.FromInt32(value);
        }

        public static BalancedTryte operator ++(BalancedTryte value)
        {
            return value.Add(1);
        }

        public static BalancedTryte operator --(BalancedTryte value)
        {
            return value.Add(-1);
        }

        public static BalancedTryte operator ~(BalancedTryte value)
        {
            return value.Negate();
        }

        public static BalancedTryte operator -(BalancedTryte value)
        {
            return value.Negate();
        }

        public static BalancedTryte operator +(BalancedTryte a, BalancedTryte b)
        {
            return a.Add(b);
        }

        public static BalancedTryte operator -(BalancedTryte a, BalancedTryte b)
        {
            return a.Subtract(b);
        }

        public static BalancedTryte operator *(BalancedTryte a, BalancedTryte b)
        {
            return a.Multiply(b);
        }

        public static BalancedTryte operator /(BalancedTryte a, BalancedTryte b)
        {
            BalancedTryte remainder;
            return a.Divide(b, out remainder);
        }

        public static BalancedTryte operator <<(BalancedTryte a, int b)
        {
            return a.Shift(b);
        }

        public static BalancedTryte operator >>(BalancedTryte a, int b)
        {
            return a.Shift(-b);
        }

        public static BalancedTryte operator &(BalancedTryte a, BalancedTryte b)
        {
            return a.And(b);
        }

        public static BalancedTryte operator |(BalancedTryte a, BalancedTryte b)
        {
            return a.Or(b);
        }

        public static BalancedTryte operator ^(BalancedTryte a, BalancedTryte b)
        {
            return a.Xor(b);
        }

        public static bool operator ==(BalancedTryte a, BalancedTryte b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(BalancedTryte a, BalancedTryte b)
        {
            return !(a == b);
        }

        public static bool operator >(BalancedTryte a, BalancedTryte b)
        {
            return a.CompareTo(b) == 1;
        }

        public static bool operator >=(BalancedTryte a, BalancedTryte b)
        {
            int result = a.CompareTo(b);
            return result == 0 || result == 1;
        }

        public static bool operator <(BalancedTryte a, BalancedTryte b)
        {
            return a.CompareTo(b) == -1;
        }

        public static bool operator <=(BalancedTryte a, BalancedTryte b)
        {
            int result = a.CompareTo(b);
            return result == 0 || result == -1;
        }

        #endregion

        #region Binary operators

        public BalancedTryte Shift(int b)
        {
            var newTrits = new BalancedTrit[TryteSize];
            for (int i = 0; i < TryteSize; i++)
            {
                int newIndex = i + b;
                if (newIndex >= 0 && newIndex < TryteSize)
                    newTrits[newIndex] = this.Trits[i];
            }
            return new BalancedTryte(newTrits);
        }
        
        private static readonly sbyte[,] _andTable =
        {
            {-1, -1, -1},
            {-1,  0,  0},
            {-1,  0,  1},
        };

        public BalancedTryte And(BalancedTryte value)
        {
            var newTrits = new BalancedTrit[TryteSize];
            for (int i = 0; i < TryteSize; i++)
                newTrits[i] = (BalancedTrit)_andTable[(int)this.Trits[i] + 1, (int)value.Trits[i] + 1];
            return new BalancedTryte(newTrits);
        }

        private static readonly sbyte[,] _orTable =
        {
            { 1,  0, -1},
            { 0,  0, -1},
            {-1, -1,  1},
        };

        public BalancedTryte Or(BalancedTryte value)
        {
            var newTrits = new BalancedTrit[TryteSize];
            for (int i = 0; i < TryteSize; i++)
                newTrits[i] = (BalancedTrit)_orTable[(int)this.Trits[i] + 1, (int)value.Trits[i] + 1];
            return new BalancedTryte(newTrits);
        }

        private static readonly sbyte[,] _xorTable =
        {
            {-1,  0,  1},
            { 0,  0,  0},
            { 1,  0, -1},
        };

        public BalancedTryte Xor(BalancedTryte value)
        {
            var newTrits = new BalancedTrit[TryteSize];
            for (int i = 0; i < TryteSize; i++)
                newTrits[i] = (BalancedTrit)_xorTable[(int)this.Trits[i] + 1, (int)value.Trits[i] + 1];
            return new BalancedTryte(newTrits);
        }

        private static readonly sbyte[] _negationTable =
        {
            1, 0, -1
        };

        public BalancedTryte Negate()
        {
            var invertedTrits = new BalancedTrit[TryteSize];
            for (int i = 0; i < TryteSize; i++)
                invertedTrits[i] = (BalancedTrit)_negationTable[(int)this.Trits[i] + 1];
            return new BalancedTryte(invertedTrits);
        }

        #endregion

        #region Arithmetic operators

        private static readonly sbyte[,] _additionTable =
        {
            {  1, -1,  0 },
            { -1,  0,  1 },
            {  0,  1, -1 },
        };

        private static readonly sbyte[,] _additionCarryTable =
        {
            { -1, 0, 0 },
            {  0, 0, 0 },
            {  0, 0, 1 },
        };

        public BalancedTryte Add(BalancedTryte addition)
        {
            var newTrits = new BalancedTrit[TryteSize];
            var carry = new BalancedTrit[TryteSize];
            var addCarry = false;

            for (int i = 0; i < TryteSize; i++)
            {
                newTrits[i] = (BalancedTrit)_additionTable[(int)this.Trits[i] + 1, (int)addition.Trits[i] + 1];

                if (i < TryteSize - 1)
                {
                    carry[i + 1] = (BalancedTrit)_additionCarryTable[(int)this.Trits[i] + 1, (int)addition.Trits[i] + 1];

                    if (!addCarry)
                        addCarry = carry[i + 1] != BalancedTrit.False;
                }
            }

            if (addCarry)
                return new BalancedTryte(newTrits).Add(new BalancedTryte(carry));
            return new BalancedTryte(newTrits);
        }

        public BalancedTryte Subtract(BalancedTryte subtraction)
        {
            return this.Add(subtraction.Negate());
        }

        private static readonly sbyte[,] _multiplicationTable =
        {
            {  1, 0, -1 },
            {  0, 0,  0 },
            { -1, 0,  1 },
        };

        public BalancedTryte Multiply(BalancedTryte multiplier)
        {
            BalancedTryte result = 0;

            for (int i = 0; i < TryteSize; i++)
            {
                BalancedTryte intermediate = 0;
                for (int j = 0; j < TryteSize; j++)
                {
                    intermediate.Trits[j] = (BalancedTrit)_multiplicationTable[(int)this.Trits[j] + 1, (int)multiplier.Trits[i] + 1];
                }
                intermediate <<= i;
                result += intermediate;
            }

            return result;
        }

        public BalancedTryte Divide(BalancedTryte divisor, out BalancedTryte remainder)
        {
            // Long division is not always possible in balanced ternary. 
            // Instead, we use some kind of hack and convert it to binary
            // in order to perform a correct division.

            var x = ToInt32();
            var y = divisor.ToInt32();
            remainder = x % y;
            return x / y;
        }

        #endregion

        #region Utilities

        public int ToInt32()
        {
            unchecked
            {
                int result = 0;

                for (int i = 0; i < TryteSize; i++)
                {
                    result += (int)Trits[i] * _tritMultipliers[i];
                }

                return result;
            }
        }

        public int GetNumberOfTritsUsed()
        {
            for (int i = TryteSize - 1; i >= 0; i--)
            {
                if (this.Trits[i] != 0)
                    return i + 1;
            }
            return 0;
        }

        public BalancedTryte GetAbsoluteValue()
        {
            if (this < 0)
                return this.Negate();
            return this;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            for (int i = TryteSize - 1; i >= 0; i--)
            {
                switch (Trits[i])
                {
                    case BalancedTrit.Anti: builder.Append('T'); break;
                    case BalancedTrit.False: builder.Append('0'); break;
                    case BalancedTrit.True: builder.Append('1'); break;

                }
            }

            return builder.ToString();
        }

        public BalancedTryte Filter(int start, int length)
        {
            // cheap
            var newTrits = new BalancedTrit[TryteSize];
            Array.Copy(this.Trits, start, newTrits, start, length);
            return new BalancedTryte(newTrits);
        }

        #endregion

        #region IComparable<BalancedTryte> Members

        public int CompareTo(BalancedTryte other)
        {
            for (int i = TryteSize - 1; i >= 0; i--)
            {
                var comparison = this.Trits[i].CompareTo(other.Trits[i]);
                if (comparison != 0)
                    return comparison;
            }
            return 0;
        }

        #endregion

        public bool Equals(BalancedTryte other)
        {
            for (int i = 0; i < TryteSize; i++)
            {
                if (this.Trits[i] != other.Trits[i])
                    return false;
            }
            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            return obj is BalancedTryte && Equals((BalancedTryte)obj);
        }

        public override int GetHashCode()
        {
            return (Trits != null ? Trits.GetHashCode() : 0);
        }
    }
}
