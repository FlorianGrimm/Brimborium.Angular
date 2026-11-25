namespace Brimborium.Angular.Repository;

public record User(
    string Name
    ) {
    public static ResultAPI<User> Convert(System.Security.Claims.ClaimsPrincipal? claimsPrincipal) {
        if (claimsPrincipal is null) { 
            return (NotAuthenticatedProblem.Create("no User.")).ToResult<User>();
        } else  if (claimsPrincipal.Identity is not { IsAuthenticated: true } identity) {
            return (NotAuthenticatedProblem.Create("User is not authenticated.")).ToResult<User>();
        } else if (identity.Name is not { Length: > 0 } name) {
            return (NotAuthenticatedProblem.Create("User has no name.")).ToResult<User>();
        } else {
            return (new User(name)).ToResult();
        }
    }
}

