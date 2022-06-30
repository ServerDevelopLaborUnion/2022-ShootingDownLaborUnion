import * as Logger from './util/logger';
import { storage } from './storage';
import process from 'process';
import WebsocketServer from './websocket/server';

const logger = Logger.getLogger('Main');

logger.info('App Started');
storage.server.listen(3000);

// process.on('uncaughtException', (err) => {
//     if (err.stack) {
//         logger.error(err.stack);
//     }
//     logger.error(err.name);
//     logger.error(err.message);
// });
