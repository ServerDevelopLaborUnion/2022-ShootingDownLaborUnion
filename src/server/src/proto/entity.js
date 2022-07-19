// source: Type.proto
/**
 * @fileoverview
 * @enhanceable
 * @suppress {missingRequire} reports error on implicit type usages.
 * @suppress {messageConventions} JS Compiler reports an error if a variable or
 *     field starts with 'MSG_' and isn't a translatable message.
 * @public
 */
// GENERATED CODE -- DO NOT EDIT!
/* eslint-disable */
// @ts-nocheck

goog.provide('proto.Protobuf.Entity');

goog.require('jspb.BinaryReader');
goog.require('jspb.BinaryWriter');
goog.require('jspb.Message');
goog.require('proto.Protobuf.Quaternion');
goog.require('proto.Protobuf.Vector2');

/**
 * Generated by JsPbCodeGenerator.
 * @param {Array=} opt_data Optional initial data array, typically from a
 * server response, or constructed directly in Javascript. The array is used
 * in place and becomes part of the constructed object. It is not cloned.
 * If no data is provided, the constructed object will be empty, but still
 * valid.
 * @extends {jspb.Message}
 * @constructor
 */
proto.Protobuf.Entity = function(opt_data) {
  jspb.Message.initialize(this, opt_data, 0, -1, null, null);
};
goog.inherits(proto.Protobuf.Entity, jspb.Message);
if (goog.DEBUG && !COMPILED) {
  /**
   * @public
   * @override
   */
  proto.Protobuf.Entity.displayName = 'proto.Protobuf.Entity';
}



if (jspb.Message.GENERATE_TO_OBJECT) {
/**
 * Creates an object representation of this proto.
 * Field names that are reserved in JavaScript and will be renamed to pb_name.
 * Optional fields that are not set will be set to undefined.
 * To access a reserved field use, foo.pb_<name>, eg, foo.pb_default.
 * For the list of reserved names please see:
 *     net/proto2/compiler/js/internal/generator.cc#kKeyword.
 * @param {boolean=} opt_includeInstance Deprecated. whether to include the
 *     JSPB instance for transitional soy proto support:
 *     http://goto/soy-param-migration
 * @return {!Object}
 */
proto.Protobuf.Entity.prototype.toObject = function(opt_includeInstance) {
  return proto.Protobuf.Entity.toObject(opt_includeInstance, this);
};


/**
 * Static version of the {@see toObject} method.
 * @param {boolean|undefined} includeInstance Deprecated. Whether to include
 *     the JSPB instance for transitional soy proto support:
 *     http://goto/soy-param-migration
 * @param {!proto.Protobuf.Entity} msg The msg instance to transform.
 * @return {!Object}
 * @suppress {unusedLocalVariables} f is only used for nested messages
 */
proto.Protobuf.Entity.toObject = function(includeInstance, msg) {
  var f, obj = {
    uuid: jspb.Message.getFieldWithDefault(msg, 1, ""),
    owneruuid: jspb.Message.getFieldWithDefault(msg, 2, ""),
    name: jspb.Message.getFieldWithDefault(msg, 3, ""),
    position: (f = msg.getPosition()) && proto.Protobuf.Vector2.toObject(includeInstance, f),
    targetposition: (f = msg.getTargetposition()) && proto.Protobuf.Vector2.toObject(includeInstance, f),
    rotation: (f = msg.getRotation()) && proto.Protobuf.Quaternion.toObject(includeInstance, f),
    data: jspb.Message.getFieldWithDefault(msg, 7, "")
  };

  if (includeInstance) {
    obj.$jspbMessageInstance = msg;
  }
  return obj;
};
}


/**
 * Deserializes binary data (in protobuf wire format).
 * @param {jspb.ByteSource} bytes The bytes to deserialize.
 * @return {!proto.Protobuf.Entity}
 */
proto.Protobuf.Entity.deserializeBinary = function(bytes) {
  var reader = new jspb.BinaryReader(bytes);
  var msg = new proto.Protobuf.Entity;
  return proto.Protobuf.Entity.deserializeBinaryFromReader(msg, reader);
};


