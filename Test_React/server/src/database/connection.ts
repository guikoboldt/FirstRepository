import knex from 'knex';
import path from 'path';

const connection = knex({
    client: 'sqlite3',
    connection: {
        filename: path.resolve(__dirname, 'database.sqlite')
    },
    useNullAsDefault: true
})

export default connection;

//Migrations = Historico do banco de dados
//create table poinst
//create table users
//com o migrations voce armazena cada mudança no banco