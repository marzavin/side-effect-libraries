using System.ComponentModel.DataAnnotations;

namespace SideEffect.Files.XML.TCX.Enums;

/// <summary>
/// Heart rate type enum.
/// </summary>
public enum HeartRateType
{
    /// <summary>
    /// Percent of Max type.
    /// </summary>
    [Display(Name = "Percent Max")]
    PercentMax,

    /// <summary>
    /// Beats per Minute type.
    /// </summary>
    [Display(Name = "Beats Per Minute")]
    BeatsPerMinute
}
