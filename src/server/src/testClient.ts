import * as websocket from 'websocket';

const client = new websocket.client();


client.connect('ws://localhost:3000');

client.on('connect', (connection) => {
    console.log('connected');
    const data = {
        username: 'test',
        password: 'test'
    }

    const buffer = Buffer.alloc(5 + data.username.length + data.password.length);
    buffer.writeUInt32BE(0, 0);
    buffer.writeUInt32BE(data.username.length, 4);
    buffer.write(data.username, 8);
    buffer.write(data.password, 8 + data.username.length);
    connection.sendBytes(buffer);
    console.log(buffer);
});