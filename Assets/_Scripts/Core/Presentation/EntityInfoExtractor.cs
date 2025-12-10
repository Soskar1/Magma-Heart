using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.Presenters;

namespace MagmaHeart.Core.Presentation
{
    public class EntityInfoExtractor : IHoverResultVisitor
    {
        public EntityModel Model { get; private set; }

        public void Visit(UIHoverResult result)
        {
            EntityPresenter presenter = result.UIElement?.GetComponent<EntityPresenter>();
            Model = presenter?.Model;
        }

        public void Visit(CombatHoverResult result) => Model = result.Entity?.Model;
        public void Visit(RaycastHoverResult result) => Model = result.Entity?.Model;
    }
}
