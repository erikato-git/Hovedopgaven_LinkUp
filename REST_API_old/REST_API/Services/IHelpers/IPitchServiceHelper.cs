using REST_API.DTOs.PitchDomain;
using REST_API.Models;

namespace REST_API.Services.IHelpers
{
    public interface IPitchServiceHelper : IAuthentication
    {
        Task<bool> CheckReceiverExist(Guid RecipientAccountId);
        Pitch? SendPitchDTOToPitch(SendPitchDTO sendPitchDTO);
    }
}
