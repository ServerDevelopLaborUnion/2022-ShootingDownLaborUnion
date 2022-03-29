const enum Level {
    DEBUG = 0,
    INFO = 1,
    WARN = 2,
    ERROR = 3,
    FATAL = 4
}

class Log {
    level: Level;
    constructor(level: Level) {
        this.level = level;
    }

    logWrite(msg: string) {
        msg = `${new Date().toISOString().replace('T', '').replace('Z', '')} ${msg}`;
        console.log("\x1b[0m", msg.trim());
        // eslint-disable-next-line no-control-regex
        msg = msg.replace(/\x1b\[\d+m/g, '');
    }

    Debug(prefix: string, msg: string) {
        if (this.level <= Level.DEBUG) {
            msg = `\x1b[32m[DEBUG]: ${prefix}\x1b[0m ${msg}`;
            this.logWrite(msg);
        }
    }

    Info(prefix: string, msg: string) {
        if (this.level <= Level.INFO) {
            msg = `\x1b[34m[INFO]: ${prefix}\x1b[0m ${msg}`;
            this.logWrite(msg);
        }
    }

    Warn(prefix: string, msg: string) {
        if (this.level <= Level.WARN) {
            msg = `\x1b[33m[WARN]: ${prefix}\x1b[0m ${msg}`;
            this.logWrite(msg);
        }
    }

    Error(prefix: string, msg: string) {
        if (this.level <= Level.ERROR) {
            msg = `\x1b[31m[ERROR]: ${prefix}\x1b[0m ${msg}`;
            this.logWrite(msg);
        }
    }

    Fatal(prefix: string, msg: string) {
        if (this.level <= Level.FATAL) {
            msg = `[FATAL]: ${prefix}\x1b[0m ${msg}`;
            this.logWrite(msg);
        }
    }
}

const Logger = new Log(Level.DEBUG);

export default function GetLogger(prefix: string) {
    return {
        Debug(msg: string) {
            Logger.Debug(`[${prefix}]`, `${msg}`);
        },
        Info(msg: string) {
            Logger.Info(`[${prefix}]`, `${msg}`);
        },
        Warn(msg: string) {
            Logger.Warn(`[${prefix}]`, `${msg}`);
        },
        Error(msg: string) {
            Logger.Error(`[${prefix}]`, `${msg}`);
        },
        Fatal(msg: string) {
            Logger.Fatal(`[${prefix}]`, `${msg}`);
        }
    }
}
