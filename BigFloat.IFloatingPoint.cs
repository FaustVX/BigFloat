using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace System.Numerics;

file static class Constants
{
    public static readonly BigInteger TenPower40 = BigInteger.Pow(10, 40);
    public static readonly BigFloat E = new(BigInteger.Parse("27182818284590452353602874713526624977572"), TenPower40);
    public static readonly BigFloat Pi = new(BigInteger.Parse("31415926535897932384626433832795028841971"), TenPower40);
    public static readonly BigFloat Tau = new(BigInteger.Parse("62831853071795864769252867665590057683943"), TenPower40);
}

partial struct BigFloat : IFloatingPoint<BigFloat>
{
    public static BigFloat E => Constants.E;

    public static BigFloat Pi => Constants.Pi;

    public static BigFloat Tau => Constants.Tau;

    public static BigFloat NegativeOne => MinusOne;

    public static int Radix => 2;

    public static BigFloat AdditiveIdentity => Zero;

    public static BigFloat MultiplicativeIdentity => One;

    public static bool IsCanonical(BigFloat value) => value.Denominator == 1 || value.Denominator == -1;

    public static bool IsComplexNumber(BigFloat value) => false;

    public static bool IsEvenInteger(BigFloat value)
    {
        if (value.Denominator == 1 || value.Denominator == -1)
            return BigInteger.IsEvenInteger(value.Numerator);
        return false;
    }

    public static bool IsFinite(BigFloat value) => true;

    public static bool IsImaginaryNumber(BigFloat value) => false;

    public static bool IsInfinity(BigFloat value) => false;

    public static bool IsInteger(BigFloat value) => value.Denominator == 1 || value.Denominator == -1;

    public static bool IsNaN(BigFloat value) => false;

    public static bool IsNegative(BigFloat value) => value.Numerator < 0 ^ value.Denominator < 0;

    public static bool IsNegativeInfinity(BigFloat value) => false;

    public static bool IsNormal(BigFloat value) => value.Numerator != 0;

    public static bool IsOddInteger(BigFloat value)
    {
        if (value.Denominator == 1 || value.Denominator == -1)
            return BigInteger.IsOddInteger(value.Numerator);
        return false;
    }

    public static bool IsPositive(BigFloat value) => value.Numerator > 0 ^ value.Denominator > 0;

    public static bool IsPositiveInfinity(BigFloat value) => false;

    public static bool IsRealNumber(BigFloat value) => true;

    public static bool IsSubnormal(BigFloat value) => false;

    public static bool IsZero(BigFloat value) => value.Numerator == 0;

    public static BigFloat MaxMagnitude(BigFloat x, BigFloat y) => x > y ? x : y;

    public static BigFloat MaxMagnitudeNumber(BigFloat x, BigFloat y) => MaxMagnitude(x, y);

    public static BigFloat MinMagnitude(BigFloat x, BigFloat y) => x < y ? x : y;

    public static BigFloat MinMagnitudeNumber(BigFloat x, BigFloat y) => MinMagnitude(x, y);

    public static BigFloat Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider)
    {
        throw new NotImplementedException();
    }

    public static BigFloat Parse(string s, NumberStyles style, IFormatProvider provider)
    {
        throw new NotImplementedException();
    }

    public static BigFloat Parse(ReadOnlySpan<char> s, IFormatProvider provider)
    {
        throw new NotImplementedException();
    }

    public static BigFloat Parse(string s, IFormatProvider provider)
    {
        throw new NotImplementedException();
    }

    public static BigFloat Round(BigFloat x, int digits, MidpointRounding mode)
    {
        throw new NotImplementedException();
    }

    public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider, [MaybeNullWhen(false)] out BigFloat result)
    {
        throw new NotImplementedException();
    }

    public static bool TryParse([NotNullWhen(true)] string s, NumberStyles style, IFormatProvider provider, [MaybeNullWhen(false)] out BigFloat result)
    {
        throw new NotImplementedException();
    }

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider provider, [MaybeNullWhen(false)] out BigFloat result)
    {
        throw new NotImplementedException();
    }

    public static bool TryParse([NotNullWhen(true)] string s, IFormatProvider provider, [MaybeNullWhen(false)] out BigFloat result)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<BigFloat>.TryConvertFromChecked<TOther>(TOther value, out BigFloat result)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<BigFloat>.TryConvertFromSaturating<TOther>(TOther value, out BigFloat result)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<BigFloat>.TryConvertFromTruncating<TOther>(TOther value, out BigFloat result)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<BigFloat>.TryConvertToChecked<TOther>(BigFloat value, out TOther result)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<BigFloat>.TryConvertToSaturating<TOther>(BigFloat value, out TOther result)
    {
        throw new NotImplementedException();
    }

    static bool INumberBase<BigFloat>.TryConvertToTruncating<TOther>(BigFloat value, out TOther result)
    {
        throw new NotImplementedException();
    }

    public int GetExponentByteCount()
    {
        // Not significant for BigFloat
        throw new NotImplementedException();
    }

    public int GetExponentShortestBitLength()
    {
        // Not significant for BigFloat
        throw new NotImplementedException();
    }

    public int GetSignificandBitLength()
    {
        // Not significant for BigFloat
        throw new NotImplementedException();
    }

    public int GetSignificandByteCount()
    {
        // Not significant for BigFloat
        throw new NotImplementedException();
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        throw new NotImplementedException();
    }

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider provider)
    {
        throw new NotImplementedException();
    }

    public bool TryWriteExponentBigEndian(Span<byte> destination, out int bytesWritten)
    {
        // Not significant for BigFloat
        throw new NotImplementedException();
    }

    public bool TryWriteExponentLittleEndian(Span<byte> destination, out int bytesWritten)
    {
        // Not significant for BigFloat
        throw new NotImplementedException();
    }

    public bool TryWriteSignificandBigEndian(Span<byte> destination, out int bytesWritten)
    {
        // Not significant for BigFloat
        throw new NotImplementedException();
    }

    public bool TryWriteSignificandLittleEndian(Span<byte> destination, out int bytesWritten)
    {
        // Not significant for BigFloat
        throw new NotImplementedException();
    }
}
