using System;

namespace Dell.Koa.Common.Http.Security
{
	public class IdentityClientInfo
	{
		public string Api { get; set; }
		public string ClientId { get; set; }
		public string ClientSecret { get; set; }
		public int ExpiresInSeconds { get; set; }
		public BearerTokenInfo BearerTokenInfo { get; set; }

		public IdentityClientInfo(string api, string clientId, string clientSecret, int expiresInSeconds)
		{
			this.Api = api;
			this.ClientId = clientId;
			this.ClientSecret = clientSecret;
			this.ExpiresInSeconds = expiresInSeconds;
			this.BearerTokenInfo = null;
		}
	}
}
