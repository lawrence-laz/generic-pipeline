namespace GenericPipeline;

/// <summary>
/// Represents a unit value, which is a value that carries no information.
/// </summary>
public readonly struct Unit : IEquatable<Unit>
{
    /// <summary>
    /// Gets a singleton instance of the <see cref="Unit"/> struct.
    /// </summary>
    public static readonly Unit Value = new();

    /// <summary>
    /// Gets a singleton instance of <see cref="Task{TResult}"/> with a result of <see cref="Value"/>.
    /// </summary>
    public static readonly Task<Unit> ValueTask = Task.FromResult(Value);

    /// <summary>
    /// Determines whether this instance of <see cref="Unit"/> is equal to another instance of <see cref="Unit"/>.
    /// </summary>
    /// <param name="other">The other instance of <see cref="Unit"/> to compare for equality.</param>
    /// <returns><c>true</c>, since <see cref="Unit"/> is a singleton and all instances are equal.</returns>
    public bool Equals(Unit other) => true;
}