/**
 * Deserializes binary data (in protobuf wire format) from the
 * given reader into the given message object.
 * @param {!proto.Protobuf.Entity} msg The message object to deserialize into.
 * @param {!jspb.BinaryReader} reader The BinaryReader to use.
 * @return {!proto.Protobuf.Entity}
 */
proto.Protobuf.Entity.deserializeBinaryFromReader = function(msg, reader) {
  while (reader.nextField()) {
    if (reader.isEndGroup()) {
      break;
    }
    var field = reader.getFieldNumber();
    switch (field) {
    case 1:
      var value = /** @type {string} */ (reader.readString());
      msg.setUuid(value);
      break;
    case 2:
      var value = /** @type {string} */ (reader.readString());
      msg.setOwneruuid(value);
      break;
    case 3:
      var value = /** @type {string} */ (reader.readString());
      msg.setName(value);
      break;
    case 4:
      var value = new proto.Protobuf.Vector2;
      reader.readMessage(value,proto.Protobuf.Vector2.deserializeBinaryFromReader);
      msg.setPosition(value);
      break;
    case 5:
      var value = new proto.Protobuf.Vector2;
      reader.readMessage(value,proto.Protobuf.Vector2.deserializeBinaryFromReader);
      msg.setTargetposition(value);
      break;
    case 6:
      var value = new proto.Protobuf.Quaternion;
      reader.readMessage(value,proto.Protobuf.Quaternion.deserializeBinaryFromReader);
      msg.setRotation(value);
      break;
    case 7:
      var value = /** @type {string} */ (reader.readString());
      msg.setData(value);
      break;
    default:
      reader.skipField();
      break;
    }
  }
  return msg;
};


/**
 * Serializes the message to binary data (in protobuf wire format).
 * @return {!Uint8Array}
 */
proto.Protobuf.Entity.prototype.serializeBinary = function() {
  var writer = new jspb.BinaryWriter();
  proto.Protobuf.Entity.serializeBinaryToWriter(this, writer);
  return writer.getResultBuffer();
};


/**
 * Serializes the given message to binary data (in protobuf wire
 * format), writing to the given BinaryWriter.
 * @param {!proto.Protobuf.Entity} message
 * @param {!jspb.BinaryWriter} writer
 * @suppress {unusedLocalVariables} f is only used for nested messages
 */
proto.Protobuf.Entity.serializeBinaryToWriter = function(message, writer) {
  var f = undefined;
  f = message.getUuid();
  if (f.length > 0) {
    writer.writeString(
      1,
      f
    );
  }
  f = message.getOwneruuid();
  if (f.length > 0) {
    writer.writeString(
      2,
      f
    );
  }
  f = message.getName();
  if (f.length > 0) {
    writer.writeString(
      3,
      f
    );
  }
  f = message.getPosition();
  if (f != null) {
    writer.writeMessage(
      4,
      f,
      proto.Protobuf.Vector2.serializeBinaryToWriter
    );
  }
  f = message.getTargetposition();
  if (f != null) {
    writer.writeMessage(
      5,
      f,
      proto.Protobuf.Vector2.serializeBinaryToWriter
    );
  }
  f = message.getRotation();
  if (f != null) {
    writer.writeMessage(
      6,
      f,
      proto.Protobuf.Quaternion.serializeBinaryToWriter
    );
  }
  f = message.getData();
  if (f.length > 0) {
    writer.writeString(
      7,
      f
    );
  }
};


/**
 * optional string UUID = 1;
 * @return {string}
 */
proto.Protobuf.Entity.prototype.getUuid = function() {
  return /** @type {string} */ (jspb.Message.getFieldWithDefault(this, 1, ""));
};


/**
 * @param {string} value
 * @return {!proto.Protobuf.Entity} returns this
 */
proto.Protobuf.Entity.prototype.setUuid = function(value) {
  return jspb.Message.setProto3StringField(this, 1, value);
};


/**
 * optional string OwnerUUID = 2;
 * @return {string}
 */
proto.Protobuf.Entity.prototype.getOwneruuid = function() {
  return /** @type {string} */ (jspb.Message.getFieldWithDefault(this, 2, ""));
};


/**
 * @param {string} value
 * @return {!proto.Protobuf.Entity} returns this
 */
