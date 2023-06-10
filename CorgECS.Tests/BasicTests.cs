using CorgECS.Components;
using CorgECS.Entities;
using CorgECS.Signals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorgECS.Tests
{
	[TestClass]
	public class BasicTests
	{

		private class TestSignal : Signal
		{ }

		private class ResponseSignal : Signal<int>
		{ }

		private class TestComponent : Component
		{

			internal bool testPassed = false;

			public override void Initialise()
			{
				RegisterSignal<TestSignal>(x => testPassed = true);
				RegisterSignal<ResponseSignal, int>(x => new SignalResult<int>(10));
			}
		}

		[TestMethod]
		public void TestSignalHandling()
		{
			Entity testEntity = new Entity();
			TestComponent createdComponent = new TestComponent();
			testEntity.AddComponent(createdComponent);
			testEntity.Raise(new TestSignal());
			Assert.IsTrue(createdComponent.testPassed);
		}

		[TestMethod]
		public void TestResponseSignal()
		{
			Entity testEntity = new Entity();
			TestComponent createdComponent = new TestComponent();
			testEntity.AddComponent(createdComponent);
			SignalResult<int> result = testEntity.Raise<ResponseSignal, int>(new ResponseSignal());
			// Check that we have a 10
			Assert.IsTrue(result.Contains(10));
		}

	}
}
