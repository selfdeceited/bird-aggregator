using System;
using Xunit;
using BirdAggregator.Domain.Birds;
using System.Collections.Generic;

namespace BirdAggregator.Tests.Unit.Domain
{
    public class BirdTests
    {
        [Theory]
        [MemberData(nameof(CorrectApiNames))]
        public void Bird_CanCreate(int id, string sourceName, string englishName, string latinName)
        {
            // Arrange and Act
            var bird = new Bird(id, sourceName);

            // Assert
            Assert.Equal(bird.EnglishName, englishName);
            Assert.Equal(bird.LatinName, latinName);            
        }

        [Theory]
        [MemberData(nameof(IncorrectApiNames))]
        public void Bird_CannotCreate(int id, string sourceName, string englishName, string latinName)
        {
            // Arrange and Act and Assert
            Assert.Throws<ArgumentException>(() => {
                var bird = new Bird(id, sourceName);
            });
            
        }

        public static IEnumerable<object[]> CorrectApiNames =>
        new List<object[]>
        {
            new object[] { 1, "Ruddy Shelduck (Tadorna ferruginea)", "Ruddy Shelduck", "Tadorna ferruginea"},
            new object[] { 33, "White-Backed Woodpecker (Dendrocopos leucotos)", "White-Backed Woodpecker", "Dendrocopos leucotos"},
            new object[] { 11, "Pintail (Anas acuta)", "Pintail", "Anas acuta" },
            new object[] { 253235235, "Fieldfare (Turdus pilaris)", "Fieldfare", "Turdus pilaris"},
        };

        public static IEnumerable<object[]> IncorrectApiNames =>
        new List<object[]>
        {
            new object[] { 0, "Eurasian Bullfinch (Pyrrhula pyrrhula)", "Eurasian Bullfinch", "Pyrrhula pyrrhula" },
            new object[] { -44, "European Herring Gull (Larus argentatus)", "European Herring Gull", "Larus argentatus" },
        };
    }
}
