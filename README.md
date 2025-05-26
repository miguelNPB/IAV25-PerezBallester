# DOCUMENTO DE DISEÑO PROYECTO FINAL - Sims Ripoff
Miguel Norberto Pérez Ballester https://github.com/miguelNPB

# Descripción

Simulación de vida al estilo del videojuego los Sims, donde tendremos varios NPCs, cada uno con su personalidad característica. Los NPC tendrán que saciar sus necesidades de hambre, de ir al baño, de socializar y de dormir por la noche.  
El jugador también puede interactuar con ellos tirando rayos al suelo, asustando a los NPC cercanos, colocando lingotes de oro atrayendolos a mirar o moviendolos con el ratón y colocandolos en lugares distintos.

# Tecnologías utilizadas

Se ha utilizado Behavior Designer para diseñar los árboles de comportamiento de los NPC, y se ha utilizado los "Smart Objects"

# Punto de partida

De cero

# Diseño

#### Apartados

- A: Los sims buscan un SmartObject y rellenan sus necesidades si lo necesitan
- B: Se puede mover sims y colocar lingotes de oro que les atraen, y el rayo deshabilita y aturde sims y deshabilita SmartObjects
- C: Los sims elijen el SmartObject en función de su prioridad de necesidad, si no pueden rellenar la más prioritaria, van a la siguiente y así.
- D: Los sims pasan a modo ocio si tienen sus necesidades cubiertas y pasan a modo berrear si no pueden cubrirlas
- E: Los sims buscan a otros libres o con condiciones de socializar bajas para hacer las tareas de ocio

#### Smart Objects
Los "Smart Objects" son objetos del entorno que contienen comportamiento para dar al NPC que decida actuar sobre ellos, manejan que animaciones reproducir, cuanto tiempo estar interactuando, que recompensar dar, etc.  
Por lo que sirve para abstraer la capa entre objetos del entorno y comportamiento del NPC, para que el NPC no tenga que conocer las formas de interactuar con todo el entorno.  
- Mis Smart Objects tienen varios factores a considerar:
    - Quien puede usarlo: Cada Smart Object sirve para una función designada y estan restringidos su uso a ciertas condiciones, por ejemplo la nevera solo podrá ser elegida si el sim tiene hambre.
    - Qué puede hacer: Cada Smart Object tiene declarados sus acciones, por ejemplo la nevera puede dar un snack pequeño (baja prioridad de hambre y regenera poca hambre) o puede dar una accion de cocinar algo (alta prioridad de hambre y regenera poca hambre), darán distintas animaciones, duración de actividad y distintas recompensas según el que acceda a él.

Cada actividad dura un cierto tiempo y restaura una cantidad de su barra de necesidad correspondiente. Una vez se empieza con una actividad, el sim tendrá que terminarla.  
Los SmartObject pertenecen a uno de los 4 grupos de smartObjects correspondientes a cada necesidad. Los Sim acceden a ese grupo si necesitan cubirir esa necesidad, y el grupo les proporciona el mejor smartObject en función de su distancia y necesidad.  
El smartObject luego le proporciona su mejor actividad para suplirle, si tiene mucha hambre, por ejemplo, le dará la opción de cocinar en vez de la de coger un snack.

#### Sims (NPCs)

Cada sim tiene 4 necesidades, comer, ir al baño, socializar y dormir.
Para cada sim distinto las necesidades bajan a ritmos distintos, cuando una necesidad baja del 75% la necesidad de ir a reponerla aumenta a 1, cuando baja del 50% al 2, y cuando baja del 25% al 3. 
El sim, si no está realizando ninguna tarea y está una estación de repuesto de necesidad disponible, la reserva y va hacia ella, hace la tarea y vuelve a su rutina.
En caso de no tener ninguna necesidad por debajo del 75%, cambia al modo ocio, donde elige un sitio aleatorio de una lista de sitios a pasar el rato, por ejemplo, para Javier, es leer un libro.
En caso de que un sim no pueda reponer su necesidad con mayor prioridad, pasa a la siguiente con menos prioridad, y en caso de no poder reponer ninguna, va a su estación de ocio y en vez de hacer su actividad favorita, se pone a berrear en el suelo al no poder quedar satisfecho.
En el caso de tener como mayor necesidad socializar, el sim buscará a otro sim que tenga como mínimo 85% de necesidad de socializar y no esté realizando ninguna otra tarea, si no lo encuentra, ignorará la necesidad hasta que se pueda cumplir, y una vez lo encuentra, van juntos a una estación de socializar.

#### Interacción del jugador

El jugador puede clicar sobre cualquier sim para ver sus estadísticas y nombre.
- Seleccionar: Al no tener ningún modo activo, si clicas en un sim podrás ver sus barritas de necesidad y datos, y si clicas en un SmartObject puedes ver todas sus actividades con sus datos.
- Mover: Al darle al 1 en el teclado, se activa el modo movimiento, donde puedes mover manualmente al sim seleccionado, y en caso de clicar un SmartObject sin ocupar, lo ocupa y realiza la mejor actividad que pueda hacer en él.
- Oro: Al darle al 2 en el teclado, se activa el modo colocar oro, que coloca un lingote de oro en el mapa que distrae a los sims más cercanos acercandolos a investigarlo durante cierto tiempo. Luego desaparece.
- Rayo: Al darle al 3 en el teclado, se activa el modo rayo, que donde clica el jugador cae un rayo que inhabilita a los sim por 5 segundos y les baja 10% en todas sus necesidades, y en caso de darle a un SmartObject desocupado, lo deshabilita por 30 segundos.

## Controles

- Mover cámara: WASD  
- Rotar cámara: Clic izquierdo y arrastrar
- Activar modo seleccionar: No tener ningun modo activo
- Activar modo mover: 1
- Activar modo colocar oro: 2
- Activar modo rayo: 3

# Pruebas y métricas 
- Ver como los sims rellenan sus necesidades solos
- Ver que pasan a modo ocio cuando tienen todas sus necesidades repletas
- Ver que funcionan los controles de jugador para mover sims
- Ver que funciona el rayo para desactivar sims y SmartObjects
- Ver que funciona el lingote de oro
- Ver como hacen una tarea de socializar cuando 1 sim tiene menos de 75% de social y otro está disponible con menos de 85% de social
- Ver como los sims pasan a modo berrear cuando tienen una necesidad pendiente y no tienen estación disponible

Ver el video adjuntado en el repo ProyectoFinalIA_MiguelNorbertoPerezBallester.mp4

# Conclusiones

Ha sido divertido crear este simulador de los sims.
He notado que gracias al sistema de SmartObjects el juego es muchísimo más escalable que creandolo sin este sistema, y se podrían añadir nuevas necesidades, actividades y sims con muchísima facilidad.

## Licencia

Este proyecto incluye recursos bajo licencia Creative Commons Attribution 4.0 International (CC BY 4.0).

## Referencias
Los recursos de terceros utilizados son de uso público.
* Assets sacados de https://poly.pizza/, animados por mí.  
* Conocimiento de SmartObjects obtenido de https://www.gamedevpensieve.com/ai/ai_knowledge/ai_knowledge_smart-objects
* Plantilla URP 3D, incluida en Unity 2022.3.40f1
* Behavior Designer 1.7.12, incluyendo Tutorials y Samples descargados desde la web de Opsive
