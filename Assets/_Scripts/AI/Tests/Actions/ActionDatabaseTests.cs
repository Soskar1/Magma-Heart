using MagmaHeart.AI.States;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace MagmaHeart.AI.Actions.Tests
{
    internal class ActionDatabaseTests
    {
        [Test]
        public void Get_AllActionsRegistered_NoExceptionsThrown()
        {
            ActionDatabase actionDatabase = new ActionDatabase();

            Assert.That(actionDatabase.AllActions.Count(), Is.EqualTo(0));
        }
    }
}
