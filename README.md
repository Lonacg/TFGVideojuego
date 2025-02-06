# TFGVideojuego
TFG - Laura Cano Gómez


## Cuerpo del juego
Se desarrolla en una ciudad con diferentes comercios, vehículos, peatones y animales.
Al inicio de la partida, te preguntan el nombre y el lugar donde te gustaría vivir, y si eres niño, niña o marciano.
A continuación, apareces en un punto aleatorio de la ciudad, y aparece un diálogo para encargarte una misión.
Por el camino, te encuentras minijuegos que saltan solos al ocurrir x cosa.
Te dan puntos de bondad por cada misión completada.


## Misiones posibles
   - Ferretería: recoger 10 monedas porque no tienen cambio
   - Peluquería: encontrar unas tijeras perdidas
   - Panadería: encontrar una llave perdida
   - Peatón en frente de la Zapatería: encontrar un perro perdido
   - Camión de la basura del Parque: recoger 5 cacas de perro
   - Estudio de televisión: participar en un concurso de V/F
   - Floristería: encontrar 3 tulipanes blancos
   - Ayuntamiento: buscar un anciano en el parque con el que jugar a las cartas -> Extra: el anciano le cuenta sus batallas y le pide resolver un minijuego extra: pares a la derecha e impares a la izq.
   - Relax: se han completado todas las misiones


## Minijuegos en la calle
   - __HECHO__ -> Pasar al lado de un coche amarillo: MJ de aparccamiento (aparcar en la sol correcta)
   - __HECHO__ -> Pasar al lado de Granny (ella corriendo): MJ de pasillos (entrar por el pasillo con la sol correcta)
   - Pisar una interrogacion en el suelo: MJ de adivinar el signo de la operacion
   - Pasar al lado de una bici aparcada en la acera: MJ carrera de bicis (cada operacion resuelta es un impulso)


## Minijuegos de misiones
   - MJ de adivinar parejas (operación y su resultado)
   - MJ de pares e impares (pares a un lado, impares a otro), batir record de x
   - MJ de V/F (varias operaciones con su solucion correcta o erronea, y con tiempo)


## Dialogos
   - Diálogo al __comienzo__ del juego: (escrito directamente en la pantalla)
      - *UI:* Buenos dias, ¿cómo te llamas? (escribe tu nombre y pulsa Enter) 
      - *Player:* *Nombre*
      - *UI:* Encantado *Nombre*. Dime, ¿cómo se llama el sitio donde te gustaría vivir? (escribe el lugar y pulsa Enter)
      - *Player:* *Ciudad*
      - *UI:* ¡Concedido! Por ultimo, ¿Eres un niño, una niña o un extraterreste? (selecciona la opcion que desees moviendote con las flechas y pulsa Enter)
      - *Player:* *ModeloGenero*
      - *UI:* Ya veo, ¡perfecto entonces! Puedes moverte por *Ciudad* pulsando las flechas de tu teclado. Para ir rápido como una bala, además de eso pulsa la tecla Shift. Para coger objetos, cruzar una puerta, o hablar con personas, sitúate cerca de ellos y pulsa X. Si se te olvida como hacerlo o tienes alguna duda, puedes pulsar el menú de Pausa situado en la esquina superior derecha de tu pantalla.
      - *UI:* ¡Una última cosa! Por favor, dirígete a *SiguienteMision*, ¡te necesitan *Nombre*!

   - Diálogo al __terminar una misión__: (Imagen 2D de la persona de la tienda)
      - *UI:* ¡Lo has conseguido! Muchas gracias *Nombre*, lo has hecho genial. Ahora que lo pienso... *SiguienteMision* también te necesita, ¡si puedes ve a ayudarle por favor!

   - Diálogo __última misión__: 
      - *UI:* ¡Bien hecho! Eso es todo por hoy *Nombre*, gracias a ti *Ciudad* es un lugar mejor. Si quieres, puedes seguir disfrutando y pasear un rato más por la ciudad. ¡Nos vemos!


## UI 
Boton de pausa permanente en la esquina superior derecha. Se abre un menú con: 
   - __Mapa:__ entra a una imagen donde se visualiza la ciudad, con el nombre de cada comercio puesto + botón de continuar
   - __Misión actual:__ entra a una imagen con el mapa de la ciudad resaltando con un círculo rojo  donde tiene que ir (cada misión tiene su imagen) + botón de continuar
   - __Controles:__ entra imagen fija en la que sale explicado + botón de continuar
   - __Salir:__ entra imagen "¿Seguro que quieres salir? No se guardará el progreso de esta partida, pero podrás empezar una nueva" + botón SI + botón NO.
    

