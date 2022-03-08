using Autofac.Extras.NSubstitute;
using AutoFixture;
using FluentAssertions;
using NSubstitute;
using PrepTime.Common.Models;
using PrepTime.Common.Models.Factories;
using PrepTime.Common.Util;
using Xunit;

namespace PrepTime.Common.Tests.Models.Factories
{
    public class DishFactoryTests
    {
        private readonly static Fixture AutoFixture = new();

        [Fact]
        public void When_create_with_id_and_name_called_then_dish_returned_has_expected_properties()
        {
            using var autoSub = new AutoSubstitute();

            var id = AutoFixture.Create<int>();
            var name = AutoFixture.Create<string>();

            var sut = autoSub.Resolve<DishFactory>();

            var result = sut.Create(id, name);

            result.ID.Should().Be(id);
            result.Name.Should().Be(name);
            result.Tasks.Should().BeEmpty();
        }

        [Fact]
        public void When_create_with_name_called_then_dish_returned_has_expected_properties()
        {
            using var autoSub = new AutoSubstitute();

            var id = AutoFixture.Create<int>();
            var name = AutoFixture.Create<string>();

            var numberGenerator = autoSub.Resolve<ISequentialNumberGenerator>();
            numberGenerator.Next(typeof(Dish)).Returns(id);

            var sut = autoSub.Resolve<DishFactory>();

            var result = sut.Create(id, name);

            result.ID.Should().Be(id);
            result.Name.Should().Be(name);
            result.Tasks.Should().BeEmpty();
        }
    }
}
