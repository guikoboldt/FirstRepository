import React from 'react';

//componente sempre com letra maiuscula
//React.FC -> function component indica que o componente esta atuando como função, funciona como um tipo generico

interface HeaderProps {
    title: string;
    // title?: string; indica aqui que a propos e opcional
}

const Header: React.FC<HeaderProps> = (props) => {
    return (
        <header>
            <h1>{props.title}</h1>
        </header>
    );
}

export default Header;