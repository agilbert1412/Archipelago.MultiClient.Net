#if NET45 || NETSTANDARD2_0 || NET6_0 || NET462
using Archipelago.MultiClient.Net.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

#if NET45 || NET462
using System.Net;
#endif

namespace Archipelago.MultiClient.Net.Helpers
{
    public class ArchipelagoSocketHelper : BaseArchipelagoSocketHelper<ClientWebSocket>, IArchipelagoSocketHelper
    {
	    /// <summary>
	    ///     The URL of the host that the socket is connected to.
	    /// </summary>
	    public Uri Uri { get; }

		internal ArchipelagoSocketHelper(Uri hostUri) : base(CreateWebSocket())
        {
            Uri = hostUri;

#if NET45 || NET462
			//this is done on constructor, rather than static constructor override any value set anywhere else in the process
			var Tls13 = (SecurityProtocolType)12288;
	        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | Tls13;
#endif
        }

        static ClientWebSocket CreateWebSocket()
        {
	        var clientWebSocket = new ClientWebSocket();

#if NET6_0
			clientWebSocket.Options.DangerousDeflateOptions = new WebSocketDeflateOptions();
#endif

	        return clientWebSocket;
		}

        private void Log(string message)
		{
			var time = DateTime.Now;
			File.AppendAllText("multiclientlog.txt", Environment.NewLine + time.ToLongTimeString() + "." + time.Millisecond + ": " + message);
        }

		/// <summary>
		///     Initiates a connection to the host asynchronously.
		///     Handle the <see cref="SocketOpened"/> event to add a callback.
		/// </summary>
		public async Task ConnectAsync()
        {
			Log("Starting Socket ConnectAsync");
			await ConnectToProvidedUri(Uri);

			Log("Before StartPolling");
			StartPolling();

			Log("Finishing Socket ConnectAsync");
}

        async Task ConnectToProvidedUri(Uri uri)
        {
	        if (uri.Scheme != "unspecified")
			{
				Log("uri.Scheme is specified");
				try
				{
					Log("Before Socket.ConnectAsync(uri, CancellationToken.None)");
					await Socket.ConnectAsync(uri, CancellationToken.None);
					Log("After Socket.ConnectAsync(uri, CancellationToken.None)");
				}
		        catch (Exception e)
				{
					Log("uri.Scheme unspecified threw an exception: " + e);
					OnError(e);
			        throw;
		        }
			}
			else
			{
				Log("uri.Scheme is unspecified");
				var errors = new List<Exception>(0);
				try
				{

					Log("Try Connect as WSS");
					await Socket.ConnectAsync(uri.AsWss(), CancellationToken.None);

					if (Socket.State == WebSocketState.Open)
						return;
				}
				catch(Exception e)
				{
					Log("WSS Error: " + e);
					errors.Add(e);
					Socket = CreateWebSocket();
					Log("Created a new WebSocket");
				}

				try
				{
					Log("Try Connect as WS");
					var assemblyInfo = typeof(ClientWebSocket).Assembly.FullName;
					Log("typeof(ClientWebSocket).Assembly.FullName: " + assemblyInfo);
					await Socket.ConnectAsync(uri.AsWs(), CancellationToken.None);
					Log("After Connect as WS");
				}
				catch (Exception e)
				{
					Log("WS Error: " + e);
					errors.Add(e);

					OnError(new AggregateException(errors));

					throw;
				}
			}
		}
    }
}
#endif