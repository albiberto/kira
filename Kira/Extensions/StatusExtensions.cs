namespace Kira.Extensions;

using Radzen;

public static class StatusExtensions
{
    public static BadgeStyle ToBadgeStyle(this int id) =>
        id switch
        {
            2 => BadgeStyle.Dark,
            3 => BadgeStyle.Success,
            4 => BadgeStyle.Info,
            _ => BadgeStyle.Dark
        };

    public static double ToDouble(this int? value) => value ?? 0;
}