using System.ComponentModel.DataAnnotations;

namespace SideEffect.Files.XML.TCX.Enums;

/// <summary>
/// Course point type enum.
/// </summary>
public enum CoursePointType
{
    /// <summary>
    /// Generic type.
    /// </summary>
    Generic,

    /// <summary>
    /// Summit type.
    /// </summary>
    Summit,

    /// <summary>
    /// Valley type.
    /// </summary>
    Valley,

    /// <summary>
    /// Water type.
    /// </summary>
    Water,

    /// <summary>
    /// Food type.
    /// </summary>
    Food,

    /// <summary>
    /// Danger type.
    /// </summary>
    Danger,

    /// <summary>
    /// Left type.
    /// </summary>
    Left,

    /// <summary>
    /// Right type.
    /// </summary>
    Right,

    /// <summary>
    /// Straight type.
    /// </summary>
    Straight,

    /// <summary>
    /// First aid type.
    /// </summary>
    [Display(Name = "First Aid")]
    FirstAid,

    /// <summary>
    /// 4st category type.
    /// </summary>
    [Display(Name = "4th Category")]
    FourthCategory,

    /// <summary>
    /// 3st category type.
    /// </summary>
    [Display(Name = "3rd Category")]
    ThirdCategory,

    /// <summary>
    /// 2st category type.
    /// </summary>
    [Display(Name = "2nd Category")]
    SecondCategory,

    /// <summary>
    /// 1st category type.
    /// </summary>
    [Display(Name = "1st Category")]
    FirstCategory,

    /// <summary>
    /// Hors category type.
    /// </summary>
    [Display(Name = "Hors Category")]
    HorsCategory,

    /// <summary>
    /// Sprint type.
    /// </summary>
    Sprint
}
