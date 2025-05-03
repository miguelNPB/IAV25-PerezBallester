# DOCUMENTO DE DISEÑO PROYECTO FINAL - Sims Ripoff
Miguel Norberto Pérez Ballester https://github.com/miguelNPB

# Descripción

Simulación de vida al estilo del videojuego los Sims, donde tendremos varios NPCs, cada uno con su personalidad característica. Los NPC tendrán que saciar sus necesidades de hambre, de ir al baño, de socializar y de dormir por la noche.  
El jugador también puede interactuar con ellos tirando rayos al suelo, asustando a los NPC cercanos, colocando lingotes de oro atrayendolos a mirar o moviendolos con el ratón y colocandolos en lugares distintos.

# Tecnologías utilizadas

Se ha utilizado Behavior Designer para diseñar los árboles de comportamiento de los NPC, y se ha utilizado los "Smart Objects"

# Diseño

#### Smart Objects
Los "Smart Objects" son objetos del entorno que contienen comportamiento para dar al NPC que decida actuar sobre ellos, manejan que animaciones reproducir, cuanto tiempo estar interactuando, que recompensar dar, etc.  
Por lo que sirve para abstraer la capa entre objetos del entorno y comportamiento del NPC, para que el NPC no tenga que conocer las formas de interactuar con todo el entorno.  
- Mis Smart Objects tienen varios factores a considerar:
    - Quien puede usarlo: Cada Smart Object sirve para una función designada y estan restringidos su uso a ciertas condiciones, por ejemplo el portátil solo lo accederá un NPC "nerd", o la nevera solo se podrá interactuar si tienes la barrita de hambre a la mitad.
    - Qué puede hacer: Cada Smart Object tiene declarados sus acciones, por ejemplo la nevera puede dar un snack pequeño (baja prioridad de hambre y regenera poca hambre) o puede dar una accion de cocinar algo (alta prioridad de hambre y regenera poca hambre), darán distintas animaciones y distintas recompensas según el que acceda a él.

#### Elementos del entorno

Cada actividad dura un cierto tiempo y restaura una cantidad de su barra de necesidad correspondiente. Una vez se empieza con una actividad, el sim tendrá que terminarla.
Las estaciones de respuesto de necesidades son Smart Objects que contienen varias actividades, el sim elegirá la estación en caso de necesitar algo de ella y una vez allí, elegirá la que más le beneficie.

#### Sims (NPCs)

Cada sim tiene 4 necesidades, comer, ir al baño, socializar y dormir.
Para cada sim distinto las necesidades bajan a ritmos distintos, cuando una necesidad baja del 75% la necesidad de ir a reponerla aumenta a 1, cuando baja del 50% al 2, y cuando baja del 25% al 3. 
El sim, si no está realizando ninguna tarea y está una estación de repuesto de necesidad disponible, la reserva y va hacia ella, hace la tarea y vuelve a su rutina.
En caso de no tener ninguna necesidad por debajo del 75%, cambia al modo ocio, donde se va a su estación favorita personal a pasar el rato, por ejemplo, para Agatha, es pintar en un lienzo.
En caso de que un sim no pueda reponer su necesidad con mayor prioridad, pasa a la siguiente con menos prioridad, y en caso de no poder reponer ninguna, va a su estación de ocio.
En el caso de tener como mayor necesidad socializar, el sim buscará a otro sim que tenga como mayor prioridad socializar, si no lo encuentra, ignorará la necesidad hasta que se pueda cumplir, y una vez lo encuentra, van juntos a una estación de socializar.

#### Interacción del jugador

El jugador puede clicar sobre cualquier sim para ver sus estadísticas y nombre.
- Mover: Al darle al 1 en el teclado, se activa el modo movimiento, donde puedes mover manualmente a un sim y ponerle a hacer cualquier actividad.
- Rayo: Al darle al 2 en el teclado, se activa el modo rayo, que donde clica el jugador cae un rayo que asusta a los sims cercanos, y en caso de darle a una estación de repuesto de necesidades la deshabilita durante un tiempo.
- Oro: Al darle al 3 en el teclado, se activa el modo colocar oro, que coloca un lingote de oro en el mapa que distrae al sim más cercano acercandolo a investigarlo durante cierto tiempo. Luego desaparece.

## Conclusiones


## Licencia

## Referencias
Los recursos de terceros utilizados son de uso público.
* Plantilla URP 3D, incluida en Unity 2022.3.40f1
* Behavior Designer 1.7.12, incluyendo Tutorials y Samples descargados desde la web de Opsive
