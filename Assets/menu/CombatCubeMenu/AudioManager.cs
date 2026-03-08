using UnityEngine;
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip level1Music;
    [SerializeField] private AudioClip level2Music;
    [SerializeField] private AudioClip level3Music;
    [SerializeField] private AudioSource sfxSource;  

    [Header("SFX")]
    [SerializeField] private AudioClip punchSFX;
    [SerializeField] private AudioClip kickSFX;
    [SerializeField] private AudioClip enemySpawnSFX;

    public void PlayPunch()
    {
        PlaySFX(punchSFX);
    }

    public void PlayKick()
    {
        PlaySFX(kickSFX);
    }


    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip);
    }
    public void PlayEnemySpawn()
    {
        PlaySFX(enemySpawnSFX);
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void PlayLevelMusic(int level)
    {
        switch (level)
        {
            case 1: PlayMusic(level1Music); break;
            case 2: PlayMusic(level2Music); break;
            case 3: PlayMusic(level3Music); break;
        }
    }
    public void StopMusic()
    {
        musicSource.Stop();
    }
    private void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }
}
