@requires_database
Funcionalidade: Estória 3 - Mostrar de alguma forma o resultado da votação.
    Eu como profissional faminto
    Quero saber antes do meio dia qual foi o restaurante escolhido
    Para que eu possa me despir de preconceitos e preparar o psicológico.

Contexto: Empate na votação
        Dados os seguintes profissionais: Bruno, Kauã, Maicon, Brian, Matheus, Tiago
        E os seguintes restaurantes: Piazza, Pinguin, Banquette, Speedy

Cenário: Caso houver empate, Os mais votados deveriam ser retornados como os escolhidos.
        Dado que o 'Matheus' votou no restaurante 'Speedy' 'hoje'
        Então o restaurante mais votado no momento deveria ser: Speedy
        Dado que 'Bruno, Kauã' votaram no restaurante 'Piazza' 'hoje'
        Então o restaurante mais votado no momento deveria ser: Piazza
        Dado que 'Maicon, Brian' votaram no restaurante 'Banquette' 'hoje'
        Então os restaurantes mais votados no momento deveriam ser: Piazza, Banquette
        Dado que o 'Tiago' votou no restaurante 'Piazza' 'hoje'
        Então os restaurantes mais votados deveriam ser: Piazza
            