proto.Protobuf.Entity.prototype.setOwneruuid = function(value) {
  return jspb.Message.setProto3StringField(this, 2, value);
};


/**
 * optional string Name = 3;
 * @return {string}
 */
proto.Protobuf.Entity.prototype.getName = function() {
  return /** @type {string} */ (jspb.Message.getFieldWithDefault(this, 3, ""));
};


/**
 * @param {string} value
 * @return {!proto.Protobuf.Entity} returns this
 */
proto.Protobuf.Entity.prototype.setName = function(value) {
  return jspb.Message.setProto3StringField(this, 3, value);
};


/**
 * optional Vector2 Position = 4;
 * @return {?proto.Protobuf.Vector2}
 */
proto.Protobuf.Entity.prototype.getPosition = function() {
  return /** @type{?proto.Protobuf.Vector2} */ (
    jspb.Message.getWrapperField(this, proto.Protobuf.Vector2, 4));
};


/**
 * @param {?proto.Protobuf.Vector2|undefined} value
 * @return {!proto.Protobuf.Entity} returns this
*/
proto.Protobuf.Entity.prototype.setPosition = function(value) {
  return jspb.Message.setWrapperField(this, 4, value);
};


/**
 * Clears the message field making it undefined.
 * @return {!proto.Protobuf.Entity} returns this
 */
proto.Protobuf.Entity.prototype.clearPosition = function() {
  return this.setPosition(undefined);
};


/**
 * Returns whether this field is set.
 * @return {boolean}
 */
proto.Protobuf.Entity.prototype.hasPosition = function() {
  return jspb.Message.getField(this, 4) != null;
};


/**
 * optional Vector2 TargetPosition = 5;
 * @return {?proto.Protobuf.Vector2}
 */
proto.Protobuf.Entity.prototype.getTargetposition = function() {
  return /** @type{?proto.Protobuf.Vector2} */ (
    jspb.Message.getWrapperField(this, proto.Protobuf.Vector2, 5));
};


/**
 * @param {?proto.Protobuf.Vector2|undefined} value
 * @return {!proto.Protobuf.Entity} returns this
*/
proto.Protobuf.Entity.prototype.setTargetposition = function(value) {
  return jspb.Message.setWrapperField(this, 5, value);
};


/**
 * Clears the message field making it undefined.
 * @return {!proto.Protobuf.Entity} returns this
 */
proto.Protobuf.Entity.prototype.clearTargetposition = function() {
  return this.setTargetposition(undefined);
};


/**
 * Returns whether this field is set.
 * @return {boolean}
 */
proto.Protobuf.Entity.prototype.hasTargetposition = function() {
  return jspb.Message.getField(this, 5) != null;
};


/**
 * optional Quaternion Rotation = 6;
 * @return {?proto.Protobuf.Quaternion}
 */
proto.Protobuf.Entity.prototype.getRotation = function() {
  return /** @type{?proto.Protobuf.Quaternion} */ (
    jspb.Message.getWrapperField(this, proto.Protobuf.Quaternion, 6));
};


/**
 * @param {?proto.Protobuf.Quaternion|undefined} value
 * @return {!proto.Protobuf.Entity} returns this
*/
proto.Protobuf.Entity.prototype.setRotation = function(value) {
  return jspb.Message.setWrapperField(this, 6, value);
};


/**
 * Clears the message field making it undefined.
 * @return {!proto.Protobuf.Entity} returns this
 */
proto.Protobuf.Entity.prototype.clearRotation = function() {
  return this.setRotation(undefined);
};


/**
 * Returns whether this field is set.
 * @return {boolean}
 */
proto.Protobuf.Entity.prototype.hasRotation = function() {
  return jspb.Message.getField(this, 6) != null;
};


/**
 * optional string Data = 7;
 * @return {string}
 */
proto.Protobuf.Entity.prototype.getData = function() {
  return /** @type {string} */ (jspb.Message.getFieldWithDefault(this, 7, ""));
};


/**
 * @param {string} value
 * @return {!proto.Protobuf.Entity} returns this
 */
proto.Protobuf.Entity.prototype.setData = function(value) {
  return jspb.Message.setProto3StringField(this, 7, value);
};

