using AutoFixture;
using FluentAssertions;
using PrepTime.Common.Models;
using Xunit;

namespace PrepTime.Common.Tests.Models
{
    public class DishTests
    {
        private readonly static Fixture AutoFixture = new();

        [Fact]
        public void When_constructor_called_then_properties_are_set()
        {
            var id = AutoFixture.Create<int>();
            var name = AutoFixture.Create<string>();

            var sut = new Dish(id, name);

            sut.ID.Should().Be(id);
            sut.Name.Should().Be(name);
        }

        [Fact]
        public void When_tostring_called_then_formatted_string_is_returned()
        {
            var id = AutoFixture.Create<int>();
            var name = AutoFixture.Create<string>();

            var sut = new Dish(id, name);

            var result = sut.ToString();

            result.Should().Be(name);
        }
    }
}
