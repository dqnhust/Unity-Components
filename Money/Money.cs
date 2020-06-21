#pragma warning disable 0649
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

    public override string ToString()
    {
        if (value == 0) return "0";
        int mag = (int)(System.Math.Floor(System.Math.Log10(value)) / 3);
        double divisor = System.Math.Pow(10, mag * 3);

        double shortNumber = value / divisor;

        string suffix = "";
        switch (mag)
        {
            case 0:
                suffix = string.Empty;
                break;
            case 1:
                suffix = "K";
                break;
            case 2:
                suffix = "M";
                break;
            case 3:
                suffix = "B";
                break;
            default:
                suffix = "XXXX";
                break;
        }
        return shortNumber.ToString("N1") + suffix;
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