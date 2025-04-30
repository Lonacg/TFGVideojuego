using UnityEngine;



public class SFXManagerCover : MonoBehaviour
{
    // DECLARACIÓN DE ELEMENTOS GLOBALES
    [Header("Audio Sources:")]
    private AudioSource audioSourceSFX;
    private AudioClip previousAudioClip;
    private float previousACTimeStamp;

    [Header("Audio Clips:")]
    [SerializeField] private AudioClip clicButton;



    // MÉTODOS HEREDADOS DE MONOBEHAVIOUR
    void Start()
    {
        audioSourceSFX = GetComponent<AudioSource>();
    }



    // MÉTODOS ESPEFICICOS DE ESTA CLASE
    public void OnPlayPressed(){
        // Reproducimos el sonido de pulsacion
        PlaySFX(clicButton, volume: 0.5f); 
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

}
