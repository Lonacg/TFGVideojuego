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
   - HECHO -> Pasar al lado de Granny (ella corriendo): MJ de pasillos (entrar por el pasillo con la sol correcta)
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
- HECHO: Los conos se mueven al ser empujados por player
- HECHO: Que los tag sean "CorrectAnswer" / "IncorrectAnswer"
- HECHO: Refactorizar codigo. Por ejemplo, si el script esta en un objeto, no hace falta pasarle por referencia ese objeto, con gameObject. se accede directamente.
- Cambiar el 3, 2, 1, ya
- La pantalla de error es una imagen en vez de una vista, asi cuando aparece el error, se puede seguir viendo la operacion y le da tiempo a pensar mientras sale el texto.
- Vehiculos pasando por la carretera en ambos sentidos

### MG LaneRace
- SOLUCIONADO: un trigger nuevo para que actualice siempre la operacion, si pasa entre dos puertas no se contabiliza como error ni como acierto. PROBLEMA: Si pasa entre 2 puertas no se contabiliza y por tanto no se actualiza currentGround ni se cambia la operacion en CanvasManager, y a partir de ahi van todo retrasado una vez, no coincide operacion con resultados. Si amplio el collider de las gates para que esten pegados, podria pasar que pase por los 2, lo cual tambien seria un problema porque la accion se contabilizaria 2 veces. Otra opcion seria que el modelo de Player no sea hijo del padre, asi el modelo puede hacer la interpolacion entre 2 posiciones para que visualmente quede bien, mientras que el que ahora es el padre y tiene los collideres y todo cambie automaticamente de carril. El problema es que las animaciones se ven afectadas, porque el animator esta en el padre. Quizas se puedan poner en el modelo, y que el modelo herede del padre o algo asi
- HECHO: Cambiar la actualizacion del texto de la operacion del canvas al stage manager
- HECHO: Poner un terreno infinito de alguna forma
- Listar las 4 operaciones en Stage manager y que set opetarion coja una de ahi (gestionando correctamente las 3 primeras que se generan al mismo tiempo... posibles poblemas de paralela)
- Poner particulas brillantes al activarse los botones de correcto e incorrecto
- Activar particulas de humo al cruzar la puerta para hacerla desaparecer
- Poner arboles y flores en el suelo 
- ARREGLAR MOV DEL SUELO
- Transicion en velolociadad del suelo



