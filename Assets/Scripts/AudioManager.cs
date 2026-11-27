using UnityEngine;
using UnityEngine.SceneManagement;

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

    [Header("Nombres de Escenas")]
    [SerializeField] private string _mainMenuSceneName = "MainMenu";
    [SerializeField] private string _gameplaySceneName = "MainScene";

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
        // Por si arrancás Play directamente desde MainScene o MainMenu
        CambiarMusicaPorEscena(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
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

        if (clipObjetivo != null && _musicSource.clip != clipObjetivo)
        {
            _musicSource.clip = clipObjetivo;
            _musicSource.loop = true;
            _musicSource.Play();
        }
    }

    // --- MÉTODOS PÚBLICOS PARA LLAMAR DESDE LOS SCRIPTS DEL EQUIPO ---

    public void PlayPiedrasChispas()
    {
        if (_sfxSource != null && _piedrasChispas != null)
        {
            _sfxSource.PlayOneShot(_piedrasChispas);
        }
    }

    public void PlayKotaixScream()
    {
        if (_sfxSource != null && _kotaixScream != null)
        {
            _sfxSource.PlayOneShot(_kotaixScream);
        }
        if (_musicSource != null) _musicSource.Stop();
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
}
