using System.Collections.Generic;
using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;

namespace RouteSnapper;

public class GlobalViewModel : ObservableObject
{
    private bool _lockUiWhenBuilding;
    private bool _uiLocked;
    private bool _lockRequested;

    public bool LockUiWhenBuilding
    {
        get => _lockUiWhenBuilding;

        set
        {
            SetProperty( ref _lockUiWhenBuilding, value );

            if( !LockRequested )
                return;

            UiLocked = value;
            LockRequested = value;
        }
    }

    public bool LockRequested
    {
        get => _lockRequested;

        set
        {
            SetProperty( ref _lockRequested, value );

            if( LockUiWhenBuilding )
                UiLocked = value;
        }
    }

    public bool UiLocked
    {
        get => _uiLocked;
        private set => SetProperty( ref _uiLocked, value );
    }
}