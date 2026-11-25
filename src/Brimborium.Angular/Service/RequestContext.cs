namespace Brimborium.Angular.Service;

public class RequestContext {
    private List<IDelayedAction?>? _ListPostCommitAction;
    private List<IPostRequestItem?>? _ListPostRequestAction;

    public RequestContext() {
        this._ListPostCommitAction = new ();
        this._ListPostRequestAction = new ();
    }


    public void AddPostCommitAction(IDelayedAction action) {
        (this._ListPostCommitAction??=new()).Add(action);
    }


    public async Task ExecutePostCommitActionAsync(CancellationToken cancellation) {
        if (this._ListPostCommitAction is not { } listPostponeAction) { return; }
            var count = listPostponeAction.Count;
        {
            for (var index = 0; index < count; index++) {
                if (listPostponeAction[index] is { } action) {
                    await action.ExecuteAsync(CancellationToken.None).ConfigureAwait(false);
                }
                listPostponeAction[index] = null;
            }
        }
        {
            if (count == listPostponeAction.Count) {
                listPostponeAction.Clear();
            } else {
                listPostponeAction.RemoveRange(0, count);
            }
        }
    }

    public void AddPostRequestAction(IPostRequestItem action) {
        (this._ListPostRequestAction ??= new()).Add(action);
    }

    public async Task TransferPostRequestActionAsync(
        IQueueService<IPostRequestItem> queueService,
        CancellationToken cancellation) {
        if (this._ListPostRequestAction is not { } listPostRequestAction) { return; }
        var count = listPostRequestAction.Count;
        for (var index = 0; index < count; index++) {
            if (listPostRequestAction[index] is { } action) {
                await queueService.Add(action, cancellation).ConfigureAwait(false);
            }
            listPostRequestAction[index] = null;
        }
        if (count == listPostRequestAction.Count) {
            listPostRequestAction.Clear();
        } else {
            listPostRequestAction.RemoveRange(0, count);
        }
    }
}

public interface IDelayedAction {
    Task ExecuteAsync(CancellationToken cancellation);
}


public interface IPreload<TParameter, TResult> {
    TParameter Parameter { get; }
    Task ExecuteAsync(CancellationToken cancellation);
    Task<TResult> GetResultAsync(CancellationToken cancellation);
}

public class BroadcastAction<T> : IDelayedAction {
    private readonly T _Value;

    public BroadcastAction(T value) {
        this._Value = value;
    }

    public Task ExecuteAsync(CancellationToken cancellation) {
        throw new NotImplementedException();
    }
}
