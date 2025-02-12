using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UoW_API.Repositories.Data;
using UoW_API.Repositories.Entities;
using System.IO;
using UoW_API.Repositories.Enums;
using UoW_API.Repositories.Exceptions;
using UoW_API.Repositories.Repository.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Azure.Security.KeyVault.Secrets;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Azure.Identity;
using Azure;

namespace UoW_API.Repositories.Repository;
public class ProjectRepository : GenericRepository<Project>, IProjectRepository
{

    private readonly BlobServiceClient _blobServiceClient;
    private readonly SecretClient _secretClient;
    private readonly Uri? _vaultUri;

    public ProjectRepository(DataContext context) : base(context)
    {
        IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        _vaultUri = new Uri(config["AzureCredentials:VaultUri"] ?? throw new InvalidOperationException("Value not found"));
        _secretClient = new SecretClient(_vaultUri, new DefaultAzureCredential());
        var storageConnectionString = _secretClient.GetSecret("STORAGE-CONNECTIONSTRING").Value.Value;
        _blobServiceClient = new BlobServiceClient(storageConnectionString);
    }


    public override async Task<Project> Get(int id, CancellationToken cancellationToken)
    {
        var dbProject = await _context.Projects.FindAsync(id, cancellationToken);

        if (dbProject == null)
        {
            throw new ProjectNotFoundException("Project not found!");
        }

        return dbProject!;
    }

    public override async Task<IEnumerable<Project>> GetAll(CancellationToken cancellationToken)
    {
        var dbProjects = await _context.Projects
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return dbProjects;
    }

    public override void Create(Project dbProject, CancellationToken cancellationToken)
    {
        if (dbProject.From < DateTimeOffset.Now && DateTimeOffset.Now < dbProject.To)
        {
            dbProject.State = CurrentState.ONGOING;
        }

        if (dbProject.From > DateTimeOffset.Now)
        {
            dbProject.State = CurrentState.PENDING;
        }

        else
        {
            dbProject.State = CurrentState.FINISHED;
        }

        _context.Projects.Add(dbProject);
    }

    public override async Task Delete(int id, CancellationToken cancellationToken)
    {
        var dbProject = await _context.Projects.FindAsync(id, cancellationToken);

        if (dbProject == null)
        {
            throw new ProjectNotFoundException("Project not found!");
        }

        _context.Projects.Remove(dbProject);
    }

    public async Task AddUser(int id, int projectId, CancellationToken cancellationToken)
    {
        var dbUser = await _context.Users.FindAsync(id, cancellationToken);

        var dbProject = await _context.Projects
            .Include(x => x.Users)
            .SingleOrDefaultAsync(x => x.Id.Equals(projectId), cancellationToken);

        if (dbUser is null)
        {
            throw new UserNotFoundException("User not found!");
        }

        if (dbProject is null)
        {
            throw new ProjectNotFoundException("Project not found!");
        }

        dbUser.ProjectId = dbProject.Id;
    }

    public async Task GeneratePDF(int projectId, CancellationToken cancellationToken)
    {
        var dbProject = await _context.Projects
            .Include(x => x.Users)
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id.Equals(projectId), cancellationToken);

        if (dbProject is null)
        {
            throw new ProjectNotFoundException("Project not found!");
        }

        var projectPDF = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Header()
                .AlignCenter()
                .Text($"Project: {dbProject.Name}")
                .FontSize(20)
                .ExtraBold();

                page.Content()
                .PaddingVertical(1, Unit.Centimetre)
                .Column(x =>
                {
                    x.Item()
                    .Text($"{dbProject.Description}")
                    .AlignCenter();

                    x.Item()
                    .PaddingVertical(20);

                    x.Item()
                    .Text($"State: {dbProject.State}")
                    .AlignCenter();

                    x.Item()
                    .Text($"{dbProject.From.ToString("d")} - {dbProject.To.ToString("d")}")
                    .AlignCenter();

                    x.Item()
                    .PaddingVertical(20);

                    x.Item()
                    .Text($"Project members: {dbProject.Users!.Count()}")
                    .AlignCenter();


                    x.Item()
                    .PaddingVertical(10);

                    if (dbProject.Users is not null)
                    {
                        x.Item()
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(50);
                                columns.RelativeColumn();
                                columns.ConstantColumn(125);
                            });

                            table.Header(header =>
                            {
                                header.Cell().BorderBottom(2).Padding(8).Text("");
                                header.Cell().BorderBottom(2).Padding(8).Text("Name");
                                header.Cell().BorderBottom(2).Padding(8).Text("Email");
                            });

                            int count = 1;

                            foreach (var user in dbProject.Users)
                            {
                                table.Cell().Padding(8).Text(count.ToString());
                                table.Cell().Padding(8).Text(user.Name);
                                table.Cell().Padding(8).Text(user.Email);

                                count++;
                            }
                        });
                    }
                });

                page.Footer()
                .AlignCenter()
                .Text(x =>
                {
                    x.Span("Page ");
                    x.CurrentPageNumber();
                });

            });
        });


        MemoryStream stream = new MemoryStream();
        projectPDF.GeneratePdf(stream);
        projectPDF.GeneratePdfAndShow();

        BlobContainerClient blobContainer = _blobServiceClient.GetBlobContainerClient("uow-container-pdf");
        BlobClient blobClient = blobContainer.GetBlobClient($"{projectId}-{DateTimeOffset.Now:G}.pdf");
        stream.Position = 0;

        try
        {
            await blobClient.UploadAsync(stream, overwrite: true, cancellationToken);
        }

        catch (RequestFailedException)
        {
            throw;
        }
    }
}