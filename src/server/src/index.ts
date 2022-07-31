import * as Logger from './util/logger';
import { storage } from './storage';
import process from 'process';
import request from 'request';
import 'dotenv/config'

const logger = Logger.getLogger('Main');

logger.info('App Started');
storage.server.listen(5000);

process.on('uncaughtException', (err) => {
    if (err.stack) {
        logger.error(err.stack);
    }
    logger.error(err.name);
    logger.error(err.message);

    const message = {
        embeds: [{
            title: 'Error',
            description: err.message,
            color: 0xFF0000,
            fields: [{
                name: 'Stack',
                value: err.stack,
                inline: false,
            }],
        }],
    };

    request.post({
        url: process.env.DISCORD_WEBHOOK_URL as string,
        json: true,
        body: message,
    }, (err, res, body) => {
        if (err) {
            logger.error(err);
        }
    });
});
