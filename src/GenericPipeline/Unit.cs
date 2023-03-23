namespace GenericPipeline;

/// <summary>
/// Represents a unit value, which is a value that carries no information.
/// </summary>
public struct Unit : IEquatable<Unit>
{
    /// <summary>
    /// Gets a singleton instance of the <see cref="Unit"/> struct.
    /// </summary>
    public static readonly Unit Value = new();

    /// TODO
    public static readonly Task<Unit> ValueTask = Task.FromResult(Value);

    /// TODO
    public bool Equals(Unit other) => true;
}

