import * as flatbuffers from 'flatbuffers';
import { LoginRequest } from "../flatbuf/login-request";

export default {
    receive: function (buffer: Buffer) {
        const type: number = buffer.readUInt32BE(0);
        const length: number = buffer.readUInt32BE(1);
        const data = new flatbuffers.ByteBuffer(buffer.slice(5, 5 + length));

        switch (type) {
            case LoginRequest.type:
                return LoginRequest.getRootAsLoginRequest(data);
                break;
        }
    }
}
