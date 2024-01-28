using RestSample.Services;

public static class UserEndpoints
{
    public static void RegisterUserEndpoints(
        this IEndpointRouteBuilder routes)
    {
        var userRoutes = routes.MapGroup("/users");
        userRoutes.MapGet("", (UserService userService)
            => userService.Get());
        userRoutes.MapGet("{id:int}", (int id, UserService userService)
            => userService.GetById(id));
        userRoutes.MapPost("", (User user, UserService userService)
            => userService.Post(user));
        userRoutes.MapPut("{id:int}", (int id, User user, UserService userService)
            => userService.Put(id, user));
        userRoutes.MapDelete("{id:int}", (int id, UserService userService)
            => userService.Delete(id));
    }
}