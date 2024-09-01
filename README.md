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
   - HECHO -> Pasar al lado de un coche amarillo: MJ de aparccamiento (aparcar en la sol correcta)
   - Pasar al lado de un corredor: MJ de pasillos (entrar por el pasillo con la sol correcta)
   - Pisar una interrogacion en el suelo: MJ de adivinar el signo de la operacion
   - Pasar al lado de una bici aparcada en la acera: MJ carrera de bicis (cada operacion resuelta es un impulso)


## Minijuegos de misiones
   - MJ de adivinar parejas (operación y su resultado)
   - MJ de pares e impares (pares a un lado, impares a otro), batir record de x
   - MJ de V/F (varias operaciones con su solucion correcta o erronea, y con tiempo)


## Dialogos
   - Diálogo al comienzo del juego: (escrito directamente en la pantalla)
            + Buenos dias, ¿cómo te llamas? (escribe tu nombre y pulsa Enter)
            - *Nombre*
            + Encantado *Nombre*. Dime, ¿cómo se llama el sitio donde te gustaría vivir? (escribe el lugar y pulsa Enter)
            - *Ciudad*
            + ¡Concedido! Por ultimo, ¿Eres un niño, una niña o un marciano? (selecciona la opcion que desees moviendote con las flechas y pulsa Enter)
            - *ModeloGenero*
            + Ya veo, ¡perfecto entonces! Puedes moverte por *Ciudad* pulsando las flechas de tu teclado. Para ir rápido como una bala, además de eso pulsa la tecla Shift. Para coger objetos, cruzar una puerta, o hablar con personas, sitúate cerca de ellos y pulsa X. Si se te olvida como hacerlo o tienes alguna duda, puedes pulsar el menú de Pausa situado en la esquina superior derecha de tu pantalla.
            ¡Una última cosa! Por favor, dirígete a *SiguienteMision*, ¡te necesitan *Nombre*!

    - Diálogo al terminar una misión: (Imagen 2D de la persona de la tienda)
            + ¡Lo has conseguido! Muchas gracias *Nombre*, lo has hecho genial. Ahora que lo pienso... *SiguienteMision* también te necesita, ¡si puedes ve a ayudarle por favor!

    - Diálogo última misión: 
            + ¡Bien hecho! Eso es todo por hoy *Nombre*, gracias a ti *Ciudad* es un lugar mejor. Si quieres, puedes seguir disfrutando y pasear un rato más por la ciudad. ¡Nos vemos!


## UI 
Boton de pausa permanente en la esquina superior derecha. Se abre un menú con: 
    - Mapa: entra a una imagen donde se visualiza la ciudad, con el nombre de cada comercio puesto + botón de continuar
    - Misión actual: entra a una imagen con el mapa de la ciudad resaltando con un círculo rojo  donde tiene que ir (cada misión tiene su imagen) + botón de continuar
    - Controles: entra imagen fija en la que sale explicado + botón de continuar
    - Salir del juego: entra imagen "¿Seguro que quieres salir? No se guardará el progreso de esta partida, pero podrás empezar una nueva" + botón SI + botón NO.
    

## Mecanicas    
   - Para superar un minijuego se debe responder bien 2 veces, si no, no se termina (umm mejor no siempre)
   - Si es posible, al fallar una respuesta, debe mostrarse la solucion correcta, antes de pasar a la pregunta siguiente, para que el niñx aprenda el error.
   - Al empezar, aparece en el punto mas alejado del mapa respecto a su misión (puntos concretos preseleccionados)
   - Al acabar una misión, la persona le da la siguiente misión de la lista, siguiendo el orden, para que las misiones esten separadas unas de otras.
   - El juego es infinito, el usuario es el que decide pararlo pulsando el botón Salir de la UI.
   Opcional: contador de tiempo de juego visible en la pantalla



## MEJORAS POSIBLES

### MG Parking
- HECHO: Que la lista de ParkingLot se coja por codigo en vez de asignarlo en el inspector 
- HECHO: Añadir una ayuda tras el primer fallo
- Vehiculos pasando por la carretera en ambos sentidos
- HECHO: Los conos se mueven al ser empujados por player
- La pantalla de error es una imagen en vez de una vista, asi cuando aparece el error, se puede seguir viendo la operacion y le da tiempo a pensar mientras sale el texto.
- Añadir que haya que aparcar 2 veces, primero una suma/resta y luego una multi/divi
- Que los tag sean "CorrectAnswer" / "IncorrectAnswer"
- Refactorizar codigo. Por ejemplo, si el script esta en un objeto, no hace falta pasarle por referencia ese objeto, con gameObject. se accede directamente.

### MG LaneRace
- Que las operaciones no puedan repetirse (es muy poco probable que pase). Habria que almacenar una tupla con los numeros que van saliendo en otro script y que SetOperation acceda a ella en la eleccion del numero para comprobar que no es ninguno
- No se que es mejor, si que los resultados salgan los 3 ordenados de menor a mayor, o que salgan desordenados random como ahora
- Poner particulas brillantes al activarse los botones de correcto e incorrecto
- Activar particulas de humo al cruzar la puerta para hacerla desaparecer


