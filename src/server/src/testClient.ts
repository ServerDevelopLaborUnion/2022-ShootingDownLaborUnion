import * as websocket from 'websocket';

const client = new websocket.client();

client.connect('ws://localhost:3000');

client.on('connect', (connection) => {
    console.log('connected');
    const data = {
        username: 'test',
        password: 'test'
    }
    
});