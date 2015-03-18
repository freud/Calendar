using System;
using FluentAssertions;
using NUnit.Framework;

namespace Calendar.Logic.Tests
{
    [TestFixture]
    public class WeeklyRecurringOptionsTests
    {
        [Test]
        public void WeeklyRecurringOptions_InitializedWithDefaultConstructor_IsInitializedWithRepatAlwaysStrategy()
        {
            // Arrange
            // Act
            var options = new WeeklyRecurringOptions();

            // Assert
            options.RepeatUntil.Should().BeOfType<RepeatAlways>();
        }

        [Test]
        public void WeeklyRecurringOptions_RepeatUntilInitializedWithNull_ThrowsInvalidOperationException()
        {
            // Arrange
            // Act
            var options = new WeeklyRecurringOptions();
            Action act = () => options.RepeatUntil = null;

            // Assert
            act.ShouldThrow<InvalidOperationException>();
        }
    }
}