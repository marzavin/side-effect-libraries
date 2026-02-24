namespace SideEffect.DataTransfer.Paging;

/// <summary>
/// Information about requested page of data.
/// </summary>
public class PagingInfo
{
    /// <summary>
    /// Gets or sets number of items to skip.
    /// </summary>
    public int Skip { get; set; }

    /// <summary>
    /// Gets or sets number of items to take.
    /// </summary>
    public int Take { get; set; }
}
