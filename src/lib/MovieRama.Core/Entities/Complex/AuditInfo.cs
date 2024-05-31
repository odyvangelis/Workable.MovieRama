namespace MovieRama.Entities.Complex;

using System;

/// <summary>
/// 
/// </summary>
public class AuditInfo
{
    /// <summary>
    ///
    /// </summary>
    public DateTime CreatedUtc { get; set; }

    /// <summary>
    ///
    /// </summary>
    public DateTime? UpdatedUtc { get; set; }

    /// <summary>
    ///
    /// </summary>
    public AuditInfo()
    {
        CreatedUtc = DateTime.UtcNow;
    }

    /// <summary>
    /// 
    /// </summary>
    public void Update()
    {
        UpdatedUtc = DateTime.UtcNow;
    }
}