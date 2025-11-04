using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace MagmaHeart.Collections.Tests
{
    // Base class for testing
    public abstract class BaseProp
    {
        public int Value { get; set; }
    }

    // Concrete subclasses
    public class PropA : BaseProp { }
    public class PropB : BaseProp { }

    [TestFixture]
    public class TypeMapTests
    {
        [Test]
        public void Add_And_Get_ShouldStoreAndReturnCorrectType()
        {
            // Arrange
            var map = new TypeMap<BaseProp>();
            var a = new PropA { Value = 42 };

            // Act
            map.Add(a);
            var result = map.Get<PropA>();

            // Assert
            Assert.That(result, Is.SameAs(a));
            Assert.That(result.Value, Is.EqualTo(42));
        }

        [Test]
        public void Get_WhenTypeNotAdded_ShouldThrowKeyNotFoundException()
        {
            var map = new TypeMap<BaseProp>();

            Assert.Throws<KeyNotFoundException>(() => map.Get<PropA>());
        }

        [Test]
        public void TryGet_WhenItemExists_ShouldReturnTrueAndItem()
        {
            // Arrange
            var map = new TypeMap<BaseProp>();
            var a = new PropA();
            map.Add(a);

            // Act
            bool result = map.TryGet<PropA>(out var found);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(found, Is.SameAs(a));
        }

        [Test]
        public void TryGet_WhenItemDoesNotExist_ShouldReturnFalseAndDefault()
        {
            var map = new TypeMap<BaseProp>();

            bool result = map.TryGet<PropA>(out var found);

            Assert.That(result, Is.False);
            Assert.That(found, Is.Null);
        }

        [Test]
        public void Contains_ShouldReturnTrue_WhenItemExists()
        {
            var map = new TypeMap<BaseProp>();
            map.Add(new PropA());

            Assert.That(map.Contains<PropA>(), Is.True);
            Assert.That(map.Contains<PropB>(), Is.False);
        }

        [Test]
        public void Remove_ShouldRemoveItem()
        {
            // Arrange
            var map = new TypeMap<BaseProp>();
            map.Add(new PropA());

            // Act
            map.Remove<PropA>();

            // Assert
            Assert.That(map.Contains<PropA>(), Is.False);
        }

        [Test]
        public void DeepCopy_ShouldCreateIndependentDictionaryReference()
        {
            // Arrange
            var a = new PropA { Value = 10 };
            var b = new PropB { Value = 20 };

            var map = new TypeMap<BaseProp>();
            map.Add(a);
            map.Add(b);

            // Act
            var copy = map.DeepCopy();

            // Assert — the map and copy are different objects
            Assert.That(copy, Is.Not.SameAs(map));

            // But the stored values are shallow-copied (same instances)
            Assert.That(copy.Get<PropA>(), Is.SameAs(map.Get<PropA>()));
            Assert.That(copy.Get<PropB>(), Is.SameAs(map.Get<PropB>()));
        }

        [Test]
        public void DeepCopy_WithCloneFunc_ShouldCloneValues()
        {
            // Arrange
            var map = new TypeMap<BaseProp>();
            var original = new PropA { Value = 99 };
            map.Add(original);

            // Act
            var copy = map.DeepCopy(p => new PropA { Value = p.Value });

            // Assert
            var originalProp = map.Get<PropA>();
            var copiedProp = copy.Get<PropA>();

            Assert.That(copiedProp, Is.Not.SameAs(originalProp));
            Assert.That(copiedProp.Value, Is.EqualTo(originalProp.Value));
        }

        [Test]
        public void Constructor_WithDictionary_ShouldInitializeWithGivenItems()
        {
            // Arrange
            var dict = new Dictionary<Type, BaseProp>
            {
                { typeof(PropA), new PropA { Value = 5 } },
                { typeof(PropB), new PropB { Value = 15 } }
            };

            // Act
            var map = new TypeMap<BaseProp>(dict);

            // Assert
            Assert.That(map.Contains<PropA>(), Is.True);
            Assert.That(map.Contains<PropB>(), Is.True);
            Assert.That(map.Get<PropA>().Value, Is.EqualTo(5));
            Assert.That(map.Get<PropB>().Value, Is.EqualTo(15));
        }
    }
}
