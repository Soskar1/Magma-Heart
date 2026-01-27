namespace MagmaHeart.AI.States.SimulationOperations
{
    internal sealed record UnitPropertyUpdateSimulationOperation(int UnitId, PropertySnapshot OldPropertyValue, PropertySnapshot NewPropertyValue) : SimulationOperation;
}
