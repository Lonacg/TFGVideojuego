using System.Collections;
using UnityEngine;

public class SFXManagerDS : MonoBehaviour
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
    [SerializeField] private AudioClip signChosen;
    [SerializeField] private AudioClip failedRound;
    [SerializeField] private AudioClip roundMovement;




    private void OnEnable()
    {
        RoundBehaviour.OnRoundMovementSFX       += HandleOnRoundMovementSFX;
        StageManagerDeduceSign.OnFailedRoundSFX += HandleOnFailedRoundSFX;
        StageManagerDeduceSign.OnCorrectAnswer  += HandleOnCorrectAnswer;
        StageManagerDeduceSign.OnWrongAnswer    += HandleOnWrongAnswer;
        StageManagerDeduceSign.OnGotIt          += HandleOnGotIt;
    }

    private void OnDisable()
    {
        RoundBehaviour.OnRoundMovementSFX       -= HandleOnRoundMovementSFX;
        StageManagerDeduceSign.OnFailedRoundSFX -= HandleOnFailedRoundSFX;
        StageManagerDeduceSign.OnCorrectAnswer  -= HandleOnCorrectAnswer;
        StageManagerDeduceSign.OnWrongAnswer    -= HandleOnWrongAnswer; 
        StageManagerDeduceSign.OnGotIt          -= HandleOnGotIt;       
    }

    void Start()
    {
        audioSourceSFX = GetComponent<AudioSource>();
        audioSourceMusic = music.GetComponent<AudioSource>();
    }






    private void HandleOnFailedRoundSFX(){
        // Sonido de ronda fallida
        PlaySFX(failedRound, volume: 1f);        
    }

    private void HandleOnRoundMovementSFX(){
        // Sonido de movimiento de la ronda
        PlaySFX(roundMovement, volume: 1f);    
    }

    private void HandleOnCorrectAnswer(){
        // Sonido de victoria
        PlaySFX(correctAnswer, volume: 0.5f);        
    }

    private void HandleOnWrongAnswer(){
        // Sonido de derrota
        PlaySFX(wrongAnswer, volume: 1f);        
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
