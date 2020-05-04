using System;
using System.Collections.Generic;
using System.Linq;
using BirdAggregator.Domain.Birds;
using BirdAggregator.Domain.Interfaces;

namespace BirdAggregator.Domain.Photos
{
    ///<summary>
    /// Photo root aggregate domain object
    ///</summary>
    public class Photo: IAggregateRoot
    {
        public Photo(int id, int locationId, IPhotoInformation photoInformation, IEnumerable<Bird> birds, DateTime dateTaken, double ratio, string description)
        {
            Id = id;
            Location = new Location(locationId);
            PhotoInformation = photoInformation;
            DateTaken = dateTaken;
            Ratio = ratio;
            Description = description;
            Birds = birds;
        }

        public int Id { get; }
        public string Description { get; }
        public double Ratio { get; } // todo: mb do not store and calculate?
        public DateTime DateTaken { get; }
        public Location Location { get; }
        public IPhotoInformation PhotoInformation { get; }
        public IEnumerable<Bird> Birds { get; } // todo: to birdIds to handle them as separate aggregates?
        public string Caption => string.Join(", ", Birds.Select(x => x.EnglishName));
    }
}