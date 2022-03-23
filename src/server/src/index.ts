import server from './Websocket/server';
import getLogger from './Utils/logger';

const logger = getLogger('Main');
logger.Info('App Started');
server.listen(3000);

process.on('uncaughtException', (err) => {
    logger.Error(err.name);
    logger.Error(err.message);
});
