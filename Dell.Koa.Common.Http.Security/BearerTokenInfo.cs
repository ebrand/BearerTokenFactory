using System;

namespace Dell.Koa.Common.Http.Security
{
	public class BearerTokenInfo
	{
		private DateTime _dtCreated;

		public string AccessToken { get; set; }
		public int ExpiresInSeconds { get; set; }
		public string TokenType { get; set; }
		public DateTime ExpireDate => _dtCreated.AddSeconds(this.ExpiresInSeconds);

		public BearerTokenInfo(int expiresInSeconds) : this(Guid.NewGuid().ToString(), expiresInSeconds, "bearer") { }
		public BearerTokenInfo(string accessToken, int expiresInSeconds, string tokenType)
		{
			_dtCreated = DateTime.UtcNow;
			this.AccessToken = accessToken;
			this.ExpiresInSeconds = expiresInSeconds;
			this.TokenType = tokenType;
		}
	}
}
