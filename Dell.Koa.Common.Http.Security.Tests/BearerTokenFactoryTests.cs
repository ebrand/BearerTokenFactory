using System;
using Xunit;
using Shouldly;
using System.Threading;

namespace Dell.Koa.Common.Http.Security.Tests
{
	public class BearerTokenFactoryTests
	{
		[Fact]
		public void ProperlyRegistersClientId()
		{
			string apiName = Guid.NewGuid().ToString();
			string clientId = Guid.NewGuid().ToString();
			string clientSecret = Guid.NewGuid().ToString();
			BearerTokenFactory.Instance.RegisterApi(apiName, clientId, clientSecret);
			BearerTokenFactory.Instance.GetClientId(apiName).ShouldBe(clientId);
		}

		[Fact]
		public void ProperlyRegistersClientSecret()
		{
			string apiName = Guid.NewGuid().ToString();
			string clientId = Guid.NewGuid().ToString();
			string clientSecret = Guid.NewGuid().ToString();
			BearerTokenFactory.Instance.RegisterApi(apiName, clientId, clientSecret);
			BearerTokenFactory.Instance.GetClientSecret(apiName).ShouldBe(clientSecret);
		}

		[Fact]
		public void ProperlyRegistersDefaultExpiry()
		{
			string apiName = Guid.NewGuid().ToString();
			string clientId = Guid.NewGuid().ToString();
			string clientSecret = Guid.NewGuid().ToString();
			BearerTokenFactory.Instance.RegisterApi(apiName, clientId, clientSecret);
			BearerTokenFactory.Instance.GetExpiresInSeconds(apiName).ShouldBe(BearerTokenFactory.DEFAULT_BT_EXPIRY_IN_SECONDS);
		}

		[Fact]
		public void ProperlyRegistersCustomExpiry()
		{
			string apiName = Guid.NewGuid().ToString();
			string clientId = Guid.NewGuid().ToString();
			string clientSecret = Guid.NewGuid().ToString();
			int expiresInSeconds = 15;
			BearerTokenFactory.Instance.RegisterApi(apiName, clientId, clientSecret, expiresInSeconds);
			BearerTokenFactory.Instance.GetExpiresInSeconds(apiName).ShouldBe(expiresInSeconds);
		}

		[Fact]
		public void ProperlyRetrievesBearerToken()
		{
			string apiName = Guid.NewGuid().ToString();
			string clientId = Guid.NewGuid().ToString();
			string clientSecret = Guid.NewGuid().ToString();
			BearerTokenFactory.Instance.RegisterApi(apiName, clientId, clientSecret);

			var bt = BearerTokenFactory.Instance.GetBearerTokenInfo(apiName);

			bt.ShouldNotBeNull();
			bt.AccessToken.ShouldNotBeNullOrWhiteSpace();
			bt.ExpireDate.ShouldBeGreaterThan(DateTime.UtcNow);
			bt.TokenType.ShouldNotBeNullOrWhiteSpace();
		}

		[Fact]
		public void ProperlyRefreshesBearerToken()
		{
			string apiName = Guid.NewGuid().ToString();
			string clientId = Guid.NewGuid().ToString();
			string clientSecret = Guid.NewGuid().ToString();
			int expiresInSeconds = 1;
			BearerTokenFactory.Instance.RegisterApi(apiName, clientId, clientSecret, expiresInSeconds);

			var bt = BearerTokenFactory.Instance.GetBearerTokenInfo(apiName);

			// get an access token
			var originalAccessToken = bt.AccessToken;

			// out-wait the expiry
			Thread.Sleep(expiresInSeconds * 1000);

			// re-get a new access token (should be different)
			bt = BearerTokenFactory.Instance.GetBearerTokenInfo(apiName);
			bt.AccessToken.ShouldNotBe(originalAccessToken);
		}
	}
}
