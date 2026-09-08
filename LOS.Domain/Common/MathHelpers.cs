namespace LOS.Domain.Common;

/// <summary>
/// Вспомогательные методы для финансовых расчетов.
/// Используют decimal для обеспечения точности (в отличие от System.Math, который работает с double).
/// </summary>
public static class MathHelpers
{
    /// <summary>
    /// Возводит decimal в целую неотрицательную степень.
    /// Использует алгоритм быстрого возведения в степень (O(log n)).
    /// </summary>
    public static decimal Pow(decimal baseValue, int exponent)
    {
        if (exponent < 0)
            throw new ArgumentOutOfRangeException(nameof(exponent), "Степень не может быть отрицательной");

        if (exponent == 0)
            return 1m;

        decimal result = 1m;
        decimal currentBase = baseValue;
        int currentExponent = exponent;

        while (currentExponent > 0)
        {
            // Если степень нечетная, умножаем результат на текущее основание
            if (currentExponent % 2 == 1)
                result *= currentBase;

            currentBase *= currentBase;
            currentExponent /= 2;
        }

        return result;
    }

    /// <summary>
    /// Округляет значение до указанного количества знаков после запятой (банковское округление)
    /// </summary>
    public static decimal Round(decimal value, int decimals = 2)
    {
        return Math.Round(value, decimals, MidpointRounding.AwayFromZero);
    }
}