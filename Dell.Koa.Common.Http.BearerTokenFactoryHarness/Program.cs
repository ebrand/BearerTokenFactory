using System;
using Dell.Koa.Common.Http.Security;

namespace Dell.Koa.Common.Http.BearerTokenFactoryHarness
{
	class Program
	{
		static void Main(string[] args)
		{
			BearerTokenFactory.Instance.RegisterApi("koa.customer", "koa.customer.client.id", "koa.customer.client.secret", 5);

			BearerTokenInfo token;
			ConsoleKeyInfo keyInfo;

			do
			{
				Console.WriteLine("Token retrieval...");
				token = BearerTokenFactory.Instance.GetBearerTokenInfo("koa.customer");
				Console.WriteLine($"Token: {token.AccessToken}.");
				keyInfo = Console.ReadKey();
			}
			while(keyInfo.Key != ConsoleKey.Escape);
		}
	}
}
