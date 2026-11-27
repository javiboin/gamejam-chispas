using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource _musicSource; // Para música en loop
    [SerializeField] private AudioSource _sfxSource;   // Para efectos puntuales

    [Header("Clips de Música (Loop)")]
    [SerializeField] private AudioClip _menuMusic;      // horror-music-for-game-menu
    [SerializeField] private AudioClip _gameplayMusic;  // heartbeatgame

    [Header("Clips de Efectos (SFX)")]
    [SerializeField] private AudioClip _piedrasChispas; // piedraschispas1
    [SerializeField] private AudioClip _kotaixScream;   // Kotaix scream
    [SerializeField] private AudioClip _victoryFire;    // sound victory final

    [Header("Ajustes de Volumen")]
    [Range(0f, 1f)][SerializeField] private float _screamVolume = 0.6f;
    [SerializeField] private float _fadeOutDuration = 0.6f;

    [Header("Nombres de Escenas")]
    [SerializeField] private string _mainMenuSceneName = "MainMenu";
    [SerializeField] private string _gameplaySceneName = "MainScene";

    private Coroutine _fadeCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        
        CambiarMusicaPorEscena(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        DifuminarEfectos();
        CambiarMusicaPorEscena(scene.name);
    }

    private void CambiarMusicaPorEscena(string sceneName)
    {

        if (_musicSource == null) return;

        AudioClip clipObjetivo = null;

        if (sceneName == _mainMenuSceneName)
        {
            clipObjetivo = _menuMusic;
        }
        else if (sceneName == _gameplaySceneName)
        {
            clipObjetivo = _gameplayMusic;
        }

        if (clipObjetivo != null && (_musicSource.clip != clipObjetivo || !_musicSource.isPlaying) ) 
        {
            _musicSource.clip = clipObjetivo;
            _musicSource.loop = true;
            _musicSource.Play();
        }
    }

    

    public void PlayPiedrasChispas()
    {
        if (_sfxSource != null && _piedrasChispas != null)
        {
            CancelarFadeYRestaurarVolumen();
            _sfxSource.PlayOneShot(_piedrasChispas);
        }
    }

    public void PlayKotaixScream()
    {
        if (_musicSource != null)
        {
            _musicSource.Stop();
        }

        if (_sfxSource == null || _kotaixScream == null) return;

        if (_sfxSource.isPlaying && _sfxSource.clip == _kotaixScream) return;

        CancelarFadeYRestaurarVolumen();
        _sfxSource.clip = _kotaixScream;
        _sfxSource.volume = _screamVolume;
        _sfxSource.loop = false;
        _sfxSource.Play();
    }

    public void PlayVictoryFire()
    {
        // Opcional: bajamos o frenamos los latidos para que destaque el fuego de victoria
        if (_musicSource != null)
        {
            _musicSource.Stop();
        }

        if (_sfxSource != null && _victoryFire != null)
        {
            _sfxSource.PlayOneShot(_victoryFire);
        }
    }

    public void DifuminarEfectos()
    {
        if (_sfxSource == null || !_sfxSource.isPlaying) return;

        if (_fadeCoroutine != null)
        {
            StopCoroutine(_fadeCoroutine);
        }
        _fadeCoroutine = StartCoroutine(FadeOutSfxRoutine());
    }

    private IEnumerator FadeOutSfxRoutine()
    {
        float startVolume = _sfxSource.volume;
        float elapsed = 0f;

        while (elapsed < _fadeOutDuration)
        {
            // Usamos unscaledDeltaTime para que funcione incluso si Time.timeScale es 0
            elapsed += Time.unscaledDeltaTime;
            _sfxSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / _fadeOutDuration);
            yield return null;
        }

        _sfxSource.Stop();
        _sfxSource.clip = null;
        _sfxSource.volume = 1f;
        _fadeCoroutine = null;
    }

    private void CancelarFadeYRestaurarVolumen()
    {
        if (_fadeCoroutine != null)
        {
            StopCoroutine(_fadeCoroutine);
            _fadeCoroutine = null;
        }
        _sfxSource.volume = 1f;
    }

}
