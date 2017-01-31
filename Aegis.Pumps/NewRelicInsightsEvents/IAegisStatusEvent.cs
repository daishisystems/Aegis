namespace Aegis.Pumps.NewRelicInsightsEvents
{
    public interface IAegisStatusEvent
    {
        long? BlackListDownloadTimeInSecs { get; set; }
        int? BlackListConsecutiveDownloadError { get; set; }
        int? BlackListItemsCount { get; set; }
        int? BlackListLastSucessfulCheckInSecs { get; set; }
        string BlackListTimeStamp { get; set; }
        int? AegisEventsCacheCount { get; set; }
        int? AegisEventsCacheLastSucessfulSentInSecs { get; set; }
        long? AegisEventsCacheLastSentTimeInSecs { get; set; }
        string SettingsOnlineTimeStamp { get; set; }
    }
}