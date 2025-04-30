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

#### Sims (NPCs)

Cada sim tiene 3 necesidades

## Conclusiones


## Licencia

## Referencias
Los recursos de terceros utilizados son de uso público.
* Plantilla URP 3D, incluida en Unity 2022.3.40f1
* Behavior Designer 1.7.12, incluyendo Tutorials y Samples descargados desde la web de Opsive
