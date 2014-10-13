using SantaHo.Domain.Configuration;

namespace SantaHo.Application.IncomingLetters
{
    [SettingsKey("IncomingLetterProcessingSettings.Working")]
    public class IncomingLetterProcessingSettings
    {
        public int MaxAwaiting { get; set; }

        public int ParallelDegree { get; set; }

        public static IncomingLetterProcessingSettings Default
        {
            get
            {
                return new IncomingLetterProcessingSettings
                {
                    MaxAwaiting = 1000,
                    ParallelDegree = 2
                };
            }
        }
    }
}