using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace SmashTracker.Api
{
	/// <summary>
	/// Hijacked from http://tech.trailmax.info/2014/02/implemnting-https-everywhere-in-asp-net-mvc-application/
	/// This will make any call to the webapi be required to be from HTTPS. If it is not, it will send a rejection for the call.
	/// This will ignore local from the requirement: only nonlocal calls force https.
	/// This is called in the webapiconfig file, and gets applied to everything.
	/// </summary>
	public class EnforceHttpsHandler : DelegatingHandler
	{
		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			// if request is local, just serve it without https
			object httpContextBaseObject;
			if (request.Properties.TryGetValue("MS_HttpContext", out httpContextBaseObject))
			{
				var httpContextBase = httpContextBaseObject as HttpContextBase;

				if (httpContextBase != null && httpContextBase.Request.IsLocal)
				{
					return base.SendAsync(request, cancellationToken);
				}
			}

			// if request is remote, enforce https
			if (request.RequestUri.Scheme != Uri.UriSchemeHttps)
			{
				return Task<HttpResponseMessage>.Factory.StartNew(
					() =>
					{
						var response = new HttpResponseMessage(HttpStatusCode.Forbidden)
						{
							Content = new StringContent("HTTPS Required")
						};

						return response;
					});
			}

			return base.SendAsync(request, cancellationToken);
		}
	}
}