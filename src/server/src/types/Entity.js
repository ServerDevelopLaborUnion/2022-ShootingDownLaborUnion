exports.Entity = class Entity {
    /**
     * 
     * @param {string} UUID 
     * @param {string} OwnerUUID 
     * @param {string} Name 
     * @param {*} Position 
     * @param {*} Rotation 
     * @param {string} Data 
     */
    constructor(uuid, ownerId, name, position, rotation, data) {
        this.UUID = uuid;
        this.OwnerUUID = ownerId;
        this.Name = name;
        this.Position = position;
        this.Rotation = rotation;
        this.Data = data;
    }
}