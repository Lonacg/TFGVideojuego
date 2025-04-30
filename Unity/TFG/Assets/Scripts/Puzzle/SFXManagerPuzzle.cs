using UnityEngine;
using System.Collections;



public class SFXManagerPuzzle : MonoBehaviour
{
    // DECLARACIÓN DE ELEMENTOS GLOBALES
    [Header("Audio Sources:")]
    private AudioSource audioSourceMusicGame;
    private AudioSource audioSourceMusicLevel;
    private AudioSource audioSourceSFX;  // Un audio source solo puede reproducir un sonido
    private AudioClip previousAudioClip;
    private float previousACTimeStamp;

    [Header("Game Objects:")]
    [SerializeField] private GameObject musicLevel;    
    [SerializeField] private GameObject musicGame;  

    [Header("Audio Clips:")]
    [SerializeField] private AudioClip clicLevel;
    [SerializeField] private AudioClip movementPiece;
    [SerializeField] private AudioClip shakePiece;
    [SerializeField] private AudioClip stamp;
    [SerializeField] private AudioClip gotIt;
    [SerializeField] private AudioClip lastShine;



    // MÉTODOS HEREDADOS DE MONOBEHAVIOUR
    private void OnEnable()
    {
        StageManagerPuzzle.OnFadeToPlay += HandleOnFadeToPlay;
        PieceCheck.OnStartingMovement   += HandleOnStartingMovement;
        PieceCheck.OnShakePiece         += HandleOnShakePiece;
        HandAnimation.OnStampSound      += HandleOnStampSound;
        PuzzleCheck.OnGotIt             += HandleOnGotIt;        
        StageManagerPuzzle.OnLastShine  += HandleOnLastShine;
    }

    private void OnDisable()
    {
        StageManagerPuzzle.OnFadeToPlay   -= HandleOnFadeToPlay;
        PieceCheck.OnStartingMovement     -= HandleOnStartingMovement;
        PieceCheck.OnShakePiece           -= HandleOnShakePiece;
        HandAnimation.OnStampSound        -= HandleOnStampSound;
        PuzzleCheck.OnGotIt               -= HandleOnGotIt;
        StageManagerPuzzle.OnLastShine    -= HandleOnLastShine;
    }

    void Start()
    {
        audioSourceSFX = GetComponent<AudioSource>();
        audioSourceMusicGame = musicGame.GetComponent<AudioSource>();
        audioSourceMusicLevel = musicLevel.GetComponent<AudioSource>();
    }



    // MÉTODOS EN RESPUESTA A EVENTOS
    private void HandleOnFadeToPlay(GameObject fadeCircleView){
        StartCoroutine(ChangeMusic());
    }

    private void HandleOnStartingMovement(){
        // Sonido al mover una pieza
        PlaySFX(movementPiece, volume: 0.8f);
    }

    private void HandleOnShakePiece(){
        // Sonido de shake de la pieza
        PlaySFX(shakePiece, volume: 0.8f);
    }

    private void HandleOnStampSound(){
        // Sonido al poner el sello
        PlaySFX(stamp, volume: 0.8f);        
    }

    private void HandleOnGotIt(){
        // Apagamos la musica de fondo y reproducimos el sonido de victoria
        StartCoroutine(ChangeVolumeMusic(startVolume: audioSourceMusicGame.volume, endVolume: 0, audioSource: audioSourceMusicGame, animationTime: 1));
        PlaySFX(gotIt, volume: 0.5f);        
    }
    
    private void HandleOnLastShine(){
        // Sonido brillante al mostrar la imagen del puzzle sin rayas
        PlaySFX(lastShine, volume: 1f);        
    }


     // MÉTODOS ESPEFICICOS DE ESTA CLASE
    public void OnSoundClic(){
        // Sonido de pulsado del boton en la seleccion de dificultad
        PlaySFX(clicLevel, volume: 0.3f);
    }

    public void PlaySFX(AudioClip audioClip, float volume = 1){
        // Impedimos que dos clips iguales puedan sonar en el mismo momento y se acople el sonido (se multiplicaria el volumen de ese sonido)
        if (previousAudioClip == audioClip){ 
            if(Time.time - previousACTimeStamp < 0.05f){
                return;
            }
        }

        // Guardamos los valores para compararlos con el proximo clip que pidamos reproducir
        previousAudioClip = audioClip;
        previousACTimeStamp = Time.time;

        // Reproducimos el sonido con el metodo PlayOneShot() que tiene la clase AudioSource
        audioSourceSFX.PlayOneShot(audioClip, volume);
    }



    // CORRUTINAS
    IEnumerator ChangeVolumeMusic(float startVolume, float endVolume, AudioSource audioSource, float animationTime = 0.5f){

        float elapsedTime = 0;
        while(elapsedTime < animationTime){
            float newVolume = Mathf.Lerp(startVolume, endVolume, elapsedTime / animationTime);
            audioSource.volume = newVolume;
            elapsedTime += Time.deltaTime;
            yield return 0;
        }
        audioSource.volume = endVolume;
    }

    IEnumerator ChangeMusic(float animationTime = 0.5f){
        // Apagamos el volumen de la musica de los niveles
        StartCoroutine(ChangeVolumeMusic(startVolume: audioSourceMusicLevel.volume, endVolume: 0, audioSource: audioSourceMusicLevel, animationTime));
        yield return new WaitForSeconds(animationTime);
        musicLevel.SetActive(false);

        // Encendemo la musica del juego
        musicGame.SetActive(true);
        StartCoroutine(ChangeVolumeMusic(startVolume: 0, endVolume: 0.5f, audioSource: audioSourceMusicLevel, animationTime));
    }

}
