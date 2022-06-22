import WebsocketServer from './websocket/server';
import * as Logger from './util/logger';
import process from 'process';
import { storage } from './storage';

const logger = Logger.getLogger('Main');

storage.server = new WebsocketServer();

logger.info('App Started');
storage.server.listen(3000);

process.on('uncaughtException', (err) => {
    if (err.stack) {
        logger.error(err.stack);
    }
    logger.error(err.name);
    logger.error(err.message);
});
