
public class AudioSettingData : BaseData
{
    public float BgmVolume;
    public float SfxVolume;


    public AudioSettingData(float bgmVolume, float sfxVolume)
    {
        BgmVolume = bgmVolume;
        SfxVolume = sfxVolume;
    }
}
