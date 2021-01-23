using Flux;

namespace BeauTambour
{
    [EnumAddress]
    public enum ExtraEvents
    {
        OnLoadStart,
        OnSceneTransitionStart,
        OnSceneTransitionReady,
        OnLoadEnd,
        
        OnIntroBitSkipped,
        OnIntroAllTextShown,
        OnIntroStoryEnd,
        
        OnRegularDownload,
        OnRegularDownloadEnd,
        
        OnDownloadOnlyConfirmed,
        OnDownloadOnly,
        
        OnIntroSkipped,
    }
}