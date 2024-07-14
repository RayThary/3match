using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public enum Clips
    {
        Block,
    }

    [SerializeField] private AudioSource m_backGroundSource;

    [SerializeField] private AudioMixer m_mixer;
    [SerializeField] private AudioClip m_BackGroundClip;

    private GameObject optionObj;

    private Slider MasterSoundSlider;
    private Slider BackGroundSlider;
    private Slider SFXSlider;

    private float masterVolum;
    private float backVolum;
    private float SFXVolum;

    [SerializeField] private GameObject SFXsource;
    [SerializeField] private List<AudioClip> clips = new List<AudioClip>();



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        StartCoroutine(bgStart());
    }

    private void Start()
    {
        m_backGroundSource = GetComponent<AudioSource>();
        masterVolum = 0.5f;
        backVolum = 0.5f;
        SFXVolum = 0.5f;
        MasterSoundSlider.value = masterVolum;
        BackGroundSlider.value = backVolum;
        SFXSlider.value = SFXVolum;
    }


    IEnumerator bgStart()
    {
        yield return new WaitForSeconds(0.5f);

        bgSoundPlay(m_BackGroundClip);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //캔버스를 찾아주고 사운드 옵션을 설정한값을 다음씬에서도 적용해준다
        
        if (optionObj == null)
        {
            optionObj = GameObject.Find("Canvas");

            if (optionObj == null)
            {
                Debug.LogError("캔버스가없음");
            }
            Transform option;


            option = optionObj.transform.Find("Sound");



            MasterSoundSlider = option.Find("Master").GetComponentInChildren<Slider>();
            BackGroundSlider = option.Find("BGM").GetComponentInChildren<Slider>();
            SFXSlider = option.Find("SFX").GetComponentInChildren<Slider>();



            MasterSoundSlider.onValueChanged.AddListener(x => m_mixer.SetFloat("Master", Mathf.Log10(x) * 20));//슬라이더연결
            MasterSoundSlider.onValueChanged.AddListener((x) => { masterVolum = x; });
            BackGroundSlider.onValueChanged.AddListener((x) => { m_mixer.SetFloat("BGM", Mathf.Log10(x) * 20); });//슬라이더연결
            BackGroundSlider.onValueChanged.AddListener((x) => { backVolum = x; });
            SFXSlider.onValueChanged.AddListener((x) => { m_mixer.SetFloat("SFX", Mathf.Log10(x) * 20); });//슬라이더연결
            SFXSlider.onValueChanged.AddListener((x) => { SFXVolum = x; });
        }

        MasterSoundSlider.value = masterVolum;
        BackGroundSlider.value = backVolum;
        SFXSlider.value = SFXVolum;

    }



    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// 내부 이넘을 통해서 사용할 클립을 선택, 볼륨 크기 , 사운드의 시작지점
    /// </summary>
    /// <param name="_clip">사용될 소리</param>
    /// <param name="_volum">소리의 크기</param>
    /// <param name="_SFXTime">소리의 크기</param>
    public void SFXCreate(Clips _clip, float _volum, float _SFXTime)
    {
        AudioClip clip = clips.Find(x => x.name == _clip.ToString());
        StartCoroutine(SFXPlaying(clip, _volum, _SFXTime));
        //SFXPlay(clip, _volum);
    }

    private void SFXPlay(AudioClip clip, float _volum)
    {
        //StartCoroutine(SFXPlaying(clip, _volum));
    }

    IEnumerator SFXPlaying(AudioClip clip, float _volum, float _SFXTime)
    {
        //= PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.SFXSource, GameManager.instance.GetSFXParent);

        GameObject SFXSource = Instantiate(SFXsource, transform);

        AudioSource m_sfxaudiosource = SFXSource.GetComponent<AudioSource>();

        m_sfxaudiosource.outputAudioMixerGroup = m_mixer.FindMatchingGroups("SFX")[0];
        m_sfxaudiosource.clip = clip;
        m_sfxaudiosource.loop = false;
        m_sfxaudiosource.volume = _volum;
        m_sfxaudiosource.time = _SFXTime;
        m_sfxaudiosource.Play();
        yield return new WaitForSeconds(clip.length);
        //PoolingManager.Instance.RemovePoolingObject(SFXSource);
    }


    public void bgSoundPlay(AudioClip clip)
    {

        m_backGroundSource.outputAudioMixerGroup = m_mixer.FindMatchingGroups("BGM")[0];
        m_backGroundSource.clip = clip;
        m_backGroundSource.loop = true;
        m_backGroundSource.time = 0;
        m_backGroundSource.volume = 1f;
        m_backGroundSource.Play();
    }

    public void bgSoundPause()
    {
        m_backGroundSource.Pause();
    }

    public void soundrVolums(float _master, float _backGround, float _SFX)
    {
        _master = masterVolum;
        _backGround = backVolum;
        _SFX = SFXVolum;
    }
}
