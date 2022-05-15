const Level = {
    DEBUG: 0,
    INFO: 1,
    WARN: 2,
    ERROR: 3,
    FATAL: 4
}

class Log {
    level;
    constructor(level) {
        this.level = level;
    }

    logWrite(msg) {
        msg = `${new Date().toISOString().replace('T', ' ').replace('Z', '')} ${msg}`;
        console.log("\x1b[0m", msg.trim());
        // eslint-disable-next-line no-control-regex
        msg = msg.replace(/\x1b\[\d+m/g, '');
    }

    Debug(prefix, msg) {
        if (this.level <= Level.DEBUG) {
            msg = `\x1b[32m[DEBUG]: ${prefix}\x1b[0m ${msg}`;
            this.logWrite(msg);
        }
    }

    Info(prefix, msg) {
        if (this.level <= Level.INFO) {
            msg = `\x1b[34m[INFO]: ${prefix}\x1b[0m ${msg}`;
            this.logWrite(msg);
        }
    }

    Warn(prefix, msg) {
        if (this.level <= Level.WARN) {
            msg = `\x1b[33m[WARN]: ${prefix}\x1b[0m ${msg}`;
            this.logWrite(msg);
        }
    }

    Error(prefix, msg) {
        if (this.level <= Level.ERROR) {
            msg = `\x1b[31m[ERROR]: ${prefix}\x1b[0m ${msg}`;
            this.logWrite(msg);
        }
    }

    Fatal(prefix, msg) {
        if (this.level <= Level.FATAL) {
            msg = `[FATAL]: ${prefix}\x1b[0m ${msg}`;
            this.logWrite(msg);
        }
    }
}

const Logger = new Log(Level.DEBUG);

exports.getLogger = (prefix) => {
    return {
        debug(msg) {
            Logger.Debug(`[${prefix}]`, `${msg}`);
        },
        info(msg) {
            Logger.Info(`[${prefix}]`, `${msg}`);
        },
        warn(msg) {
            Logger.Warn(`[${prefix}]`, `${msg}`);
        },
        error(msg) {
            Logger.Error(`[${prefix}]`, `${msg}`);
        },
        fatal(msg) {
            Logger.Fatal(`[${prefix}]`, `${msg}`);
        }
    }
}
