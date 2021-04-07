using System;
using UnityEngine;
using GameEvents;
using System.Collections;

[Serializable]
public class SoundSettings
{
    public bool MusicOn = true;
    public bool SfxOn = true;
}

public class AudioManager : MonoBehaviour
{
    [Header("Settings")]
    public SoundSettings Settings;

    [Header("Sound Effects")]
    /// true if the sound fx are enabled
    //public bool SfxOn=true;
    /// the sound fx volume
    [Range(0, 1)]
    public float SfxVolume = 1f;

    protected const string _saveFolderName = "Engine/";
    protected const string _saveFileName = "sound.settings";


    /// <summary>
    /// Plays a sound
    /// </summary>
    /// <returns>An audiosource</returns>
    /// <param name="sfx">The sound clip you want to play.</param>
    /// <param name="location">The location of the sound.</param>
    /// <param name="loop">If set to true, the sound will loop.</param>
    public virtual AudioSource PlaySound(AudioClip sfx, bool loop = false)
    {
        if (!Settings.SfxOn)
            return null;
        // we create a temporary game object to host our audio source
        GameObject temporaryAudioHost = new GameObject("TempAudio");
        // we add an audio source to that host
        AudioSource audioSource = temporaryAudioHost.AddComponent<AudioSource>() as AudioSource;
        // we set that audio source clip to the one in paramaters
        audioSource.clip = sfx;
        // we set the audio source volume to the one in parameters
        audioSource.volume = SfxVolume;
        // we set our loop setting
        audioSource.loop = loop;
        // we start playing the sound
        audioSource.Play();

        if (!loop)
        {
            // we destroy the host after the clip has played
            Destroy(temporaryAudioHost, sfx.length);
        }

        // we return the audiosource reference
        return audioSource;
    }

    /// <summary>
    /// Stops the looping sounds if there are any
    /// </summary>
    /// <param name="source">Source.</param>
    public virtual void StopLoopingSound(AudioSource source)
    {
        if (source != null)
        {
            Destroy(source.gameObject);
        }
    }

    public AudioClip BagOpenSound;
    public AudioClip BagCloseSound;
    public AudioClip PosterSound;
    public AudioClip ButtonPressSound;
    public AudioClip MoneyReceivedSound;
    public AudioClip NetSwingSound;
    public AudioClip DrinkingSound;

    protected virtual void OnEnable()
    {
        GameEventManager.AddListener<BagOpenEvent>(OnBagUpdate);
        GameEventManager.AddListener<PosterAcceptedEvent>(OnPosterAccept);
        GameEventManager.AddListener<GenericButtonPressedEvent>(OnButtonPressed);
        GameEventManager.AddListener<MoneyUpdate>(OnMoneyUpdate);
        GameEventManager.AddListener<PosterSoundEvent>(OnPosterSound);
        GameEventManager.AddListener<SwingEvent>(OnSwing);
        GameEventManager.AddListener<SpeedUpgradeBought>(OnDrinking);

    }

    protected virtual void OnDisable()
    {
        GameEventManager.RemoveListener<BagOpenEvent>(OnBagUpdate);
        GameEventManager.RemoveListener<PosterAcceptedEvent>(OnPosterAccept);
        GameEventManager.RemoveListener<GenericButtonPressedEvent>(OnButtonPressed);
        GameEventManager.RemoveListener<MoneyUpdate>(OnMoneyUpdate);
        GameEventManager.RemoveListener<PosterSoundEvent>(OnPosterSound);
        GameEventManager.RemoveListener<SwingEvent>(OnSwing);
        GameEventManager.RemoveListener<SpeedUpgradeBought>(OnDrinking);

    }

    private void OnSwing(SwingEvent e)
    {
        PlaySound(NetSwingSound);
    }

    private void OnDrinking(SpeedUpgradeBought e)
    {
        PlaySound(DrinkingSound);
    }

    private void OnBagUpdate(BagOpenEvent e)
    {
        if (e.openBag)
        {
            PlaySound(BagOpenSound);
        }
        else if (!e.openBag){
            PlaySound(BagCloseSound);
        }
    }

    private void OnPosterAccept(PosterAcceptedEvent e)
    {
        PlaySound(PosterSound);
    }

    private void OnButtonPressed(GenericButtonPressedEvent e)
    {
        PlaySound(ButtonPressSound);
    }

    private void OnMoneyUpdate(MoneyUpdate e)
    {
        PlaySound(MoneyReceivedSound);
    }

    private void OnPosterSound(PosterSoundEvent e)
    {
        PlaySound(PosterSound);
    }
}