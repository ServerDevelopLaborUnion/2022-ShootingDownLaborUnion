import * as Logger from './util/logger';
import { storage } from './storage';
import process from 'process';
import request from 'request';
import 'dotenv/config'

// Main의 Prefix를 가진 Logger 받아와서 변수에 저장
const logger = Logger.getLogger('Main');

logger.info('App Started');
// 서버 시작
storage.server.listen(5000);

// 대처하지 못한 예외 처리
process.on('uncaughtException', (err) => {
    if (err.stack) {
        logger.error(err.stack);
    }
    logger.error(err.name);
    logger.error(err.message);

    // Disccord 웹훅으로 예외 전송
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
