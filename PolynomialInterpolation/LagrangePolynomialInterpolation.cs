[System.Serializable]
public class LagrangePolynomialInterpolation
{
    [System.Serializable]
    public struct TableData
    {
        public double x;
        public double y;
    }
    public TableData[] table;

    public double GetResult(double x)
    {
        foreach (var item in table)
        {
            if (x == item.x)
                return item.y;
        }
        double y = 0;
        int n = table.Length;
        for (int i = 0; i < n; i++)
        {
            double pi = 1;
            for (int j = 0; j < n; j++)
            {
                if (j == i)
                    continue;
                pi *= x - table[j].x;
            }
            double xi = table[i].x;
            for (int j = 0; j < n; j++)
            {
                if (j == i)
                    continue;
                pi /= xi - table[j].x;
            }
            y += table[i].y * pi;
        }
        return y;
    }
}