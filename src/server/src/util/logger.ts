import { WritieLog } from "./notion";

const Level = {
    DEBUG: 0,
    INFO: 1,
    WARN: 2,
    ERROR: 3,
    FATAL: 4
}

class Log {
    level;
    constructor(level: number) {
        this.level = level;
    }

    logWrite(msg: string) {
        msg = `${new Date().toISOString().replace('T', ' ').replace('Z', '')} ${msg}`;
        console.log("\x1b[0m", msg.trim());
        // eslint-disable-next-line no-control-regex
        msg = msg.replace(/\x1b\[\d+m/g, '');
    }

    Debug(prefix: string, msg: string) {
        if (this.level <= Level.DEBUG) {
            msg = `\x1b[32m[DEBUG]: ${prefix}\x1b[0m ${msg}`;
            WritieLog(msg, "Debug");
            this.logWrite(msg);
        }
    }

    Info(prefix: string, msg: string) {
        if (this.level <= Level.INFO) {
            msg = `\x1b[34m[INFO]: ${prefix}\x1b[0m ${msg}`;
            WritieLog(msg, "Info");
            this.logWrite(msg);
        }
    }

    Warn(prefix: string, msg: string) {
        if (this.level <= Level.WARN) {
            msg = `\x1b[33m[WARN]: ${prefix}\x1b[0m ${msg}`;
            WritieLog(msg, "Warn");
            this.logWrite(msg);
        }
    }

    Error(prefix: string, msg: string) {
        if (this.level <= Level.ERROR) {
            msg = `\x1b[31m[ERROR]: ${prefix}\x1b[0m ${msg}`;
            WritieLog(msg, "Error");
            this.logWrite(msg);
        }
    }

    Fatal(prefix: string, msg: string) {
        if (this.level <= Level.FATAL) {
            msg = `[FATAL]: ${prefix}\x1b[0m ${msg}`;
            WritieLog(msg, "Fatal");
            this.logWrite(msg);
        }
    }
}

const Logger = new Log(Level.DEBUG);

export function getLogger(prefix: string) {
    return {
        debug(msg: string) {
            Logger.Debug(`[${prefix}]`, `${msg}`);
        },
        info(msg: string) {
            Logger.Info(`[${prefix}]`, `${msg}`);
        },
        warn(msg: string) {
            Logger.Warn(`[${prefix}]`, `${msg}`);
        },
        error(msg: string) {
            Logger.Error(`[${prefix}]`, `${msg}`);
        },
        fatal(msg: any) {
            Logger.Fatal(`[${prefix}]`, `${msg}`);
        }
    }
}
