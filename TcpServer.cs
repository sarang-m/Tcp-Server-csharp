using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TcpServerApp.Network
{
    class TcpServer
    {
        private TcpListener _tcpListener;
        private bool _isRunning;
        public TcpServer(int port)
        {
            _tcpListener = new TcpListener(IPAddress.Any, port);
        }

        public void Start()
        {
            try
            {
                _tcpListener.Start();
                _isRunning = true;
                Console.WriteLine("Server started. Waiting for connections...");

                // Handle clients in a separate thread
                ThreadPool.QueueUserWorkItem(HandleClients);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting the server: {ex.Message}");
            }
        }

        public void HandleClients(object state)
        {
            while (_isRunning)
            {
                try
                {
                    // Accept a new client connection
                    TcpClient client = _tcpListener.AcceptTcpClient();
                    Console.WriteLine("Client connected.");

                    // Handle client communication in a separate thread
                    ThreadPool.QueueUserWorkItem(HandleClientCommunication, client);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error accepting client connection: {ex.Message}");
                }
            }
        }

        private void HandleClientCommunication(object clientObj)
        {
            TcpClient client = clientObj as TcpClient;
            if (client == null) return;

            try
            {
                // Get the network stream for reading/writing data
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];
                int bytesRead;

                // Continuously read data from the client
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    byte[] responseBytes;
                    string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"Received: {receivedData}");

                    if (receivedData.StartsWith("GET"))
                    {
                        // Define the response message
                        string responseBody = "Hello, HTTP client!";

                        // Build the HTTP response headers with Content-Length
                        string httpResponse =
                            "HTTP/1.1 200 OK\r\n" +
                            "Content-Type: text/plain\r\n" +
                            "Content-Length: " + responseBody.Length + "\r\n" +  // Add Content-Length header
                            "\r\n" + responseBody;

                        // Convert the response to byte array
                        responseBytes = Encoding.UTF8.GetBytes(httpResponse);

                        // Send the response back to the client
                        stream.Write(responseBytes, 0, responseBytes.Length);
                    }
                    else
                    {
                        // Send a response back to the client
                        string response = "Message received!";
                        responseBytes = Encoding.UTF8.GetBytes(response);
                        stream.Write(responseBytes, 0, responseBytes.Length);
                    }


                }

                Console.WriteLine("Client disconnected.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling client communication: {ex.Message}");
            }
            finally
            {
                // Close the connection
                client.Close();
            }
        }

        public void Stop()
        {
            _isRunning = false;
            _tcpListener.Stop();
            Console.WriteLine("Server stopped.");
        }
    }
}
