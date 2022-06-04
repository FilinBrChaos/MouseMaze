using System.Collections.Generic;
using System.Windows.Controls;
using GameParts.GameCore.OtherSoftware;

namespace GameParts.GameCore.Players
{
    public class EasyComputerPlayer : ComputerPlayer
    {
        public EasyComputerPlayer() : base() { }

        public EasyComputerPlayer(Image image, bool[,] maze) : base(image, maze) { }

        public EasyComputerPlayer(string imgPath, bool[,] maze) : base(imgPath, maze) { }

        public override void Step(StepArgs args)
        {
            List<Directions> possibleSteps = PossibleSteps();
            if (possibleSteps.Count > 1) possibleSteps.Remove(GetOppositDirection(previousStep));
            previousStep = possibleSteps[random.Next(possibleSteps.Count)];
            base.Step(new StepArgs(new StepArgs.DirectionField(previousStep)));
        }

    }
}
