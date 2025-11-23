using TestTankProject.Runtime.Bootstrap;

namespace TestTankProject.Runtime.UI.EndGamePanel.Commands
{
    [Message]
    public readonly struct DrawEndGamePanel
    {
        public readonly string Caption;

        public DrawEndGamePanel(string caption)
        {
            Caption = caption;
        }
    }
}