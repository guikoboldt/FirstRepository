@requires_database
Funcionalidade: Estória 2 - O mesmo restaurante não pode ser escolhido mais de uma vez durante a semana.
        Eu como facilitador do processo de votação
        Quero que um restaurante não possa ser repetido durante a semana
        Para que não precise ouvir reclamações infinitas!

Cenário: O mesmo restaurante não deve poder ser escolhido mais de uma vez durante a semana.
        Dados os seguintes profissionais: Aderopo, Paulo
        Dados os seguintes restaurantes: Pallato, Badoo, Pizza Hut, Arizona
        Dado que o 'Aderopo' votou no restaurante 'Badoo' 'ontem'
        Então os restaurantes elegíveis 'hoje' deveriam ser: Pallato, Pizza Hut, Arizona

Esquema do Cenário: O mesmo restaurante não deve poder ser escolhido mais de uma vez durante a semana(alternativa).
        Dados os seguintes profissionais: <profissionais>
        Dados os seguintes restaurantes: <restaurantes>
        Dado que o '<eleitor>' votou no restaurante '<votado>' '<data_voto_passado>'
        Então os restaurantes elegíveis '<data_consulta>' deveriam ser: <disponíveis_para_votação>

        Exemplos:
            | restaurantes                    | profissionais  | eleitor | votado   | data_voto_passado | data_consulta | disponíveis_para_votação        |
            | Big E, Plazza, Calibu, Bellview | Aderopo, Paulo | paulo   | Bellview | ontem             | hoje          | Big E, Plazza, Calibu           |
            | Big E, Plazza, Calibu, Bellview | Aderopo, Paulo | paulo   | Bellview | hoje              | hoje          | Big E, Plazza, Calibu, Bellview |

