using MagmaHeart.AI.Actions;
using MagmaHeart.Core.BoardStateSystem.Actions;
using NUnit.Framework;
using System;
using System.Linq;
using System.Reflection;

namespace MagmaHeart.Core.Tests
{
    public class ActionDatabaseTests
    {
        [Test]
        public void Get_AllActionsRegistered_NoExceptionsThrown()
        {
            Assembly assembly = FindAssembly("MagmaHeart.Core");

            ActionDatabase actionDatabase = new ActionDatabase(assembly);

            Assert.That(actionDatabase.AllActions.Count(), Is.EqualTo(3));
            Assert.That(actionDatabase.Get<MovementAction>(), Is.Not.Null);
            Assert.That(actionDatabase.Get<AttackAction>(), Is.Not.Null);
        }

        Assembly FindAssembly(string assemblyName)
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name == assemblyName);
        }
    }
}
