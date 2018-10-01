using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Dell.Koa.Common.Http.Security
{
	/// <summary>
	/// Singleton factory to ensure a fresh bearer token is available for multiple clients.
	/// </summary>
	public class BearerTokenFactory
	{
		public const int DEFAULT_BT_EXPIRY_IN_SECONDS = 36400;
		#region Singleton implementation
		private static readonly BearerTokenFactory _instance = new BearerTokenFactory();
		public static BearerTokenFactory Instance => _instance;
		static BearerTokenFactory() { }
		#endregion

		private readonly List<IdentityClientInfo> _identityClients;

		private BearerTokenFactory()
		{
			_identityClients = new List<IdentityClientInfo>();
		}

		public void RegisterApi(string apiName, string clientId, string clientSecret, int expiresInSeconds = DEFAULT_BT_EXPIRY_IN_SECONDS)
		{
			var clientInfo = FindIdentityClient(apiName);
			if(clientInfo == null)
				_identityClients.Add(new IdentityClientInfo(apiName, clientId, clientSecret, expiresInSeconds));
		}
		public string GetClientId(string apiName)
		{
			var clientInfo = FindIdentityClient(apiName);

			if(clientInfo == null)
				throw new InvalidOperationException($"The requested API name ('{apiName}') is not registered.");

			return clientInfo.ClientId;
		}
		public string GetClientSecret(string apiName)
		{
			var clientInfo = FindIdentityClient(apiName);

			if(clientInfo == null)
				throw new InvalidOperationException($"The requested API name ('{apiName}') is not registered.");

			return clientInfo.ClientSecret;
		}
		public int GetExpiresInSeconds(string apiName)
		{
			var clientInfo = FindIdentityClient(apiName);

			if(clientInfo == null)
				throw new InvalidOperationException($"The requested API name ('{apiName}') is not registered.");

			return clientInfo.ExpiresInSeconds;
		}
		public BearerTokenInfo GetBearerTokenInfo(string apiName)
		{
			var clientInfo = FindIdentityClient(apiName);

			if(clientInfo == null)
				throw new InvalidOperationException($"The requested API name ('{apiName}') is not registered.");
			
			EnsureValidBearerToken(clientInfo);
			return clientInfo.BearerTokenInfo;
		}

		private void EnsureValidBearerToken(IdentityClientInfo clientInfo)
		{
			if(clientInfo.BearerTokenInfo == null || DateTime.UtcNow > clientInfo.BearerTokenInfo.ExpireDate)
			{
				RefreshBearerToken(clientInfo);
			}
		}
		private void RefreshBearerToken(IdentityClientInfo clientInfo)
		{
			if(clientInfo == null)
				throw new ArgumentNullException(nameof(clientInfo));
				                            
			// TODO: call the Identity service to receive a new bearer token
			Console.WriteLine($"Calling the Koa Identity service to retrieve a valid bearer token ({clientInfo.ClientId}/{clientInfo.ClientSecret})...");
			Thread.Sleep(2000);

			// TODO: set the 'clientInfo.BearerTokenInfo' to the BTI retrieved from the Identity service
			clientInfo.BearerTokenInfo = new BearerTokenInfo(clientInfo.ExpiresInSeconds);
		}
		private IdentityClientInfo FindIdentityClient(string api)
		{
			return _identityClients.FirstOrDefault(ic => ic.Api == api);
		}
	}
}
