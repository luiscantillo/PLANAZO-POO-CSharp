# Parcial 2.

# PLANAZO - Sistema de Reservas de Eventos (POO en C#)

**Asignatura:** Electiva de Profundización I - 4BR
**Autor:** _Luis Miguel Cantillo Sarmiento_ — Ingeniería de Sistemas.

---

## 1. Descripción de la aplicación

PLANAZO es una empresa que organiza eventos de entretenimiento (cine, conciertos y talleres). Esta aplicación de consola permite:

- Ver los eventos disponibles con su precio base, su precio final y los cupos libres.
- Consultar el detalle de un evento por su ID.
- Reservar entradas para un evento validando la cantidad y los cupos.
- Ver todas las reservas realizadas.
- Cancelar reservas (devolviendo los cupos al evento).
- Ver un resumen con las reservas activas, las entradas reservadas y el valor total.
- Consultar las reservas de un usuario por su número de documento (reto 2).

El objetivo del proyecto era aplicar los conceptos de Programación Orientada a Objetos vistos en clase: clases, constructores, encapsulamiento, herencia, abstracción, polimorfismo, composición, colecciones y excepciones.

## 2. Funcionalidades implementadas

| Opción | Funcionalidad | Qué hace |
|:------:|---------------|----------|
| 1 | Ver eventos disponibles | Muestra una tabla con los 7 eventos, su precio base, su precio final (calculado por cada tipo) y los cupos. |
| 2 | Consultar evento | Pide el ID y muestra el detalle completo del evento. Si el ID no existe muestra un error y no se cierra. |
| 3 | Reservar entradas | Pide el documento (si el usuario es nuevo pide el nombre), el evento y la cantidad. Valida la cantidad y los cupos, muestra el total y pide confirmación. Al confirmar descuenta los cupos. |
| 4 | Ver reservas realizadas | Lista todas las reservas con su estado (Activa / Cancelada). |
| 5 | Cancelar una reserva | Cancela la reserva, devuelve los cupos al evento y muestra los cupos antes y después. No deja cancelar dos veces. |
| 6 | Ver resumen de reservas | Cantidad de reservas activas, canceladas, entradas reservadas y valor total de las activas. |
| 7 | Consultar reservas por usuario | **(Reto 2)** Busca un usuario por documento y muestra todas sus reservas. |
| 0 | Salir | Termina el programa. |

El programa inicia con **7 eventos**: 2 películas, 2 conciertos, 2 talleres y 1 festival (este último es el del reto 1).

| ID | Tipo | Evento | Precio base | Precio final |
|:--:|------|--------|------------:|-------------:|
| 1 | Cine | El Último Vallenato | $18.000 | $18.000 |
| 2 | Cine | Galaxia Perdida | $22.000 | $22.000 |
| 3 | Concierto | Noche de Salsa en el Malecón | $50.000 | $57.500 |
| 4 | Concierto | Rock en la Arenosa | $120.000 | $138.000 |
| 5 | Taller | Introducción a la Fotografía | $80.000 | $72.000 |
| 6 | Taller | Cocina Costeña Tradicional | $60.000 | $54.000 |
| 7 | Festival | Festival Sonidos del Caribe (3 días) | $90.000 | $216.000 |

## 3. Cómo ejecutar el proyecto

