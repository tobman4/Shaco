
using System.CommandLine;
using Shaco.cli;
using Shaco.cli.Commands;

var app = new App();
var root = new RootCommand();

Link.AddCommands(root);

await root.InvokeAsync(args);
