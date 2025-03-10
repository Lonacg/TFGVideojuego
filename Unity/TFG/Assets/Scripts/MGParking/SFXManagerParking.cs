using UnityEngine;
using System.Collections;

public class SFXManagerParking : MonoBehaviour
{
    // DECLARACIÓN DE ELEMENTOS GLOBALES
    [Header("Audio Sources:")]
    private AudioSource audioSourceMusic;
    private AudioSource audioSourceSFX;  // Un audio source solo puede reproducir un sonido
    private AudioClip previousAudioClip;
    private float previousACTimeStamp;
    [SerializeField] private AudioSource engineStaticAudioSource;

    [Header("Game Objects:")]
    [SerializeField] private GameObject music;

    [Header("Audio Clips:")]
    [SerializeField] private AudioClip startCar;
    [SerializeField] private AudioClip collisionCar;
    [SerializeField] private AudioClip collisionCone;
    [SerializeField] private AudioClip correctAnswer;
    [SerializeField] private AudioClip wrongAnswer;
    [SerializeField] private AudioClip gotIt;



    // MÉTODOS HEREDADOS DE MONOBEHAVIOUR
    private void OnEnable()
    {
        CanvasManagerParking.OnPlay   += HandleOnPlay;
        CollisionCar.OnCollisionCar   += HandleOnCollisionCar;
        CollisionCone.OnCollisionCone += HandleOnCollisionCone;
        ParkingTrigger.OnWellParked   += HandleOnWellParked;
        ParkingTrigger.OnWrongParked  += HandleOnWrongParked;
        CanvasManagerParking.OnGotIt  += HandleOnGotIt;
    }

    private void OnDisable()
    {
        CanvasManagerParking.OnPlay   -= HandleOnPlay;
        CollisionCar.OnCollisionCar   -= HandleOnCollisionCar;
        CollisionCone.OnCollisionCone -= HandleOnCollisionCone;
        ParkingTrigger.OnWellParked   -= HandleOnWellParked;
        ParkingTrigger.OnWrongParked  -= HandleOnWrongParked;
        CanvasManagerParking.OnGotIt  -= HandleOnGotIt;
    }

    void Start()
    {
        audioSourceSFX = GetComponent<AudioSource>();
        audioSourceMusic = music.GetComponent<AudioSource>();
    }



    // MÉTODOS EN RESPUESTA A EVENTOS
    private void HandleOnPlay(){
        // Sonido de arranque del coche y motor estatico
        StartCoroutine(WaitAndStartMotor(1f)); // 1f porque el sonido de arranque dura un segundo

        // El sonido de acelerar (MotorInMotion) lo reproduce CarMovement.cs porque es cuando se pulsa la tecla de avance o retroceso
    }

    private void HandleOnCollisionCar(){
        // Sonido de colision con los coches aparcados
        PlaySFX(collisionCar, volume: 0.1f);
    }

    private void HandleOnCollisionCone(){
        // Sonido de colision con los conos
        PlaySFX(collisionCone, volume: 0.6f);
    }

    private void HandleOnWellParked(GameObject go){
        // Sonido de victoria
        PlaySFX(correctAnswer, volume: 0.3f);        
    }

    private void HandleOnWrongParked(GameObject go){
        // Sonido de derrota
        PlaySFX(wrongAnswer, volume: 1);        
    }

    private void HandleOnGotIt(){
        // Apagamos la musica de fondo y reproducimos el sonido de victoria
        StartCoroutine(StopMusic(endVolume: 0, animationTime: 1));
        PlaySFX(gotIt, volume: 0.5f);        
    }



    // MÉTODOS DE ESTA CLASE
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
    IEnumerator WaitAndStartMotor(float seconds){
        yield return new WaitForSeconds(0.5f);  // Damos medio segundo para que la transicion de FadeInCircle este ya a medias

        // Sonido de arranque
        PlaySFX(startCar, 0.3f);

        yield return new WaitForSeconds(seconds);

        // Activamos el AudioSource puesto en el hijo del coche, y automaticamente empieza a reproducirse, como la musica de fondo
        engineStaticAudioSource.enabled = true;
    }

    IEnumerator StopMusic(float endVolume, float animationTime = 0.5f){
        float elapsedTime = 0;
        float startVolume = audioSourceMusic.volume;
        while(elapsedTime < animationTime){
            float newVolume = Mathf.Lerp(startVolume, endVolume, elapsedTime / animationTime);
            audioSourceMusic.volume = newVolume;
            elapsedTime += Time.deltaTime;
            yield return 0;
        }
        audioSourceMusic.volume = 0;
        
        // Desctivamos el AudioSource puesto en el hijo del coche, quees el sonido del motor en marcha
        engineStaticAudioSource.enabled = true;
    }

}
