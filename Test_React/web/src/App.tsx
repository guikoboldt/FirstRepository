//import React, { useState } from 'react';
import React from 'react';
import './App.css';
import Routes from './routes';
//import Home from './pages/Home'; nao precisa colocar o index.tsx aqui, sempre eque nao informação final do arquivo, ele procura por index
//import Header from './Header';

function App() {
    return (
      <Routes />
  );
}


// JSX: Sintaxe de XML dentro do JavaScript
//propriedades sao imutaveis em react, entao precisamos de setters para altera-los, garantindo assim a performance geral do react

// function App() {
//   const [counter, setCounter] = useState(0); //[valor da propriedade, func para manipular o valor]

//   function handleButtonClick() {
//     setCounter(counter + 1);
//   }

//   return (
//     <div>
//       <Header title="Hello World"/>

//       <h1>{counter}</h1>
//       <button type="button" onClick={handleButtonClick}>Aumentar</button>
//     </div>
//   );
// }

export default App;
