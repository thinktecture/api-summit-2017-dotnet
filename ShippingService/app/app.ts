import * as Consul from "consul";
import RegisterOptions = Consul.Agent.Service.RegisterOptions;
import Service = Consul.Agent.Service;

import * as restify from 'restify';
import StatusController from './controllers/statusController';
import {settings} from './config/settings';

// Consul service agent
let registerOptions: RegisterOptions = {
    name: 'shipping',
    address: 'http://localhost:' + settings.port,
    check: {
        http: 'http://localhost:' + settings.port + '/api/ping',
        interval: '30s'
    }
};

let consul: Consul.Consul = new Consul();
let service: Consul.Agent.Service = consul.agent.service;

service.register(registerOptions, data => {
    if (data) {
        console.log("From Consul: " + data)
    }
});

// Restify Web API
export let server = restify.createServer({
    name: settings.name
});

server.use(restify.CORS());
server.use(restify.bodyParser());
server.use(restify.queryParser());
server.use(restify.fullResponse());

server.get('/api/ping', new StatusController().get);

server.listen(settings.port, function () {
    console.log('Shipping Service running - listening at %s', server.url);
});
