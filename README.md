# TFGVideojuego
TFG - Laura Cano Gómez



### En GENERAL
- HECHO: Buscar una fuente bonita para las letras
- Cambiar LaunchFireworks por LaunchConfetti
- Unificar nombres de archivos
- FadeCircle del canvas cambiarlo a FadeCircleView y lo mismo en el codigo


### MG Parking
- HECHO: Que la lista de ParkingLot se coja por codigo en vez de asignarlo en el inspector 
- HECHO: Añadir una ayuda tras el primer fallo
- HECHO: Los conos se mueven al ser empujados por player
- HECHO: Que los tag sean "CorrectAnswer" / "IncorrectAnswer"
- HECHO: Refactorizar codigo. Por ejemplo, si el script esta en un objeto, no hace falta pasarle por referencia ese objeto, con 'gameObject.' se accede directamente, y tampoco hace falta poner gameObject. para acceder un componente, coge directamente el objeto donde eta el script.
- HECHO: Cambiar el 3, 2, 1, ya
- HECHO: La pantalla de error es una imagen en vez de una vista, asi cuando aparece el error, se puede seguir viendo la operacion y le da tiempo a pensar mientras sale el texto.
- HECHO: Añadir inercia
- HECHO: Poner que la X con el order in layer porque asi a veces falla
- HECHO: Cambiar fuente de letra a la de MG DeduceSign
- HECHO: Poner las cosas como [SerializeField] en vez de como public
- HECHO: Vehiculos pasando por la carretera en ambos sentidos (meterlo en la memoria si se hace)
- HECHO: Uno de los coches deberia estar si o si instanciado de cara, otro si o si de culo y los otros aleatorio.
- HECHO: Ajustar mejor la  deteccion de coche aparcado, entrando muy torcido lo damos por bueno
- HECHO: Añadir música
- HECHO: Despues del texto de intro, deberia haber una imagen con la explicacion de los controles, y que tenga que pulsar una tecla para continuar
- HECHO: ¿No seria mejor que la ayuda se muestre tras el segundo fallo en vez de tras el primero? Porque hay 6 posibilidades, dar la ayuda en la primera es como regalarlo un poco.
- Cuando aparece un 1 en la operacion se descolocan las columnas de la segunda forma
- Apagar el sonido del motor en marcha cuando apagamos la musica en la vistoria

### MG LaneRace
- HECHO: (sol= no se cuenta como acierto ni fallo y el juego sigue correctamente) arreglar problema de que pase entre dos puertas
- HECHO: Cambiar la actualizacion del texto de la operacion del canvas al stage manager
- HECHO: Poner un terreno infinito de alguna forma
- HECHO: arreglar el movimiento del terreno y el suelo infinito
- HECHO: Añadir "correctos:" a la imagen de score
- HECHO: Cambiar fuente del texto
- HECHO: Hacer el degradado del texto a la vez que se borran las imagenes en el Ready, Steady, Go.
- HECHO: Poner particulas brillantes al activarse los botones de correcto e incorrecto
- HECHO: Poner arboles y flores en el suelo, algo mas VERDE BRILLANTE, asi queda muy apagado y no pega con Granny
- HECHO: Importante! despues de pasar por el medio de dos puertas pone otra vez la misma operacion en vez de la siguiente, asi que van decaladas
- HECHO: Añadir nubecitas o particulas tipo polvo que salgan en el suelo al correr, cuando corre rapido salen mas y cuando va ams despacio salen menos, como ayuda visual del cambio de velicidad
- HECHO: Hacer una transicion suave cuando el suelo cambia de velocidad
- HECHO: Añadir musica
- HECHO: Deberia haber una imagen con la explicacion de los controles, y que tenga que pulsar una tecla para continuar



### MG DeduceSign
- HECHO: Impedir que se pueda pasar el raton por algun boton (ni pulsar) hasta que la operacion sea completamente visible
- HECHO: Añadir rondas al juego
- HECHO: Añadir en Stage manager que el totalRounds tenga que ser un numero mayor que 3
- HECHO: cuando fallas la ronda, se ven por un momento los intentos de la proxima ronda. Deberia mantenerse o reducirse en uno y mantenerse.
- Añadir particulas cuando baja un intento
- Añadir particulas en el boton al ponerse verde
- Añadir musica
- Deberia haber una imagen con la explicacion de los controles, y que tenga que pulsar una tecla para continuar



### MG Klotski
- 


### Menu principal
- 


