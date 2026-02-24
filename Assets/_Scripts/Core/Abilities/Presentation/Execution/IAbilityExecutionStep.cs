using System.Threading;
using System.Threading.Tasks;

namespace MagmaHeart.Core.Abilities.Presentation.Execution
{
    public interface IAbilityExecutionStep
    {
        public Task Run(AbilityExecutionContext context, CancellationToken cancellationToken);
    }
}
