// See https://aka.ms/new-console-template for more information
using EmailOTPConsoleApplication.Interfaces;
using EmailOTPConsoleApplication.Services;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Hello, World!");

var services = new ServiceCollection();

services.AddTransient<IEmailOneTimePasswordService, EmailOneTimePasswordService>();
services.AddTransient<ISignUpConsole, SignupConsoleService>();

var serviceBuilder = services.BuildServiceProvider();
var signUpConsole = serviceBuilder.GetRequiredService<ISignUpConsole>();
await signUpConsole.Start();
