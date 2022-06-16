import WebsocketServer from './websocket/server.js';
import * as Logger from './util/logger.js';
import process from 'process';
import { Storage } from './storage.js';

const logger = Logger.getLogger('Main');

Storage.server = new WebsocketServer();

logger.info('App Started');
Storage.server.listen(3000);

process.on('uncaughtException', (err) => {
    if (err.stack) {
        logger.error(err.stack);
    }
    logger.error(err.name);
    logger.error(err.message);
});
