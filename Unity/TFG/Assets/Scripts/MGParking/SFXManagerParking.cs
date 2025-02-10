using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class SFXManagerParking : MonoBehaviour
{

    [Header("Audio Sources:")]
    public AudioSource audioSourceSFX;  // Un audio source solo puede reproducir un sonido
    public AudioSource engineStaticAudioSource;
    private AudioClip previousAudioClip;
    private float previousACTimeStamp;



    [Header("Audio Clips:")]
    public AudioClip startCar;
    public AudioClip collisionCar;
    public AudioClip collisionCone;
    public AudioClip correctAnswer;
    public AudioClip wrongAnswer;
    public AudioClip gotIt;


    private void OnEnable(){
        CanvasManagerParking.OnPlay   += OnPlay;
        CollisionCar.OnCollisionCar   += OnCollisionCar;
        CollisionCone.OnCollisionCone += OnCollisionCone;
        ParkingTrigger.OnWellParked   += OnWellParked;
        ParkingTrigger.OnWrongParked  += OnWrongParked;
        CanvasManagerParking.OnGotIt  += OnGotIt;


    }

    private void OnDisable(){

        CanvasManagerParking.OnPlay   -= OnPlay;
        CollisionCar.OnCollisionCar   -= OnCollisionCar;
        CollisionCone.OnCollisionCone -= OnCollisionCone;
        ParkingTrigger.OnWellParked   -= OnWellParked;
        ParkingTrigger.OnWrongParked  -= OnWrongParked;
        CanvasManagerParking.OnGotIt  -= OnGotIt;

    }





    private void OnPlay(){
        // Sonido de arranque
        PlaySFX(startCar, 0.3f);
        
        // Sonido de motor estatico
        StartCoroutine(WaitAndStartMotor(1f)); // 1f porque el sonido de arranque dura un segundo

        // El sonido de acelerar (MotorInMotion) lo reproduce CarMovement.cs porque es cuando se pulsa la tecla de avance o retroceso
    }

    private void OnCollisionCar(){
        // Sonido de colision con los coches aparcados
        PlaySFX(collisionCar, 0.1f);

    }


    private void OnCollisionCone(){
        // Sonido de colision con los conos
        PlaySFX(collisionCone, 0.6f);
    }


    private void OnWellParked(GameObject go){
        // Sonido de victoria
        PlaySFX(correctAnswer, 0.3f);        
    }


    private void OnWrongParked(GameObject go){
        // Sonido de derrota
        PlaySFX(wrongAnswer, 0.2f);        
    }

    private void OnGotIt(){
        // Sonido al mostrar el conseguido
        PlaySFX(gotIt, 0.6f);        
    }



    public void PlaySFX(AudioClip audioClip, float volume = 1){
        if (previousAudioClip == audioClip){ //Para que 2 sonidos no puedan sonar en el mismo momento y se acople el sonido (se multiplicaria el volumen de ese sonido)
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
        // Activamos el AudioSource puesto en el hijo del coche, y automaticamente empieza a reproducirse, como la musica de fondo
        engineStaticAudioSource.enabled = true;
        
    }

}
