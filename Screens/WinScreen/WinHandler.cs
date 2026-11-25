using _2D_Satisfactory.Screens.MainGame;
using Microsoft.Xna.Framework;

namespace _2D_Satisfactory.Screens.WinScreenClasses;

public class WinHandler : GameComponent
{
    #region States
    private enum WinScreenState
    {
        Incomplete,
        Completed,
        Continued
    }

    #endregion

    #region Private Fields

    private WinScreenState _state;
    private SetVisibilityDelegate _setProgressBarVisibility;
    private SetVisibilityDelegate _setWinDisplayVisibility;
    private const int SmartPlatingTargetAmount = 50;

    #endregion

    #region Constructor

    public WinHandler(Game game) : base(game)
    {
        _state = WinScreenState.Incomplete;
    }

    #endregion

    #region Public Methods

    public void AssignVisibilityCallbacks(SetVisibilityDelegate setProgressBarVisibility, SetVisibilityDelegate setWinDisplayVisibility)
    {
        _setProgressBarVisibility = setProgressBarVisibility;
        _setWinDisplayVisibility = setWinDisplayVisibility;
    }

    public void Continue()
    {
        _state = WinScreenState.Continued;
    }

    #endregion

    #region Override Methods

    public override void Update(GameTime gameTime)
    {
        int smartPlating = GameData.PlayerInventory.ContainsKey("smartPlate")
            ? GameData.PlayerInventory["smartPlate"] : 0;

        if (smartPlating < SmartPlatingTargetAmount) _state = WinScreenState.Incomplete;

        else if (smartPlating >= SmartPlatingTargetAmount && _state == WinScreenState.Incomplete)
            _state = WinScreenState.Completed;

        switch (_state)
        {
            case WinScreenState.Incomplete:
            case WinScreenState.Continued:
                _setProgressBarVisibility(true);
                _setWinDisplayVisibility(false);
                break;
            case WinScreenState.Completed:
                _setProgressBarVisibility(false);
                _setWinDisplayVisibility(true);
                break;
        }
    }

    #endregion
}