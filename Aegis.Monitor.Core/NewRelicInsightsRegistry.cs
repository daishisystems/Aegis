using FluentScheduler;

namespace Aegis.Monitor.Core
{
    /// <summary>
    ///     <see cref="NewRelicInsightsRegistry" /> is a task-manager that controls the
    ///     execution of scheduled <see cref="NewRelicInsightsTask" /> commands.
    /// </summary>
    public class NewRelicInsightsRegistry : Registry
    {
        public NewRelicInsightsRegistry()
        {
            string newRelicPublishFrequencySeconds;

            var newRelicPublishFrequencySecondsIsAvailable =
                AegisHelper.TryParseAppSetting(
                    "AegisNewRelicPublishFrequencySeconds",
                    out newRelicPublishFrequencySeconds);

            if (newRelicPublishFrequencySecondsIsAvailable)
            {
                int interval;
                var canParse = int.TryParse(newRelicPublishFrequencySeconds,
                    out interval);

                if (canParse)
                {
                    Schedule<NewRelicInsightsTask>()
                        .ToRunNow()
                        .AndEvery(interval)
                        .Seconds();
                }
                else
                {
                    Schedule<NewRelicInsightsTask>()
                        .ToRunNow()
                        .AndEvery(5)
                        .Seconds();
                }
            }
            else
            {
                Schedule<NewRelicInsightsTask>()
                    .ToRunNow()
                    .AndEvery(5)
                    .Seconds();
            }
        }
    }
}