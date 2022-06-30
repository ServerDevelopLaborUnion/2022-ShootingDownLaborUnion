import mariadb from 'mariadb';

export default class DatabaseManager
{
    pool: mariadb.Pool;
    connection: mariadb.Connection | null;
    constructor() {
        this.pool  = mariadb.createPool({
            host: '172.31.2.199', 
            user:'wassabi', 
            password: 'wa!sans10101',
            connectionLimit: 1
        });
        
        this.connection = null;
        this.pool.getConnection().then(connection => {
            this.connection = connection;
        });
    }

    Read(table:string, attribute:string) {
        this.connection?.query(`Select ${attribute} from ${table}; `);
    }

    Write(table:string, attribute:string, value:any){
        this.connection?.query(`INSERT INTO ${table} { SET ${attribute} = '${value}' `);
    }
}