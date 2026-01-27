using MagmaHeart.AI.Reasoning.Tests;
using NUnit.Framework;
using System;
using System.Linq;
using System.Reflection;

namespace MagmaHeart.AI.Actions.Tests
{
    internal class ActionDatabaseTests
    {
        [Test]
        public void Get_AllActionsRegistered_NoExceptionsThrown()
        {
            Assembly assembly = FindAssembly("MagmaHeart.AI.Tests");
            Assert.That(assembly, Is.Not.Null, "Could not find MagmaHeart.AI.Tests assembly.");

            ActionDatabase actionDatabase = new ActionDatabase(assembly);

            Assert.That(actionDatabase.AllActions.Count(), Is.EqualTo(3));
            Assert.That(actionDatabase.Get<MoveAction>(), Is.Not.Null);
            Assert.That(actionDatabase.Get<AttackAction>(), Is.Not.Null);
            Assert.That(actionDatabase.Get<RunAwayAction>(), Is.Not.Null);
        }

        Assembly FindAssembly(string assemblyName)
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name == assemblyName);
        }
    }
}
