using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Sounds : MonoBehaviour {
    public float sfxDivision = 3;
    public float bgmDivision = 2;
    //Master Volume
    public AudioListener audioListener;

    //Sliders
    public Slider masterSlider;
    public Slider BGMSlider;
    public Slider SFXSlider;

    //BGM
    public AudioClip clipDialog;
    public AudioClip clipWave;
    public AudioClip clipBg;
    public AudioClip clipSamurai;
    public AudioClip clipUsingSound2;
    public AudioClip clipOkami;
    public AudioClip clipBossTheme;

    //SFX
        //Yasushi
    public AudioClip clipJump;
    public AudioClip clipPunch1;
    public AudioClip clipPunch2;
    public AudioClip clipConsume;
    public AudioClip clipPunch1OnCast;
    public AudioClip clipPunch2OnCast;
    public AudioClip clipPunch3OnCast;
    public AudioClip clipYasushiFlinch;
    public AudioClip clipYasushiTCharge;
    public AudioClip clipYasushiRoll;
    public AudioClip clipFever;
    public AudioClip clipTornado;

    //Menu
    public AudioClip clipDrumSFX;
    public AudioClip clipYOO;

        //Goon
    public AudioClip clipGoonAttack;
    public AudioClip clipGoonFlinch;
    public AudioClip clipGoonJump;
    public AudioClip clipGoonDeath;

        //Maki
    public AudioClip clipMakiAttack;
    public AudioClip clipMakiFlinch;
    public AudioClip clipMakiJump;
    public AudioClip clipMakiDeath;
    public AudioClip clipMakiAniki;

        //Tempura
    public AudioClip clipTempuraAttack;
    public AudioClip clipTempuraFlinch;
    public AudioClip clipTempuraJump;
    public AudioClip clipTempuraDeath;
    public AudioClip clipTempuraSkill;
    public AudioClip clipTempuraKora;

        //Kamii
    public AudioClip clipKamiiHello;

        //Chukka
    public AudioClip clipChukkaHey;
    public AudioClip clipChukkaFlinch;
    public AudioClip clipChukkaSkill;
    public AudioClip clipChukkaAttack;

    //Boss
    public AudioClip clipBossDialog;
    public AudioClip clipBossFlinch;
    public AudioClip clipBossDeath;
    public AudioClip clipBossStab;

    //Knife
    public AudioClip clipKnife;

        //Victory
    public AudioClip clipVictory;

    //BGM
    private AudioSource audioDialog;
    private AudioSource audioWave;
    private AudioSource audioBg;
    private AudioSource audioSamurai;
    private AudioSource audioUsingSound2;
    private AudioSource audioOkami;
    private AudioSource audioBossTheme;

    //SFX Audiosources
    //Yasushi
    private AudioSource audioJump;
    private AudioSource audioPunch1;
    private AudioSource audioPunch2;
    private AudioSource audioConsume;
    private AudioSource audioPunch1OnCast;
    private AudioSource audioPunch2OnCast;
    private AudioSource audioPunch3OnCast;
    private AudioSource audioYasushiTCharge;
    private AudioSource audioYasushiFlinch;
    private AudioSource audioYasushiRoll;
    private AudioSource audioFever;
    private AudioSource audioTornado;

    //Menu
    private AudioSource audioDrumSFX;
    private AudioSource audioYOO;

        //Goon
    private AudioSource audioGoonAttack;
    private AudioSource audioGoonFlinch;
    private AudioSource audioGoonJump;
    private AudioSource audioGoonDeath;

        //Maki
    private AudioSource audioMakiAniki;
    private AudioSource audioMakiAttack;
    private AudioSource audioMakiFlinch;
    private AudioSource audioMakiJump;
    private AudioSource audioMakiDeath;

        //Kamii
    private AudioSource audioKamiiHello;

        //Chukka
    private AudioSource audioChukkaHey;
    private AudioSource audioChukkaFlinch;
    private AudioSource audioChukkaAttack;
    private AudioSource audioChukkaSkill;

    //Tempura
    private AudioSource audioTempuraKora;
    private AudioSource audioTempuraAttack;
    private AudioSource audioTempuraFlinch;
    private AudioSource audioTempuraJump;
    private AudioSource audioTempuraDeath;
    private AudioSource audioTempuraSkill;

        //Boss
    private AudioSource audioBossDialog;
    private AudioSource audioBossFlinch;
    private AudioSource audioBossDeath;
    private AudioSource audioBossStab;

    //Knife
    private AudioSource audioKnife;

        //Victory
    private AudioSource audioVictory;

    //Fading Boolean For Fight Stages Dialog/Wave
    public bool fading1 = false;
    public bool fadeDone1 = false;

    //Fading Boolean For Platforming Stages Dialog/Wave
    public bool fading2 = false;
    public bool fadeDone2 = false;

    //Fading Boolean For Boss Stage Victory
    public bool fading3 = false;
    public bool fadeDone3 = false;

    public AudioSource AddAudio(AudioClip clip, bool loop, bool playAwake, float vol)
    {
        AudioSource newAudio = gameObject.AddComponent<AudioSource>();
        newAudio.clip = clip;
        newAudio.loop = loop;
        newAudio.playOnAwake = playAwake;
        newAudio.volume = vol;
        return newAudio;
    }

    // Use this for initialization
    void Awake()
    {
        //BGM
        audioDialog = AddAudio(clipDialog, true, true, 0.1f);
        audioWave = AddAudio(clipWave, true, false, 0.2f);
        audioBg = AddAudio(clipBg, true, true, 0.7f);
        audioSamurai = AddAudio(clipSamurai, true, true, 0.7f);
        audioUsingSound2 = AddAudio(clipUsingSound2, true, true, 0.7f);
        audioOkami = AddAudio(clipOkami, true, true, 0.7f);
        audioBossTheme = AddAudio(clipBossTheme, true, true, 0.7f);


        //SFX
        audioJump = AddAudio(clipJump, false, false, 0.2f);
        audioPunch1 = AddAudio(clipPunch1, false, false, 0.5f);
        audioPunch2 = AddAudio(clipPunch2, false, false, 0.5f);
        audioConsume = AddAudio(clipConsume, false, false, 0.15f);
        audioPunch1OnCast = AddAudio(clipPunch1OnCast, false, false, 0.1f);
        audioPunch2OnCast = AddAudio(clipPunch2OnCast, false, false, 0.07f);
        audioPunch3OnCast = AddAudio(clipPunch3OnCast, false, false, 0.1f);
        audioYasushiTCharge = AddAudio(clipYasushiTCharge, false, false, 0.1f);
        audioYasushiFlinch = AddAudio(clipYasushiFlinch, false, false, 0.07f);
        audioYasushiRoll = AddAudio(clipYasushiRoll, false, false, 0.1f);
        audioFever = AddAudio(clipFever, false, false, 0.1f);
        audioTornado = AddAudio(clipTornado, false, false, 0.1f);

        audioDrumSFX = AddAudio(clipDrumSFX, false, false, 0.8f);

        audioGoonAttack = AddAudio(clipGoonAttack, false, false, 0.8f);
        audioGoonFlinch = AddAudio(clipGoonFlinch, false, false, 0.8f);
        audioGoonJump = AddAudio(clipGoonJump, false, false, 0.8f);
        audioGoonDeath = AddAudio(clipGoonDeath, false, false, 0.8f);

        audioMakiAttack= AddAudio(clipMakiAttack, false, false, 0.8f);
        audioMakiFlinch = AddAudio(clipMakiFlinch, false, false, 0.8f);
        audioMakiJump = AddAudio(clipMakiJump, false, false, 0.8f);
        audioMakiDeath = AddAudio(clipMakiDeath, false, false, 0.8f);
        audioMakiAniki = AddAudio(clipMakiAniki, false, false, 0.8f);

        audioTempuraAttack = AddAudio(clipTempuraAttack, false, false, 0.8f);
        audioTempuraFlinch = AddAudio(clipTempuraFlinch, false, false, 0.8f);
        audioTempuraJump = AddAudio(clipTempuraJump, false, false, 0.8f);
        audioTempuraDeath = AddAudio(clipTempuraDeath, false, false, 0.8f);
        audioTempuraKora = AddAudio(clipTempuraKora, false, false, 0.8f);
        audioTempuraSkill = AddAudio(clipTempuraSkill, false, false, 0.8f);

        audioBossDialog = AddAudio(clipBossDialog, false, false, 0.8f);
        audioBossFlinch = AddAudio(clipBossFlinch, false, false, 0.8f);
        audioBossDeath = AddAudio(clipBossDeath, false, false, 0.8f);

        audioKamiiHello = AddAudio(clipKamiiHello, false, false, 0.8f);

        audioChukkaHey = AddAudio(clipChukkaHey, false, false, 0.8f);
        audioChukkaAttack = AddAudio(clipChukkaAttack, false, false, 0.8f);
        audioChukkaFlinch = AddAudio(clipChukkaFlinch, false, false, 0.8f);
        audioChukkaSkill = AddAudio(clipChukkaSkill, false, false, 0.8f);

        audioYOO = AddAudio(clipYOO, false, false, 0.8f);

        audioKnife = AddAudio(clipKnife, false, false, 0.8f);

        audioBossStab = AddAudio(clipBossStab, false, false, 0.8f);

        audioVictory = AddAudio(clipVictory, false, false, 0.8f);
    }

    void Update()
    {
        //BGM
        if (fading1 == false)
        {
            if(fadeDone1 == true)
            {
                audioWave.volume = BGMSlider.value / bgmDivision;
                audioDialog.volume = 0f;
            }
            else
            {
                audioDialog.volume = BGMSlider.value / bgmDivision;
                audioWave.volume = BGMSlider.value / bgmDivision;
                audioBg.volume = BGMSlider.value;
            }
        }
        else
        {
            return;
        }

        if (fading2 == false)
        {
            if (fadeDone2 == true)
            {
                audioSamurai.volume = BGMSlider.value / bgmDivision;
                audioUsingSound2.volume = BGMSlider.value / bgmDivision;
                audioBg.volume = 0f;
            }
            else
            {
                audioSamurai.volume = BGMSlider.value / bgmDivision;
                audioBg.volume = BGMSlider.value;
                audioUsingSound2.volume = BGMSlider.value / bgmDivision;
            }
        }
        else
        {
            return;
        }

        if (fading3 == false)
        {
            if (fadeDone3 == true)
            {
                audioVictory.volume = BGMSlider.value / bgmDivision;
                audioBossTheme.volume = 0f;
            }
            else
            {
                audioBossTheme.volume = BGMSlider.value / bgmDivision;
            }
        }
        else
        {
            return;
        }

        //BGM
        audioOkami.volume = BGMSlider.value;

        audioBossTheme.volume = BGMSlider.value / bgmDivision;

        audioDrumSFX.volume = SFXSlider.value;


        //SFX
        audioJump.volume = SFXSlider.value/sfxDivision;
        audioPunch1.volume = SFXSlider.value;
        audioPunch2.volume = SFXSlider.value;
        audioConsume.volume = SFXSlider.value/sfxDivision;
        audioPunch1OnCast.volume = SFXSlider.value/sfxDivision;
        audioPunch2OnCast.volume = SFXSlider.value/sfxDivision;
        audioPunch3OnCast.volume = SFXSlider.value/sfxDivision;
        audioYasushiFlinch.volume = SFXSlider.value / (sfxDivision + 1);
        audioYasushiTCharge.volume = SFXSlider.value/ sfxDivision;
        audioFever.volume = SFXSlider.value / (sfxDivision - 1);
        audioYasushiRoll.volume = SFXSlider.value/ sfxDivision;
        audioTornado.volume = SFXSlider.value / sfxDivision;

        audioGoonAttack.volume = SFXSlider.value/sfxDivision;
        audioGoonFlinch.volume = SFXSlider.value/sfxDivision;
        audioGoonJump.volume = SFXSlider.value/sfxDivision;
        audioGoonDeath.volume = SFXSlider.value;

        audioMakiAttack.volume = SFXSlider.value / sfxDivision;
        audioMakiFlinch.volume = SFXSlider.value / sfxDivision;
        audioMakiJump.volume = SFXSlider.value / sfxDivision;
        audioMakiDeath.volume = SFXSlider.value / sfxDivision;
        audioMakiAniki.volume = SFXSlider.value;

        audioTempuraAttack.volume = SFXSlider.value / sfxDivision;
        audioTempuraFlinch.volume = SFXSlider.value / sfxDivision;
        audioTempuraJump.volume = SFXSlider.value / sfxDivision;
        audioTempuraDeath.volume = SFXSlider.value / sfxDivision;
        audioTempuraSkill.volume = SFXSlider.value / sfxDivision;
        audioTempuraKora.volume = SFXSlider.value / sfxDivision;

        audioBossDialog.volume = SFXSlider.value / sfxDivision;
        audioBossDeath.volume = SFXSlider.value / sfxDivision;
        audioBossFlinch.volume = SFXSlider.value / sfxDivision;
        audioBossStab.volume = SFXSlider.value / sfxDivision;

        audioKnife.volume = SFXSlider.value / sfxDivision;

        audioKamiiHello.volume = SFXSlider.value;

        audioChukkaHey.volume = SFXSlider.value;
        audioChukkaFlinch.volume = SFXSlider.value / sfxDivision;
        audioChukkaAttack.volume = SFXSlider.value / sfxDivision;
        audioChukkaSkill.volume = SFXSlider.value / sfxDivision;

        audioYOO.volume = SFXSlider.value / sfxDivision; ;

        //Master
        AudioListener.volume = masterSlider.value;
    }

    public void PlayPunch1()
    {
        audioPunch1.Play();
    }

    public void PlayPunch2()
    {
        audioPunch2.Play();
    }

    public void PlayPunch1OnCast()
    {
        audioPunch1OnCast.Play();
    }

    public void PlayPunch2OnCast()
    {
        audioPunch2OnCast.Play();
    }

    public void PlayPunch3OnCast()
    {
        audioPunch3OnCast.Play();
    }

    public void PlayFlinch()
    {
        audioYasushiFlinch.PlayOneShot(clipYasushiFlinch);
    }

    public void PlayRoll()
    {
        audioYasushiRoll.PlayOneShot(clipYasushiRoll);
    }

    public void PlayTornado()
    {
        audioTornado.PlayOneShot(clipTornado);
    }

    public void PlayTCharge()
    {
        audioYasushiTCharge.PlayOneShot(clipYasushiTCharge);
    }

    public void PlayDrum()
    {
        audioDrumSFX.PlayOneShot(clipDrumSFX);
    }

    public void PlayJump()
    {
        audioJump.pitch = 0.8f;
        audioJump.Play();
    }

    public void PlayConsume()
    {
        audioConsume.Play();
    }

    public void PlayFever()
    {
        audioFever.PlayOneShot(clipFever);
    }

    public void PlayDialog()
    {
        StartCoroutine(FadeIn(audioDialog, 1f));
    }

    public void PlayWave()
    {
        StartCoroutine(FadeOut(audioDialog, 1f));
        StartCoroutine(FadeIn(audioWave, 1f));
    }

    public void PlayBG()
    {
        audioBg.Play();
    }

    public void PlaySamurai()
    {
        StartCoroutine(FadeOut(audioBg, 1f));
        StartCoroutine(FadeIn(audioSamurai, 1f));
    }

    public void PlayVictory()
    {
        StartCoroutine(FadeOut(audioBossTheme, 1f));
        StartCoroutine(FadeIn(audioVictory, 1f));
    }

    public void PlaySound2()
    {
        audioUsingSound2.Play();
    }

    public void PlayYOO()
    {
        audioYOO.PlayOneShot(clipYOO);
    }

    public void PlayBossTheme()
    {
        audioBossTheme.Play();
    }

    //Kamii Voids
    public void PlayKamiiHello()
    {
        audioKamiiHello.Play();
    }

    //Okami Void
    public void PlayOkami()
    {
        audioOkami.Play();
    }

    //Goon Voids
    public void PlayGoonAttack()
    {
        audioGoonAttack.PlayOneShot(clipGoonAttack);
    }

    public void PlayGoonFlinch()
    {
        audioGoonFlinch.PlayOneShot(clipGoonFlinch);
    }

    public void PlayGoonJump()
    {
        audioGoonJump.Play();
    }

    public void PlayGoonDeath()
    {
        audioGoonDeath.PlayOneShot(clipGoonDeath);
    }

    //Maki Voids
    public void PlayMakiAttack()
    {
        audioMakiAttack.PlayOneShot(clipMakiAttack);
    }

    public void PlayMakiFlinch()
    {
        audioMakiFlinch.PlayOneShot(clipMakiFlinch);
    }

    public void PlayMakiJump()
    {
        audioMakiJump.Play();
    }

    public void PlayMakiDeath()
    {
        audioMakiDeath.PlayOneShot(clipMakiDeath);
    }

    public void PlayMakiAniki()
    {
        audioMakiAniki.PlayOneShot(clipMakiAniki);
    }

    //Tempura Voids
    public void PlayTempuraAttack()
    {
        audioTempuraAttack.PlayOneShot(clipTempuraAttack);
    }

    public void PlayTempuraFlinch()
    {
        audioTempuraFlinch.PlayOneShot(clipTempuraFlinch);
    }

    public void PlayTempuraJump()
    {
        audioTempuraJump.Play();
    }

    public void PlayTempuraDeath()
    {
        audioTempuraDeath.PlayOneShot(clipTempuraDeath);
    }

    public void PlayTempuraKora()
    {
        audioTempuraKora.PlayOneShot(clipTempuraKora);
    }

    public void PlayTempuraSkill()
    {
        audioTempuraKora.PlayOneShot(clipTempuraSkill);
    }

    //Boss Voids
    public void PlayBossDialog()
    {
        audioBossDialog.PlayOneShot(clipBossDialog);
    }

    public void PlayBossFlinch()
    {
        audioBossFlinch.PlayOneShot(clipBossFlinch);
    }

    public void PlayBossDeath()
    {
        audioBossDeath.PlayOneShot(clipBossDeath);
    }

    public void PlayBossStab()
    {
        audioBossStab.PlayOneShot(clipBossStab);
    }

    //Chukka Voids
    public void PlayChukkaHey()
    {
        audioChukkaHey.PlayOneShot(clipChukkaHey);
    }

    public void PlayChukkaAttack()
    {
        audioChukkaAttack.PlayOneShot(clipChukkaAttack);
    }

    public void PlayChukkaSkill()
    {
        audioChukkaSkill.PlayOneShot(clipChukkaSkill);
    }

    public void PlayChukkaFlinch()
    {
        audioChukkaFlinch.PlayOneShot(clipChukkaFlinch);
    }

    public void PlayKnife()
    {
        audioKnife.PlayOneShot(clipKnife);
    }

    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    public static IEnumerator FadeIn(AudioSource audioSource, float FadeTime)
    {
        float volumeIncrement = 0.2f;
        float startVolume = audioSource.volume;

        audioSource.volume = 0;
        audioSource.Play();

        while (audioSource.volume < startVolume)
        {
            audioSource.volume += volumeIncrement * Time.deltaTime / FadeTime;

            yield return null;
        }
        audioSource.volume = startVolume;
    }
}
