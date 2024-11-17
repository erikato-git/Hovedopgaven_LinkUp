using REST_API.DTOs;
using REST_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_API_TESTS.Helpers
{
    public class PitchTestHelper
    {
        public static SendPitchDTO GenerateValidSendPitchDTO()
        {
            return new SendPitchDTO()
            {
                ProfileId = Guid.NewGuid(),
                ProfilePicture = "https://example.com/images/profile.jpg",
                Name = "Alice Smith",
                Title = "Guitarist",
                TextMessage = "Looking forward to collaborating on your next project!",
                SendingDate = DateTime.UtcNow
            };
        }


        public static Pitch GenerateValidPitch()
        {
            return new Pitch
            {
                PitchId = Guid.NewGuid(),
                SendingDate = DateTime.UtcNow,
                TextMessage = "I am excited about the opportunity to collaborate with you!",
                RecipientProfileId = Guid.NewGuid(),
                RecipientAccountId = Guid.NewGuid(),
                ProfileId = Guid.NewGuid(),
                Profile = new Profile
                {
                    ProfileId = Guid.NewGuid(),
                    Profession = "Musician",
                    Title = "Guitarist",
                    AlternativeTitle = "Music Teacher",
                    ProfilePicture = "https://example.com/profile-picture.jpg",
                    ProfileDescription = "Experienced guitarist with a passion for teaching and performing.",
                }
            };
        }

    }
}