## Mecanicas    
   - Para superar un minijuego se debe responder bien 2 veces, si no, no se termina (umm mejor no siempre)
   - Si es posible, al fallar una respuesta, debe mostrarse la solucion correcta, antes de pasar a la pregunta siguiente, para que el niñx aprenda el error.
   - Al empezar, aparece en el punto mas alejado del mapa respecto a su misión (puntos concretos preseleccionados)
   - Al acabar una misión, la persona le da la siguiente misión de la lista, siguiendo el orden, para que las misiones esten separadas unas de otras.
   - El juego es infinito, el usuario es el que decide pararlo pulsando el botón Salir de la UI.
   Opcional: contador de tiempo de juego visible en la pantalla



## MEJORAS POSIBLES

- HECHO: Buscar una fuente bonita para las letras

### En GENERAL
- Cambiar LaunchFireworks por LaunchConfetti
- Unificar nombres de archivos


### MG Parking
- HECHO: Que la lista de ParkingLot se coja por codigo en vez de asignarlo en el inspector 
- HECHO: Añadir una ayuda tras el primer fallo
- HECHO: Los conos se mueven al ser empujados por player
- HECHO: Que los tag sean "CorrectAnswer" / "IncorrectAnswer"
- HECHO: Refactorizar codigo. Por ejemplo, si el script esta en un objeto, no hace falta pasarle por referencia ese objeto, con 'gameObject.' se accede directamente, y tampoco hace falta poner gameObject. para acceder un componente, coge directamente el objeto donde eta el script.
- HECHO:Cambiar el 3, 2, 1, ya
- HECHO: La pantalla de error es una imagen en vez de una vista, asi cuando aparece el error, se puede seguir viendo la operacion y le da tiempo a pensar mientras sale el texto.
- HECHO: Añadir inercia
- HECHO: Poner que la X con el order in layer porque asi a veces falla
- HECHO: Cambiar fuente de letra a la de MG DeduceSign
- HECHO: Poner las cosas como [SerializeField] en vez de como public
- HECHO: Vehiculos pasando por la carretera en ambos sentidos (meterlo en la memoria si se hace)
- Uno de los coches deberia estar si o si instanciado de cara, otro si o si de culo y los otros aleatorio.
- Añadir música
- Despues del texto de intro, deberia haber una imagen con la explicacion de los controles, y que tenga que pulsar una tecla para continuar
- Que se instancien entre 3 y 4 coches, no siempre 4 


### MG LaneRace
- HECHO: (no se cuenta como acierto ni fallo y el juego sigue correctamente) arreglar problema de que pase entre dos puertas
- HECHO: Cambiar la actualizacion del texto de la operacion del canvas al stage manager
- HECHO: Poner un terreno infinito de alguna forma
- HECHO: arreglar el movimiento del terreno y el suelo infinito
- Añadir "correctos:" a la imagen de score
- Poner particulas brillantes al activarse los botones de correcto e incorrecto
- Poner arboles y flores en el suelo, algo mas VERDE BRILLANTE, asi queda muy apagado y no pega con Granny
- Añadir musica
- Añadir nubecitas o particulas tipo polvo que salgan en el suelo al correr, cuando corre rapido salen mas y cuando va ams despacio salen menos, como ayuda visual del cambio de velicidad
- Despues del texto de intro, deberia haber una imagen con la explicacion de los controles, y que tenga que pulsar una tecla para continuar
- Transicion en velolociadad del suelo
- Valorar el uso de diccionarios
- Listar las 4 operaciones en Stage manager y que set opetarion coja una de ahi (gestionando correctamente las 3 primeras que se generan al mismo tiempo... posibles poblemas de paralela)


### MG DeduceSign
- HECHO: Impedir que se pueda pasar el raton por algun boton (ni pulsar) hasta que la operacion sea completamente visible
- HECHO: Añadir rondas al juego
- HECHO: Añadir en Stage manager que el totalRounds tenga que ser un numero mayor que 3
- IMPORTANTE: cuando fallas la ronda, se ven por un momento los intentos de la proxima ronda. Deberia mantenerse o reducirse en uno y mantenerse.
- Añadir particulas cuando baja un intento
- Añadir particulas en el boton al ponerse verde
- ¿El intento deberia bajar en uno cuando se acierta?
- Despues del texto de intro, deberia haber una imagen con la explicacion de los controles, y que tenga que pulsar una tecla para continuar
