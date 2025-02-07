using AutoMapper;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UoW_API.Repositories.Data;
using UoW_API.Repositories.Entities;
using UoW_API.Repositories.Entities.Dtos.User;
using UoW_API.Repositories.Repository.Interfaces;

namespace UoW_API.Repositories.Repository;

public class UserRepository : IUserRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly BlobServiceClient _blobServiceClient;
    private readonly SecretClient _secretClient;
    private readonly Uri? _vaultUri;
    const string getUserStoredProcedure = "GET_USER";

    public UserRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;

        IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        _vaultUri = new Uri(config["AzureCredentials:VaultUri"] ?? throw new InvalidOperationException("Value not found"));
        _secretClient = new SecretClient(_vaultUri, new DefaultAzureCredential());
        var storageConnectionString = _secretClient.GetSecret("STORAGE-CONNECTIONSTRING").Value.Value;
        _blobServiceClient = new BlobServiceClient(storageConnectionString);
    }


    public async Task<UserGetDto> CreateUser(UserCreateDto dto)
    {
        var dbUser = _mapper.Map<User>(dto);
        _context.Users.Add(dbUser);
        return _mapper.Map<UserGetDto>(dbUser);
    }

    public async Task DeleteUser(int id, CancellationToken cancellationToken)
    {
        var dbUser = await _context.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);

        _context.Users.Remove(dbUser);
    }

    public async Task<UserGetDto> GetUser(int id, CancellationToken cancellationToken)
    {
        

        var dbUser = await _context.Users
            .FromSqlInterpolated($"{getUserStoredProcedure} {id}")
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        if (dbUser == null)
        {
            throw new InvalidOperationException("User not found");
        }

        return _mapper.Map<UserGetDto>(dbUser.Single());
    }

    public async Task UploadImageAsync(int id, string localFilePath, CancellationToken cancellationToken)
    {
        localFilePath = localFilePath.Replace('"', ' ').Trim();
        BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient("uow-container");
        string fileName = Path.GetFileName(localFilePath);
        BlobClient blobClient = blobContainerClient.GetBlobClient(fileName);
        FileStream stream = File.OpenRead(localFilePath);
        await blobClient.UploadAsync(stream, true, cancellationToken);

        var dbUserQuery = await _context.Users
            .FromSqlInterpolated($"{getUserStoredProcedure} {id}")
            .ToListAsync (cancellationToken);

        var dbUser = dbUserQuery.Single();

        if (dbUser == null)
        {
            throw new InvalidOperationException("User not found!");
        }
        
        dbUser.ImageURL = blobClient.Uri.ToString();

    }
    
    public async Task<IEnumerable<UserGetDto>> GetUsers(CancellationToken cancellationToken)
    {
        var dbUsers = await _context.Users
            .AsNoTracking()
            .ToListAsync(cancellationToken);



        return _mapper.Map<List<UserGetDto>>(dbUsers);
    }
}
