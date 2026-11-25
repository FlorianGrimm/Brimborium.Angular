namespace Brimborium.Angular.Service;

public static class ResponseExtensions {
    public static async Task<ResponseAPI<TContent>> ToResponseAsync<TContent>(this Task<ResultAPI<TContent>> taskResult)
        where TContent : class {
        var result = await taskResult.ConfigureAwait(false);
        var response = result switch {
            Problem problem => new ResponseAPI<TContent>(problem.Status ?? 0, default!, problem),
            TContent content => new ResponseAPI<TContent>(200, content, default),
            _ => throw new InvalidCastException(),
        };
        return response;
    }
    public static ResponseAPI<TContent> ToResponse<TContent>(this ResultAPI<TContent> result)
        where TContent : class {
        var response = result switch {
            Problem problem => new ResponseAPI<TContent>(problem.Status ?? 0, default!, problem),
            TContent content => new ResponseAPI<TContent>(200, content, default),
            _ => throw new InvalidCastException(),
        };
        return response;
    }
}