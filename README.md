# API &amp; Microservices Summit 2017 in Munich
Code and other resources for the "APIs &amp; mehr mit .NET – von Null auf Hundert" workshop at the API &amp; Microservices Summit 2017 in Munich

## Running the infrastructure with Docker
Install Docker for Windows:
https://www.docker.com/docker-windows

### Consul
https://hub.docker.com/_/consul/
(docker pull consul)

`docker run -d -p 8400:8400 -p 8500:8500 -p 8600:53/udp consul agent -server -bootstrap -client=0.0.0.0 -ui
`
### RabbitMQ
https://hub.docker.com/_/rabbitmq/
(docker pull rabbitmq)

`docker run -d --hostname my-rabbit --name my-rabbit -p 15672:15672 rabbitmq:3-management
`
### IdentityServer
`git clone https://github.com/IdentityServer/IdentityServer4.Samples/tree/release/Docker
`

cd into repository folder

`docker-compose up --build
`

This should be it ;-)

The open the VS solutions and the node.js respectively Angular projects and give it a try.
