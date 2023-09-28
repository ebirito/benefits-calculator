using Xunit;
using System;
using System.Collections.Generic;
using Api.Services;
using FluentAssertions;
using System.Linq;

namespace ApiTests.UnitTests
{
    public class PaydateCalculatorUnitTests
    {
        [Fact]
        public void CalculatePaydates_WhenNumberOfPaydatesIsZero_ShouldReturnEmptyList()
        {
            // Arrange
            var firstPayDate = new DateTime(2023, 1, 13);

            // Act
            var paydates = PaydateCalculator.CalculatePaydates(firstPayDate, 0, 14);

            // Assert
            paydates.Count.Should().Be(0);
        }

        [Fact]
        public void CalculatePaydates_WhenNumberOfPaydatesIs26AndFrequencyIsBiweekly_ShouldReturn26PaydatesForTheYear()
        {
            // Arrange
            var firstPayDate = new DateTime(2023, 1, 13);

            // Act
            var paydates = PaydateCalculator.CalculatePaydates(firstPayDate, 26, 14);

            // Assert
            paydates.Count.Should().Be(26);
            paydates[0].Should().Be(firstPayDate);
            paydates[1].Should().Be(new DateTime(2023, 1, 27));
            paydates[24].Should().Be(new DateTime(2023, 12, 15));
            paydates[25].Should().Be(new DateTime(2023, 12, 29));
        }
    }
}
