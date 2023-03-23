namespace GenericPipeline.Tests;

public class UnitValueTests
{
    [Fact]
    public void All_Unit_values_are_equal()
    {
        // Arrange
        var unit1 = new Unit();
        var unit2 = new Unit();

        // Act
        var actual = unit1.Equals(unit2);

        // Assert
        actual.Should().BeTrue();
    }
}
