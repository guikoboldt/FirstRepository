import express from 'express';

const app = express();

app.get('/users', (request, response) => 
{
    console.log('listagem de usuários');

    //response.send('Hello world'); resposta direta (html)
    response.json([
        'Diego',
        'Cleiton',
        'Robson',
        'Daniel'
    ]);
});

app.listen(3333);