import server from './websocket/server';
import getLogger from './util/logger';

const logger = getLogger('Main');
logger.Info('App Started');
server.listen(3000);

process.on('uncaughtException', (err) => {
    if (err.stack) {
        logger.Error(err.stack);
    }
    logger.Error(err.name);
    logger.Error(err.message);
});
