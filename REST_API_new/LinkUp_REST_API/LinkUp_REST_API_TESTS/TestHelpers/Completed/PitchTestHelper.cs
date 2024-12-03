using LinkUp_REST_API.DTOs.Requests.Completed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkUp_REST_API_TESTS.TestHelpers.Completed
{
    public class PitchTestHelper
    {
        public static PitchCreateInput GenerateValidPitchCreateInput()
        {
            return new PitchCreateInput
            {
                SendingDate = DateTime.Now,
                TextMessage = "This is a sample pitch message.",
                RecipientProfileId = ProfileTestHelper.GetValidProfileId2(),
                SenderProfileId = ProfileTestHelper.GetValidProfileId1()
            };
        }


        public static Guid GetValidPitchId1()
        {
            return Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");
        }

        public static Guid GetValidPitchId2()
        {
            return Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd");
        }

        public static Guid GetPitchIdWithNoAssociated()
        {
            return Guid.NewGuid();
        }

    }
}
