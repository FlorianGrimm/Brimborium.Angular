using System.Diagnostics.CodeAnalysis;

namespace Brimborium.Angular.API;

[Tapper.TranspilationSource]
public sealed record ResponseAPI<TContent>(
    int Status,
    TContent? Content,
    Problem? Problem
    ) where TContent : class;


public readonly union ResultAPI<Content>(
    Content,
    Problem
    ) {
    public bool TryGet([MaybeNullWhen(false)] out Content content, [MaybeNullWhen(true)] out Problem problem) {
        if (this.Value is Content contentValue) {
            content = contentValue;
            problem = default;
            return true;
        } else if (this.Value is Problem problemValue) {
            content = default;
            problem = problemValue;
            return false;
        } else {
            throw new Exception();
        }
    }

    public bool TryGetContent([MaybeNullWhen(false)] out Content content) {
        if (this.Value is Content contentValue) {
            content = contentValue;
            return true;
        } else {
            content = default;
            return false;
        }
    }
    public bool TryGetProblem([MaybeNullWhen(false)] out Problem problem) {
        if (this.Value is Problem problemValue) {
            problem = problemValue;
            return true;
        } else {
            problem = default;
            return false;
        }
    }
}


public static class ResultExtensions {

    public static ResultAPI<TContent> ToResult<TContent>(this TContent content) {
        return new ResultAPI<TContent>(content);
    }

    public static ResultAPI<TContent> ToResult<TContent>(this Problem problem) {
        return new ResultAPI<TContent>(problem);
    }



    public static Task<ResultAPI<TContent>> ToTaskResult<TContent>(this TContent content) {
        return Task.FromResult(new ResultAPI<TContent>(content));
    }

    public static Task<ResultAPI<TContent>> ToTaskResult<TContent>(this Problem problem) {
        return Task.FromResult(new ResultAPI<TContent>(problem));
    }
}