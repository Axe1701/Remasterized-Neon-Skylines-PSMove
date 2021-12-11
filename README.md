# Remasterized-Neon-Skylines-PSMove

Variación del juego Neon Skylines para realizar los movimientos de la nave con los mandos Move de Play Station 3 y la cámara PS3 Eye utilizando PSMove API y Unity Move para el curso de Interacción Humano Computador. Tambien se realizo una variacion Networking utilizando HLAPI y UNET de Unity para una propuesta multijugador cooperativa.


## Funcionalidades 📦
Movimientos PSMove:
  1. Arriba -> Mover los Controles Move hacia Atras
  2. Abajo -> Mover los Controles Move hacia Adelante
  3. Derecha -> Girar los controles Move hacia la Derecha
  4. Izquierda -> Girar los controles Move hacia la Izquierda

Dispositivo Movil (Captado por Voz):
  1. Arriba -> Destruir una torreta que aparezca por la parte de arriba del mapa.
  2. Abajo -> Destruir una torreta que aparezca por la parte de abajo del mapa.
  3. Derecha -> Destruir una torreta que aparezca por la parte de derecha del mapa.
  4. Izquierda -> Destruir una torreta que aparezca por la parte de izquierda del mapa.


## Instalación 📖
Para reutilizar el proyecto original, se descargo el repositorio de [cj2big](https://github.com/cj2big/Neon-Skylines) y se configuro el espacio de trabajo en la version de Unity 2019.4. 

Para conectar los mandos PSMove, se utilizo la API desarrollada por Thomas Perl-[thp](https://github.com/thp) (https://github.com/thp/psmoveapi), siguiendo los pasos descritos en su documentación. Cabe resaltar algo que no se menciona, que son las dependencias necesarias que se necesita para lograr seguir esos pasos. Es necesario trabajar con VisualStudio  2017  con  todas  las  SDK  de  Windows  8.1 instaladas. Por otra parte tambien se necesitaba las herramientas especificas  de  depuracion  y  testing  que  vienen  con  Python  y la libreria ”Numpy” para poder trabajar con OpenCV.

Finalmente, para la conexión de los mandos PSMove a Unity se utilizo la extension brindada por [CopenhagenGameCollective](https://github.com/CopenhagenGameCollective/UniMove) siguiendo los pasos descritos en el repositorio. Cabe resaltar que se utilizo una versión antigua donde se puedan utilizar los dll´s generados por la API del PSMove: "psmove.dll" y "psmove_tracker.dll", ya que, el dll propuesto en la ultima versión del proyecto "libpsmoveapi.dll" es no funcional.

Para el lado del Networking es necesario instalar la dependencia "com.unity.transport" del Package Manager.
Para el lado de las torretas se necesita el Asset "Starbugs" de la Assets Store.

## Resultados
![Resultados](https://github.com/Axe1701/Remasterized-Neon-Skylines-PSMove/HCI_final.mp4)

## Autores y Contacto✒️

* **Dennis Marcell Sumiri Fernandez** - [DennisMSF](https://github.com/dennisMSF)
* **José Miguel Guzmán Chauca** - [Axe1701](https://github.com/Axe1701)
