using System.Text;

namespace System.Numerics;

[Serializable]
public readonly partial struct BigFloat : IComparable, IComparable<BigFloat>, IEquatable<BigFloat>
{
    public readonly BigInteger Numerator;
    public readonly BigInteger Denominator;

    public static BigFloat One => new(BigInteger.One);
    public static BigFloat Zero => new(BigInteger.Zero);
    public static BigFloat MinusOne => new(BigInteger.MinusOne);
    public static BigFloat OneHalf => new(BigInteger.One, 2);

    public int Sign
        => (Numerator.Sign + Denominator.Sign) switch
        {
            2 or -2 => 1,
            0 => -1,
            _ => 0,
        };

    #region Constructors

    public BigFloat()
        => (Numerator, Denominator) = (BigInteger.Zero, BigInteger.One);

    private BigFloat(string value)
        => (Numerator, Denominator) = Parse(value);

    public BigFloat(BigInteger numerator, BigInteger denominator)
    {
        if (denominator == 0)
            throw new DivideByZeroException($"{nameof(denominator)} equals 0");
        (Numerator, Denominator) = (numerator, denominator);
    }

    public BigFloat(BigInteger value)
        => (Numerator, Denominator) = (value, BigInteger.One);

    public BigFloat(BigFloat value)
    {
        if (object.Equals(value, null))
            (Numerator, Denominator) = (BigInteger.Zero, BigInteger.One);
        else
            (Numerator, Denominator) = value;
    }

    public BigFloat(ulong value)
        : this(new BigInteger(value))
    { }

    public BigFloat(long value)
        : this(new BigInteger(value))
    { }

    public BigFloat(uint value)
        : this(new BigInteger(value))
    { }

    public BigFloat(int value)
        : this(new BigInteger(value))
    { }

    public BigFloat(float value)
        : this(value.ToString("N99"))
    { }

    public BigFloat(double value)
        : this(value.ToString("N99"))
    { }

    public BigFloat(decimal value)
        : this(value.ToString("N99"))
    { }

    #endregion

    #region Static Methods

    public static BigFloat Add(BigFloat value, BigFloat other)
    {
        if (object.Equals(other, null))
            throw new ArgumentNullException(nameof(other));

        var numerator = value.Numerator * other.Denominator + other.Numerator * value.Denominator;
        return new(numerator, value.Denominator * other.Denominator);
    }

    public static BigFloat Subtract(BigFloat value, BigFloat other)
    {
        if (object.Equals(other, null))
            throw new ArgumentNullException(nameof(other));

        var numerator = value.Numerator * other.Denominator - other.Numerator * value.Denominator;
        return new(numerator, value.Denominator * other.Denominator);
    }

    public static BigFloat Multiply(BigFloat value, BigFloat other)
    {
        if (object.Equals(other, null))
            throw new ArgumentNullException(nameof(other));

        return new(value.Numerator * other.Numerator, value.Denominator * other.Denominator);
    }

    public static BigFloat Divide(BigFloat value, BigFloat other)
    {
        if (BigInteger.Equals(other, null))
            throw new ArgumentNullException(nameof(other));
        if (other.Numerator == 0)
            throw new DivideByZeroException(nameof(other));

        return new(value.Numerator * other.Denominator, value.Denominator * other.Numerator);
    }

    public static BigFloat Remainder(BigFloat value, BigFloat other)
    {
        if (BigInteger.Equals(other, null))
            throw new ArgumentNullException(nameof(other));

        return value - Floor(value / other) * other;
    }

    public static BigFloat DivideRemainder(BigFloat value, BigFloat other, out BigFloat remainder)
    {
        value = Divide(value, other);

        remainder = Remainder(value, other);

        return value;
    }

    public static BigFloat Pow(BigFloat value, int exponent)
    {
        if (value.Numerator.IsZero)
        {
            // Nothing to do
            return value;
        }
        else if (exponent < 0)
        {
            var savedNumerator = value.Numerator;
            var numerator = BigInteger.Pow(value.Denominator, -exponent);
            var denominator = BigInteger.Pow(savedNumerator, -exponent);
            return new(numerator, denominator);
        }
        else
        {
            var numerator = BigInteger.Pow(value.Numerator, exponent);
            var denominator = BigInteger.Pow(value.Denominator, exponent);
            return new(numerator, denominator);
        }
    }

    public static BigFloat Abs(BigFloat value)
        => new(BigInteger.Abs(value.Numerator), value.Denominator);

    public static BigFloat Negate(BigFloat value)
        => new(BigInteger.Negate(value.Numerator), value.Denominator);

    public static BigFloat Inverse(BigFloat value)
        => new(value.Denominator, value.Numerator);

    public static BigFloat Increment(BigFloat value)
        => new(value.Numerator + value.Denominator, value.Denominator);

    public static BigFloat Decrement(BigFloat value)
        => new(value.Numerator - value.Denominator, value.Denominator);

    public static BigFloat Ceil(BigFloat value)
    {
        var numerator = value.Numerator;
        if (numerator < 0)
            numerator -= BigInteger.Remainder(numerator, value.Denominator);
        else
            numerator += value.Denominator - BigInteger.Remainder(numerator, value.Denominator);

        return Factor(new(numerator, value.Denominator));
    }

    public static BigFloat Floor(BigFloat value)
    {
        var numerator = value.Numerator;
        if (numerator < 0)
            numerator += value.Denominator - BigInteger.Remainder(numerator, value.Denominator);
        else
            numerator -= BigInteger.Remainder(numerator, value.Denominator);

        return Factor(new(numerator, value.Denominator));
    }

    public static BigFloat Round(BigFloat value)
    {
        //get remainder. Over divisor see if it is > new BigFloat(0.5)

        if (BigFloat.Decimals(value).CompareTo(OneHalf) >= 0)
            return Ceil(value);
        else
            return Floor(value);
    }

    public static BigFloat Truncate(BigFloat value)
    {
        var numerator = value.Numerator;
        numerator -= BigInteger.Remainder(numerator, value.Denominator);

        return Factor(new(numerator, value.Denominator));
    }

    public static BigFloat Decimals(BigFloat value)
        => new(BigInteger.Remainder(value.Numerator, value.Denominator), value.Denominator);

    public static BigFloat ShiftDecimalLeft(BigFloat value, int shift)
    {
        if (shift < 0)
            return ShiftDecimalRight(value, -shift);

        var numerator = value.Numerator * BigInteger.Pow(10, shift);
        return new(numerator, value.Denominator);
    }

    public static BigFloat ShiftDecimalRight(BigFloat value, int shift)
    {
        if (shift < 0)
            return ShiftDecimalLeft(value, -shift);

        var denominator = value.Denominator * BigInteger.Pow(10, shift);
        return new(value.Numerator, denominator);
    }

    public static BigFloat Sqrt(BigFloat value)
        => Divide(Math.Pow(10, BigInteger.Log10(value.Numerator) / 2), Math.Pow(10, BigInteger.Log10(value.Denominator) / 2));

    public static double Log10(BigFloat value)
        => BigInteger.Log10(value.Numerator) - BigInteger.Log10(value.Denominator);

    public static double Log(BigFloat value, double baseValue)
        => BigInteger.Log(value.Numerator, baseValue) - BigInteger.Log(value.Numerator, baseValue);

    ///<summary> factoring can be very slow. So use only when neccessary (ToString, and comparisons) </summary>
    public static BigFloat Factor(BigFloat value)
    {
        if (value.Denominator == 1)
            return value;

        //factor numerator and denominator
        var factor = BigInteger.GreatestCommonDivisor(value.Numerator, value.Denominator);

        return new(value.Numerator / factor, value.Denominator / factor);
    }
    public new static bool Equals(object left, object right)
    {
        if (left == null && right == null)
            return true;
        if (left == null || right == null)
            return false;
        if (left.GetType() != right.GetType())
            return false;
        return ((BigInteger)left).Equals((BigInteger)right);
    }

    public static string ToString(BigFloat value)
        => value.ToString();

    public static BigFloat Parse(string value)
    {
        if (value == null)
            throw new ArgumentNullException(nameof(value));

        value = value.Trim();
        var nf = Threading.Thread.CurrentThread.CurrentCulture.NumberFormat;
        value = value.Replace(nf.NumberGroupSeparator, "");
        var pos = value.IndexOf(nf.NumberDecimalSeparator);
        value = value.Replace(nf.NumberDecimalSeparator, "");

        if (pos < 0)
        {
            //no decimal point
            return Factor(BigInteger.Parse(value));
        }
        else
        {
            //decimal point (length - pos - 1)
            var numerator = BigInteger.Parse(value);
            var denominator = BigInteger.Pow(10, value.Length - pos);

            return Factor(new(numerator, denominator));
        }
    }

    public static bool TryParse(string value, out BigFloat result)
    {
        try
        {
            result = BigFloat.Parse(value);
            return true;
        }
        catch (ArgumentNullException)
        {
            result = default;
            return false;
        }
        catch (FormatException)
        {
            result = default;
            return false;
        }
    }

    public static int Compare(BigFloat left, BigFloat right)
    {
        if (object.Equals(left, null))
            throw new ArgumentNullException(nameof(left));
        if (Equals(right, null))
            throw new ArgumentNullException(nameof(right));

        return new BigFloat(left).CompareTo(right);
    }

    #endregion

    #region Instance Methods

    public override string ToString()
        //default precision = 100
        => ToString(100);

    public string ToString(int precision, bool trailingZeros = false)
    {
        var value = Factor(this);
        var nf = Threading.Thread.CurrentThread.CurrentCulture.NumberFormat;

        var result = BigInteger.DivRem(value.Numerator, value.Denominator, out var remainder);

        if (remainder == 0 && trailingZeros)
            return result + nf.NumberDecimalSeparator + "0";
        else if (remainder == 0)
            return result.ToString();

        var decimals = (value.Numerator * BigInteger.Pow(10, precision)) / value.Denominator;

        if (decimals == 0 && trailingZeros)
            return result + nf.NumberDecimalSeparator + "0";
        else if (decimals == 0)
            return result.ToString();

        var sb = new StringBuilder();

        while (precision-- > 0)
        {
            sb.Append(decimals % 10);
            decimals /= 10;
        }

        var r = result + nf.NumberDecimalSeparator + new string(sb.ToString().Reverse().ToArray());
        return trailingZeros ? r : r.TrimEnd('0');
    }

    public string ToMixString()
    {
        var value = Factor(this);

        var result = BigInteger.DivRem(value.Numerator, value.Denominator, out var remainder);

        if (remainder == 0)
            return result.ToString();
        else
            return result + ", " + remainder + "/" + value.Denominator;
    }

    public string ToRationalString()
    {
        var value = Factor(this);
        return value.Numerator + " / " + value.Denominator;
    }

    public int CompareTo(BigFloat other)
    {
        if (object.Equals(other, null))
            throw new ArgumentNullException(nameof(other));

        //Make copies
        var one = Numerator;
        var two = other.Numerator;

        //cross multiply
        one *= other.Denominator;
        two *= Denominator;

        //test
        return BigInteger.Compare(one, two);
    }

    public int CompareTo(object obj)
    {
        if (obj == null)
            throw new ArgumentNullException(nameof(obj));

        if (obj is not BigFloat bf)
            throw new ArgumentException($"{nameof(obj)} is not a {nameof(BigFloat)}");

        return CompareTo(bf);
    }

    public override bool Equals(object obj)
    {
        if (obj is not BigFloat bf)
            return false;

        return Numerator == bf.Numerator && Denominator == bf.Denominator;
    }

    public bool Equals(BigFloat other)
        => other.Numerator * Denominator == Numerator * other.Denominator;

    public override int GetHashCode()
        => (Numerator, Denominator).GetHashCode();

    private void Deconstruct(out BigInteger numerator, out BigInteger denominator)
        => (numerator, denominator) = (Numerator, Denominator);

    #endregion

    #region Operators

    public static BigFloat operator -(BigFloat value)
        => Negate(value);

    public static BigFloat operator -(BigFloat left, BigFloat right)
        => Subtract(left, right);

    public static BigFloat operator --(BigFloat value)
        => Decrement(value);

    public static BigFloat operator +(BigFloat left, BigFloat right)
        => Add(left, right);

    public static BigFloat operator +(BigFloat value)
        => Abs(value);

    public static BigFloat operator ++(BigFloat value)
        => Increment(value);

    public static BigFloat operator %(BigFloat left, BigFloat right)
        => Remainder(left, right);

    public static BigFloat operator *(BigFloat left, BigFloat right)
        => Multiply(left, right);

    public static BigFloat operator /(BigFloat left, BigFloat right)
        => Divide(left, right);

    public static BigFloat operator >>(BigFloat value, int shift)
        => ShiftDecimalRight(value, shift);

    public static BigFloat operator <<(BigFloat value, int shift)
        => ShiftDecimalLeft(value, shift);

    public static BigFloat operator ^(BigFloat left, int right)
        => Pow(left, right);

    public static BigFloat operator ~(BigFloat value)
        => Inverse(value);

    public static bool operator !=(BigFloat left, BigFloat right)
        => Compare(left, right) != 0;

    public static bool operator ==(BigFloat left, BigFloat right)
        => Compare(left, right) == 0;

    public static bool operator <(BigFloat left, BigFloat right)
        => Compare(left, right) < 0;

    public static bool operator <=(BigFloat left, BigFloat right)
        => Compare(left, right) <= 0;

    public static bool operator >(BigFloat left, BigFloat right)
        => Compare(left, right) > 0;

    public static bool operator >=(BigFloat left, BigFloat right)
        => Compare(left, right) >= 0;

    public static bool operator true(BigFloat value)
        => value != 0;

    public static bool operator false(BigFloat value)
        => value == 0;

    #endregion

    #region Casts

    public static explicit operator decimal(BigFloat value)
    {
        if (decimal.MinValue > value)
            throw new OverflowException($"{nameof(value)} is less than decimal.MinValue.");
        if (decimal.MaxValue < value)
            throw new OverflowException($"{nameof(value)} is greater than decimal.MaxValue.");

        return (decimal)value.Numerator / (decimal)value.Denominator;
    }

    public static explicit operator double(BigFloat value)
    {
        if (double.MinValue > value)
            throw new OverflowException($"{nameof(value)} is less than double.MinValue.");
        if (double.MaxValue < value)
            throw new OverflowException($"{nameof(value)} is greater than double.MaxValue.");

        return (double)value.Numerator / (double)value.Denominator;
    }

    public static explicit operator float(BigFloat value)
    {
        if (float.MinValue > value)
            throw new OverflowException($"{nameof(value)} is less than float.MinValue.");
        if (float.MaxValue < value)
            throw new OverflowException($"{nameof(value)} is greater than float.MaxValue.");

        return (float)value.Numerator / (float)value.Denominator;
    }

    public static implicit operator BigFloat(byte value)
        => new((uint)value);

    public static implicit operator BigFloat(sbyte value)
        => new(value);

    public static implicit operator BigFloat(short value)
        => new(value);

    public static implicit operator BigFloat(ushort value)
        => new((uint)value);

    public static implicit operator BigFloat(int value)
        => new(value);

    public static implicit operator BigFloat(long value)
        => new(value);

    public static implicit operator BigFloat(uint value)
        => new(value);

    public static implicit operator BigFloat(ulong value)
        => new(value);

    public static implicit operator BigFloat(decimal value)
        => new(value);

    public static implicit operator BigFloat(double value)
        => new(value);

    public static implicit operator BigFloat(float value)
        => new(value);

    public static implicit operator BigFloat(BigInteger value)
        => new(value);

    public static explicit operator BigFloat(string value)
        => new(value);

    #endregion
}
