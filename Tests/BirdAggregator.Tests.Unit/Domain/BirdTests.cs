using Xunit;
using BirdAggregator.Domain.Birds;
using System.Collections.Generic;

namespace BirdAggregator.Tests.Unit.Domain
{
    public class BirdTests
    {
        [Theory]
        [MemberData(nameof(CorrectApiNames))]
        public void Bird_CanCreate(string id, string sourceName, string englishName, string latinName)
        {
            // Arrange and Act
            var bird = new Bird(id, sourceName);

            // Assert
            Assert.Equal(bird.EnglishName, englishName);
            Assert.Equal(bird.LatinName, latinName);
        }

        public static IEnumerable<object[]> CorrectApiNames =>
            new List<object[]>
            {
                new object[] {"1", "Ruddy Shelduck (Tadorna ferruginea)", "Ruddy Shelduck", "Tadorna ferruginea"},
                new object[]
                {
                    "33", "White-Backed Woodpecker (Dendrocopos leucotos)", "White-Backed Woodpecker",
                    "Dendrocopos leucotos"
                },
                new object[] {"11", "Pintail (Anas acuta)", "Pintail", "Anas acuta"},
                new object[] {"253235235", "Fieldfare (Turdus pilaris)", "Fieldfare", "Turdus pilaris"},
            };
    }
}
