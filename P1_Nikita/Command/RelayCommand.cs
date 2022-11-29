using System;
using System.Threading.Tasks;

namespace P1_Nikita.Command;

public class RelayCommand<T> : CommandBase
{
    private readonly Action<T> _action;
    private readonly Func<T?, bool>? _canExecute;

    public RelayCommand(Action<T> action, Func<T?, bool>? canExecute = null)
    {
        _action = action;
        _canExecute = canExecute;
    }

    public override bool CanExecute(object? parameter)
    {
        if (parameter is not T src) return false;
        return (_canExecute == null || _canExecute(src)) && base.CanExecute(parameter);
    }

    public override void Execute(object? parameter)
    {
        if (parameter is not T src) return;
        _action(src);
    }
}

public class RelayCommand : CommandBase
{
    private readonly Action<object?> _action;
    private readonly Func<object?, bool>? _canExecute;

    public RelayCommand(Action<object?> action, Func<object?, bool>? canExecute = null)
    {
        _action = action;
        _canExecute = canExecute;
    }


    public override bool CanExecute(object? parameter)
    {
        return (_canExecute == null || _canExecute(parameter)) && base.CanExecute(parameter);
    }

    public override void Execute(object? parameter)
    {
        _action(parameter);
    }
}

public class AsyncRelayCommand<T> : AsyncCommandBase
{
    private readonly Action<T> _action;
    private readonly Func<T?, bool>? _canExecute;

    public AsyncRelayCommand(Action<T> action, Func<T?, bool>? canExecute = null)
    {
        _action = action;
        _canExecute = canExecute;
    }

    public override bool CanExecute(object? parameter)
    {
        if (parameter is not T src) return false;
        return (_canExecute == null || _canExecute(src)) && base.CanExecute(parameter);
    }

    public override Task ExecuteAsync(object? parameter)
    {
        if (parameter is not T src) return Task.CompletedTask;
        _action(src);
        return Task.CompletedTask;
    }
}

public class AsyncRelayCommand : AsyncCommandBase
{
    private readonly Action<object?> _action;
    private readonly Func<object?, bool>? _canExecute;

    public AsyncRelayCommand(Action<object?> action, Func<object?, bool>? canExecute = null)
    {
        _action = action;
        _canExecute = canExecute;
    }

    public override bool CanExecute(object? parameter)
    {
        return (_canExecute == null || _canExecute(parameter)) && base.CanExecute(parameter);
    }

    public override async Task ExecuteAsync(object? parameter)
    {
        await Task.Run(() => _action(parameter));
    }
}