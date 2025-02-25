using System.Collections;
using UnityEngine;

public class SFXManagerLaneRace : MonoBehaviour
{
    [Header("Audio Sources:")]
    private AudioSource audioSourceMusic;
    private AudioSource audioSourceSFX;
    private AudioClip previousAudioClip;
    private float previousACTimeStamp;

    [Header("Game Objects:")]
    [SerializeField] private GameObject music;

    [Header("Audio Clips:")]
    [SerializeField] private AudioClip correctAnswer;
    [SerializeField] private AudioClip wrongAnswer;
    [SerializeField] private AudioClip gotIt;
    [SerializeField] private AudioClip readySteady;
    [SerializeField] private AudioClip go;
    [SerializeField] private AudioClip footstepsRunning;



    private void OnEnable()
    {
        GrannyMovement.OnReady         += HandleOnReady;
        GrannyMovement.OnSteady        += HandleOnSteady;
        GrannyMovement.OnGo            += HandleOnGo;
        GrannyMovement.OnFootstepSound += HandleOnFootstepSound;
        TriggerGate.OnCorrectSol       += HandleOnCorrectSol;
        TriggerGate.OnWrongSol         += HandleOnWrongSol;
        GrannyMovement.OnGotIt         += HandleOnGotIt;

    }

    private void OnDisable()
    {
        GrannyMovement.OnReady         -= HandleOnReady;
        GrannyMovement.OnSteady        -= HandleOnSteady;
        GrannyMovement.OnGo            -= HandleOnGo;
        GrannyMovement.OnFootstepSound -= HandleOnFootstepSound;
        TriggerGate.OnCorrectSol       -= HandleOnCorrectSol;
        TriggerGate.OnWrongSol         -= HandleOnWrongSol;
        GrannyMovement.OnGotIt         -= HandleOnGotIt;        
    }

    void Start()
    {
        audioSourceSFX = GetComponent<AudioSource>();
        audioSourceMusic = music.GetComponent<AudioSource>();
    }



    private void HandleOnReady(){
        // Sonido de preparacion de salida
        PlaySFX(readySteady, volume: 0.5f);    
    }

    private void HandleOnSteady(){
        // Sonido de preparacion de salida
        PlaySFX(readySteady, volume: 0.5f);      
    }

    private void HandleOnGo(){
        // Sonido de salida
        PlaySFX(go, volume: 0.5f);
    }

    private void HandleOnFootstepSound(){
        // Sonido en cada pisada al correr
        PlaySFX(footstepsRunning, volume: 0.3f);
    }

    private void HandleOnCorrectSol(){
        // Sonido de victoria
        PlaySFX(correctAnswer, 0.3f);        
    }

    private void HandleOnWrongSol(){
        // Sonido de derrota
        PlaySFX(wrongAnswer);        
    }


    private void HandleOnGotIt(){
        // Sonido de victoria final con el conseguido
        StartCoroutine(StopMusic(endVolume: 0f));
        PlaySFX(gotIt, 0.5f);        
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

    IEnumerator StopMusic(float endVolume, float animationTime = 1f){
        float elapsedTime = 0;
        float startVolume = audioSourceMusic.volume;
        while(elapsedTime < animationTime){
            float newVolume = Mathf.Lerp(startVolume, endVolume, elapsedTime / animationTime);
            audioSourceMusic.volume = newVolume;
            elapsedTime += Time.deltaTime;
            yield return 0;
        }
    }

}