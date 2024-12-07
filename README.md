# TCP Server in C#

## Overview
This project implements a simple TCP server using C#. The server listens for incoming connections on a specified IP and port. Upon receiving a connection, it reads incoming data and sends a response back to the client. If the client sends an HTTP GET request, the server responds with a minimal HTTP response (HTTP/1.1 200 OK) along with a plain text message. For other types of requests, it responds with a generic message.


