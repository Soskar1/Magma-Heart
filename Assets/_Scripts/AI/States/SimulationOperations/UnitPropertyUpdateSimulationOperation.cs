namespace MagmaHeart.AI.States.SimulationOperations
{
    internal sealed record UnitPropertyUpdateSimulationOperation(AIUnit Unit, PropertySnapshot OldPropertyValue, PropertySnapshot NewPropertyValue) : SimulationOperation;
}
