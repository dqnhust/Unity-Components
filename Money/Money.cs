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
        return "";
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