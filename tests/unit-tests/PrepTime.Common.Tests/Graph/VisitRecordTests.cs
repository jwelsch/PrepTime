using Autofac.Extras.NSubstitute;
using FluentAssertions;
using PrepTime.Common.Graph;
using Xunit;

namespace PrepTime.Common.Tests.Graph
{
    public class VisitRecordTests
    {
        [Fact]
        public void When_ctor_called_then_properties_have_expected_defaults()
        {
            using var autoSub = new AutoSubstitute();

            var vertex = autoSub.Resolve<IVertex>();

            var sut = new VisitRecord(vertex);

            sut.Vertex.Should().Be(vertex);
            sut.IsUnmarked.Should().BeTrue();
            sut.IsTemporary.Should().BeFalse();
            sut.IsPermanent.Should().BeFalse();
        }

        [Fact]
        public void When_marktemporary_called_then_flags_should_be_set_correctly()
        {
            using var autoSub = new AutoSubstitute();

            var vertex = autoSub.Resolve<IVertex>();

            var sut = new VisitRecord(vertex);

            sut.MarkTemporary();

            sut.IsUnmarked.Should().BeFalse();
            sut.IsTemporary.Should().BeTrue();
            sut.IsPermanent.Should().BeFalse();
        }

        [Fact]
        public void When_markpermanent_called_then_flags_should_be_set_correctly()
        {
            using var autoSub = new AutoSubstitute();

            var vertex = autoSub.Resolve<IVertex>();

            var sut = new VisitRecord(vertex);

            sut.MarkPermanent();

            sut.IsUnmarked.Should().BeFalse();
            sut.IsTemporary.Should().BeFalse();
            sut.IsPermanent.Should().BeTrue();
        }

        [Fact]
        public void When_marktemporary_called_and_markpermanent_called_then_flags_should_be_set_correctly()
        {
            using var autoSub = new AutoSubstitute();

            var vertex = autoSub.Resolve<IVertex>();

            var sut = new VisitRecord(vertex);

            sut.MarkTemporary();
            sut.MarkPermanent();

            sut.IsUnmarked.Should().BeFalse();
            sut.IsTemporary.Should().BeFalse();
            sut.IsPermanent.Should().BeTrue();
        }
    }
}
