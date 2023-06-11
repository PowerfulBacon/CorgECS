# CorgECS
A lightweight, simple and concise E/C library for C#. This library is as simple
as possible to reduce the difficulty with working with ECS-style frameworks for
games.

## Entities

Entities hold components. You can add a component to an entity by calling the `AddComponent`
method.

```csharp
World world = new World();
Entity testEntity = world.CreateEntity();
testEntity.AddComponent(new TestComponent());
```

## Components

Components store data and provide functionality to entities when a signal is raised
against the parent entity.

**Components must implement the Initialise() method.** This is called when the
component is added to an entity, and allows for it to start listening to signals.

```csharp
public class TestComponent : Component
{

	private int componentData = 0;

	public override void Initialise()
	{
		RegisterSignal<TestSignal>(HandleSignal);
	}

	private void HandleSignal(TestSignal raised)
	{
		// Do something with the signal
	}

}
```

When a TestSignal is raised against an entity that has the TestComponent added,
`HandleSignal` inside of the TestComponent will be called.

## Signals

Signals are ways of communicating changes within the world to entities. Typically
signals represent events that may be listened to, but don't necessarilly need to be
handled.

Signals can store data, which can be used to send messages to components.

There are 2 types of signals:
- Asynchronous Signals (Fire and forget)
- Response Signals (Yield a response)

### Asynchronous Signals

Asynchronous signals represent an event occuring which may be listened to by a
component.
Creating a new signal is as simple as creating a class which is a subtype of
the signal class.

```csharp
public class ExampleSignal : Signal
{

	public int ExampleData { get; }

	public ExampleSignal(int value)
	{
		ExampleData = value;
	}

}
```

Signals can be raised against an entity by calling `Entity.Raise()`.

```csharp
World world = new World();
Entity exampleEntity = world.CreateEntity();
// Add a component to the entity which listens for the Example signal here.
exampleEntity.Raise(new ExampleSignal(5));
```

### Response Signals

Response signals are signals which can return a response. Any signal handlers
that listen to a reponse signal can return no value.

Here is an example of a response signal which returns an integer value.

```csharp
public class ResponseSignal : Signal<int>
{ /* Insert signal data here */ }
```

```csharp
public class TestComponent : Component
{

	public override void Initialise()
	{
		RegisterSignal<ResponseSignal, int>(HandleSignal);
	}

	private SignalResponse<int> HandleSignal(TestSignal raised)
	{
		// Do something with the signal
		// Return a value
		return new SignalResponse<int>(10);
	}

}
```

```csharp
World world = new World();
Entity testEntity = world.CreateEntity();
testEntity.AddComponent(new TestComponent());
SignalResult<int> result = testEntity.Raise<ResponseSignal, int>(new ResponseSignal());
```

Unfortunately, when registering signals for response types you must specify the return
value as a generic constraint. This is due to generics being unable to determine the type
of the return value despite it being defined in the signal class itself. It would be possible
to modify the code so that it only requires the return type, however this could get
confusing. If you have any idea of a workaround of this, please open a PR or contact
me on discord.

## Entity Systems

Entity systems allow for tracking components and sharing states between different components
easilly.

You can fetch a singleton EntitySystem by calling World.GetEntitySystem().

```csharp
World world = new World();
world.GetEntitySystem<ExampleSystem>();
```

Components can join an entity system by calling EntitySystem.JoinSystem(). This will cause
the entity system to begin tracking the entity, so that it can be queried later.

```csharp
/// We are required to specify the type of components that we want to track.
/// This could just be Component if we want to track any component type.
public class TestSystem : EntitySystem<TestComponent>
{ }

class TestComponent : Component
{

	public override void Initialise()
	{
		World.GetEntitySystem<TestSystem>()
			.JoinSystem(this);
	}

}
```

We can now get the entity system from anywhere in the code and execute queries across
all components that are being tracked.
