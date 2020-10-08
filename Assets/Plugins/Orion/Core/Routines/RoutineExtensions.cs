using System;

namespace Orion
{
    public static class RoutineExtensions
    {
        public static ActionRoutine ToRoutine(this Action action) => new ActionRoutine() {action = action};
        public static InstructionRoutine ToRoutine(this object instruction) => new InstructionRoutine() {instruction = instruction};
    }
}