**Indice**


- [Integrandes](#1)
    
- [Websockets](#2) 
    * [Descripcion del protocolo](#2-1)
        + [Apertura de apretón de manos](#2.1.1)
        + [Apretón de manos de cierre](#2.1.2)  
    * [Filosofia de diseño](#2.2)       
    * [Modelo de Seguridad](#2.3)        
    * [El protocolo Websockets](#2.4) 
        + [Subprotocolos](#2.4.1)
        + [Bit de aleta](#2.4.2)
        + [ Bits RSV1, RSV2, RSV3](#2.4.3)    
        + [Código de operación](#2.4.4) 
        + [Máscara](#2.4.5)
        + [Carga útil len](#2.4.5)
        + [Llave de enmascaramiento](#2.4.6)
        + [Datos de carga útil](#2.4.7) 
    * [Apertura de apretón de manos: Requisitos del cliente](#2.5)
    * [Requisitos del lado del servidor](#2.6)  
            
    * [Envío del protocolo de enlace de apertura del servidor](#2.7) 
            
    * [Cerrar una conexión de WebSocket: el protocolo de enlace de cierre de WebSocket](#2.8)                    
                
- [Requerimientos de instalación](#3)

- [Requerimientos para ejecutar el sistema](#3.1)    
    
- [Cómo ejecutar los componentes de cada servidor](#4)    
    
- [Cómo ejecutar el/los clientes](#5)
          
- [Documentación de un API de servicios ofrecidos por el Servidor.](#6)   
          
### 1 
### Integrantes
            
-Eric Ruiz Diaz

-Jorge Uriarte

-Florencia Viera

## 2          
### Websockets
Un WebSocket es una conexión persistente entre un cliente y un servidor. WebSockets proporciona un canal de comunicaciones bidireccional de dúplex completo que opera a través de HTTP a través de una única conexión de socket TCP / IP. En esencia, el protocolo WebSocket facilita el paso de mensajes entre un cliente y un servidor. Este artículo proporciona una introducción al protocolo WebSocket, incluido el problema que resuelve WebSockets, y una descripción general de cómo se describen los WebSockets a nivel de protocolo.

La idea de WebSockets surgió de las limitaciones de la tecnología basada en HTTP. Con HTTP, un cliente solicita un recurso y el servidor responde con los datos solicitados. HTTP es un protocolo estrictamente unidireccional: cualquier dato enviado desde el servidor al cliente debe ser solicitado primero por el cliente. El sondeo largo ha actuado tradicionalmente como una solución para esta limitación. Con el sondeo prolongado, un cliente realiza una solicitud HTTP con un período de tiempo de espera prolongado, y el servidor usa ese tiempo de espera prolongado para enviar datos al cliente. El sondeo largo funciona, pero tiene un inconveniente: los recursos del servidor están atados a lo largo del sondeo largo, incluso cuando no hay datos disponibles para enviar.
 
### 2-1          
#### Descripcion del protocolo

> El protocolo consiste en un apretón de manos de apertura seguido de un encuadre de mensaje básico, en capas sobre TCP.

Los WebSockets comienzan su vida como una solicitud y respuesta HTTP estándar. Dentro de esa cadena de respuesta de solicitud, el cliente solicita abrir una conexión WebSocket y el servidor responde (si puede). Si este protocolo de enlace inicial tiene éxito, el cliente y el servidor han acordado utilizar la conexión TCP / IP existente que se estableció para la solicitud HTTP como una conexión WebSocket. Los datos ahora pueden fluir a través de esta conexión utilizando un protocolo básico de mensajes enmarcados. Una vez que ambas partes reconocen que la conexión WebSocket debe cerrarse, la conexión TCP se rompe.

**Establecimiento de una conexión WebSocket: el protocolo de enlace abierto de WebSocket**
    
Los WebSockets no utilizan el esquema $http://$ o $https://$(porque no siguen el protocolo HTTP). Más bien, los URI de WebSocket utilizan un nuevo esquema **ws:(o wss:para un WebSocket seguro)**. El resto del URI es el mismo que un URI HTTP: un host, puerto, ruta y cualquier parámetro de consulta.
                
---

$$"ws:" "//" host [ ":" port ] path [ "?" query ]$$

$$"wss:" "//" host [ ":" port ] path [ "?" query ]$$

---
### 2.1.1     
#### Apertura de apretón de manos


   El apretón de manos de apertura está destinado a ser compatible con HTTP
   software e intermediarios del lado del servidor, de modo que se pueda
   utilizado por ambos clientes HTTP que hablan con ese servidor y WebSocket
   clientes hablando con ese servidor. Con este fin, el cliente de WebSocket
   handshake es una solicitud de actualización HTTP:

<p>
<pre> 
        OBTENER / chat HTTP / 1.1
        Anfitrión: server.example.com
        Actualización: websocket
        Conexión: actualización
        Sec-WebSocket-Key: dGhlIHNhbXBsZSBub25jZQ ==
        Origen: http://example.com
        Sec-WebSocket-Protocol: chat, superchat
        Sec-WebSocket-Versión: 13
          
</p>
</pre>           

   De acuerdo con [ RFC2616 ], los campos de encabezado en el protocolo de enlace pueden ser
   enviado por el cliente en cualquier orden, por lo que el orden en el que diferentes
   los campos de encabezado que se reciben no es significativo.

   El "Request-URI" del método GET [ RFC2616 ] se utiliza para identificar el
   punto final de la conexión WebSocket, ambos para permitir múltiples dominios
   para ser servido desde una dirección IP y permitir múltiples WebSocket
   puntos finales para ser atendidos por un solo servidor.

   El cliente incluye el nombre de host en | Host | campo de encabezado de su
   apretón de manos según [ RFC2616 ], de modo que tanto el cliente como el servidor
   puede verificar que están de acuerdo sobre qué host está en uso.
          
Los campos de encabezado adicionales se utilizan para seleccionar opciones en WebSocket
   Protocolo. Las opciones típicas disponibles en esta versión son las
   selector de subprotocolo (| Sec-WebSocket-Protocol |), lista de extensiones
   soporte por parte del cliente (| Sec-WebSocket-Extensions |), | Origin | encabezamiento
   campo, etc. El | Sec-WebSocket-Protocol | El campo de encabezado de solicitud puede ser
   utilizado para indicar qué subprotocols (protocolos de nivel de aplicación
   capas sobre el protocolo WebSocket) son aceptables para el cliente.
   El servidor selecciona uno o ninguno de los protocolos aceptables y repite
   ese valor en su apretón de manos para indicar que ha seleccionado que
   protocolo.

        **Sec-WebSocket-Protocol: chat**

   El | Origen | El campo de encabezado [ RFC6454 ] se utiliza para proteger contra
   uso no autorizado de origen cruzado de un servidor WebSocket por scripts que utilizan
   la API de WebSocket en un navegador web. Se informa al servidor de la
   origen del script que genera la solicitud de conexión de WebSocket. Si el
   servidor no desea aceptar conexiones de este origen, puede
   elija rechazar la conexión enviando un error HTTP apropiado
   código. Este campo de encabezado lo envían los clientes del navegador; para no navegador
   clientes, este campo de encabezado se puede enviar si tiene sentido en el
   contexto de esos clientes.

   Finalmente, el servidor tiene que demostrarle al cliente que recibió el
   Apretón de manos de WebSocket del cliente, para que el servidor no acepte
   conexiones que no son conexiones WebSocket. Esto evita una
   atacante de engañar a un servidor WebSocket enviándolo con cuidado
   paquetes elaborados mediante XMLHttpRequest [ XMLHttpRequest ] o un formulario
   sumisión.

   Para probar que se recibió el apretón de manos, el servidor debe tomar dos
   piezas de información y combinarlas para formar una respuesta. El primero
   pieza de información proviene de la | Sec-WebSocket-Key | campo de encabezado
   en el apretón de manos del cliente:

        **Sec-WebSocket-Key: dGhlIHNhbXBsZSBub25jZQ ==**

   Para este campo de encabezado, el servidor debe tomar el valor (como presente
   en el campo de encabezado, p. ej., la versión [ RFC4648 ] codificada en base64 menos
   cualquier espacio en blanco inicial y final) y concatenar esto con el
   Identificador único global (GUID, [ RFC4122 ]) "258EAFA5-E914-47DA-
   95CA-C5AB0DC85B11 "en forma de cadena, que es poco probable que sea utilizada por
   terminales de red que no comprenden el protocolo WebSocket. A
   Hash SHA-1 (160 bits) [ FIPS.180-3 ], codificado en base64 (consulte la Sección 4 de
   [RFC4648] ), de esta concatenación se devuelve en el servidor
   apretón de manos.

### 2.1.2          
#### Apretón de manos de cierre

  
   El apretón de manos de cierre es mucho más simple que el apretón de manos de apertura.

   Cualquiera de los pares puede enviar un marco de control con datos que contengan un
   secuencia de control para comenzar el apretón de manos de cierre (detallado en
   Sección 5.5.1 ). Al recibir dicha trama, el otro par envía un
   Cerrar fotograma en respuesta, si aún no ha enviado uno. Sobre
   recibiendo _ ese_ marco de control, el primer par luego cierra el
   conexión, con la certeza de que no hay más datos
   próximo.

   Después de enviar una trama de control que indica que la conexión debe ser
   cerrado, un par no envía más datos; después de recibir un
   marco de control que indica que la conexión debe estar cerrada, un par
   descarta cualquier dato adicional recibido.

   Es seguro para ambos compañeros iniciar este apretón de manos simultáneamente.

   El apretón de manos de cierre está destinado a complementar el cierre de TCP
   apretón de manos (FIN / ACK), sobre la base de que el apretón de manos de cierre de TCP es
   no siempre es fiable de principio a fin, especialmente en presencia de
   interceptar apoderados y otros intermediarios.

   Al enviar un cuadro de cierre y esperar un cuadro de cierre en respuesta,
   se evitan ciertos casos en los que se pueden perder datos innecesariamente. Para
   Por ejemplo, en algunas plataformas, si un socket se cierra con datos en el
   cola de recepción, se envía un paquete RST, que luego hará que recv ()
   fallar para la parte que recibió el RST, incluso si había datos
   esperando ser leído.
  
### 2.2       
#### Filosofía de diseño

    El protocolo WebSocket está diseñado según el principio de que debe
   ser un encuadre mínimo (el único encuadre que existe es hacer que el
   protocolo basado en tramas en lugar de basado en flujo y para admitir un
   distinción entre texto Unicode y marcos binarios). Se espera
   que la aplicación superpondría los metadatos a WebSocket       
          
    capa, de la misma manera que los metadatos se superponen sobre TCP por el
   capa de aplicación (por ejemplo, HTTP).

   Conceptualmente, WebSocket es en realidad solo una capa sobre TCP que
   hace lo siguiente:

   o agrega un modelo de seguridad basado en el origen web para navegadores

   o agrega un mecanismo de asignación de nombres de protocolo y direccionamiento para admitir
      múltiples servicios en un puerto y múltiples nombres de host en una IP
      Dirección

   o capas de un mecanismo de encuadre sobre TCP para volver a la IP
      Mecanismo de paquetes en el que se basa TCP, pero sin límites de longitud.

   o incluye un apretón de manos de cierre adicional en la banda que está diseñado
      trabajar en presencia de apoderados y otros intermediarios

   Aparte de eso, WebSocket no agrega nada. Básicamente está destinado a
   estar lo más cerca posible de exponer TCP sin procesar a la secuencia de comandos, dado el
   limitaciones de la Web. También está diseñado de tal manera que su
   los servidores pueden compartir un puerto con los servidores HTTP, al tener su protocolo de enlace
   ser una solicitud de actualización HTTP válida. Se podría utilizar conceptualmente otro
   protocolos para establecer la mensajería cliente-servidor, pero la intención de
   WebSockets es proporcionar un protocolo relativamente simple que puede
   coexistir con HTTP y la infraestructura HTTP implementada (como proxies)
   y eso es lo más cercano a TCP que es seguro para usar con tales
   infraestructura dadas las consideraciones de seguridad, con adiciones específicas
   para simplificar el uso y mantener las cosas simples simples (como la adición
   de semántica de mensajes).

   El protocolo está destinado a ser extensible; futuras versiones
   probablemente introduzca conceptos adicionales como multiplexación      
   
### 2.3         
####  Modelo de seguridad

   El protocolo WebSocket utiliza el modelo de origen utilizado por los navegadores web para
   restringir qué páginas web pueden contactar con un servidor WebSocket cuando el
   El protocolo WebSocket se utiliza desde una página web. Naturalmente, cuando el
   El protocolo WebSocket lo utiliza un cliente dedicado directamente (es decir, no
   desde una página web a través de un navegador web), el modelo de origen no es
   útil, ya que el cliente puede proporcionar cualquier cadena de origen arbitraria.

   Este protocolo está diseñado para no establecer una conexión con
   servidores de protocolos preexistentes como SMTP [ RFC5321 ] y HTTP, mientras
   Permitir que los servidores HTTP opten por admitir este protocolo si   
          
    deseado. Esto se logra con un estricto y elaborado apretón de manos.
   y limitando los datos que se pueden insertar en la conexión
   antes de que finalice el apretón de manos (lo que limita la cantidad
   puede ser influenciado).

   Asimismo, tiene la intención de no establecer una conexión cuando los datos
   desde otros protocolos, especialmente HTTP, se envía a un servidor WebSocket,
   por ejemplo, como podría suceder si se enviara un "formulario" HTML a un
   Servidor WebSocket. Esto se logra principalmente al exigir que el
   servidor demuestre que leyó el protocolo de enlace, lo cual solo puede hacer si el
   apretón de manos contiene las partes apropiadas, que sólo pueden ser enviadas por un
   Cliente WebSocket. En particular, en el momento de redactar este
   especificación, campos que comienzan con | Sec- | no puede ser configurado por un
   atacante desde un navegador web utilizando solo API HTML y JavaScript como
   como XMLHttpRequest [ XMLHttpRequest ].      


### 2.4
    
#### El protocolo Websockets

WebSocket es un protocolo enmarcado , lo que significa que un fragmento de datos (un mensaje) se divide en varios fragmentos discretos, con el tamaño del fragmento codificado en el marco. La trama incluye un tipo de trama, una longitud de carga útil y una parte de datos. En RFC 6455 se ofrece una descripción general de la trama y se reproduce aquí.


<p>
<pre>
 0                   1                   2                   3
 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
+-+-+-+-+-------+-+-------------+-------------------------------+
|F|R|R|R| opcode|M| Payload len |    Extended payload length    |
|I|S|S|S|  (4)  |A|     (7)     |             (16/64)           |
|N|V|V|V|       |S|             |   (if payload len==126/127)   |
| |1|2|3|       |K|             |                               |
+-+-+-+-+-------+-+-------------+ - - - - - - - - - - - - - - - +
|     Extended payload length continued, if payload len == 127  |
+ - - - - - - - - - - - - - - - +-------------------------------+
|                               |Masking-key, if MASK set to 1  |
+-------------------------------+-------------------------------+
| Masking-key (continued)       |          Payload Data         |
+-------------------------------- - - - - - - - - - - - - - - - +
:                     Payload Data continued ...                :
+ - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - +
|                     Payload Data continued ...                |
+---------------------------------------------------------------+
</pre>
</p>
### 2.4.1
 #### Subprotocolos que utilizan el protocolo WebSocket

   El cliente puede solicitar que el servidor utilice un subprotocolo específico mediante
   incluido el | Sec-WebSocket-Protocol | campo en su apretón de manos. Si se
   se especifica, el servidor debe incluir el mismo campo y uno de
   los valores del subprotocolo seleccionado en su respuesta para la conexión a
   estar establesido.

   Estos nombres de subprotocolo deben registrarse según la Sección 11.5 . Para
   evitar posibles colisiones, se recomienda utilizar nombres que
   contener la versión ASCII del nombre de dominio del subprotocolo
   autor. Por ejemplo, si Example Corporation creara una
   Subprotocolo de chat para ser implementado por muchos servidores en la Web,
   podrían llamarlo "chat.example.com". Si la organización de ejemplo
   llamó a su subprotocolo competidor "chat.example.org", luego los dos
   Los servidores pueden implementar subprotocolos simultáneamente, con la
   servidor seleccionando dinámicamente qué subprotocolo usar según el
   valor enviado por el cliente.

   Los subprotocols se pueden versionar de formas incompatibles con versiones anteriores mediante
   cambiar el nombre del subprotocolo, por ejemplo, pasando de
   "bookings.example.net" a "v2.bookings.example.net". Estas
   Los subprotocolos serían considerados completamente separados por WebSocket
   clientela. El control de versiones compatible con versiones anteriores se puede implementar
   reutilizando la misma cadena de subprotocolo pero diseñando cuidadosamente el
   subprotocolo real para soportar este tipo de extensibilidad.          
### 2.4.2         
#### Bit de aleta
El primer bit del encabezado de WebSocket es el bit Fin. Este bit se establece si esta trama es el último dato para completar este mensaje. 

### 2.4.3   
#### Bits RSV1, RSV2, RSV3
Estos bits están reservados para uso futuro.

### 2.4.4    
#### Código de operación
Cada cuadro tiene un código de operación que determina cómo interpretar los datos de carga útil de este cuadro.

<p>
<pre>

|Valor de código de operación|-->|Descripción|

0x00----------------->Esta trama continúa la carga útil de la trama anterior.

0x01----------------->Denota un marco de texto. Los marcos de texto son decodificados en UTF-8 por el servidor.

0x02----------------->Denota un marco binario. Las tramas binarias son entregadas sin cambios por el servidor.

0x03-0x07------------>Reservado para uso futuro.

0x08----------------->Indica que el cliente desea cerrar la conexión.

0x09----------------->Un marco de ping. Sirve como un mecanismo de latido que garantiza que la conexión sigue viva. El receptor debe responder con un pong.

0x0a----------------->Un cuadro de pong. Sirve como un mecanismo de latido que garantiza que la conexión sigue viva. El receptor debe responder con una trama de ping.

0x0b-0x0f------------>Reservado para uso futuro.

</pre>
</p>

### 2.4.5
#### Máscara

Establecer este bit en 1 habilita el enmascaramiento . Los WebSockets requieren que toda la carga útil se ofusque con una clave aleatoria (la máscara) elegida por el cliente. La clave de enmascaramiento se combina con los datos de la carga útil mediante una operación XOR antes de enviar datos a la carga útil. Este enmascaramiento evita que los cachés malinterpreten los marcos de WebSocket como datos que se pueden almacenar en caché. ¿Por qué deberíamos evitar el almacenamiento en caché de los datos de WebSocket? Seguridad.

Durante el desarrollo del protocolo WebSocket, se demostró que si se implementa un servidor comprometido y los clientes se conectan a ese servidor, es posible que los proxies intermedios o la infraestructura almacenen en caché las respuestas del servidor comprometido para que los futuros clientes que soliciten esos datos reciban la información. respuesta incorrecta. Este ataque se denomina envenenamiento de caché y es el resultado del hecho de que no podemos controlar cómo se comportan los proxies que se comportan mal en la naturaleza. Esto es especialmente problemático cuando se introduce un nuevo protocolo como WebSocket que tiene que interactuar con la infraestructura existente de Internet.

### 2.4.6
#### Carga útil len

El Payload lencampo y el Extended payload lengthcampo se utilizan para codificar la longitud total de los datos de carga útil para esta trama. Si los datos de la carga útil son pequeños (menos de 126 bytes), la longitud se codifica en el Payload lencampo. A medida que aumentan los datos de la carga útil, usamos los campos adicionales para codificar la longitud de la carga útil.

### 2.4.6
#### Llave de enmascaramiento

Como se discutió con el MASKbit, todas las tramas enviadas desde el cliente al servidor están enmascaradas por un valor de 32 bits que está contenido dentro de la trama. Este campo está presente si el bit de máscara se establece en 1 y está ausente si el bit de máscara se establece en 0.

### 2.4.7
#### Datos de carga útil

La Payload dataincluye datos de aplicaciones arbitrarias y cualquier dato de extensión que se ha negociado entre el cliente y el servidor. Las extensiones se negocian durante el protocolo de enlace inicial y le permiten extender el protocolo WebSocket para usos adicionales.

###  2.5
#### Apertura de apretón de manos: Requisitos del cliente

   Para _Establecer una conexión WebSocket_, un cliente abre una conexión
   y envía un apretón de manos como se define en esta sección. Una conexión es
   definido para estar inicialmente en un estado CONECTANDO. Un cliente necesitará
   proporcione un / host /, / puerto /, / nombre de recurso / y un indicador / seguro /, que
   son los componentes de un URI de WebSocket como se discutió en la Sección 3 ,
   junto con una lista de / protocolos / y / extensiones / que se utilizarán.
   Además, si el cliente es un navegador web, proporciona / origin /.



   Clientes que se ejecutan en entornos controlados, por ejemplo, navegadores en dispositivos móviles
   teléfonos atados a operadores específicos, PUEDEN descargar la gestión del
   conexión a otro agente en la red. En tal situación, el
   cliente para los propósitos de esta especificación se considera
   incluir tanto el software del teléfono como cualquier agente de este tipo.

   Cuando el cliente debe _Establecer una conexión WebSocket_ dado un conjunto
   of (/ host /, / port /, / resource name / y / secure / flag), junto con un
   lista de / protocolos / y / extensiones / que se utilizarán, y un / origen / en
   el caso de los navegadores web, DEBE abrir una conexión, enviar una apertura
   apretón de manos y leer el apretón de manos del servidor en respuesta. El exacto
   requisitos de cómo se debe abrir la conexión, qué se debe
   enviado en el apretón de manos de apertura, y cómo la respuesta del servidor debe
   ser interpretados son los siguientes en esta sección. En el siguiente
   texto, usaremos términos de la Sección 3 , como "/ host /" y
   "/ secure / flag" como se define en esa sección.

   1. Los componentes del URI de WebSocket pasados ​​a este algoritmo
       (/ host /, / port /, / resource name / y / secure / flag) DEBE ser
       válido según la especificación de los URI de WebSocket especificados
       en la Sección 3 . Si alguno de los componentes no es válido, el cliente
       DEBE _Fallar la conexión WebSocket_ y abortar estos pasos.

   2. Si el cliente ya tiene una conexión WebSocket al control remoto
       host (dirección IP) identificado por / host / y puerto / puerto / par, incluso
       si el host remoto es conocido por otro nombre, el cliente DEBE esperar
       hasta que se haya establecido esa conexión o para esa conexión
       haber fallado. No DEBE haber más de una conexión en un
       CONECTANDO estado. Si hay varias conexiones a la misma dirección IP
       se intentan simultáneamente, el cliente DEBE serializarlos para
       que no hay más de una conexión a la vez en ejecución
       a través de los siguientes pasos.

       Si el cliente no puede determinar la dirección IP del host remoto
       (por ejemplo, porque toda la comunicación se realiza a través de un
       servidor proxy que realiza consultas de DNS por sí mismo), luego el cliente
       DEBE asumir para los propósitos de este paso que cada nombre de host
       se refiere a un host remoto distinto y, en su lugar, el cliente DEBE
       limitar el número total de conexiones pendientes simultáneas a un
       número razonablemente bajo (p. ej., el cliente puede permitir
       conexiones pendientes a a.example.com y b.example.com, pero si
       Se solicitan treinta conexiones simultáneas a un solo host,
       que puede no estar permitido). Por ejemplo, en el contexto de un navegador web,
       el cliente debe considerar la cantidad de pestañas que el usuario tiene abiertas
       en el establecimiento de un límite al número de pendientes simultáneas
       conexiones.


       NOTA: Esto dificulta que un script realice una denegación de
       ataque de servicio con solo abrir una gran cantidad de WebSocket
       conexiones a un host remoto. Un servidor puede reducir aún más la
       carga sobre sí mismo cuando es atacado haciendo una pausa antes de cerrar el
       conexión, ya que eso reducirá la velocidad a la que el cliente
       se vuelve a conectar.

       NOTA: No hay límite para la cantidad de WebSocket establecidos
       conexiones que un cliente puede tener con un solo host remoto. Servidores
       puede negarse a aceptar conexiones de hosts / direcciones IP con un
       número excesivo de conexiones existentes o desconexión de recursos
       acaparando conexiones cuando sufren alta carga.

   3. _Uso de proxy_: si el cliente está configurado para usar un proxy cuando
       utilizando el protocolo WebSocket para conectarse al host / host / y al puerto
       / port /, entonces el cliente DEBE conectarse a ese proxy y preguntarle
       para abrir una conexión TCP al host dada por / host / y el puerto
       dado por / port /.

          EJEMPLO: Por ejemplo, si el cliente usa un proxy HTTP para todos
          tráfico, entonces si intentaba conectarse al puerto 80 en el servidor
          example.com, podría enviar las siguientes líneas al proxy
          servidor:

              CONECTAR example.com:80 HTTP / 1.1
              Anfitrión: example.com

          Si hubiera una contraseña, la conexión podría verse así:

              CONECTAR example.com:80 HTTP / 1.1
              Anfitrión: example.com
              Autorización de proxy: Básico ZWRuYW1vZGU6bm9jYXBlcyE =

       Si el cliente no está configurado para usar un proxy, entonces un TCP directo
       La conexión DEBE estar abierta al host proporcionado por / host / y el
       puerto dado por / port /.

       NOTA: Las implementaciones que no exponen la IU explícita para
       seleccionar un proxy para conexiones WebSocket separado de otros
       Se recomienda a los proxies que utilicen un proxy SOCKS5 [ RFC1928 ] para
       Conexiones WebSocket, si están disponibles, o en su defecto, para preferir
       el proxy configurado para conexiones HTTPS a través del proxy
       configurado para conexiones HTTP.

       Para el propósito de los scripts de autoconfiguración de proxy, el URI para
       pasar la función DEBE estar construida desde / host /, / port /,
       / nombre del recurso / y el indicador / seguro / utilizando la definición de un
       URI de WebSocket como se indica en la Sección 3 .

       NOTA: El protocolo WebSocket se puede identificar en proxy
       scripts de autoconfiguración del esquema ("ws" para sin cifrar
       conexiones y "wss" para conexiones cifradas).

   4. Si no se pudo abrir la conexión, ya sea porque un
       la conexión falló o porque cualquier proxy utilizado devolvió un error,
       entonces el cliente DEBE _Fail the WebSocket Connection_ y abortar
       el intento de conexión.

   5. Si / secure / es verdadero, el cliente DEBE realizar un protocolo de enlace TLS
       la conexión después de abrir la conexión y antes de enviar
       los datos del protocolo de enlace [ RFC2818 ]. Si esto falla (por ejemplo, el servidor
       certificado no se pudo verificar), entonces el cliente DEBE _Fail
       WebSocket Connection_ y anule la conexión. De lo contrario,
       toda la comunicación adicional en este canal DEBE pasar a través del
       túnel cifrado [ RFC5246 ].

       Los clientes DEBEN usar la extensión de indicación de nombre de servidor en el TLS
       apretón de manos [ RFC6066 ].

   Una vez que se ha establecido una conexión con el servidor (incluida una
   conexión a través de un proxy o sobre un túnel cifrado TLS), el cliente
   DEBE enviar un apretón de manos de apertura al servidor. El apretón de manos consiste
   de una solicitud de actualización HTTP, junto con una lista de
   campos de encabezado opcionales. Los requisitos para este apretón de manos son los
   sigue.

   1. El protocolo de enlace DEBE ser una solicitud HTTP válida según lo especificado por
        [ RFC2616 ].

   2. El método de la solicitud DEBE ser OBTENER, y la versión HTTP DEBE
        ser al menos 1.1.

        Por ejemplo, si el URI de WebSocket es "ws: //example.com/chat",
        la primera línea enviada debe ser "GET / chat HTTP / 1.1".

   3. La parte "Request-URI" de la solicitud DEBE coincidir con / resource
        nombre / definido en la Sección 3 (un URI relativo) o ser un valor absoluto
        http / https URI que, cuando se analiza, tiene un / nombre de recurso /, / host /,
        y / port / que coincidan con el URI ws / wss correspondiente.

   4. La solicitud DEBE contener un | Host | campo de encabezado cuyo valor
        contiene / host / plus opcionalmente ":" seguido de / port / (cuando no
        utilizando el puerto predeterminado).

   5. La solicitud DEBE contener una | Actualización | campo de encabezado cuyo valor
        DEBE incluir la palabra clave "websocket".


   6. La solicitud DEBE contener un | Conexión | campo de encabezado cuyo valor
        DEBE incluir el token de "Actualización".

   7. La solicitud DEBE incluir un campo de encabezado con el nombre
        | Sec-WebSocket-Key |. El valor de este campo de encabezado DEBE ser un
        nonce que consiste en un valor de 16 bytes seleccionado al azar que tiene
        ha sido codificado en base64 (consulte la Sección 4 de [RFC4648] ). El nonce
        DEBE seleccionarse al azar para cada conexión.

        NOTA: Como ejemplo, si el valor seleccionado al azar fue el
        secuencia de bytes 0x01 0x02 0x03 0x04 0x05 0x06 0x07 0x08 0x09
        0x0a 0x0b 0x0c 0x0d 0x0e 0x0f 0x10, el valor del encabezado
        el campo sería "AQIDBAUGBwgJCgsMDQ4PEC =="

   8. La solicitud DEBE incluir un campo de encabezado con el nombre | Origen |
        [ RFC6454 ] si la solicitud proviene de un cliente de navegador. Si
        la conexión es de un cliente que no es un navegador, la solicitud PUEDE
        incluir este campo de encabezado si la semántica de ese cliente coincide
        el caso de uso descrito aquí para clientes de navegador. El valor de
        este campo de encabezado es la serialización ASCII de origen del
        contexto en el que el código que establece la conexión es
        corriendo. Consulte [ RFC6454 ] para obtener detalles sobre cómo este campo de encabezado
        se construye el valor.

        Por ejemplo, si el código descargado de www.example.com intenta
        para establecer una conexión con ww2.example.com, el valor del
        el campo de encabezado sería "http://www.example.com".

   9. La solicitud DEBE incluir un campo de encabezado con el nombre
        | Versión Sec-WebSocket |. El valor de este campo de encabezado DEBE ser
        13.

        NOTA: Aunque las versiones preliminares de este documento (-09, -10, -11,
        y -12) se publicaron (en su mayoría se componían de editoriales
        cambios y aclaraciones y no cambios en el cable
        protocolo), los valores 9, 10, 11 y 12 no se utilizaron como válidos
        valores para Sec-WebSocket-Version. Estos valores estaban reservados en
        el registro de la IANA, pero no se utilizaron ni se utilizarán.

   10. La solicitud PUEDE incluir un campo de encabezado con el nombre
        | Protocolo Sec-WebSocket |. Si está presente, este valor indica uno
        o más subprotocol separados por comas que el cliente desea hablar,
        ordenados por preferencia. Los elementos que componen este valor
        DEBEN ser cadenas no vacías con caracteres en el rango U + 0021 a
        U + 007E sin incluir los caracteres separadores como se define en
        [ RFC2616 ] y DEBEN ser cadenas únicas. El ABNF para el
        El valor de este campo de encabezado es 1 # token, donde las definiciones de
        las construcciones y las reglas se dan en [ RFC2616 ].

   11. La solicitud PUEDE incluir un campo de encabezado con el nombre
        | Sec-WebSocket-Extensions |. Si está presente, este valor indica
        la (s) extensión (es) de nivel de protocolo que el cliente desea hablar. los
        La interpretación y el formato de este campo de encabezado se describen en
        Sección 9.1 .

   12. La solicitud PUEDE incluir cualquier otro campo de encabezado, por ejemplo,
        cookies [ RFC6265 ] y / o campos de encabezado relacionados con la autenticación
        como la | Autorización | campo de encabezado [ RFC2616 ], que son
        procesados ​​de acuerdo a los documentos que los definen.

   Una vez que se ha enviado el apretón de manos de apertura del cliente, el cliente DEBE
   espere una respuesta del servidor antes de enviar más datos.
   El cliente DEBE validar la respuesta del servidor de la siguiente manera:

   1. Si el código de estado recibido del servidor no es 101, el
       el cliente maneja la respuesta según los procedimientos HTTP [ RFC2616 ]. En
       en particular, el cliente puede realizar la autenticación si
       recibe un código de estado 401; el servidor puede redirigir al cliente
       utilizando un código de estado 3xx (pero los clientes no están obligados a seguir
       ellos), etc. De lo contrario, proceda de la siguiente manera.

   2. Si la respuesta carece de un | Actualizar | el campo de encabezado o el | Actualizar |
       El campo de encabezado contiene un valor que no es un caso ASCII.
       coincidencia insensible para el valor "websocket", el cliente DEBE
       _Falla de la conexión WebSocket_.

   3. Si la respuesta carece de | Conexión | campo de encabezado o el
       | Conexión | El campo de encabezado no contiene un token que sea un
       Coincidencia ASCII que no distingue entre mayúsculas y minúsculas para el valor "Actualizar", el cliente
       DEBE _Facultar la conexión WebSocket_.

   4. Si la respuesta carece de un | Sec-WebSocket-Accept | campo de encabezado o
       el | Sec-WebSocket-Accept | contiene un valor distinto del
       SHA-1 codificado en base64 de la concatenación de | Sec-WebSocket-
       Clave | (como una cadena, no decodificada en base64) con la cadena "258EAFA5-
       E914-47DA-95CA-C5AB0DC85B11 "pero ignorando cualquier
       espacio en blanco final, el cliente DEBE _Fail the WebSocket
       Conexión_.

   5. Si la respuesta incluye un | Sec-WebSocket-Extensions | encabezamiento
       campo y este campo de encabezado indica el uso de una extensión
       que no estaba presente en el protocolo de enlace del cliente (el servidor ha
       indicó una extensión no solicitada por el cliente), el cliente
       DEBE _Facultar la conexión WebSocket_. (El análisis de este
       El campo de encabezado para determinar qué extensiones se solicitan es
       discutido en la Sección 9.1 .)

   6. Si la respuesta incluye un | Sec-WebSocket-Protocol | campo de encabezado
       y este campo de encabezado indica el uso de un subprotocolo que fue
       no presente en el protocolo de enlace del cliente (el servidor ha indicado un
       subprotocolo no solicitado por el cliente), el cliente DEBE _Fail
       la conexión WebSocket_.

   Si la respuesta del servidor no se ajusta a los requisitos del
   Apretón de manos del servidor como se define en esta sección y en la Sección 4.2.2 ,
   el cliente DEBE _Facultar la conexión WebSocket_.

   Tenga en cuenta que de acuerdo con [ RFC2616 ], todos los nombres de campo de encabezado en
   tanto las solicitudes HTTP como las respuestas HTTP no distinguen entre mayúsculas y minúsculas.

   Si la respuesta del servidor se valida como se indica arriba, es
   dijo que _La conexión WebSocket está establecida_ y que el
   La conexión WebSocket está en estado ABIERTO. Las _Extensiones en uso_
   se define como una cadena (posiblemente vacía), cuyo valor es
   igual al valor de | Sec-WebSocket-Extensions | campo de encabezado
   proporcionado por el protocolo de enlace del servidor o el valor nulo si ese encabezado
   El campo no estaba presente en el protocolo de enlace del servidor. El _Subprotocol In
   Use_ se define como el valor de | Sec-WebSocket-Protocol |
   campo de encabezado en el protocolo de enlace del servidor o el valor nulo si ese
   El campo de encabezado no estaba presente en el protocolo de enlace del servidor.
   Además, si algún campo de encabezado en el protocolo de enlace del servidor indica
   que se deben configurar las cookies (como se define en [ RFC6265 ]), estas cookies
   se denominan _Cookies configuradas durante la apertura del servidor
   Apretón de manos_.

### 2.6    
#### Requisitos del lado del servidor

   Los servidores PUEDEN descargar la gestión de la conexión a otros agentes
   en la red, por ejemplo, equilibradores de carga y proxies inversos. En
   tal situación, el servidor a los efectos de esta especificación
   se considera que incluye todas las partes de la infraestructura del lado del servidor
   desde el primer dispositivo para terminar la conexión TCP hasta
   el servidor que procesa solicitudes y envía respuestas.

   EJEMPLO: Un centro de datos puede tener un servidor que responde a WebSocket
   solicita con un apretón de manos apropiado y luego pasa la conexión
   a otro servidor para procesar realmente las tramas de datos. Para el
   propósitos de esta especificación, el "servidor" es la combinación de
   ambas computadoras.



   Cuando un cliente inicia una conexión WebSocket, envía su parte del
   apretón de manos de apertura. El servidor debe analizar al menos parte de este
   apretón de manos con el fin de obtener la información necesaria para generar
   la parte del servidor del apretón de manos.

   El apretón de manos de apertura del cliente consta de las siguientes partes. Si
   el servidor, mientras lee el protocolo de enlace, encuentra que el cliente no
   no envíe un apretón de manos que coincida con la descripción a continuación (tenga en cuenta que como
   según [ RFC2616 ], el orden de los campos del encabezado no es importante),
   incluyendo pero no limitado a cualquier violación de la gramática ABNF
   especificado para los componentes del protocolo de enlace, el servidor DEBE detenerse
   procesar el protocolo de enlace del cliente y devolver una respuesta HTTP con un
   código de error apropiado (como 400 Bad Request).

   1. Una solicitud GET HTTP / 1.1 o superior, incluido un "URI de solicitud"
        [ RFC2616 ] que debe interpretarse como / nombre de recurso /
        definido en la Sección 3 (o un URI HTTP / HTTPS absoluto que contenga
        el / nombre del recurso /).

   2. A | Anfitrión | campo de encabezado que contiene la autoridad del servidor.

   3. Una | Actualización | campo de encabezado que contiene el valor "websocket",
        tratado como un valor ASCII que no distingue entre mayúsculas y minúsculas.

   4. A | Conexión | campo de encabezado que incluye el token "Actualización",
        tratado como un valor ASCII que no distingue entre mayúsculas y minúsculas.

   5. A | Sec-WebSocket-Key | campo de encabezado con codificación en base64 (ver
        Sección 4 de [RFC4648] ) valor que, cuando se decodifica, tiene 16 bytes en
        largo.

   6. A | Sec-WebSocket-Version | campo de encabezado, con un valor de 13.

   7. Opcionalmente, un | Origen | campo de encabezado. Este campo de encabezado se envía
        por todos los clientes del navegador. Un intento de conexión que carece de esto
        el campo de encabezado NO DEBE interpretarse como proveniente de un navegador
        cliente.

   8. Opcionalmente, un | Sec-WebSocket-Protocol | campo de encabezado, con una lista
        de valores que indican qué protocolos le gustaría al cliente
        hablar, ordenado de preferencia.

   9. Opcionalmente, un | Sec-WebSocket-Extensions | campo de encabezado, con un
        lista de valores que indican qué extensiones le gustaría al cliente
        hablar. Se discute la interpretación de este campo de encabezado.
        en la Sección 9.1 .

   10. Opcionalmente, otros campos de encabezado, como los que se usan para enviar
        cookies o solicitar autenticación a un servidor. Encabezado desconocido
        los campos se ignoran, según [ RFC2616 ].

### 2.7    
##### Envío del protocolo de enlace de apertura del servidor

   Cuando un cliente establece una conexión WebSocket a un servidor, el
   servidor DEBE completar los siguientes pasos para aceptar la conexión y
   enviar el apretón de manos de apertura del servidor.

   1. Si la conexión se realiza en un puerto HTTPS (HTTP-over-TLS),
       realice un protocolo de enlace TLS sobre la conexión. Si esto falla
       (por ejemplo, el cliente indicó un nombre de host en el cliente extendido
       hola extensión "server_name" que el servidor no aloja),
       luego cierre la conexión; de lo contrario, todas las comunicaciones posteriores
       para la conexión (incluido el protocolo de enlace del servidor) DEBE ejecutarse
       a través del túnel cifrado [ RFC5246 ].

   2. El servidor puede realizar una autenticación de cliente adicional, para
       ejemplo, devolviendo un código de estado 401 con el correspondiente
       | Autenticación WWW | campo de encabezado como se describe en [ RFC2616 ].

   3. El servidor PUEDE redirigir al cliente usando un código de estado 3xx
       [ RFC2616 ]. Tenga en cuenta que este paso puede ocurrir junto con, antes,
       o después del paso de autenticación opcional descrito anteriormente.

   4. Establezca la siguiente información:

       /origen/
          El | Origen | El campo de encabezado en el apretón de manos del cliente indica
          el origen del guión que establece la conexión. los
          el origen se serializa a ASCII y se convierte a minúsculas. los
          El servidor PUEDE utilizar esta información como parte de una determinación de
          si aceptar la conexión entrante. Si el servidor lo hace
          no validar el origen, aceptará conexiones de
          en cualquier sitio. Si el servidor no desea aceptar esto
          conexión, DEBE devolver un código de error HTTP apropiado
          (p. ej., 403 Forbidden) y anule el protocolo de enlace de WebSocket
          descrito en esta sección. Para obtener más detalles, consulte
          Sección 10 .

       /llave/
          El | Sec-WebSocket-Key | campo de encabezado en el apretón de manos del cliente
          incluye un valor codificado en base64 que, si se decodifica, es de 16 bytes
          en longitud. Este valor (codificado) se utiliza en la creación de
          el apretón de manos del servidor para indicar la aceptación del
          conexión. No es necesario que el servidor base64-
          decodificar el | Sec-WebSocket-Key | valor.

       /versión/
          La versión | Sec-WebSocket-Version | campo de encabezado en el cliente
          handshake incluye la versión del protocolo WebSocket con
          que el cliente está intentando comunicar. Si esto
          versión no coincide con una versión comprendida por el servidor, la
          servidor DEBE abortar el protocolo de enlace de WebSocket descrito en este
          sección y, en su lugar, envíe un código de error HTTP apropiado (como
          como se requiere actualización 426) y una versión | Sec-WebSocket | encabezamiento
          campo que indica la (s) versión (es) que el servidor es capaz de
          comprensión.

       /nombre del recurso/
          Un identificador del servicio proporcionado por el servidor. Si el
          servidor proporciona múltiples servicios, entonces el valor debe ser
          derivado del nombre del recurso dado en el apretón de manos del cliente
          en el "Request-URI" [ RFC2616 ] del método GET. Si el
          El servicio solicitado no está disponible, el servidor DEBE enviar un
          código de error HTTP apropiado (como 404 No encontrado) y abortar
          el protocolo de enlace de WebSocket.

       / subprotocolo /
          O un solo valor que representa el subprotocolo del servidor
          está listo para usar o es nulo. El valor elegido DEBE derivarse
          del apretón de manos del cliente, específicamente seleccionando uno de
          los valores del | Sec-WebSocket-Protocol | campo que el
          servidor está dispuesto a utilizar para esta conexión (si existe). Si el
          el apretón de manos del cliente no contenía tal campo de encabezado o si
          el servidor no está de acuerdo con ninguna de las solicitudes del cliente
          subprotocolos, el único valor aceptable es nulo. La ausencia
          de tal campo es equivalente al valor nulo (lo que significa que
          si el servidor no desea aceptar una de las sugerencias
          subprotocolos, NO DEBE devolver un | Sec-WebSocket-Protocol |
          campo de encabezado en su respuesta). La cadena vacía no es la
          mismo que el valor nulo para estos fines y no es un valor legal
          valor para este campo. El ABNF para el valor de este encabezado
          campo es (token), donde las definiciones de construcciones y
          las reglas se dan en [ RFC2616 ].

       / extensiones /
          Una lista (posiblemente vacía) que representa el nivel de protocolo
          extensiones, el servidor está listo para usar. Si el servidor admite
          múltiples extensiones, entonces el valor DEBE derivarse de la
          apretón de manos del cliente, específicamente seleccionando uno o más de
          los valores de | Sec-WebSocket-Extensions | campo. los
          la ausencia de dicho campo es equivalente al valor nulo. los
          cadena vacía no es lo mismo que el valor nulo para estos

         El protocolo WebSocket Diciembre de 2011


          propósitos. Las extensiones no enumeradas por el cliente NO DEBEN ser
          enumerados. El método por el cual se deben seleccionar estos valores
          e interpretado se discute en la Sección 9.1 .

   5. Si el servidor elige aceptar la conexión entrante, DEBE
       responda con una respuesta HTTP válida que indique lo siguiente.

       1. Una línea de estado con un código de respuesta 101 según RFC 2616 
           [ RFC2616 ]. Esta respuesta podría verse como "HTTP / 1.1 101
           Protocolos de conmutación ".

       2. Una | Actualización | campo de encabezado con valor "websocket" según RFC 
           2616 [ RFC2616 ].

       3. A | Conexión | campo de encabezado con valor "Actualizar".

       4. A | Sec-WebSocket-Accept | campo de encabezado. El valor de esto
           El campo de encabezado se construye concatenando / key /, definido
           arriba en el paso 4 en la Sección 4.2.2 , con la cadena "258EAFA5-
           E914-47DA-95CA-C5AB0DC85B11 ", tomando el hash SHA-1 de este
           valor concatenado para obtener un valor de 20 bytes y base64-
           codificación (consulte la Sección 4 de [RFC4648] ) este hash de 20 bytes.

           El ABNF [ RFC2616 ] de este campo de encabezado se define como
           sigue:

           Sec-WebSocket-Accept = valor-base64-no-vacío
           valor-base64-no-vacío = (1 * datos-base64 [relleno-base64]) |
                                    relleno base64
           base64-data = 4base64-character
           base64-padding = (2base64-character "==") |
                              (3base64-carácter "=")
           base64-character = ALPHA | DIGIT | "+" | "/"

   NOTA: Como ejemplo, si el valor de | Sec-WebSocket-Key | encabezamiento
   campo en el protocolo de enlace del cliente eran "dGhlIHNhbXBsZSBub25jZQ ==", el
   el servidor agregaría la cadena "258EAFA5-E914-47DA-95CA-C5AB0DC85B11"
   para formar la cadena "dGhlIHNhbXBsZSBub25jZQ == 258EAFA5-E914-47DA-95CA-
   C5AB0DC85B11 ". El servidor tomaría el hash SHA-1 de este
   cadena, dando el valor 0xb3 0x7a 0x4f 0x2c 0xc0 0x62 0x4f 0x16 0x90
   0xf6 0x46 0x06 0xcf 0x38 0x59 0x45 0xb2 0xbe 0xc4 0xea. Este valor
   luego está codificado en base64, para dar el valor
   "s3pPLMBiTxaQ9kYGzzhZRbK + xOo =", que se devolvería en el
   | Sec-WebSocket-Accept | campo de encabezado.

       5. Opcionalmente, un | Sec-WebSocket-Protocol | campo de encabezado, con un
           value / subprotocol / como se define en el paso 4 de la Sección 4.2.2 .
          
### 2.8          
### Cerrar una conexión de WebSocket: el protocolo de enlace de cierre de WebSocket

Para cerrar una conexión WebSocket, se envía un marco de cierre (código de operación 0x08). Además del código de operación, el marco de cierre puede contener un cuerpo que indique el motivo del cierre. Si cualquiera de los lados de una conexión recibe una trama cerrada, debe enviar una trama cerrada en respuesta y no se deben enviar más datos a través de la conexión. Una vez que ambas partes han recibido la trama cerrada, la conexión TCP se interrumpe. El servidor siempre inicia el cierre de la conexión TCP.

          
### 3          
### Requerimientos de Instalacion
          
- Visual Studio Code
          
- .NET 5.0 (SDK 5.0.400)
          
-  Node.js
          
-  npm 5.2.0 o npm v6.1          
          
- Nuxt.js          
          
### 3.1                    
### Requerimientos para ejecutar el sistema
          
 - Sistema Operativo Windows 10
          
 - Minimo 1 gb de ram
          
 - Almacenamiento SSD        
          

### 4          
### Cómo ejecutar los componentes de cada servidor
          
  En el caso del servidor, seria descargar el .net 5.0 y dentro de la carpeta del proyecto ejecutar un comando llamado dotnet run   

### 5          
### Cómo ejecutar el/los cliente        
          
     En el caso del cliente, debe descargar NPM y dentro de la carpeta del proyecto debe ejecutarse un comando llamado "npm update" y despues "npm run dev"     
          
### 6
## Documentación de un API de servicios ofrecidos por el Servidor.          
El nombre de los metodos que el cliente debe prepararse para invocar son

VerEstadoActual(), sin parametros, su resultado se escucha por el metodo EstadoRecibido, donde este ultimo devuelve una lista de Hospitales con sus respectivas Camas

CrearHospital(string nombreDelHospital), con un parametro que se usa para nombrar el nuevo hospital, su resultado se escucha por el metodo HospitalCreado, donde este ultimo devuelve un valor numerico que indica si se cargó con exito (0) el dato o si hubo algun error(-1)
 
Lo mismo va para EliminarHospital(int idDelHospital), con un parametro que se usa para borrar el id del hospital asignado,  donde este ultimo devuelve un valor numerico que indica si se borró con exito (0) el dato o si hubo algun error(-1)

Despues todo va exactamente lo mismo para las funciones CrearCama(int hospitalId), su resultado sale del metodo CamaCreada
 
EliminarCama(int camaId), su resultado sale del metodo CamaEliminada

[ OcuparCama(int camaId), su resultado sale del metodo CamaOcupada

DesocuparCama(int camaId), su resultado sale del metodo CamaDesocupada      
 
 
        
