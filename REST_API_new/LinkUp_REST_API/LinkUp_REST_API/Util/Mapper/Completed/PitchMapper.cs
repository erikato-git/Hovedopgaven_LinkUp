using LinkUp_REST_API.DTOs.Requests.Completed;
using LinkUp_REST_API.Models;

namespace LinkUp_REST_API.Util.Mapper.Completed
{
    public class PitchMapper
    {
        public static Pitch MapToPitch(PitchCreateInput pitchCreateInput)
        {
            return new Pitch
            {
                PitchId = Guid.NewGuid(), // Generates a new Guid for the Pitch
                SendingDate = pitchCreateInput.SendingDate,
                TextMessage = pitchCreateInput.TextMessage,
                RecipientProfileId = pitchCreateInput.RecipientProfileId,
                ProfileId = pitchCreateInput.SenderProfileId // Assuming ProfileId is the SenderProfileId
            };
        }


    }
}
