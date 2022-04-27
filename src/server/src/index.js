const { WebsocketServer } = require('./websocket/server');
const logger = require('./util/logger').getLogger('Main');

const server = WebsocketServer;

logger.info('App Started');
server.listen(3000);

// eslint-disable-next-line no-undef
process.on('uncaughtException', (err) => {
    if (err.stack) {
        logger.error(err.stack);
    }
    logger.error(err.name);
    logger.error(err.message);
});
