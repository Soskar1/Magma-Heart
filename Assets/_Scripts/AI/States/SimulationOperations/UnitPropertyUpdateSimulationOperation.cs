namespace MagmaHeart.AI.States.SimulationOperations
{
    internal sealed record UnitPropertyUpdateSimulationOperation(AIUnitModel Unit, PropertySnapshot OldPropertyValue, PropertySnapshot NewPropertyValue) : SimulationOperation;
}
