import React from 'react';
import logo from '../../assets/logo.svg';
import { Link } from 'react-router-dom';
import './styles.css';
import { FiLogIn } from 'react-icons/fi';

//Link faz a mesma coisa q o a href, porem ele nao recarrega a pagina, fazendo uso de spa mesmo

const Home = () => {
    return (
        <div id="page-home">
            <div className="content">
                <header>
                    <img src={logo} alt="Ecoleta" />
                </header>

                <main>
                    <h1>Seu marketplace de coleta de resíduos.</h1>
                    <p>Ajudamos pessoas a encontrarem pontos de coleta de forma eficiente.</p>

                    <Link to="/create-point">
                        <span>
                            <FiLogIn />
                        </span>
                        <strong>Cadastre um ponto de coleta</strong>
                    </Link>
                </main>

            </div>
        </div>
    );
}

export default Home;