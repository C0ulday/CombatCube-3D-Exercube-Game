# Koboldgames Primitives <!-- omit in toc -->
A set of primitive types exposed as shared variables via _[ScriptableObjects][scriptable object]_.
This library also contains useful helpers and predefined structures for extending upon the principle
of shared types.

## Table of Contents <!-- omit in toc -->
- [Description](#description)
  - [Architecture Principles](#architecture-principles)
    - [Indipendence of Systems](#indipendence-of-systems)
    - [Scenes as Clean Slates](#scenes-as-clean-slates)
    - [Self-contained Prefabs](#self-contained-prefabs)
    - [Focusing on Data](#focusing-on-data)
    - [Simple Modifyability of Logic](#simple-modifyability-of-logic)
  - [Additional Benefits](#additional-benefits)
- [Installation](#installation)
- [Usage](#usage)
  - [Shared Variables](#shared-variables)
    - [Creation](#creation)
    - [Basic Implementation](#basic-implementation)
    - [Specific Value Representation](#specific-value-representation)
    - [Read-Only Variables](#read-only-variables)
    - [Observable Variables](#observable-variables)
    - [Variable Wrappers](#variable-wrappers)
    - [Extendability](#extendability)
    - [Notes on Architecture and Performance](#notes-on-architecture-and-performance)
  - [Shared Collections](#shared-collections)
    - [Creation](#creation-1)
    - [Basic Implementation](#basic-implementation-1)
    - [Extendability](#extendability-1)
  - [Shared Events](#shared-events)
    - [Creation](#creation-2)
    - [Basic Implementation](#basic-implementation-2)
    - [Listener Components](#listener-components)
    - [Coroutines & Generators](#coroutines--generators)
    - [Extendability](#extendability-2)
  - [Lifetime and Cleanup](#lifetime-and-cleanup)
- [License](#license)

## Description
In the context of Unity's object oriented component-based workflow, a modular architecture for
sharing and accessing runtime data is important. This library introduces a way of decoupling data
from the actual logic in order to provide a clear and non-destructive way of communication between
components that suits a highly iterative workflow. This package establishes the concept of _shared
variables_ (as well as _shared collections_ and _shared events_): Containers that are based on
_[ScriptableObjects][scriptable object]_ and wrap (usually) a primitive type. As they are individual
assets, they can be referenced (and therefore be shared) between different components without them
needing to directly address each other. The key purposes of this concept are:

- Avoiding god-like singleton components (e.g. managers/services)
- Not needing to depend on object-oriented design patterns (producer/consumer, factories, static
  locality, etc.)
- Reasonable integration with the project workflow that Unity provides
- Providing a method for global state/data sharing where a streamlike data flow pattern is not
  possible or preferable

### Architecture Principles
The following code architecture and design principles shaped the layout of this library:

#### Indipendence of Systems
Systems and logic should not depend on each other. Minimizing hard-referencing between components
makes them independent and modular. A system should only transform data, doesn't matter where that
data comes from.

#### Scenes as Clean Slates
Reducing transient data streams between scenes. In Unity, scenes define what gets loaded into memory
and therefore setup the resources and data needed for the objects and components associated to the
scene. Systems and logic should work independent of the scene they're loaded in. Reducing the
appearance of _[Object.DontDestroyOnLoad][dontdestroyonload]_ to a minimum is preferable. There is
nothing worse than requiring a specific scene to be loaded to test or debug a component or system.
Sharing data between scene loads should be possible without rooting them in a scene definition.

#### Self-contained Prefabs
Prefabs usually serve as templates of a gameobject or component structure and hierarchy. As they are
stored as asset definitions, they can not save hard references to dynamic objects or objects in
scenes. Keeping them self-contained is important. It should be possible to instantiate any prefab in
any arbitrary scene or location without breaking logic.

This also benefits source-control: Scenes are basically just a list of prefabs/objects, and then
prefabs store the individual functionality. This naturally leads to fewer conflicts in scenes.

#### Focusing on Data
Separating data from logic makes it possible to edit and change data without messing with the
structure of a system.

#### Simple Modifyability of Logic
Having modular data independent of logic makes it easier to change behavior quickly without changing
the code. Every designer or artist should be able to create and/or test different data and
parameters for various components in the game, even on runtime.

### Additional Benefits
Using _[ScriptableObjects][scriptable object]_ as base for the shared variables has additional
advantages:
- They are serialized and can be viewed in the inspector which decouples their data
  from the source code or object they're used on. This makes it also easier to change game data for
  designers that have none or little notion of the inner workings of system code.
- They are treated as asset files. This makes it possible to (for example) structure them in the
  project and categorize them in folders/subfolders.
- They can easily be tracked by source-control and versioning tools.

## Installation
Install this package through Unity's [package manager] or by directly copying this repository into
the _Packages_ folder of your project.

## Usage
### Shared Variables
Shared variables are data containers based on _[ScriptableObjects][scriptable object]_ that wrap a
specific type and are intended to be stored globally as asset file in the project. A shared variable
mimics the behavior of its underlying type and can usually be implicitly converted to it.

This library already provides common variable types as listed below:

| Variable         | Underlying Type        |
| ---------------- | ---------------------- |
| SharedObject     | System.Object          |
| SharedBoolean    | System.Boolean         |
| SharedInteger    | System.Int32           |
| SharedLong       | System.Int64           |
| SharedFloat      | System.Single          |
| SharedDouble     | System.Double          |
| SharedString     | System.String          |
| SharedColor      | UnityEngine.Color      |
| SharedVector2    | UnityEngine.Vector2    |
| SharedVector2Int | UnityEngine.Vector2Int |
| SharedVector3    | UnityEngine.Vector3    |
| SharedVector3Int | UnityEngine.Vector3Int |
| SharedVector4    | UnityEngine.Vector4    |
| SharedQuaternion | UnityEngine.Quaternion |
| SharedRect       | UnityEngine.Rect       |
| SharedRectInt    | UnityEngine.RectInt    |
| SharedBounds     | UnityEngine.Bounds     |
| SharedBoundsInt  | UnityEngine.BoundsInt  |

> For each included type, there is also a read-only, as well as an observable version available.
> Have a look at [Read-Only Variables](#read-only-variables) and
> [Observable Variables](#observable-variables)...

A variable holds a value and a description field (both are serialized and displayed in the
inspector). Specific to the variable type, there can also be an initial value defined.

#### Creation
New shared variables can be created in a project through the `Create` menu:
```
Create > Koboldgames > Primitives > Variables > (...)
```
Shared variables can be created anywhere in the projects `Assets` folder.

#### Basic Implementation
Working with shared variables is straight forward:
```cs
using UnityEngine;
using Koboldgames.Primitives.Variables;

public class Demo : MonoBehaviour
{
    [SerializeField] private SharedFloat myFloat;

    private void Update()
    {
        // Measure 2 seconds, repeat

        myFloat.Value += Time.deltaTime;

        if(myFloat >= 2f)
            myFloat.Value -= 2f;
    }
}
```
As shared variables cast implicitly to their underlying type, math and comparation operators work
without explicitly accessing the `Value` property. Also, shared variables implement the non-generic
_[IComparable][icomparable]_ as well as the generic _[IComparable&lt;T&gt;][icomparable-1]_ and
_[IEquatable&lt;T&gt;][iequatable-1]_ interface for their specific underlying type.

> **Note:** Caution with value types especially booleans! As shared variables are objects and
> therefore of reference type, make sure you do not accidentally compare or validate the object
> reference instead of the underlying type.
> ```cs
> if(myBoolean)
> {
>     // This is bad!
> }
>
> if(myBoolean.Value)
> {
>     // This is fine!
> }
>
> if(myBoolean == true)
> {
>     // This is fine too!
> }
> ```

#### Specific Value Representation

Some of the provided shared variables implement one of the following interfaces: `IStateVariable`,
`INumericVariable` or `IStringVariable`. These interfaces categorize the different variables and
define properties that access the variables value by trying to convert it to a specified type.

This can be useful if the underlying type of a shared variable is uncertain on runtime.

**`IStateVariable`**<br>
| Properties | Return Type    |
| ---------- | -------------- |
| StateValue | System.Boolean |

**`INumericVariable`**<br>
| Properties  | Return Type   |
| ----------- | ------------- |
| IntValue    | System.Int32  |
| LongValue   | System.Int64  |
| FloatValue  | System.Single |
| DoubleValue | System.Double |

**`IStringVariable`**<br>
| Properties  | Return Type   |
| ----------- | ------------- |
| StringValue | System.String |

> **Note:** Properties of these interfaces are implemented by calls to the _[Convert][convert]_
> class. The overhead of retrieving data from shared variables using this approach is quite high.
> If the underlying type of a shared variable is known or the value needs to be accessed very
> frequently, getting it by calling the `Value` property should always be prefered!

#### Read-Only Variables
Shared read-only variables are a different variant of shared variables which values remain
unchanged on runtime. The variables `Value` property does not implement a `set` accessor. They also
do not use an additional field specifying an initial value or a `Reset()` method.

Shared read-only variables inherit from the same base class as standard shared variables:
`SharedVariableBase<T>`.

#### Observable Variables
Shared observable variables are different variant of shared variables which can dynamically
react to changes. The variables contain an `OnChange` event (`System.Action<T>` delegate) that fires
immediately after the variables value changes.

```cs
using UnityEngine;
using Koboldgames.Primitives.Variables;

public class Demo : MonoBehaviour
{
    [SerializeField] private SharedObservableInteger myInt;

    private void OnEnable() => myInt.OnChange += EventHandler;
    private void OnDisable() => myInt.OnChange -= EventHandler;

    private void Update()
    {
        // Press <space> to increment value
        if(Input.GetKeyDown(KeyCode.Space))
            myInt.Value++;
    }

    private void EventHandler(int newValue)
    {
        Debug.Log($"The value changed to: {newValue}!");
    }
}
```

Shared observable variables inherit from the same base class as standard shared variables:
`SharedVariableBase<T>`.

> **Note:** As observable shared variables always need to compare changes for equality and
> potentially call a list of delegates (of unknown behavior and size), their `Value` property can
> not be inlined which destroys efficient field accessability and caching. Be aware of this
> significant overhead and consider doing heavy frequent operations on cache variables (copies) and
> applying results to the shared variable afterwards.
>
> Also, overusing observable variables for a reactive code structure might lead to a more obscure
> and less manageable chain of responsibility. It is not always clear what the event handlers will
> do and how much performance a single variable change actually costs.

#### Variable Wrappers
Wrappers are small serializeable classes that are able to wrap different types of shared variables
and provide a more convenient structure for working with them. These wrappers are drawn using
specialized [property drawers] in the Unity inspector.

The wrapper types, similar to the shared variables itself, mimic their underlying type as well as
implement the non-generic _[IComparable][icomparable]_ as well as the generic
_[IComparable&lt;T&gt;][icomparable-1]_ and _[IEquatable&lt;T&gt;][iequatable-1]_ interface.

| Dynamic Variables | Flexible Variables |
| ----------------- | ------------------ |
| DynamicObject     | FlexibleObject     |
| DynamicBoolean    | FlexibleBoolean    |
| DynamicInteger    | FlexibleInteger    |
| DynamicLong       | FlexibleLong       |
| DynamicFloat      | FlexibleFloat      |
| DynamicDouble     | FlexibleDouble     |
| DynamicString     | FlexibleString     |
| DynamicColor      | FlexibleColor      |
| DynamicVector2    | FlexibleVector2    |
| DynamicVector2Int | FlexibleVector2Int |
| DynamicVector3    | FlexibleVector3    |
| DynamicVector3Int | FlexibleVector3Int |
| DynamicVector4    | FlexibleVector4    |
| DynamicQuaternion | FlexibleQuaternion |
| DynamicRect       | FlexibleRect       |
| DynamicRectInt    | FlexibleRectInt    |
| DynamicBounds     | FlexibleBounds     |
| DynamicBoundsInt  | FlexibleBoundsInt  |

**`Dynamic(...)` Wrappers:**<br>
Dynamic variable wrappers can hold a shared variable of any variation. It does not matter if the
variable is read-only, observable or just a standard shared variable. This makes it a little easier
to handle them in code.

**`Flexible(...)` Wrappers:**<br>
Flexible variable wrappers can hold a shared as well as a local variable. This can be really handy
if currently there is no point in sharing a serialized variable on an object. As soon as
requirements change a shared variable can be assigned without adapting source code.

```cs
using UnityEngine;
using Koboldgames.Primitives.Variables;

public class Demo : MonoBehaviour
{
    // Does not matter what kind of shared integer variable gets assigned
    [SerializeField] private DynamicInteger myInt;

    // Underlying value could also be a local per-component serialized variable
    [SerializeField] private FlexibleString myString;

    private void Start()
    {
        // Display values
        Debug.Log(myInt.Value);
        Debug.Log(myString.Value);

        // Display type of wrapped shared variables
        Debug.Log(myInt.SharedVariable?.GetType());
        Debug.Log(myString.SharedVariable?.GetType());

        // Display the value of the local and the shared variable of the flexible wrapper
        Debug.Log(myString.LocalVariable);
        Debug.Log(myString.SharedVariable?.Value);
    }
}
```

#### Extendability
It is possible extend this library with additional customized shared variables. The simplest way to
extend on their concept is by creating a class that inherits from one of the following base classes.

| Variable Base                           |
| --------------------------------------- |
| SharedVariableBase&lt;T&gt;             |
| &#10149; SharedStateVariable&lt;T&gt;   |
| &#10149; SharedNumericVariable&lt;T&gt; |
| &#10149; SharedStringVariable&lt;T&gt;  |

The generic type will be the type of the wrapped underlying value of the variable.

> **Note:** The base classes `SharedStateVariable<T>`, `SharedNumericVariable<T>`,
> `SharedStringVariable<T>` already implement the corresponding `IStateVariable`,
> `INumericVariable` or `IStringVariable` interface. They also inherit from `SharedVariableBase<T>`.

When inheriting from the abstract base class, the the generic
_[IComparable&lt;T&gt;][icomparable-1]_ and _[IEquatable&lt;T&gt;][iequatable-1]_ interfaces remain
to be implemented.

The structure and functionality of the variable can be designed as desired. For following the
architecture of the variables in this library, default interfaces that control the read and write
accessibility can be used: `IReadableVariable<T>` and `IWritableVariable<T>`.

```cs
public enum SomeEnum
{
    ItemOne,
    ItemTwo,
    ItemThree
}

[CreateAssetMenu(fileName = "SharedCustomEnum.asset", menuName = "Custom Primitives/Some Enum", order = 100)]
public class SharedCustomEnum : SharedVariableBase<SomeEnum>, IReadableVariable<SomeEnum>, IWriteableVariable<SomeEnum>
{
    // Accessing the value of this variable
    public SomeEnum Value
    {
        get { return value; }
        set { this.value = value; }
    }

    // Reset this variable
    public void Reset() => value = SomeEnum.ItemOne;

    // Make custom enum comparable
    public override int CompareTo(SomeEnum other) => ((int)value).CompareTo((int)other);

    // Make custom enum equatable
    public override bool Equals(SomeEnum other) => value == other;

    // Reset on enable...
    private void OnEnable() => Reset();
}
```

#### Notes on Architecture and Performance
In general, shared variables are designed so that they behave like simple variable wrappers. The
level of inheritance and nested behavior is not as deep for the most relevant aspects of the class
as it could be. The `Value` property for example, is implemented strongly typed only in the last
child (therefore the specialized class that eventually defines the variable) and serves as a direct
accessor to the `value` field of the base. This lets the call to be inlined by the compiler (except
for special cases like wrappers or observable shared variables) and sometimes eliminates
cache-thrashing on frequent access calls.

Keeping this in mind, in scenarios where a variable gets read or written to very often (e.g. doing
math operations), it might benefit to cache the value in a local variable and later apply it back to
the shared variable.

> **Note:** This is especially true for shared variable wrapper objects and observable variables.

### Shared Collections
Shared collections are data containers based on _[ScriptableObjects][scriptable object]_ that wrap a
generic collection (e.g. `List<T>`) and are intended to be stored globally as asset file in the
project. A shared collection mimics the behavior of its wrapped collection by implementing the same
interfaces and forwarding methods and properties.

This library already provides common generic list collections as listed below:

| Collection           | Underlying Type                                                |
| -------------------- | -------------------------------------------------------------- |
| SharedObjectList     | System.Collections.Generics.List&lt;System.Object&gt;          |
| SharedBooleanList    | System.Collections.Generics.List&lt;System.Boolean&gt;         |
| SharedIntegerList    | System.Collections.Generics.List&lt;System.Int32&gt;           |
| SharedLongList       | System.Collections.Generics.List&lt;System.Int64&gt;           |
| SharedFloatList      | System.Collections.Generics.List&lt;System.Single&gt;          |
| SharedDoubleList     | System.Collections.Generics.List&lt;System.Double&gt;          |
| SharedStringList     | System.Collections.Generics.List&lt;System.String&gt;          |
| SharedColorList      | System.Collections.Generics.List&lt;UnityEngine.Color&gt;      |
| SharedVector2List    | System.Collections.Generics.List&lt;UnityEngine.Vector2&gt;    |
| SharedVector2IntList | System.Collections.Generics.List&lt;UnityEngine.Vector2Int&gt; |
| SharedVector3List    | System.Collections.Generics.List&lt;UnityEngine.Vector3&gt;    |
| SharedVector3IntList | System.Collections.Generics.List&lt;UnityEngine.Vector3Int&gt; |
| SharedVector4List    | System.Collections.Generics.List&lt;UnityEngine.Vector4&gt;    |
| SharedQuaternionList | System.Collections.Generics.List&lt;UnityEngine.Quaternion&gt; |
| SharedRectList       | System.Collections.Generics.List&lt;UnityEngine.Rect&gt;       |
| SharedRectIntList    | System.Collections.Generics.List&lt;UnityEngine.RectInt&gt;    |
| SharedBoundsList     | System.Collections.Generics.List&lt;UnityEngine.Bounds&gt;     |
| SharedBoundsIntList  | System.Collections.Generics.List&lt;UnityEngine.BoundsInt&gt;  |

Different types of collections can be created by inheriting of a specific shared collection base
class. See the [Extendability](#extendability-1) section for further details. The library provides
implementations of _[List&lt;T&gt;][list]_, _[Dictionary&lt;TKey, TValue&gt;][dictionary]_ and
_[HashSet&lt;T&gt;][hashset]_.

#### Creation
New shared collections can be created in a project through the `Create` menu:
```
Create > Koboldgames > Primitives > Collections > (...)
```
Shared collections can be created anywhere in the projects `Assets` folder.

#### Basic Implementation
Working with shared collections is straight forward:
```cs
using UnityEngine;
using Koboldgames.Primitives.Collections;

public class Demo : MonoBehaviour
{
    [SerializeField] private SharedIntegerList myList;

    private void Start()
    {
        myList.Clear();

        // Add 1024 items to the list
        for(int i = 0; i < 1024 i++)
            myList.Add(i * i);

        // Print index of the element with a value of 1764
        Debug.Log(myList.IndexOf(1764));
    }
}
```
As shared collections implement all the interfaces of the wrapped collection type, they can be used
likewise.

#### Extendability
This library can and sometimes should be extended with additional customized shared collections.
The following base classes provide functionality for specific types of collections:

| Collection Base                          | Wrapped Type                                               |
| ---------------------------------------- | ---------------------------------------------------------- |
| SharedListBase&lt;T&gt;                  | System.Collections.Generics.List&lt;T&gt;                  |
| SharedDictionaryBase&lt;TKey, TValue&gt; | System.Collections.Generics.Dictionary&lt;TKey, TValue&gt; |
| SharedHashSetBase&lt;T&gt;               | System.Collections.Generics.HashSet&lt;T&gt;               |

```cs
[CreateAssetMenu(fileName = "SharedInventory.asset", menuName = "Custom Primitives/Inventory", order = 100)]
public class SharedInventory : SharedDictionaryBase<int, UnityEngine.Object>
{
    // No more functionality needed, but of course can be added...
    // As it is, this is already a dictionary
}
```

### Shared Events
Shared events are reactive event wrappers based on _[ScriptableObjects][scriptable object]_ that
use an underlying action delegate and are intended to be stored globally as asset file in the
project.

This library already provides common event wrappers as listed below:

| Event                 | Underlying Delegate                         |
| --------------------- | ------------------------------------------- |
| SharedEvent           | System.Action                               |
| SharedObjectEvent     | System.Action&lt;System.Object&gt;          |
| SharedBooleanEvent    | System.Action&lt;System.Boolean&gt;         |
| SharedIntegerEvent    | System.Action&lt;System.Int32&gt;           |
| SharedLongEvent       | System.Action&lt;System.Int64&gt;           |
| SharedFloatEvent      | System.Action&lt;System.Single&gt;          |
| SharedDoubleEvent     | System.Action&lt;System.Double&gt;          |
| SharedStringEvent     | System.Action&lt;System.String&gt;          |
| SharedColorEvent      | System.Action&lt;UnityEngine.Color&gt;      |
| SharedVector2Event    | System.Action&lt;UnityEngine.Vector2&gt;    |
| SharedVector2IntEvent | System.Action&lt;UnityEngine.Vector2Int&gt; |
| SharedVector3Event    | System.Action&lt;UnityEngine.Vector3&gt;    |
| SharedVector3IntEvent | System.Action&lt;UnityEngine.Vector3Int&gt; |
| SharedVector4Event    | System.Action&lt;UnityEngine.Vector4&gt;    |
| SharedQuaternionEvent | System.Action&lt;UnityEngine.Quaternion&gt; |
| SharedRectEvent       | System.Action&lt;UnityEngine.Rect&gt;       |
| SharedRectIntEvent    | System.Action&lt;UnityEngine.RectInt&gt;    |
| SharedBoundsEvent     | System.Action&lt;UnityEngine.Bounds&gt;     |
| SharedBoundsIntEvent  | System.Action&lt;UnityEngine.BoundsInt&gt;  |

#### Creation
New shared events can be created in a project through the `Create` menu:
```
Create > Koboldgames > Primitives > Events > (...)
```
Shared events can be created anywhere in the projects `Assets` folder.

#### Basic Implementation
Working with shared events is straight forward:
```cs
using UnityEngine;
using Koboldgames.Primitives.Events;

public class Demo : MonoBehaviour
{
    [SerializeField] private SharedEvent myEvent;

    private void OnEnable()
    {
        myEvent += Handler;
    }

    private void OnDisable()
    {
        myEvent -= Handler;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            myEvent.Invoke();
        }
    }

    private void Handler()
    {
        Debug.Log("Pressed Space Button!");
    }
}
```

#### Listener Components
For easily adding event listeners to game logic, this library provides simple event listener
components. They can be added to gameobjects without writing code and expose invoke calls to
_[UnityEvents][unity event]_. This works also for shared events that have a custom amount of
arguments on its delegate. The shared event that has been fired will reference itself when invoking
the _UnityEvent_.

> **Note:** As events are wrapped to _[UnityEvents][unity event]_ with the fired event instance as
> its argument, there is quite a bit of wrapping overhead involved.

The library contains following listener components:

| Listener Component    | Description                                                                                                                                     |
| --------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------- |
| `SharedEventListener` | Basic listener by forwarding shared events to _[UnityEvents][unity event]_. Can listen to multiple different events.                            |
| `SharedEventAll`      | Calls a single shared event after all of a variable amount of shared events have been invoked. Can be resetted by calling the `Reset()` method. |
| `SharedEventAny`      | Calls a single shared event after any of a variable amount of shared events have been invoked. Can be resetted by calling the `Reset()` method. |

Arguments of an event are usually used by its handler/listener. However, since these are ignored by
the forwarding call to the _UnityEvent_ on listener components, they can be accessed using the
`GetLastArgs()` method:

```cs
public void SomeEventHandler(SharedEventBase firedEvent)
{
    SharedEvent<int, string> casted = firedEvent as SharedEvent<int, string>;
    (int id, string msg) = casted?.GetLastArgs() ?? (-1, String.Empty);

    Debug.Log($"Event fired for object with id: {id}, '{msg}'");
}
```

#### Coroutines & Generators
This library contains [yield instruction classes][customyield] that can suspend Unity coroutines or
generators. They work with shared events as well as with basic _[UnityEvents][unity event]_. The
following objects can be yielded:

| Yield Instruction | Description                                       |
| ----------------- | ------------------------------------------------- |
| `WaitForEvent`    | Wait for a specified event to be fired.           |
| `WaitForAllEvent` | Wait for all specified events to be fired.        |
| `WaitForAnyEvent` | Wait for any of the specified events to be fired. |

> **Note:** Yield instructions start listening when their constructor has been called.

#### Extendability
This library can and sometimes should be extended with additional customized shared events. The
following base classes provide functionality for specific types of events:

| Event Base                                        | Underlying Delegate                                 |
| ------------------------------------------------- | --------------------------------------------------- |
| SharedEventBase                                   | System.Action                                       |
| SharedEvent&lt;T&gt;                              | System.Action&lt;T&gt;                              |
| SharedEvent&lt;T0, T1&gt;                         | System.Action&lt;T0, T1&gt;                         |
| SharedEvent&lt;T0, T1, T2&gt;                     | System.Action&lt;T0, T1, T2&gt;                     |
| SharedEvent&lt;T0, T1, T2, T3&gt;                 | System.Action&lt;T0, T1, T2, T3&gt;                 |
| SharedEvent&lt;T0, T1, T2, T3, T4&gt;             | System.Action&lt;T0, T1, T2, T3, T4&gt;             |
| SharedEvent&lt;T0, T1, T2, T3, T4, T5&gt;         | System.Action&lt;T0, T1, T2, T3, T4, T5&gt;         |
| SharedEvent&lt;T0, T1, T2, T3, T4, T5, T6&gt;     | System.Action&lt;T0, T1, T2, T3, T4, T5, T6&gt;     |
| SharedEvent&lt;T0, T1, T2, T3, T4, T5, T6, T7&gt; | System.Action&lt;T0, T1, T2, T3, T4, T5, T6, T7&gt; |

> **Note:** When inheriting directly from `SharedEventBase` also implement the `ISharedEvent`
> interface!

```cs
[CreateAssetMenu(fileName = "OnPressedKey.asset", menuName = "Custom Primitives/Key Event", order = 100)]
public class KeyEvent : SharedEvent<KeyCode>
{
    // No more functionality needed, but of course can be added...
}
```

### Lifetime and Cleanup
Shared primitives are (and therefore) behave like _[ScriptableObjects][scriptable object]_. This
also means that they are able to live through scene changes and that serializable fields are picked
up by the editor to store their values.

In general, shared primitives are guaranteed to save their state as long as a reference in an active
or currently loaded scene to them exists. This includes switching to a new scene on runtime that
again holds a reference to the shared primitive. If no reference to a shared primitive that is
currently loaded exists anymore, it is staged to be collected by the GC.

> **Note:** It is good practice to clear or reset shared primitives on sync points before use. They
> do not replace save/loading systems.

Changing the objects [hide flags] can alter the behavior of the shared primitive.

## License
This package is licensed under the [Apache License, Version 2.0][license].

&copy; 2020, Koboldgames GmbH

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.


[package manager]: https://docs.unity3d.com/Manual/PackagesList.html
[scriptable object]: https://docs.unity3d.com/Manual/class-ScriptableObject.html
[property drawers]: https://docs.unity3d.com/Manual/editor-PropertyDrawers.html
[dontdestroyonload]: https://docs.unity3d.com/ScriptReference/Object.DontDestroyOnLoad.html
[unity event]: https://docs.unity3d.com/ScriptReference/Events.UnityEvent.html
[customyield]: https://docs.unity3d.com/ScriptReference/CustomYieldInstruction.html
[hide flags]: https://docs.unity3d.com/ScriptReference/HideFlags.html
[icomparable]: https://docs.microsoft.com/en-us/dotnet/api/system.icomparable
[icomparable-1]: https://docs.microsoft.com/en-us/dotnet/api/system.icomparable-1
[iequatable-1]: https://docs.microsoft.com/en-us/dotnet/api/system.iequatable-1
[convert]: https://docs.microsoft.com/en-us/dotnet/api/system.convert
[list]: https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1
[dictionary]: https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2
[hashset]: https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.hashset-1
[license]: ./LICENSE.md
