using SantaHo.Core.Configuration;

namespace SantaHo.SantaOffice.Service.IncomingLetters
{
    [SettingsKey("IncomingLetterSettings")]
    public class IncomingLetterSettings
    {
        public int MaxAwaiting { get; set; }

        public int ParallelDegree { get; set; }

        public static IncomingLetterSettings Default
        {
            get
            {
                return new IncomingLetterSettings
                {
                    MaxAwaiting = 1000,
                    ParallelDegree = 2
                };
            }
        }
    }
}