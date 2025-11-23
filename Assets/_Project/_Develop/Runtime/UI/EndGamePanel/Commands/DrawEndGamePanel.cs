namespace TestTankProject.Runtime.UI.EndGamePanel.Commands
{
    public readonly struct DrawEndGamePanel
    {
        public readonly string Caption;

        public DrawEndGamePanel(string caption)
        {
            Caption = caption;
        }
    }
}