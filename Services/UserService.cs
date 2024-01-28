using Microsoft.EntityFrameworkCore;

namespace RestSample.Services;

public class UserService
{
    private readonly ILogger<UserService> _logger;
    
    private readonly RestSampleContext _dbContext;

    public UserService(
        ILogger<UserService> logger,
        RestSampleContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<IResult> Post(User user)
    {
        var exists = await _dbContext.Users
            .AnyAsync(u => u.Id == user.Id);
        if (exists)
        {
            return TypedResults.Conflict();
        }

        var entry = await _dbContext.AddAsync(user);
        await _dbContext.SaveChangesAsync();

        var newUser = entry.Entity;
        return TypedResults.Created(
            $"/users/{newUser.Id}",
            newUser
        );
    }

    public async Task<IResult> Get()
    {
        return TypedResults.Ok(await _dbContext.Users.ToListAsync());
    }

    public async Task<IResult> GetById(int id)
    {
        var exisitingUser = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == id);
        if (exisitingUser == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(exisitingUser);
    }

    public async Task<IResult> Put(int id, User user)
    {
        var existingUser = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == id)!;
        if (existingUser == null)
        {
            return TypedResults.NotFound();
        }

        existingUser.Name = user.Name;
        _dbContext.Update(existingUser);
        await _dbContext.SaveChangesAsync();

        return TypedResults.NoContent();
    }

    public async Task<IResult> Delete(int id)
    {
        var existingUser = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == id);
        if (existingUser == null)
        {
            return TypedResults.NotFound();
        }

        _dbContext.Users.Remove(existingUser);
        await _dbContext.SaveChangesAsync();

        return TypedResults.Ok();
    }
}
