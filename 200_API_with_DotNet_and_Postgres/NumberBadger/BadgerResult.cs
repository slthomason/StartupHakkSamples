namespace NumberBadger;

public class BadgerResult<T>
{
    /// <summary>
    /// True if this badger conversion operation succeeded.
    /// </summary>
    public bool Success { get; set; }
    
    /// <summary>
    /// If this badger conversion operation failed, this message explains why
    /// </summary>
    public string? Message { get; set; }
    
    /// <summary>
    /// If the badger conversion succeeded, this is the value
    /// </summary>
    public T? Value { get; set; }
}