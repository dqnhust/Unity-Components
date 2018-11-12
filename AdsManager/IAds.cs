namespace VnzITComponent
{
    public interface IAds
    {
        void ShowAds();
        void ShowReward(System.Action callBackReward, System.Action callBackFail);
        bool AdsReady
        {
            get;
        }
        bool RewardReady
        {
            get;
        }
    }
}