**Requisitos:** tener instalado el [.NET SDK 8](https://dotnet.microsoft.com/download) o superior.

```bash
# 1. Clonar el repositorio
git clone https://github.com/luiscantillo/PLANAZO-POO-CSharp.git

# 2. Entrar a la carpeta
cd PLANAZO-POO-CSharp

# 3. Ejecutar
dotnet run
```

> Si se abre en Visual Studio, basta con abrir el archivo `PLANAZO.csproj` y darle a ▶ Ejecutar.

## 4. Estructura del proyecto

```text
PLANAZO-POO-CSharp/
│
├── Program.cs                      → Punto de entrada (solo crea el gestor y el menú)
├── PLANAZO.csproj
│
├── Modelos/
│   ├── Evento.cs                   → Clase abstracta base
│   ├── Cine.cs                     → Hereda de Evento (precio base)
│   ├── Concierto.cs                → Hereda de Evento (+15 %)
│   ├── Taller.cs                   → Hereda de Evento (-10 %)
│   ├── Festival.cs                 → Hereda de Evento (Reto 1)
│   ├── Reserva.cs                  → Asocia un usuario con un evento
│   ├── Usuario.cs                  → Persona que reserva (Reto 2)
│   └── EstadoReserva.cs            → Enum: Activa / Cancelada
│
├── Servicios/
│   └── GestorReservas.cs           → Lógica de negocio y listas (List<T>)
│
├── Vistas/
│   └── MenuConsola.cs              → Menú, lectura de datos y mensajes
│
├── Utilidades/
│   ├── Entrada.cs                  → Lectura segura del teclado
│   ├── Pantalla.cs                 → Mensajes con colores, pausar, limpiar
│   └── Formato.cs                  → Formato de pesos colombianos ($57.500)
│
├── Excepciones/
│   └── EventoNoEncontradoException.cs → Excepción propia
│
├── Capturas/                       → Capturas de las pruebas
├── README.md
└── .gitignore
```

Tomé como base la estructura propuesta en el enunciado y le agregué las carpetas `Vistas`, `Utilidades` y `Excepciones` para que `Program.cs` quedara casi vacío y cada clase tuviera una sola responsabilidad: los **Modelos** solo guardan datos y sus reglas, el **Gestor** maneja la lógica, y el **Menú** solo se encarga de hablar con el usuario.

## 5. Clases desarrolladas y cómo se relacionan

| Clase | Responsabilidad |
|-------|-----------------|
| `Evento` (abstracta) | Tiene lo común de todos los eventos: `Id`, `Nombre`, `Fecha`, `PrecioBase`, `CapacidadMaxima` y `CuposDisponibles`. Define el método abstracto `CalcularPrecioFinal()` y los métodos `DescontarCupos()` y `DevolverCupos()`. |
| `Cine` | Hereda de `Evento`. Agrega `Sala` y `Clasificacion`. Su precio final es el mismo precio base. |
| `Concierto` | Hereda de `Evento`. Agrega `Artista`. Su precio final es precio base + 15 %. |
| `Taller` | Hereda de `Evento`. Agrega `Instructor` y `DuracionHoras`. Su precio final es precio base − 10 %. |
| `Festival` | **(Reto 1)** Hereda de `Evento`. Agrega `CantidadDias`. Precio final = (precio base × días) − 20 % por ser abono. |
| `Usuario` | **(Reto 2)** Identifica a la persona con `Documento` y `Nombre`, y guarda la lista de sus reservas. |
| `Reserva` | Asocia un `Usuario` con un `Evento`. Guarda `Id`, `NombrePersona`, `Cantidad`, `ValorTotal`, `FechaReserva` y `Estado`. Tiene el método `Cancelar()`. |
| `EstadoReserva` | Enum con los estados `Activa` y `Cancelada`. |
| `GestorReservas` | Contiene las listas `List<Evento>`, `List<Reserva>` y `List<Usuario>`. Carga los eventos iniciales, busca eventos, registra usuarios, crea y cancela reservas y calcula el resumen. |
| `MenuConsola` | Muestra el menú, pide los datos con `Entrada` y llama al `GestorReservas`. Atrapa las excepciones para que el programa no se cierre. |
| `EventoNoEncontradoException` | Excepción propia que se lanza cuando se busca un evento que no existe. |

**Relaciones:**

- **Herencia:** `Cine`, `Concierto`, `Taller` y `Festival` **son un** `Evento`.
- **Composición:** `GestorReservas` **contiene** las listas de eventos y reservas; él mismo crea los eventos en `CargarEventosIniciales()` y las reservas en `Reservar()`.
- **Asociación:** cada `Reserva` **tiene** un `Evento` y un `Usuario`. Un evento puede estar en muchas reservas y un usuario puede tener muchas reservas.
- **Agregación:** `Usuario` guarda la lista de sus `Reserva`.
- **Dependencia:** `MenuConsola` **usa** al `GestorReservas` (lo recibe en el constructor).

## 6. Diagrama de clases UML (Reto 3)

Un **diagrama de clases UML** es un diagrama que muestra las clases de un sistema con sus atributos, sus métodos y cómo se relacionan entre ellas (herencia, asociación, composición, etc). Sirve para entender el diseño del programa sin tener que leer todo el código. En el diagrama, `+` significa público, `-` privado, `#` protegido, y los métodos en *cursiva* son abstractos.

```mermaid
classDiagram
    direction TB

    class Evento {
        <<abstract>>
        -string nombre
        -decimal precioBase
        -int capacidadMaxima
        -int cuposDisponibles
        +int Id
        +string Nombre
        +DateTime Fecha
        +decimal PrecioBase
        +int CapacidadMaxima
        +int CuposDisponibles
        #Evento(id, nombre, fecha, precioBase, capacidadMaxima)
        +CalcularPrecioFinal() decimal*
        +ObtenerTipo() string*
        +DescontarCupos(cantidad) void
        +DevolverCupos(cantidad) void
        +TieneCupos() bool
        +ObtenerDetalle() string
    }

    class Cine {
        +string Sala
        +string Clasificacion
        +CalcularPrecioFinal() decimal
        +ObtenerTipo() string
        +ObtenerDetalle() string
    }

    class Concierto {
        -decimal PorcentajeRecargo = 0.15
        +string Artista
        +CalcularPrecioFinal() decimal
        +ObtenerTipo() string
        +ObtenerDetalle() string
    }

    class Taller {
        -decimal PorcentajeDescuento = 0.10
        +string Instructor
        +int DuracionHoras
        +CalcularPrecioFinal() decimal
        +ObtenerTipo() string
        +ObtenerDetalle() string
    }

    class Festival {
        -decimal DescuentoAbono = 0.20
        +int CantidadDias
        +CalcularPrecioFinal() decimal
        +ObtenerTipo() string
        +ObtenerDetalle() string
    }

    class Reserva {
        +int Id
        +Usuario Usuario
        +Evento Evento
        +int Cantidad
        +decimal ValorTotal
        +DateTime FechaReserva
        +EstadoReserva Estado
        +string NombrePersona
        +Reserva(id, usuario, evento, cantidad)
        +EstaActiva() bool
        +Cancelar() void
    }

    class Usuario {
        -string nombre
        -List~Reserva~ reservas
        +string Documento
        +string Nombre
        +IReadOnlyList~Reserva~ Reservas
        +Usuario(documento, nombre)
        +AgregarReserva(reserva) void
        +ContarReservasActivas() int
        +TotalGastado() decimal
    }

    class EstadoReserva {
        <<enumeration>>
        Activa
        Cancelada
    }

    class GestorReservas {
        -List~Evento~ eventos
        -List~Reserva~ reservas
        -List~Usuario~ usuarios
        -int siguienteIdReserva
        +GestorReservas()
        -CargarEventosIniciales() void
        +ObtenerEventos() IReadOnlyList~Evento~
        +BuscarEvento(idEvento) Evento
        +BuscarUsuario(documento) Usuario
        +RegistrarUsuario(documento, nombre) Usuario
        +ObtenerReservasDeUsuario(documento) IReadOnlyList~Reserva~
        +ValidarCantidad(evento, cantidad) void
        +Reservar(usuario, idEvento, cantidad) Reserva
        +ObtenerReservas() IReadOnlyList~Reserva~
        +BuscarReserva(idReserva) Reserva
        +CancelarReserva(idReserva) Reserva
        +ContarReservasActivas() int
        +ContarReservasCanceladas() int
        +TotalEntradasReservadas() int
        +ValorTotalReservasActivas() decimal
    }

    class MenuConsola {
        -GestorReservas gestor
        +MenuConsola(gestor)
        +Iniciar() void
    }

    class EventoNoEncontradoException {
        +int IdBuscado
    }

    Evento <|-- Cine
    Evento <|-- Concierto
    Evento <|-- Taller
    Evento <|-- Festival

    GestorReservas "1" *-- "many" Evento : contiene
    GestorReservas "1" *-- "many" Reserva : contiene
    GestorReservas "1" o-- "many" Usuario : registra
    Usuario "1" o-- "many" Reserva : tiene
    Reserva "many" --> "1" Evento : reserva
    Reserva --> EstadoReserva
    MenuConsola --> GestorReservas : usa
    GestorReservas ..> EventoNoEncontradoException : lanza
    Exception <|-- EventoNoEncontradoException
```

> GitHub dibuja el diagrama automáticamente porque está escrito en Mermaid. :D

## 7. Conceptos de POO aplicados

### Clases y objetos
Cada evento, reserva y usuario es un objeto. Por ejemplo, en `GestorReservas.CargarEventosIniciales()`:

```csharp
eventos.Add(new Concierto(3, "Noche de Salsa en el Malecón", new DateTime(2026, 11, 14, 20, 0, 0), 50000m, 300, "Orquesta La Sabrosura"));
```

### Constructores
Todas las clases inicializan sus datos en el constructor y validan que sean correctos. El constructor de `Evento` es `protected` porque solo lo pueden usar las clases hijas, y las hijas lo llaman con `: base(...)`:

```csharp
public Taller(int id, string nombre, DateTime fecha, decimal precioBase, int capacidadMaxima,
              string instructor, int duracionHoras)
    : base(id, nombre, fecha, precioBase, capacidadMaxima)
```

En `Reserva`, el constructor también calcula el `ValorTotal` y deja el estado en `Activa`.

### Encapsulamiento
- Los atributos son `private` y se acceden por **propiedades**. Los `set` son `private` y validan los datos (por ejemplo, el precio base no puede ser 0 o negativo).
- `CuposDisponibles` **solo tiene `get`**. La única forma de cambiar los cupos es con `DescontarCupos()` y `DevolverCupos()`, que revisan que no se pase de la capacidad ni quede negativo:

```csharp
public int CuposDisponibles
{
    get { return cuposDisponibles; }
}

public void DescontarCupos(int cantidad)
{
    if (cantidad <= 0)
        throw new ArgumentException("La cantidad de entradas debe ser mayor que cero.");
    if (cantidad > cuposDisponibles)
        throw new InvalidOperationException(...);

    cuposDisponibles -= cantidad;
}
```

- El `Estado` de la `Reserva` tiene `private set`, así que solo cambia con el método `Cancelar()`.
- Las listas del `GestorReservas` y del `Usuario` son privadas y hacia afuera se entregan como `IReadOnlyList` (con `AsReadOnly()`), para que nadie pueda agregar o borrar elementos sin pasar por el gestor.

### Abstracción
`Evento` es una clase `abstract`: no se puede hacer `new Evento(...)`, porque un "evento" en general no existe, siempre es un cine, un concierto, etc. Define **qué** deben hacer todos los eventos sin decir **cómo**:

```csharp
public abstract decimal CalcularPrecioFinal();
public abstract string ObtenerTipo();
```

### Herencia
`Cine`, `Concierto`, `Taller` y `Festival` heredan de `Evento` (`public class Concierto : Evento`). Así reciben el Id, nombre, fecha, precio, cupos y los métodos de cupos sin tener que volver a escribirlos, y solo agregan lo propio (artista, sala, instructor, días). También reutilizan `ObtenerDetalle()` llamando a `base.ObtenerDetalle()` y le agregan sus datos.

### Polimorfismo
Cada clase hija hace `override` de `CalcularPrecioFinal()` con su regla:

```csharp
// Cine
public override decimal CalcularPrecioFinal() { return PrecioBase; }

// Concierto
public override decimal CalcularPrecioFinal() { return PrecioBase + (PrecioBase * PorcentajeRecargo); }

// Taller
public override decimal CalcularPrecioFinal() { return PrecioBase - (PrecioBase * PorcentajeDescuento); }
```

Y en el resto del programa se llama el método sobre una variable de tipo `Evento`, sin saber de qué tipo es. Por ejemplo, en el constructor de `Reserva`:

```csharp
ValorTotal = evento.CalcularPrecioFinal() * cantidad;
```

o en `MenuConsola.VerEventos()`, donde se recorre la lista `List<Evento>` y cada uno devuelve un precio distinto con el mismo método.

### Composición y asociaciones
- `GestorReservas` crea y guarda los eventos y reservas (composición).
- `Reserva` tiene una referencia a un `Evento` y a un `Usuario` (asociación). Por eso cuando se cancela una reserva, ella misma le devuelve los cupos a su evento: `Evento.DevolverCupos(Cantidad);`
- `MenuConsola` recibe el `GestorReservas` por el constructor.

### Colecciones
Se usa `List<Evento>`, `List<Reserva>` y `List<Usuario>` en `GestorReservas`, y `List<Reserva>` en `Usuario`. Para buscar y sumar usé métodos de LINQ como `FirstOrDefault`, `Where`, `Count` y `Sum`.

### Excepciones
- Las clases lanzan excepciones cuando algo no es válido:
  - `ArgumentException` → cantidad menor o igual a cero, nombre vacío, precio inválido.
  - `InvalidOperationException` → no hay cupos suficientes, la reserva ya estaba cancelada.
  - `KeyNotFoundException` → la reserva o el usuario no existen.
  - `EventoNoEncontradoException` (propia) → el evento no existe.
- En `MenuConsola.Iniciar()` todo el `switch` está dentro de un `try/catch` que atrapa estas excepciones y muestra el mensaje en rojo, así **ningún error cierra el programa**.
- Para leer números uso `int.TryParse` dentro de un ciclo, así si el usuario escribe letras se le vuelve a pedir el dato.
- Todos los valores de dinero son `decimal`.

## 8. Casos de prueba

Todas las capturas están en la carpeta [`Capturas/`](Capturas/).

| # | Caso de prueba | Resultado esperado | Resultado | Captura |
|:-:|----------------|--------------------|:---------:|---------|
| 1 | Registrar una reserva válida | Se crea la reserva y se descuentan los cupos (300 → 298) | ✅ | `06_caso1_reserva_valida.png` |
| 2 | Reservar más entradas que los cupos | Error "No hay cupos suficientes", no se crea la reserva | ✅ | `07_caso2_supera_cupos.png` |
| 3 | Reserva con cantidad negativa | Error "La cantidad debe ser mayor que cero" | ✅ | `08_caso3_cantidad_negativa.png` |
| 4 | Cancelar y comprobar que se devuelven cupos | Los cupos del taller vuelven de 11 a 15 | ✅ | `12_...png`, `13_...png` |
| 5 | Cancelar dos veces la misma reserva | Error "La reserva #2 ya fue cancelada anteriormente" | ✅ | `14_caso5_cancelar_dos_veces.png` |
| 6 | Consultar un evento inexistente | Error "No existe ningún evento con el ID 99" | ✅ | `15_caso6_evento_inexistente.png` |
| 7 | Precio final por tipo de evento | Cine igual, Concierto +15 % (50.000 → 57.500), Taller −10 %, Festival (base × días) −20 % | ✅ | `02_...png`, `03_...png`, `04_...png`, `05_...png` |

### Menú principal
![Menú principal](Capturas/01_menu_principal.png)

### Caso 7 — Cada tipo de evento calcula su precio final
En la tabla se ve que con el mismo método `CalcularPrecioFinal()` cada tipo da un resultado diferente:

![Eventos disponibles](Capturas/02_eventos_disponibles_precios.png)

Concierto de $50.000 → $57.500 (el ejemplo del enunciado):

![Concierto](Capturas/03_consultar_concierto_57500.png)

Taller de $80.000 → $72.000:

![Taller](Capturas/04_consultar_taller.png)

### Caso 1 — Reserva válida
Juan Pérez (usuario nuevo) reserva 2 entradas para el concierto. Total $115.000 y los cupos bajan de 300 a 298.

![Reserva válida](Capturas/06_caso1_reserva_valida.png)

### Caso 2 — Más entradas que los cupos disponibles
El taller de fotografía tiene 15 cupos y se piden 20.

![Supera cupos](Capturas/07_caso2_supera_cupos.png)

### Caso 3 — Cantidad negativa
Primero se escribe `abc` (el programa vuelve a pedir el número) y luego `-3`.

![Cantidad negativa](Capturas/08_caso3_cantidad_negativa.png)

### Caso 4 — Cancelar una reserva y devolver los cupos
Antes de cancelar, María Gómez reservó 4 entradas del taller (los cupos quedaron en 11):

![Reserva taller](Capturas/09_reserva_taller_yesenia.png)

Todas las reservas hechas:

![Ver reservas](Capturas/11_ver_reservas.png)

Se cancela la reserva #2 y los cupos pasan de 11 a 15:

![Cancelar reserva](Capturas/12_caso4_cancelar_devuelve_cupos.png)

Al consultar el taller otra vez, tiene sus 15 cupos:

![Cupos restaurados](Capturas/13_caso4_cupos_restaurados.png)

### Caso 5 — Cancelar dos veces la misma reserva
![Cancelar dos veces](Capturas/14_caso5_cancelar_dos_veces.png)

### Caso 6 — Consultar un evento inexistente
![Evento inexistente](Capturas/15_caso6_evento_inexistente.png)

### Resumen de reservas
Reservas activas: #1 ($115.000) + #3 ($216.000) + #4 ($414.000) = **$745.000** y 2 + 1 + 3 = **6 entradas**. La #2 está cancelada y no se cuenta.

![Resumen](Capturas/16_resumen_reservas.png)

### Opción inválida en el menú
Si se escribe algo que no está en el menú, se muestra el error y el programa sigue funcionando.

![Opción inválida](Capturas/19_opcion_invalida.png)

## 9. Retos adicionales

### Reto 1 — Nuevo tipo de evento: `Festival` ✅
Creé la clase `Festival` que hereda de `Evento` y agrega `CantidadDias`. La entrada es un abono para todos los días con 20 % de descuento:

```csharp
public override decimal CalcularPrecioFinal()
{
    // Ej: 90.000 x 3 días = 270.000, menos 20 % = 216.000
    decimal precioTodosLosDias = PrecioBase * CantidadDias;
    return precioTodosLosDias - (precioTodosLosDias * DescuentoAbono);
}
```

Para integrarlo **no modifiqué** `Cine`, `Concierto`, `Taller`, `Reserva` ni los métodos del gestor; solo agregué la línea del festival en `CargarEventosIniciales()`. Las reservas, la cancelación y el resumen funcionaron sin cambiar nada porque todos trabajan con `Evento`.

![Festival](Capturas/05_consultar_festival_reto1.png)

### Reto 2 — Gestión de usuarios ✅
Creé la clase `Usuario` (documento + nombre + lista de reservas). Al reservar, el sistema pide el documento: si el usuario ya existe lo saluda, y si no, pide el nombre y lo registra cuando se confirma la reserva. Con la opción **7** se consultan todas las reservas de un usuario.

Juan Pérez ya existía, así que no se le vuelve a pedir el nombre:

![Usuario existente](Capturas/10_reserva_usuario_existente.png)

Reservas de Juan Pérez:

![Reservas por usuario](Capturas/17_reto2_reservas_por_usuario.png)

Documento que no está registrado:

![Usuario inexistente](Capturas/18_reto2_usuario_inexistente.png)

### Reto 3 — Diagrama UML ✅
Está en la [sección 6](#6-diagrama-de-clases-uml-reto-3).

## 10. Pregunta de sustentación

**¿Por qué utilicé una clase abstracta `Evento` en lugar de crear clases completamente independientes para Cine, Concierto y Taller? ¿Qué ventajas tiene esta decisión cuando necesitamos agregar un nuevo tipo de evento?**

Usé una clase abstracta porque los tres tipos de evento comparten casi todo: todos tienen Id, nombre, fecha, precio base, capacidad y cupos, y todos necesitan descontar y devolver cupos. Lo único que de verdad cambia entre ellos es cómo se calcula el precio final. Si hubiera hecho tres clases independientes, tendría que copiar y pegar los mismos atributos, las mismas validaciones y los métodos `DescontarCupos()` y `DevolverCupos()` tres veces. Si después encuentro un error en la validación de cupos, tendría que arreglarlo en tres partes y es fácil que se me olvide alguna.

Hice `Evento` **abstracta** (y no una clase normal) porque no tiene sentido crear un "evento" que no sea de ningún tipo, y porque así **obligo** a que cada clase hija implemente `CalcularPrecioFinal()`. Si creo una clase hija y se me olvida ese método, el programa ni siquiera compila.

La otra ventaja grande es que el resto del sistema trabaja con el tipo `Evento` y no con cada tipo por separado. Por ejemplo, el gestor guarda todo en una sola lista `List<Evento> eventos`, y la `Reserva` calcula el total así:

```csharp
ValorTotal = evento.CalcularPrecioFinal() * cantidad;
```

La reserva no sabe si es un cine o un concierto, solo llama el método y cada objeto responde con su propia regla (polimorfismo). Si las clases fueran independientes, necesitaría tres listas distintas o muchos `if (evento is Cine) ... else if (evento is Concierto) ...` en la reserva, el menú y el resumen.

**Cuando hay que agregar un tipo nuevo** esto se nota mucho. Eso fue exactamente lo que pasó con el reto 1: para agregar `Festival` solo tuve que:

1. Crear `Festival.cs` heredando de `Evento` y escribir su `CalcularPrecioFinal()`.
2. Agregar una línea en `CargarEventosIniciales()`:

```csharp
eventos.Add(new Festival(7, "Festival Sonidos del Caribe", new DateTime(2026, 12, 11, 15, 0, 0), 90000m, 1000, 3));
```

No toqué `Cine`, `Concierto`, `Taller`, `Reserva`, ni la lógica de reservas, cancelación o resumen, y todo siguió funcionando con el festival (se puede reservar, cancelar y aparece en el resumen). Es decir, el sistema se puede **extender sin modificar** lo que ya estaba funcionando, el código queda más ordenado, hay menos código repetido y hay menos riesgo de dañar algo.

---

_Parcial 2. Electiva de Profundización I. Luis Miguel Cantillo Sarmiento_