using Blueboard.Features.Import.Commands;
using Blueboard.Features.Import.Queries;
using Helpers.WebApi.Interfaces;
using Helpers.WebApi.Utils;
using MediatR;

namespace Blueboard.Features.Import.ConsoleCommands;

public class CreateImportKeyCommand : IConsoleCommand
{
    private readonly ISender _mediator;

    public CreateImportKeyCommand(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        _mediator = scope.ServiceProvider.GetRequiredService<ISender>();
    }

    public string Name { get; } = "import-key:create";

    public void Execute(string[] args)
    {
        if (args.Length != 1)
        {
            ConsoleUtils.Error("Please provide a name for the import key!");
            return;
        }

        var createResponse = _mediator.Send(new CreateImportKey.Command
        {
            Body = new CreateImportKey.RequestBody
            {
                Enabled = true,
                Name = args[0]
            }
        }).GetAwaiter().GetResult();

        ConsoleUtils.Success("Successfully created a new import key!");

        var importKey = _mediator.Send(new ViewImportKey.Query
        {
            Id = createResponse.Id
        }).GetAwaiter().GetResult();

        ConsoleUtils.Success($"Your key is: {importKey.Key}");
    }
}