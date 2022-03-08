using Autofac.Extras.NSubstitute;
using AutoFixture;
using FluentAssertions;
using PrepTime.Common.Util;
using Xunit;

namespace PrepTime.Common.Tests.Util
{
    public class SequentialNumberGeneratorTests
    {
        private readonly static Fixture AutoFixture = new();

        [Fact]
        public void When_next_called_once_then_return_default()
        {
            using var autoSub = new AutoSubstitute();

            var key = AutoFixture.Create<object>();

            var sut = autoSub.Resolve<SequentialNumberGenerator>();

            var result = sut.Next(key);

            result.Should().Be(0);
        }

        [Fact]
        public void When_next_called_more_than_once_with_same_key_then_return_sequential_numbers()
        {
            using var autoSub = new AutoSubstitute();

            var key = AutoFixture.Create<object>();

            var sut = autoSub.Resolve<SequentialNumberGenerator>();

            var result = sut.Next(key);

            result.Should().Be(0);

            result = sut.Next(key);

            result.Should().Be(1);
        }

        [Fact]
        public void When_next_called_more_than_once_with_different_keys_then_return_sequential_numbers_for_each_key()
        {
            using var autoSub = new AutoSubstitute();

            var key1 = AutoFixture.Create<object>();
            var key2 = AutoFixture.Create<object>();

            var sut = autoSub.Resolve<SequentialNumberGenerator>();

            var result = sut.Next(key1);
            result.Should().Be(0);

            result = sut.Next(key1);
            result.Should().Be(1);

            result = sut.Next(key2);
            result.Should().Be(0);

            result = sut.Next(key2);
            result.Should().Be(1);
        }

        [Fact]
        public void When_setnextvalue_called_then_next_returns_value()
        {
            using var autoSub = new AutoSubstitute();

            var key = AutoFixture.Create<object>();
            var value = AutoFixture.Create<int>();

            var sut = autoSub.Resolve<SequentialNumberGenerator>();

            sut.SetNextValue(key, value);

            var result = sut.Next(key);

            result.Should().Be(value);
        }

        [Fact]
        public void When_setnextvalue_called_for_different_keys_then_next_returns_different_values_for_each_key()
        {
            using var autoSub = new AutoSubstitute();

            var key1 = AutoFixture.Create<object>();
            var key2 = AutoFixture.Create<object>();
            var value1 = AutoFixture.Create<int>();
            var value2 = AutoFixture.Create<int>();

            var sut = autoSub.Resolve<SequentialNumberGenerator>();

            sut.SetNextValue(key1, value1);
            var result = sut.Next(key1);
            result.Should().Be(value1);

            sut.SetNextValue(key2, value2);
            result = sut.Next(key2);
            result.Should().Be(value2);
        }
    }
}
