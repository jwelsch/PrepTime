using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using System;
using System.Linq;

namespace PrepTime.Common.Tests.Testing
{
    public static class FluentAssertionExtensions
    {
        public static AndConstraint<ObjectAssertions> BeOneOf(this ObjectAssertions subject, object[] validValues, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                   .ForCondition(validValues.Contains(subject.Subject))
                   .BecauseOf(because, becauseArgs)
                   .FailWith("Expected {context:object} to be one of {0}{reason}, but found {1}.", validValues, subject.Subject);

            return new AndConstraint<ObjectAssertions>(subject);
        }

        public static AndConstraint<ObjectAssertions> BeOneOf(this ObjectAssertions subject, params object[] validValues)
        {
            return BeOneOf(subject, validValues, string.Empty, Array.Empty<object>());
        }
    }
}
