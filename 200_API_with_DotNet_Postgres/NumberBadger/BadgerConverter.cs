using SimpleBase;

namespace NumberBadger;

/// <summary>
/// Represents a badger table definition
/// </summary>
public static class Badger
{
    /// <summary>
    /// Encodes a GUID into a badge with an optional prefix
    /// </summary>
    /// <param name="value">The guid to encode</param>
    /// <param name="prefix">A prefix to prepend to the badge</param>
    /// <returns>The encoded badger string</returns>
    public static string CreateBadge(Guid value, string? prefix)
    {
        var encodedString = Base58.Ripple.Encode(value.ToByteArray());
        return $"{prefix}{encodedString}";
    }

    /// <summary>
    /// Encodes an Int64 into a badge with an optional prefix
    /// </summary>
    /// <param name="value">The guid to encode</param>
    /// <param name="prefix">A prefix to prepend to the badge</param>
    /// <returns>The encoded badger string</returns>
    public static string CreateBadge(Int64 value, string? prefix)
    {
        var bytes = BitConverter.GetBytes(value);
        var encodedString = Base58.Ripple.Encode(bytes);
        return $"{prefix}{encodedString}";
    }

    /// <summary>
    /// Attempt to decode a badger string into an Int64
    /// </summary>
    /// <param name="badge">The badger string to attempt to decode</param>
    /// <param name="expectedPrefix">The prefix of this table; if null, the raw string will be decoded</param>
    /// <returns>The Int64 represented by the badge</returns>
    public static BadgerResult<Int64> ParseInt64(string badge, string? expectedPrefix)
    {
        var bytesResult = ExtractBytes(badge, expectedPrefix, 8);
        if (!bytesResult.Success || bytesResult.Value == null)
        {
            return new BadgerResult<Int64>()
            {
                Message = bytesResult.Message,
                Success = false
            };
        }

        return new BadgerResult<Int64>()
        {
            Success = true,
            Value = BitConverter.ToInt64(bytesResult.Value)
        };
    }

    /// <summary>
    /// Attempt to decode a badger string into a GUID
    /// </summary>
    /// <param name="badge">The badger string to attempt to decode</param>
    /// <param name="expectedPrefix">The prefix of this table; if null, the raw string will be decoded</param>
    /// <returns>The GUID represented by the badge</returns>
    public static BadgerResult<Guid> ParseGuid(string badge, string? expectedPrefix)
    {
        var bytesResult = ExtractBytes(badge, expectedPrefix, 16);
        if (!bytesResult.Success || bytesResult.Value == null)
        {
            return new BadgerResult<Guid>()
            {
                Message = bytesResult.Message,
                Success = false
            };
        }

        return new BadgerResult<Guid>()
        {
            Success = true,
            Value = new Guid(bytesResult.Value)
        };
    }

    /// <summary>
    /// Attempt to convert an encoded badge into a byte array 
    /// </summary>
    /// <param name="badge">The original input string</param>
    /// <param name="expectedPrefix">The expected table prefix</param>
    /// <param name="numExpectedBytes">The number of bytes expected</param>
    /// <returns></returns>
    private static BadgerResult<byte[]> ExtractBytes(string badge, string? expectedPrefix, int numExpectedBytes)
    {
        var codeString = badge;

        // If we expect a prefix, make sure it exists
        if (expectedPrefix != null)
        {
            if (!badge.StartsWith(expectedPrefix))
            {
                {
                    return new BadgerResult<byte[]>()
                    {
                        Message = $"The ID '{badge}' does not begin with the correct prefix '{expectedPrefix}'.",
                        Success = false
                    };
                }
            }

            // Remove the prefix from the badge
            codeString = badge[expectedPrefix.Length..];
        }

        // Convert the code portion into raw bytes
        var data = new byte[16];
        var success = false;
        int numBytesWritten = 0;
        try
        {
            success = Base58.Ripple.TryDecode(codeString, data, out numBytesWritten);
        }
        // Bummer! SimpleBase throws exceptions rather than just returning false
        catch
        {
            success = false;
        }

        // Detect non success
        if (!success)
        {
            {
                return new BadgerResult<byte[]>()
                {
                    Message = $"The ID '{badge}' is not a valid identity code.",
                    Success = false
                };
            }
        }

        // Did we not get exactly the correct number of bytes?
        if (numBytesWritten != numExpectedBytes)
        {
            {
                return new BadgerResult<byte[]>()
                {
                    Message = $"The ID '{badge}' is incomplete. Did you forget a few characters?",
                    Success = false
                };
            }
        }

        // All is good
        return new BadgerResult<byte[]>()
        {
            Success = true,
            Value = data
        };
    }
}