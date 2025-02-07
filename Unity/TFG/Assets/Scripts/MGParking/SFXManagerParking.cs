using UnityEngine;
using System.Collections;

public class SFXManagerParking : MonoBehaviour
{

    [Header("Audio Sources:")]
    public AudioSource audioSourceSFX;  // Un audio source solo puede reproducir un sonido
    public AudioSource engineStaticAudioSource;
    private AudioClip previousAudioClip;
    private float previousACTimeStamp;



    [Header("Audio Clips:")]
    public AudioClip startCar;
    public AudioClip motorInMotion;



    private void OnEnable(){
        CanvasManagerParking.OnPlay += OnPlay;

        //script.evento += evento;
    }

    private void OnDisable(){

        CanvasManagerParking.OnPlay -= OnPlay;




        //script.evento -= evento;
    }





    private void OnPlay(){
        // Sonido de arranque
        PlaySFX(startCar, 0.3f);
        
        // Sonido de motor arrancado
        StartCoroutine(WaitAndStartMotor(1f)); // 1f porque el sonido de arranque dura un segundo

        // El sonido de acelerar lo reproduce CarMovement.cs porque es cuando se pulsa la tecla de avance o retroceso
    }

    private void MotorInMotion(){
        PlaySFX(motorInMotion, 0.3f);
    }


    public void PlaySFX(AudioClip audioClip, float volume = 1){
        if (previousAudioClip == audioClip){ //Para que 2 sonidos no puedan sonar en el mismo momento y se acople el sonido(se multiplicaria el volumen de ese sonido)
            if(Time.time - previousACTimeStamp < 0.05f){
                return;
            }
        }
        previousAudioClip = audioClip;
        previousACTimeStamp = Time.time;

        audioSourceSFX.PlayOneShot(audioClip, volume);
    }

    IEnumerator WaitAndStartMotor(float seconds){
        yield return new WaitForSeconds(seconds);
        engineStaticAudioSource.enabled = true;
        
    }

}
