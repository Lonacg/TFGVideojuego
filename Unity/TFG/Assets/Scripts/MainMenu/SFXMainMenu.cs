using UnityEngine;
using System.Collections;



public class SFXMainMenu : MonoBehaviour
{
    // DECLARACIÓN DE ELEMENTOS GLOBALES
    [Header("Audio Sources:")]
    private AudioSource audioSourceMusic;
    private AudioSource audioSourceSFX;
    private AudioClip previousAudioClip;
    private float previousACTimeStamp;

    [Header("Game Objects:")]
    [SerializeField] private GameObject music;

    [Header("Audio Clips:")]
    [SerializeField] private AudioClip clicButton;
    [SerializeField] private AudioClip movementDS;
    [SerializeField] private AudioClip formulaAppearance;
    [SerializeField] private AudioClip quitButton;



    // MÉTODOS HEREDADOS DE MONOBEHAVIOUR
    private void OnEnable()
    {
        StageManagerMainMenu.OnMoveButtonDS      += HandleOnMoveButtonDS;
        StageManagerMainMenu.OnFormulaAppearance += HandleOnFormulaAppearance;
    }

    private void OnDisable()
    {
        StageManagerMainMenu.OnMoveButtonDS      -= HandleOnMoveButtonDS;
        StageManagerMainMenu.OnFormulaAppearance -= HandleOnFormulaAppearance;         
    }

    void Start()
    {
        audioSourceSFX = GetComponent<AudioSource>();
        audioSourceMusic = music.GetComponent<AudioSource>();
    }



    // MÉTODOS EN RESPUESTA A EVENTOS
    private void HandleOnMoveButtonDS(){
        // Sonido de movimiento del boton
        PlaySFX(movementDS, volume: 0.3f); 
    }
    private void HandleOnFormulaAppearance(){
        // Sonido de aparicion del cuarto minijuego
        PlaySFX(formulaAppearance, volume: 0.3f); 
    }



    // MÉTODOS ESPEFICICOS DE ESTA CLASE
    public void OnButtonPressed(){
        // Se llama internamente desde el evento del boton, luego debe ser publica
        StartCoroutine(StopMusic(endVolume: 0));
        PlaySFX(clicButton, volume: 0.3f); 
    }
    
    public void OnQuitButton(){
        // Se llama internamente desde el evento del boton, luego debe ser publica
        PlaySFX(quitButton, volume: 0.7f); 
    }
    
    public void OnCancelQuitButton(){
        // Se llama internamente desde el evento del boton, luego debe ser publica
        PlaySFX(clicButton, volume: 0.3f); 
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

        // Reproducimos el sonido
        audioSourceSFX.PlayOneShot(audioClip, volume);
    }



    // CORRUTINAS
    IEnumerator StopMusic(float endVolume, float animationTime = 2f){
        float elapsedTime = 0;
        float startVolume = audioSourceMusic.volume;
        while(elapsedTime < animationTime){
            float newVolume = Mathf.Lerp(startVolume, endVolume, elapsedTime / animationTime);
            audioSourceMusic.volume = newVolume;
            elapsedTime += Time.deltaTime;
            yield return 0;
        }
        audioSourceMusic.volume = 0;
    }
    
}
