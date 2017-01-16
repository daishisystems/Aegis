namespace Aegis.Pumps.NewRelicInsightsEvents
{
    public interface IAegisStatusEvent
    {
        long? BlackListDownloadTime { get; set; }
        int? BlackListConsecutiveDownloadError { get; set; }
        int? BlackListItemsCount { get; set; }
        int? BlackListLastSucessfulCheck { get; set; }
        string BlackListTimeStamp { get; set; }
        int? AegisEventsCacheCount { get; set; }
        int? AegisEventsCacheLastSucessfulSent { get; set; }
        long? AegisEventsCacheLastSentTime { get; set; }
        string SettingsOnlineTimeStamp { get; set; }
    }
}