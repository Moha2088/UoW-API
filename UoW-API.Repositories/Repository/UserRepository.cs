using AutoMapper;
using Azure;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UoW_API.Repositories.Data;
using UoW_API.Repositories.Entities;
using UoW_API.Repositories.Entities.Dtos.User;
using UoW_API.Repositories.Exceptions;
using UoW_API.Repositories.Repository.Interfaces;

namespace UoW_API.Repositories.Repository;

public class UserRepository : GenericRepository<User>, IUserRepository
{

    private readonly BlobServiceClient _blobServiceClient;
    private readonly SecretClient _secretClient;
    private readonly Uri? _vaultUri;

    public UserRepository(DataContext context) : base(context)
    {
        IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        _vaultUri = new Uri(config["AzureCredentials:VaultUri"] ?? throw new InvalidOperationException("Value not found"));
        _secretClient = new SecretClient(_vaultUri, new DefaultAzureCredential());
        var storageConnectionString = _secretClient.GetSecret("STORAGE-CONNECTIONSTRING").Value.Value;
        _blobServiceClient = new BlobServiceClient(storageConnectionString);
    }


    public async Task UploadImageAsync(int id, string localFilePath, CancellationToken cancellationToken)
    {
        var dbUser = await _context.Users
            .SingleOrDefaultAsync(x => x.Id == id);

        if (dbUser == null)
        {
            throw new UserNotFoundException("User not found!");
        }

        localFilePath = localFilePath.Replace('"', ' ').Trim();
        BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient("uow-container");
        string fileName = Path.GetFileName(localFilePath);

        var hasBlob = blobContainerClient
            .GetBlobs()
            .Any(x => x.Name.StartsWith(id.ToString()));

        if (hasBlob) 
        {
            throw new InvalidOperationException("A profile picture has already been set");
        }

        BlobClient blobClient = blobContainerClient.GetBlobClient($"{dbUser.Id}-{fileName}");
        FileStream stream = File.OpenRead(localFilePath);


        var metadata = new Dictionary<string, string>
        {
            { "ImageType", "ProfilePicture" }
        };

        var blobOptions = new BlobUploadOptions
        {
            Metadata = metadata,
        };

        await blobClient.UploadAsync(stream, blobOptions, cancellationToken);
    }

    public async Task DeleteImageAsync(int id, CancellationToken cancellationToken)
    {
        var dbUser = await _context.Users.FindAsync(id);

        if(dbUser == null)
        {
            throw new UserNotFoundException("User not found!");
        }

        var blobContainer = _blobServiceClient.GetBlobContainerClient("uow-container");
        var blobToDelete = blobContainer.GetBlobs().SingleOrDefault(x => x.Name.StartsWith(id.ToString()));

        if (blobToDelete == null) 
        {
            throw new ArgumentNullException("Blob not found!");
        }
        
        BlobClient blobClient = blobContainer.GetBlobClient(blobToDelete.Name);

        try
        {
            await blobClient.DeleteIfExistsAsync();
        }

        catch (RequestFailedException) 
        {
            throw;
        }
    }

    public override void Create(User entity, CancellationToken cancellationToken)
    {
        if(_context.Users.Any(u => u.Email.Equals(entity.Email)))
        {
            throw new InvalidOperationException("User already exists");
        }

        _context.Users.Add(entity);
    }

    public async override Task Delete(int id, CancellationToken cancellationToken)
    {
        var dbUser = await _context.Users.FindAsync(id, cancellationToken);

        if (dbUser == null) 
        {
            throw new UserNotFoundException("User not found!");
        }

        _context.Users.Remove(dbUser!);
    }

    public async override Task<IEnumerable<User>> GetAll(CancellationToken cancellationToken)
    {
        var dbUsers = await _context.Users
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return dbUsers;
    }

    public async override Task<User> Get(int id, CancellationToken cancellationToken)
    {
        var dbUser = await _context.Users.FindAsync(id, cancellationToken);

        if(dbUser == null)
        {
            throw new UserNotFoundException("User not found!");
        }

        return dbUser;
    }
}
