@requires_database
Funcionalidade: Estória 1 - Um professional só pode votar em um restaurante por dia.
    Eu como profissional faminto
    Quero votar no meu restaurante favorito
    Para que eu consiga democraticamente levar meus colegas a comer onde eu gusto.

Contexto: 
        Dados os seguintes profissionais: Bruno, Kauã, Ahmed, Maicon
        E os seguintes restaurantes: Piazza, Colegiais, Banquette

Cenário: Um professional só poderia votar em um restaurante por dia.
        Dado que o 'Ahmed' votou no restaurante 'Piazza' 'hoje'
        Então o 'Ahmed' não deveria poder votar no restaurante 'Piazza' 'hoje' novamente
        E o 'Ahmed' não deveria poder votar no restaurante 'Colegiais' 'hoje' novamente
        Mas 'Kauã, Maicon' deveriam poder votar no restaurante 'Piazza' 'hoje'
        E o 'Bruno' deveria poder votar no restaurante 'Colegiais' 'hoje'


