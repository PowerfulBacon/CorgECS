using CorgECS.Components;
using CorgECS.Entities;
using CorgECS.Systems;
using CorgECS.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorgECS.Tests
{
	[TestClass]
	public class SystemTests
	{

		public class TestSystem : EntitySystem<TestComponent>
		{
		}

		public class TestComponent : Component
		{

			public bool tested = false;

			public override void Initialise()
			{
				World.GetEntitySystem<TestSystem>()
					.JoinSystem(this);
			}

			public void Test()
			{
				tested = true;
			}
		}

		[TestMethod]
		public void TestSystems()
		{
			World world = new World();
			Entity createdEntity = world.CreateEntity()
				.WithComponent(new TestComponent());
			world.GetEntitySystem<TestSystem>()
				.Components
				.ToList()
				.ForEach(x => x.tested = true);
			Assert.IsTrue(createdEntity.GetComponent<TestComponent>()?.tested);
		}

	}
}
