#pragma warning disable 0649
using System;
using UnityEngine;

[System.Serializable]
public struct Money
{
    [SerializeField] private double value;

    public static implicit operator Money(double d)
    {
        return new Money()
        {
            value = d
        };
    }

    public static implicit operator double(Money m)
    {
        return m.value;
    }

    public override string ToString() => ToString("0.#");
    public string ToString(string format = "0.#")
    {
        if (value == 0) return "0";
        double exponent = Math.Log10(value);
        if (exponent < 3)
        {
            return value.ToString(format);
        }
        string[] abbreviation = new string[] { "K", "M", "B", "t", "q", "Q", "s", "S", "o", "n", "d", "U", "D", "T", "Qt", "Qd", "Sd", "St", "O", "N", "v", "c" };

        int mag = (int)(Math.Floor(exponent / 3));
        double divisor = Math.Pow(10, mag * 3);

        double shortNumber = value / divisor;

        if (mag >= abbreviation.Length)
        {
            return "VeryBigNumber!";
        }
        else
        {
            string suffix = abbreviation[mag - 1];
            return shortNumber.ToString(format) + suffix;
        }
    }

    public override bool Equals(object obj)
    {
        if (obj is Money m)
        {
            return m.value == value;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return value.GetHashCode();
    }
    #region Operator

    public static Money operator +(Money m1, Money m2)
    {
        return m1.value + m2.value;
    }

    public static Money operator -(Money m1, Money m2)
    {
        return m1.value - m2.value;
    }

    public static Money operator *(Money m1, Money m2)
    {
        return m1.value * m2.value;
    }

    public static Money operator /(Money m1, Money m2)
    {
        return m1.value / m2.value;
    }
    #endregion
    #region Compare Operator
    public static bool operator ==(Money m1, Money m2)
    {
        return m1.value == m2.value;
    }

    public static bool operator !=(Money m1, Money m2)
    {
        return m1.value != m2.value;
    }

    public static bool operator >(Money m1, Money m2)
    {
        return m1.value > m2.value;
    }
    public static bool operator <(Money m1, Money m2)
    {
        return m1.value < m2.value;
    }

    public static bool operator >=(Money m1, Money m2)
    {
        return m1.value >= m2.value;
    }
    public static bool operator <=(Money m1, Money m2)
    {
        return m1.value <= m2.value;
    }
    #endregion

}